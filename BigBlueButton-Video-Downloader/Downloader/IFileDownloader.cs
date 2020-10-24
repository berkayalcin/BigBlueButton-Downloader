using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Enums;

namespace BigBlueButton_Video_Downloader.Downloader
{
    public interface IFileDownloader
    {
        Task DownloadVideoFileAsync(string videoUrl,
            string fileName,
            VideoType videoType,
            Action<object, DownloadProgressChangedEventArgs> progressChangedEventHandler = null,
            Action<object, AsyncCompletedEventArgs> asyncCompletedEventHandler = null);
    }
}