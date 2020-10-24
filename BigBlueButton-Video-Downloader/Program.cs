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
                    "https://bbb22.pau.edu.tr/playback/presentation/2.0/playback.html?meetingId=12c8395af12fab1af1d10342665ffd9329c241d4-1603263277417")
                .EnableMultiThread()
                .SetDriverType(WebDriverType.Chrome)
                .EnableDownloadDeskshareVideo()
                .EnableDownloadWebcamVideo()
                .EnableDownloadPresentation()
                .SetOutputFileName("BigBlueButtonVideo")
                .StartAsync();
        }
    }
}