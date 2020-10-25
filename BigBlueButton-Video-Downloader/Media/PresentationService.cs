using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Models;
using Konsole;
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
            var tempOutput = $"temp_{outputFileName}";

            await using var streamWriter = new StreamWriter("videoList.txt");

            var parallelLoopResult = Parallel.ForEach(presentationItems, (presentationItem, state, index) =>
            {
                var videoPath = $"{index}.mp4";
                var presentationItemOut = 1.0 / (presentationItem.Out - presentationItem.In);
                Console.WriteLine(presentationItemOut);
                var duration = TimeSpan.FromSeconds(presentationItem.Out - presentationItem.In).ToString(@"hh\:mm\:ss");
                try
                {
                    var conversionResult = FFmpeg.Conversions.New()
                        // .AddParameter("-n -threads 1")
                        .AddParameter("-y")
                        .AddParameter($"-framerate {presentationItemOut:0.00000}")
                        .AddParameter($"-i {presentationItem.LocalSource}")
                        .AddParameter("-vcodec libx264")
                        .AddParameter("-crf 27")
                        .AddParameter("-preset veryfast")
                        .AddParameter("-vf fps=5")
                        .AddParameter("-pix_fmt yuv420p")
                        .AddParameter("-ss 00:00:00")
                        .AddParameter($"-t {duration}")
                        .AddParameter(videoPath)
                        .SetOverwriteOutput(true)
                        .Start().GetAwaiter().GetResult();

                    Console.WriteLine(conversionResult.Arguments);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                File.Delete(presentationItem.LocalSource);
            });

            for (var i = 0; i < presentationItems.Count; i++)
            {
                var videoPath = $"{i}.mp4";
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
            Thread.Sleep(4000);
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
            Directory.Delete(directory);
        }
    }
}