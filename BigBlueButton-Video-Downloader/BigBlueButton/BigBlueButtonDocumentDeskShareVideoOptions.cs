using BigBlueButton_Video_Downloader.Enums;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public sealed class BigBlueButtonDocumentDeskShareVideoOptions
    {
        private readonly VideoType _preferredVideoType;
        private readonly string _videoElementSelector;

        public BigBlueButtonDocumentDeskShareVideoOptions(VideoType preferredVideoType, string videoElementSelector)
        {
            _preferredVideoType = preferredVideoType;
            _videoElementSelector = videoElementSelector;
        }

        public VideoType VideoType => _preferredVideoType;

        public string ElementSelector => _videoElementSelector;
    }
}