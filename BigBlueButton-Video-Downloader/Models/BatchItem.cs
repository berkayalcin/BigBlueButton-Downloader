namespace BigBlueButton_Video_Downloader.Models
{
    public class BatchItem
    {
        public string Url { get; set; }
        public string OutputName { get; set; }
        public bool DownloadWebcam { get; set; }
        public bool DownloadDeskShare { get; set; }
        public bool DownloadPresentation { get; set; }
        
    }
}