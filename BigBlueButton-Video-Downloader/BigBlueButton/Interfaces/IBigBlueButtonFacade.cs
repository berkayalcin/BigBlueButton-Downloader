using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Webdriver;

namespace BigBlueButton_Video_Downloader.BigBlueButton.Interfaces
{
    public interface IBigBlueButtonFacade
    {
        IBigBlueButtonFacade SetUrl(string url);
        IBigBlueButtonFacade SetDriverType(WebDriverType driverType);
        IBigBlueButtonFacade EnableDownloadDeskshareVideo();
        IBigBlueButtonFacade DisableDownloadDeskshareVideo();
        IBigBlueButtonFacade EnableDownloadWebcamVideo();
        IBigBlueButtonFacade DisableDownloadWebcamVideo();
        IBigBlueButtonFacade EnableMultiThread();
        IBigBlueButtonFacade DisableMultiThread();
        IBigBlueButtonFacade SetOutputFileName(string fileName);
        IBigBlueButtonFacade SetVideoType(VideoType videoType);
        IBigBlueButtonFacade SetDeskshareOptions(BigBlueButtonDocumentOptions deskShareOptions);
        IBigBlueButtonFacade SetWebcamOptions(BigBlueButtonDocumentOptions webcamOptions);
        Task StartAsync();
        void RemoveTempFiles(string webcamVideoName, string audioOutputName, string deskShareVideoName);
    }
}