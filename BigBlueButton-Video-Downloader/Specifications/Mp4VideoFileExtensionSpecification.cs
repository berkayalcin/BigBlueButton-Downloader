namespace BigBlueButton_Video_Downloader.Specifications
{
    public class Mp4VideoFileExtensionSpecification : IVideoFileExtensionSpecification
    {
        public bool IsSatisfiedBy(string url)
        {
            return url.Contains(".mp4");
        }
    }
}