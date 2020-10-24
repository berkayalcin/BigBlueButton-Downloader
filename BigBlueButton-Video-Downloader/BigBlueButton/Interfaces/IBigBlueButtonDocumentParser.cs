using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton.Interfaces
{
    public interface IBigBlueButtonDocumentParser
    {
        IWebElement GetByCssSelector(string selector);
        string GetRecordingTitle();
        string GetVideoSource();
        void SetOptions(BigBlueButtonDocumentOptions options);
    }
}