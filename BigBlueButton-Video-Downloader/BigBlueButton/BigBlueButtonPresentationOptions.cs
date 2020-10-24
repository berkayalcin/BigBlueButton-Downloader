namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public sealed class BigBlueButtonPresentationOptions
    {
        private readonly string _presentationElementSelector;

        public BigBlueButtonPresentationOptions(string presentationElementSelector)
        {
            _presentationElementSelector = presentationElementSelector;
        }

        public string PresentationElementSelector => _presentationElementSelector;
    }
}