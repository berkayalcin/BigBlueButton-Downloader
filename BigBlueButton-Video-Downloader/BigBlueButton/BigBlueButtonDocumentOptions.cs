namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public sealed class BigBlueButtonDocumentOptions
    {
        private readonly string _recordingTitleSelector = "#recording-title";
        private readonly string _playButtonSelector = ".acorn-play-button";
        private readonly string _outputDirectory;
        private readonly int _timeOutSeconds;
        private readonly BigBlueButtonDocumentDeskShareVideoOptions _deskShareVideoOptions;
        private readonly BigBlueButtonDocumentWebcamVideoOptions _webcamVideoOptions;
        private readonly BigBlueButtonPresentationOptions _presentationOptions;

        private readonly string _documentUrl;


        public BigBlueButtonDocumentOptions(int timeOutSeconds,
            string documentUrl,
            string outputDirectory,
            BigBlueButtonDocumentWebcamVideoOptions webcamVideoOptions,
            BigBlueButtonDocumentDeskShareVideoOptions deskShareVideoOptions,
            BigBlueButtonPresentationOptions presentationOptions)
        {
            _timeOutSeconds = timeOutSeconds;
            _webcamVideoOptions = webcamVideoOptions;
            _deskShareVideoOptions = deskShareVideoOptions;
            _presentationOptions = presentationOptions;
            _documentUrl = documentUrl;
            _outputDirectory = outputDirectory;
        }


        public string PlayButtonSelector => _playButtonSelector;

        public string RecordingTitleSelector => _recordingTitleSelector;

        public int TimeOutSeconds => _timeOutSeconds;

        public BigBlueButtonDocumentWebcamVideoOptions WebcamVideoOptions => _webcamVideoOptions;

        public BigBlueButtonDocumentDeskShareVideoOptions DeskShareVideoOptions => _deskShareVideoOptions;

        public BigBlueButtonPresentationOptions PresentationOptions => _presentationOptions;

        public string DocumentUrl => _documentUrl;
    }
}