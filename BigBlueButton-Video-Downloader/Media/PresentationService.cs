using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Models;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace BigBlueButton_Video_Downloader.Media
{
    public class PresentationService : IPresentationService
    {
        private IVideoService _videoService;

        public PresentationService(IVideoService videoService)
        {
            _videoService = videoService;
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full).GetAwaiter().GetResult();
        }

        public async Task CreatePresentation(List<PresentationItem> presentationItems,
            string directory,
            string outputFileName,
            string audioPath = null)
        {
            Directory.SetCurrentDirectory(directory);

            var tempOutput = $"Temp{outputFileName}";
            var videoPaths = new List<string>();

            await using var streamWriter = new StreamWriter("videoList.txt");

            foreach (var presentationItem in presentationItems)
            {
                var index = presentationItems.IndexOf(presentationItem);
                var videoPath = $"{index}.mp4";
                videoPaths.Add(videoPath);
                var presentationItemOut = 1.0 / (presentationItem.Out - presentationItem.In);

                await FFmpeg.Conversions.New()
                    .BuildVideoFromImages(new[] {presentationItem.LocalSource})
                    .SetFrameRate(presentationItemOut)
                    .SetInputFrameRate(1)
                    .SetPixelFormat(PixelFormat.yuv420p)
                    .SetOutput(videoPath)
                    .Start();

                await streamWriter.WriteLineAsync($"file '{videoPath}'");
            }

            streamWriter.Close();


            await FFmpeg.Conversions.New()
                .AddParameter("-f concat")
                .AddParameter("-safe 0")
                .AddParameter("-i videoList.txt -c copy")
                .AddParameter(tempOutput)
                .Start();


            if (!string.IsNullOrEmpty(audioPath))
            {
                await _videoService.AddAudio(tempOutput, audioPath, outputFileName);
                File.Delete(tempOutput);
                return;
            }


            for (int i = 0; i < presentationItems.Count; i++)
            {
                if (File.Exists($"{i}.mp4"))
                    File.Delete($"{i}.mp4");
                if (File.Exists($"{i}.png"))
                    File.Delete($"{i}.png");
            }

            File.Delete("videoList.txt");

            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var newFileDestination = Path.Combine(parentDirectory, outputFileName);
            File.Move(tempOutput, newFileDestination, true);
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
            Directory.Delete(directory);
        }
    }
}