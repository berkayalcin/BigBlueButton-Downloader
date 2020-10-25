using System;
using System.Threading.Tasks;
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