namespace BigBlueButton_Video_Downloader.Specifications
{
    public class WebmVideoFileExtensionSpecification : IVideoFileExtensionSpecification
    {
        public bool IsSatisfiedBy(string url)
        {
            return url.Contains(".webm");
        }
    }
}