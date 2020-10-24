using System;

namespace BigBlueButton_Video_Downloader.Extensions
{
    public static class StringExtensions
    {
        public static Uri ConvertToUri(this string url)
        {
            var uri = new Uri(url);
            return uri;
        }
    }
}