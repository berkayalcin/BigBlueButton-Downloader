using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Enums;

namespace BigBlueButton_Video_Downloader.Downloader
{
    public class FileDownloader : IFileDownloader
    {
        public  async Task DownloadVideoFileAsync(string videoUrl,
            string fileName,
            VideoType videoType,
            Action<object, DownloadProgressChangedEventArgs> progressChangedEventHandler = null,
            Action<object, AsyncCompletedEventArgs> asyncCompletedEventHandler = null)
        {
            if (videoUrl == null) throw new ArgumentNullException(nameof(videoUrl));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (!Enum.IsDefined(typeof(VideoType), videoType))
                throw new InvalidEnumArgumentException(nameof(videoType), (int) videoType, typeof(VideoType));

            var fileExtension = Enum.GetName(typeof(VideoType), videoType);
            if (string.IsNullOrEmpty(fileExtension))
                throw new ArgumentNullException(nameof(fileExtension));
            
            using var client = new WebClient();

            if (progressChangedEventHandler != null)
                client.DownloadProgressChanged += progressChangedEventHandler.Invoke;

            if (asyncCompletedEventHandler != null)
                client.DownloadFileCompleted += asyncCompletedEventHandler.Invoke;

            await client.DownloadFileTaskAsync(videoUrl, $"{fileName}.{fileExtension.ToLowerInvariant()}");
        }
    }
}