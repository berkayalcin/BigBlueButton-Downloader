using System;
using System.IO;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Helpers;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.Webdriver;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton.Interfaces
{
    public class BigBlueButtonFacade : IBigBlueButtonFacade
    {
        private IWebDriver _webDriver;
        private IBigBlueButtonDocumentParser _bigBlueButtonDocumentParser;
        private readonly IFileDownloader _fileDownloader;
        private readonly IVideoService _videoService;
        private string _url;
        private WebDriverType _webDriverType;
        private bool _isDownloadDeskShareVideo;
        private bool _isDownloadWebcamVideo;
        private bool _isUseMultithread;
        private VideoType _videoType;
        private string _outputFileName;
        private BigBlueButtonDocumentOptions _deskShareOptions;
        private BigBlueButtonDocumentOptions _webcamOptions;


        public BigBlueButtonFacade(IFileDownloader fileDownloader,
            IVideoService videoService)
        {
            _fileDownloader = fileDownloader;
            _videoService = videoService;
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
            _isDownloadDeskShareVideo = true;
            return this;
        }

        public IBigBlueButtonFacade EnableDownloadWebcamVideo()
        {
            _isDownloadWebcamVideo = true;
            return this;
        }

        public IBigBlueButtonFacade DisableDownloadWebcamVideo()
        {
            _isDownloadWebcamVideo = true;
            return this;
        }

        public IBigBlueButtonFacade EnableMultiThread()
        {
            _isUseMultithread = true;
            return this;
        }

        public IBigBlueButtonFacade DisableMultiThread()
        {
            _isUseMultithread = false;
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

        public IBigBlueButtonFacade SetDeskshareOptions(BigBlueButtonDocumentOptions deskShareOptions)
        {
            _deskShareOptions = deskShareOptions;
            return this;
        }

        public IBigBlueButtonFacade SetWebcamOptions(BigBlueButtonDocumentOptions webcamOptions)
        {
            _webcamOptions = webcamOptions;
            return this;
        }

        public async Task StartAsync()
        {
            if (!_isDownloadWebcamVideo && !_isDownloadDeskShareVideo)
                throw new Exception("Either Webcam or Desk-share Video Must Be Selected");

            _webDriver = WebDriverFactory.Create(_webDriverType);
            _webDriver.Url = _url;
            _bigBlueButtonDocumentParser = new BigBlueButtonDocumentParser(_webDriver);


            var deskShareVideoName = $"{_outputFileName}-desk";
            var webcamVideoName = $"{_outputFileName}-webcam";
            var audioOutputName = $"{_outputFileName}-output";


            if (_isDownloadDeskShareVideo)
            {
                Console.WriteLine("Downloading Desk-Share Video");
                _bigBlueButtonDocumentParser.SetOptions(_deskShareOptions);
                var deskShareVideoSource = _bigBlueButtonDocumentParser.GetVideoSource();
                await _fileDownloader.DownloadVideoFileAsync(deskShareVideoSource,
                    deskShareVideoName,
                    _videoType,
                    (sender, args) => { }
                );
            }

            if (_isDownloadWebcamVideo)
            {
                Console.WriteLine("Downloading Webcam Video");
                _bigBlueButtonDocumentParser.SetOptions(_webcamOptions);
                var webCamVideoSource = _bigBlueButtonDocumentParser.GetVideoSource();
                await _fileDownloader.DownloadVideoFileAsync(webCamVideoSource,
                    webcamVideoName,
                    _videoType,
                    (sender, args) => { }
                );
            }

            var audioInputPath = AppDomainHelpers.GetPath(webcamVideoName, ".mp4");
            var audioOutputPath = AppDomainHelpers.GetPath($"{audioOutputName}", ".mp3");
            var deskShareInputPath = AppDomainHelpers.GetPath($"{deskShareVideoName}", ".mp4");
            var videoOutputPath = AppDomainHelpers.GetPath(_outputFileName, ".mp4");

            if (File.Exists(audioInputPath))
            {
                Console.WriteLine("Extracting Audio");
                await _videoService.ExtractAudio(audioInputPath,
                    audioOutputPath,
                    (o, args) => { },
                    _isUseMultithread
                );

                Console.WriteLine("Adding Audio");
                await _videoService.AddAudio(deskShareInputPath,
                    audioOutputPath,
                    videoOutputPath,
                    (o, args) => { },
                    _isUseMultithread
                );

                RemoveTempFiles(webcamVideoName, audioOutputName, deskShareVideoName);
                return;
            }
            
            File.Move(deskShareInputPath, videoOutputPath, true);
            RemoveTempFiles(webcamVideoName, audioOutputName, deskShareVideoName);
        }

        public void RemoveTempFiles(string webcamVideoName, string audioOutputName, string deskShareVideoName)
        {
            Console.WriteLine("Removing Temp Files");

            if (File.Exists(webcamVideoName))
                File.Delete($"{webcamVideoName}.mp4");
            if (File.Exists(audioOutputName))
                File.Delete($"{audioOutputName}.mp3");
            if (File.Exists(deskShareVideoName))
                File.Delete($"{deskShareVideoName}.mp4");

            Console.WriteLine($"{_outputFileName} Done!");
        }
    }
}