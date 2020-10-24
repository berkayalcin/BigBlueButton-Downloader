using System.IO;
using System.Runtime.CompilerServices;
using BigBlueButton_Video_Downloader.Helpers;

namespace BigBlueButton_Video_Downloader.Extensions
{
    public static class FileExtensions
    {
        public static void DeleteIfExists(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void DeleteFileInAppDirectory(string fileName, string fileExtension)
        {
            DeleteIfExists(AppDomainHelpers.GetPath(fileName, fileExtension));
        }
    }
}