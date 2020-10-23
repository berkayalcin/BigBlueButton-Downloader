using BigBlueButton_Video_Downloader.Enums;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public sealed class BigBlueButtonDocumentOptions
    {
        private readonly string _recordingTitleSelector = "#recording-title";
        private readonly string _playButtonSelector = ".acorn-play-button";
        private readonly VideoType _preferredVideoType;
        private readonly string _videoElementSelector;
        private readonly int _timeOutSeconds;

        public BigBlueButtonDocumentOptions(string videoElementSelector, int timeOutSeconds,
            VideoType preferredVideoType = VideoType.Mp4)
        {
            _videoElementSelector = videoElementSelector;
            _timeOutSeconds = timeOutSeconds;
            _preferredVideoType = preferredVideoType;
        }

        public string VideoElementSelector => _videoElementSelector;

        public VideoType PreferredVideoType => _preferredVideoType;

        public string PlayButtonSelector => _playButtonSelector;

        public string RecordingTitleSelector => _recordingTitleSelector;

        public int TimeOutSeconds => _timeOutSeconds;
    }
}