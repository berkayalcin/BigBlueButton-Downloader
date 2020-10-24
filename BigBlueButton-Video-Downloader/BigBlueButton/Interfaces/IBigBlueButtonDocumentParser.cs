using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Models;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton.Interfaces
{
    public interface IBigBlueButtonDocumentParser
    {
        IWebElement GetByCssSelector(string selector);
        RecordingTitleModel GetRecordingTitle();
        VideoSourceModel GetVideoSource(string selector, VideoType videoType);
        VideoSourceModel GetDeskShareVideoSource();
        VideoSourceModel GetWebcamVideoSource();
        PresentationModel GetPresentation();
    }
}