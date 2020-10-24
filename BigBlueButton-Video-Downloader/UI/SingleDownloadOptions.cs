using CommandLine;

namespace BigBlueButton_Video_Downloader.UI
{
    [Verb("download", HelpText = "Single Download")]
    public class SingleDownloadOptions
    {
        [Option('m', "useMultiThread", Required = false, HelpText = "Optional - Enables Multi-Thread Processing.")]
        public bool UseMultiThread { get; set; }

        [Option('u', "url", Required = true, HelpText = "Download Url.")]
        public string DownloadUrl { get; set; }

        [Option('o', "outputFile", Required = true, HelpText = "Output File Name")]
        public string OutputFile { get; set; }

        [Option('d', "outputDirectory", Required = true, HelpText = "Output Directory Path")]
        public string OutputDirectory { get; set; }

        [Option('w', "downloadWebcam", Required = true, HelpText = "Sets Whether Download Webcam Video.")]
        public bool DownloadWebcam { get; set; }


        [Option('v', "downloadDeskShare", Required = true, HelpText = "Sets Whether Download Desk-Share Video.")]
        public bool DownloadDeskShare { get; set; }

        [Option('p', "downloadPresentation", Required = true, HelpText = "Sets Whether Download Presentation Video.")]
        public bool DownloadPresentation { get; set; }
    }
}