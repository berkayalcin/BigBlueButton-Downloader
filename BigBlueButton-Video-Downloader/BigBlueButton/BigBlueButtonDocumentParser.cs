using System;
using System.Linq;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Specifications;
using BigBlueButton_Video_Downloader.Webdriver;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public class BigBlueButtonDocumentParser
    {
        private readonly IWebDriver _webDriver;
        private readonly BigBlueButtonDocumentOptions _options;

        public BigBlueButtonDocumentParser(IWebDriver webDriver, BigBlueButtonDocumentOptions options)
        {
            _webDriver = webDriver;
            _options = options;
        }

        private IWebElement GetByCssSelector(string selector)
        {
            return _webDriver.ExactlyFindElement(By.CssSelector(selector), _options.TimeOutSeconds);
        }

        public string GetRecordingTitle()
        {
            var recordTitle = GetByCssSelector(_options.RecordingTitleSelector);

            if (recordTitle == null)
                throw new BigBlueButtonDocumentException("Record Title Cannot Be Found");

            return recordTitle.Text;
        }

        public string GetVideoSource()
        {
            var playButton = GetByCssSelector(_options.PlayButtonSelector);

            if (playButton == null)
                throw new BigBlueButtonDocumentException("Play Button Cannot Be Found");

            playButton.Click();

            var videoElement = GetByCssSelector(_options.VideoElementSelector);

            if (videoElement == null)
                throw new BigBlueButtonDocumentException("Video Cannot Be Found");

            var videoSources =
                videoElement.FindElements(By.TagName("source")).Select(t => t.GetProperty("src")).ToList();

            var specification = VideoFileExtensionSpecificationFactory.Create(_options.PreferredVideoType);

            var videoSource = videoSources.FirstOrDefault(src => specification.IsSatisfiedBy(src));

            if (string.IsNullOrEmpty(videoSource))
                throw new BigBlueButtonDocumentException(
                    $"Video Source Not Found For Preferred Type {Enum.GetName(typeof(VideoType), _options.PreferredVideoType)}");

            return videoSource;
        }
    }
}