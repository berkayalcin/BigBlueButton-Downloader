using System;
using System.IO;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Webdriver;
using OpenQA.Selenium;
using Xabe.FFmpeg;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public class BigBlueButtonVideoDownloader
    {
        private readonly IWebDriver _webDriver;

        public BigBlueButtonVideoDownloader(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public async Task DownloadVideo(string fileName)
        {
                        
            var deskShareVideoName = $"{fileName}-desk";
            var webcamVideoName = $"{fileName}-web";
            var audioOutputName = $"{fileName}-output";
            var audioInputPath = GetPath($"{webcamVideoName}", ".mp4");
            var audioOutputPath = GetPath($"{audioOutputName}", ".mp3");
            var deskShareInputPath = GetPath($"{deskShareVideoName}", ".mp4");
            var videoOutputPath = GetPath(fileName, ".mp4");

            await DownloadDeskShareVideo(deskShareVideoName);
            await DownloadWebcamVideo(webcamVideoName);

            
            Console.Write("Extracting Audio");
            var extractAudio = await FFmpeg.Conversions.FromSnippet.ExtractAudio(audioInputPath, audioOutputPath);
            extractAudio.UseMultiThread(true);
            extractAudio.OnProgress += (sender, args) => {  };
            await extractAudio.Start();

            Console.Write("Adding Audio");
            var addAudio =
                await FFmpeg.Conversions.FromSnippet.AddAudio(deskShareInputPath, audioOutputPath, videoOutputPath);
            addAudio.UseMultiThread(true);
            addAudio.OnProgress += (sender, args) => {  };
            await addAudio.Start();

            Console.WriteLine("Removing Temp Files");
            File.Delete($"{webcamVideoName}.mp4");
            File.Delete($"{audioOutputName}.mp3");
            File.Delete($"{deskShareVideoName}.mp4");
            Console.WriteLine($"{fileName} Done!");
        }

        private async Task DownloadDeskShareVideo(string fileName = null)
        {
            var videoType = VideoType.Mp4;

            var bigBlueButtonDocumentParser = new BigBlueButtonDocumentParser(_webDriver,
                new BigBlueButtonDocumentOptions("#deskshare-video", 120, videoType));


            var videoSource = bigBlueButtonDocumentParser.GetVideoSource();
            var recordingTitle = bigBlueButtonDocumentParser.GetRecordingTitle();
            var nowTicks = DateTime.Now.Ticks;

            fileName ??= $"{recordingTitle.Replace(" ", "")}-{nowTicks}";

            Console.Write("Downloading Desk Share Video");
            await FileDownloader.DownloadVideoFileAsync(videoSource,
                fileName,
                videoType,
                (sender, args) =>
                {
                    
                }
            );
            Console.WriteLine("\n");
        }

        private string GetPath() => AppDomain.CurrentDomain.BaseDirectory;

        private string GetPath(string fileName, string fileExtension) =>
            $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}{fileExtension}";

        private async Task DownloadWebcamVideo(string fileName = null)
        {
            var videoType = VideoType.Mp4;

            var bigBlueButtonDocumentParser = new BigBlueButtonDocumentParser(_webDriver,
                new BigBlueButtonDocumentOptions("#video", 120, videoType));


            var videoSource = bigBlueButtonDocumentParser.GetVideoSource();
            var recordingTitle = bigBlueButtonDocumentParser.GetRecordingTitle();
            fileName ??= $"{recordingTitle.Replace(" ", "")}-{DateTime.Now.Ticks}";

            Console.Write("Downloading Webcam Video And Audio");
            await FileDownloader.DownloadVideoFileAsync(videoSource,
                fileName,
                videoType,
                (sender, args) => {  }
            );
        }
    }
}