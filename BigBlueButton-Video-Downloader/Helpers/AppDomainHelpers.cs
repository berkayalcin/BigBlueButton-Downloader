using System;
using System.IO;

namespace BigBlueButton_Video_Downloader.Helpers
{
    public class AppDomainHelpers
    {
        public static string GetPath() => AppDomain.CurrentDomain.BaseDirectory;

        public static string GetPath(string fileName, string fileExtension) =>
            $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}{fileExtension}";

        public static string GetPath(string directory, string fileName, string fileExtensions) =>
            Path.Combine(directory, $"{fileName}{fileExtensions}");
    }
}