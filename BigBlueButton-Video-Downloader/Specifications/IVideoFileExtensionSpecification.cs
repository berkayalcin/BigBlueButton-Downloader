namespace BigBlueButton_Video_Downloader.Specifications
{
    public interface IVideoFileExtensionSpecification
    {
        bool IsSatisfiedBy(string url);
    }
}