using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.BigBlueButton;
using BigBlueButton_Video_Downloader.Webdriver;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace BigBlueButton_Video_Downloader
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full);

            var url = args[0];
            var fileName = args[1];
            using var driver = WebDriverFactory.Create(WebDriverType.Chrome);
            driver.Url = url;

            var downloader = new BigBlueButtonVideoDownloader(driver);
            await downloader.DownloadVideo(fileName);
        }
    }
}