using System;
using System.Linq;
using BigBlueButton_Video_Downloader.Attributes;
using BigBlueButton_Video_Downloader.BigBlueButton.Interfaces;
using BigBlueButton_Video_Downloader.Constants;
using BigBlueButton_Video_Downloader.Enums;
using BigBlueButton_Video_Downloader.Exceptions;
using BigBlueButton_Video_Downloader.Extensions;
using BigBlueButton_Video_Downloader.Mappers;
using BigBlueButton_Video_Downloader.Models;
using BigBlueButton_Video_Downloader.Specifications;
using BigBlueButton_Video_Downloader.Webdriver;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public class BigBlueButtonDocumentParser : IBigBlueButtonDocumentParser
    {
        private readonly IWebDriver _webDriver;
        private BigBlueButtonDocumentOptions _options;

        public BigBlueButtonDocumentParser(IWebDriver webDriver, BigBlueButtonDocumentOptions options)
        {
            _webDriver = webDriver;
            _options = options;
        }


        public IWebElement GetByCssSelector(string selector)
        {
            return _webDriver.GetByCssSelector(selector, _options.TimeOutSeconds);
        }

        public RecordingTitleModel GetRecordingTitle()
        {
            var recordTitle = GetByCssSelector(_options.RecordingTitleSelector);

            if (recordTitle == null)
                return new RecordingTitleModel()
                {
                    ErrorCode = ExceptionCodeConstants.NoRecordingTitleFound
                };

            return new RecordingTitleModel()
            {
                Title = recordTitle.Text
            };
        }

        public VideoSourceModel GetDeskShareVideoSource()
        {
            return GetVideoSource(_options.DeskShareVideoOptions.ElementSelector,
                _options.DeskShareVideoOptions.VideoType);
        }

        public VideoSourceModel GetWebcamVideoSource()
        {
            return GetVideoSource(_options.WebcamVideoOptions.ElementSelector, _options.WebcamVideoOptions.VideoType);
        }

        public PresentationModel GetPresentation()
        {
            var playButton = GetByCssSelector(_options.PlayButtonSelector);

            if (playButton == null)
                return new PresentationModel()
                {
                    ErrorCode = ExceptionCodeConstants.NoPlayButtonFound
                };

            playButton.Click();

            var presentationElement = GetByCssSelector(BigBlueButtonConstants.PresentationSelector);
            if (presentationElement == null)
            {
                return new PresentationModel()
                {
                    ErrorCode = ExceptionCodeConstants.NoPresentationFound
                };
            }

            var uri = _options.DocumentUrl.ConvertToUri();

            var baseUrl = uri.Scheme + "://" + uri.Host;
            var presentationItems = presentationElement
                .ConvertToPresentationItems()
                .Where(i => i.In != 0 || i.Out != 0)
                .Select(i =>
                {
                    var source = i.Source;
                    i.Source = $"{baseUrl}{source}";
                    return i;
                })
                .OrderBy(t => t.In)
                .ToList();


            return new PresentationModel()
            {
                Items = presentationItems,
            };
        }


        public VideoSourceModel GetVideoSource(string selector, VideoType videoType)
        {
            var playButton = GetByCssSelector(_options.PlayButtonSelector);

            if (playButton == null)
                return new VideoSourceModel()
                {
                    ErrorCode = ExceptionCodeConstants.NoPlayButtonFound
                };

            playButton.Click();

            var videoElement = GetByCssSelector(selector);

            if (videoElement == null)
                return new VideoSourceModel()
                {
                    ErrorCode = ExceptionCodeConstants.NoVideoSourceFound,
                };

            var videoSources =
                videoElement.FindElements(By.TagName("source")).Select(t => t.GetProperty("src")).ToList();

            var specification = VideoFileExtensionSpecificationFactory.Create(videoType);

            var videoSource = videoSources.FirstOrDefault(src => specification.IsSatisfiedBy(src));

            if (string.IsNullOrEmpty(videoSource))
                throw new BigBlueButtonDocumentException(
                    $"Video Source Not Found For Preferred Type {Enum.GetName(typeof(VideoType), videoType)}");

            return new VideoSourceModel()
            {
                SourceUrl = videoSource
            };
        }
    }
}