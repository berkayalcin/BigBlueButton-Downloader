using System;
using System.Threading.Tasks;
using Xabe.FFmpeg.Events;

namespace BigBlueButton_Video_Downloader.Media
{
    public interface IVideoService
    {
        Task ExtractAudio(string inputPath, string outputPath,
            Action<object, ConversionProgressEventArgs> onProgress = null,
            bool useMultiThread = false);

        Task AddAudio(string inputPath, string audioPath, string outputPath,
            Action<object, ConversionProgressEventArgs> onProgress = null,
            bool useMultiThread = false);

        Task ExtractAndAddAudio(string audioInputPath, string audioOutputPath,
            string inputPath, string outputPath, string tempDirectory, bool isMultiThread = false);
    }
}