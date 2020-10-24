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
            CommandLineManager.Parse(args);
        }
    }
}