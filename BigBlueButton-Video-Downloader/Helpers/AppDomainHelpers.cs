using System;

namespace BigBlueButton_Video_Downloader.Helpers
{
    public class AppDomainHelpers
    {
        public static string GetPath() => AppDomain.CurrentDomain.BaseDirectory;

        public static string GetPath(string fileName, string fileExtension) =>
            $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}{fileExtension}";
    }
}