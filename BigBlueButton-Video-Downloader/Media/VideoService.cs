using System;
using System.IO;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Helpers;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using Xabe.FFmpeg.Events;

namespace BigBlueButton_Video_Downloader.Media
{
    public class VideoService : IVideoService
    {
        public VideoService()
        {
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full).GetAwaiter().GetResult();
        }

        public async Task ExtractAndAddAudio(string audioInputPath, string audioOutputPath,
            string inputPath, string outputPath, string tempDirectory, bool isMultiThread = false)
        {
            Console.WriteLine("Extracting Audio");
            if (!File.Exists(audioOutputPath))
                await ExtractAudio(audioInputPath,
                    audioOutputPath,
                    (o, args) => { },
                    isMultiThread
                );

            var audio = await FFmpeg.GetMediaInfo(audioOutputPath);
            var inputVideo = await FFmpeg.GetMediaInfo(inputPath);

            if (TimeSpan.Compare(audio.Duration, inputVideo.Duration) == -1)
            {
                // Split Video

                Console.WriteLine("Splitting Video");
                var tempOutput = AppDomainHelpers
                    .GetPath(tempDirectory,
                        $"temp_split_video_output_{Guid.NewGuid().ToString().Replace("-", "")}",
                        ".mp4");

                var split = await FFmpeg.Conversions.FromSnippet.Split(inputPath, tempOutput, TimeSpan.Zero,
                    audio.Duration);
                split.UseMultiThread(isMultiThread);
                await split.Start();

                File.Move(tempOutput, inputPath, true);
            }

            Console.WriteLine("Adding Audio");
            await AddAudio(inputPath,
                audioOutputPath,
                outputPath,
                (o, args) => { },
                isMultiThread
            );
        }

        public async Task ExtractAudio(string inputPath, string outputPath,
            Action<object, ConversionProgressEventArgs> onProgress = null,
            bool useMultiThread = false)
        {
            var extractAudio = await FFmpeg.Conversions.FromSnippet.ExtractAudio(inputPath, outputPath);
            extractAudio.UseMultiThread(useMultiThread);
            if (onProgress != null)
                extractAudio.OnProgress += onProgress.Invoke;
            extractAudio.SetOverwriteOutput(true);
            await extractAudio.Start();
        }

        public async Task AddAudio(string inputPath, string audioPath, string outputPath,
            Action<object, ConversionProgressEventArgs> onProgress = null,
            bool useMultiThread = false)
        {
            var addAudio =
                await FFmpeg.Conversions.FromSnippet.AddAudio(inputPath, audioPath, outputPath);
            addAudio.UseMultiThread(useMultiThread);
            if (onProgress != null)
                addAudio.OnProgress += onProgress.Invoke;
            addAudio.SetOverwriteOutput(true);
            await addAudio.Start();
        }
    }
}