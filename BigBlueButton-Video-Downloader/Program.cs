using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.BigBlueButton;
using BigBlueButton_Video_Downloader.BigBlueButton.Interfaces;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.UI;
using BigBlueButton_Video_Downloader.Webdriver;
using Colorful;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using static BigBlueButton_Video_Downloader.UI.ConsoleManager;

namespace BigBlueButton_Video_Downloader
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            ShowWelcome();
            // CommandLineManager.Parse(args);

            await new BigBlueButtonFacade(
                    new FileDownloader(),
                    new VideoService(),
                    new PresentationService(new VideoService()))
                .SetUrl(
                    "https://bbb16.pau.edu.tr/playback/presentation/2.0/playback.html?meetingId=12c8395af12fab1af1d10342665ffd9329c241d4-1602053283330")
                .EnableMultiThread()
                .SetDriverType(WebDriverType.Chrome)
                .EnableDownloadWebcamVideo()
                .EnableDownloadDeskshareVideo()
                .EnableDownloadPresentation()
                .SetOutputFileName("BigBlueButtonVideo_1")
                .SetOutputDirectory("/Users/berkay.yalcin/Desktop/deneme")
                .SetTimeoutSeconds(60)
                .StartAsync();
        }
    }
}