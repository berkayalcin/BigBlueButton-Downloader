using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.BigBlueButton.Interfaces;
using BigBlueButton_Video_Downloader.Constants;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Exceptions;
using BigBlueButton_Video_Downloader.Helpers;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.Models;
using BigBlueButton_Video_Downloader.Webdriver;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public class BigBlueButtonFacade : IBigBlueButtonFacade
    {
        private IWebDriver _webDriver;
        private IBigBlueButtonDocumentParser _bigBlueButtonDocumentParser;
        private readonly IFileDownloader _fileDownloader;
        private readonly IVideoService _videoService;
        private readonly IPresentationService _presentationService;
        private string _url;
        private WebDriverType _webDriverType;
        private bool _isDownloadDeskShareVideo;
        private bool _isDownloadWebcamVideo;
        private bool _isDownloadPresentation;
        private bool _isUseMultiThread;
        private int _timeOutSeconds = 15;
        private VideoType _videoType;
        private string _outputFileName;
        private BigBlueButtonDocumentOptions _options;

        public BigBlueButtonDocumentOptions Options
        {
            get
            {
                if (_options == null)
                    return (_options = new BigBlueButtonDocumentOptions(_timeOutSeconds,
                        _url,
                        new BigBlueButtonDocumentWebcamVideoOptions(_videoType, BigBlueButtonConstants.WebcamSelector),
                        new BigBlueButtonDocumentDeskShareVideoOptions(_videoType,
                            BigBlueButtonConstants.DeskShareSelector),
                        new BigBlueButtonPresentationOptions(BigBlueButtonConstants.PresentationSelector)));
                return _options;
            }
        }


        public BigBlueButtonFacade(IFileDownloader fileDownloader,
            IVideoService videoService, IPresentationService presentationService)
        {
            _fileDownloader = fileDownloader;
            _videoService = videoService;
            _presentationService = presentationService;
        }

        public IBigBlueButtonFacade SetTimeoutSeconds(int seconds)
        {
            _timeOutSeconds = seconds;
            return this;
        }

        public IBigBlueButtonFacade SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public IBigBlueButtonFacade SetDriverType(WebDriverType driverType)
        {
            _webDriverType = driverType;
            return this;
        }

        public IBigBlueButtonFacade EnableDownloadDeskshareVideo()
        {
            _isDownloadDeskShareVideo = true;
            return this;
        }

        public IBigBlueButtonFacade DisableDownloadDeskshareVideo()
        {
            _isDownloadDeskShareVideo = false;
            return this;
        }

        public IBigBlueButtonFacade EnableDownloadPresentation()
        {
            _isDownloadPresentation = true;
            return this;
        }

        public IBigBlueButtonFacade DisableDownloadPresentation()
        {
            _isDownloadPresentation = false;
            return this;
        }

        public IBigBlueButtonFacade EnableDownloadWebcamVideo()
        {
            _isDownloadWebcamVideo = true;
            return this;
        }

        public IBigBlueButtonFacade DisableDownloadWebcamVideo()
        {
            _isDownloadWebcamVideo = false;
            return this;
        }

        public IBigBlueButtonFacade EnableMultiThread()
        {
            _isUseMultiThread = true;
            return this;
        }

        public IBigBlueButtonFacade DisableMultiThread()
        {
            _isUseMultiThread = false;
            return this;
        }

        public IBigBlueButtonFacade SetOutputFileName(string fileName)
        {
            _outputFileName = fileName;
            return this;
        }

        public IBigBlueButtonFacade SetVideoType(VideoType videoType)
        {
            _videoType = videoType;
            return this;
        }


        public async Task StartAsync()
        {
            if (!_isDownloadWebcamVideo && !_isDownloadPresentation)
                throw new Exception("Either Presentaton or Desk-share Video Must Be Selected");

            bool downloadVideoResponse = false,
                downloadPresentationResponse = false,
                hasAudio = false;

            _webDriver = WebDriverFactory.Create(_webDriverType);
            _webDriver.Url = _url;

            _bigBlueButtonDocumentParser = new BigBlueButtonDocumentParser(_webDriver, Options);

            var deskShareVideoName = $"{_outputFileName}-desk";
            var webcamVideoName = $"{_outputFileName}-webcam";
            var audioOutputName = $"{_outputFileName}-output";
            var presentationName = $"{_outputFileName}-presentation";
            var tmpPresentationName = $"{_outputFileName}-tmp";

            if (_isDownloadDeskShareVideo)
            {
                var response = await DownloadVideo(deskShareVideoName);
                downloadVideoResponse = response.IsSuccess;
            }

            if (_isDownloadWebcamVideo)
            {
                var response = await DownloadWebcam(webcamVideoName);
                hasAudio = response.IsSuccess;
            }

            if (_isDownloadPresentation)
            {
                var response = await DownloadPresentation(tmpPresentationName);
                downloadPresentationResponse = response.IsSuccess;
            }

            if (!downloadVideoResponse && !downloadPresentationResponse)
                throw new BigBlueButtonDocumentException("Neither Video Nor Presentation Couldn't Be Downloaded.");


            var audioInputPath = AppDomainHelpers.GetPath(webcamVideoName, ".mp4");
            var audioOutputPath = AppDomainHelpers.GetPath($"{audioOutputName}", ".mp3");
            var deskShareInputPath = AppDomainHelpers.GetPath($"{deskShareVideoName}", ".mp4");
            var videoOutputPath = AppDomainHelpers.GetPath(_outputFileName, ".mp4");
            var presentationOutputPath = AppDomainHelpers.GetPath(presentationName, ".mp4");

            if (downloadVideoResponse)
            {
                if (hasAudio)
                    await ExtractAndAddAudio(audioInputPath, audioOutputPath, deskShareInputPath,
                        videoOutputPath);
                else
                    File.Move(deskShareInputPath, videoOutputPath, true);
            }

            if (downloadPresentationResponse)
            {
                if (hasAudio)
                    await _videoService.AddAudio(tmpPresentationName, audioOutputPath, presentationName);
                else
                    File.Move(tmpPresentationName, presentationName, true);
            }

            RemoveTempFiles(
                webcamVideoName,
                audioOutputName,
                deskShareVideoName,
                tmpPresentationName
            );
        }

        private async Task ExtractAndAddAudio(string audioInputPath, string audioOutputPath,
            string inputPath, string outputPath)
        {
            Console.WriteLine("Extracting Audio");
            await _videoService.ExtractAudio(audioInputPath,
                audioOutputPath,
                (o, args) => { },
                _isUseMultiThread
            );

            Console.WriteLine("Adding Audio");
            await _videoService.AddAudio(inputPath,
                audioOutputPath,
                outputPath,
                (o, args) => { },
                _isUseMultiThread
            );
        }

        private async Task<FacadeInnerResponseModel> DownloadVideo(string deskShareVideoName)
        {
            Console.WriteLine("Downloading Desk-Share Video");
            var deskShareVideoSource = _bigBlueButtonDocumentParser.GetDeskShareVideoSource();

            if (!deskShareVideoSource.IsSuccess)
                return new FacadeInnerResponseModel()
                {
                    ErrorMessage = ExceptionCodeConstants.NoDeskShareSourceFound,
                    IsSuccess = false
                };
            await _fileDownloader.DownloadVideoFileAsync(deskShareVideoSource.SourceUrl,
                deskShareVideoName,
                _videoType,
                (sender, args) => { }
            );
            return new FacadeInnerResponseModel()
            {
                IsSuccess = true
            };
        }

        private async Task<FacadeInnerResponseModel> DownloadWebcam(string webcamVideoName)
        {
            Console.WriteLine("Downloading Webcam Video");
            var webCamVideoSource = _bigBlueButtonDocumentParser.GetWebcamVideoSource();
            if (!webCamVideoSource.IsSuccess)
                return new FacadeInnerResponseModel()
                {
                    ErrorMessage = ExceptionCodeConstants.NoWebcamSourceFound,
                    IsSuccess = false
                };
            await _fileDownloader.DownloadVideoFileAsync(webCamVideoSource.SourceUrl,
                webcamVideoName,
                _videoType,
                (sender, args) => { }
            );
            return new FacadeInnerResponseModel()
            {
                IsSuccess = true
            };
        }

        private async Task<FacadeInnerResponseModel> DownloadPresentation(string presentationName)
        {
            Console.WriteLine("Downloading Presentation");
            var presentation = _bigBlueButtonDocumentParser.GetPresentation();
            if (!presentation.IsSuccess)
                return new FacadeInnerResponseModel()
                {
                    ErrorMessage = ExceptionCodeConstants.NoPresentationFound,
                    IsSuccess = false
                };


            var tempDirectory = $"temp-{presentationName}";
            Directory.CreateDirectory(tempDirectory);
            Directory.SetCurrentDirectory(tempDirectory);

            var presentationItems = presentation.Items.ToList();
            foreach (var presentationItem in presentationItems)
            {
                var index = presentationItems.IndexOf(presentationItem);

                await _fileDownloader.DownloadPngFileAsync(presentationItem.Source,
                    index.ToString(),
                    null,
                    null
                );
                presentationItems[index].LocalSource = $"{index}.png";
            }

            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);

            await _presentationService.CreatePresentation(presentationItems, tempDirectory, $"{presentationName}.mp4");


            return new FacadeInnerResponseModel()
            {
                IsSuccess = true
            };
        }

        public void RemoveTempFiles(string webcamVideoName, string audioOutputName, string deskShareVideoName,
            string presentationName)
        {
            Console.WriteLine("Removing Temp Files");

            if (File.Exists(webcamVideoName))
                File.Delete($"{webcamVideoName}.mp4");
            if (File.Exists(audioOutputName))
                File.Delete($"{audioOutputName}.mp3");
            if (File.Exists(deskShareVideoName))
                File.Delete($"{deskShareVideoName}.mp4");
            if (File.Exists(presentationName))
                File.Delete($"{presentationName}.mp4");

            Console.WriteLine($"{_outputFileName} Done!");
        }
    }
}