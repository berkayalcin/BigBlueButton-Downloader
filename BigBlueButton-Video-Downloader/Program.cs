using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.BigBlueButton;
using BigBlueButton_Video_Downloader.BigBlueButton.Interfaces;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.Webdriver;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace BigBlueButton_Video_Downloader
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await new BigBlueButtonFacade(
                    new FileDownloader(),
                    new VideoService(),
                    new PresentationService(new VideoService()))
                .SetUrl(
                    "https://bbb22.pau.edu.tr/playback/presentation/2.0/playback.html?meetingId=3d6ac0b859d6b392e65437f66c9a66fa759089c1-1602246232677")
                .EnableMultiThread()
                .SetDriverType(WebDriverType.Chrome)
                .EnableDownloadDeskshareVideo()
                .EnableDownloadWebcamVideo()
                .EnableDownloadPresentation()
                .SetOutputFileName("BigBlueButtonVideo")
                .SetOutputDirectory("/Users/berkay.yalcin/Desktop/deneme")
                .StartAsync();
        }
    }
}