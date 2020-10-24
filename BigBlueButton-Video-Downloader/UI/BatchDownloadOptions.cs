using CommandLine;

namespace BigBlueButton_Video_Downloader.UI
{
    [Verb("batch-download", HelpText = "Batch Download")]
    public class BatchDownloadOptions
    {
        [Option('m', "useMultiThread", Required = false, HelpText = "Optional - Enables Multi-Thread Processing.")]
        public bool UseMultiThread { get; set; }
        
        [Option('f', "batchFile", Required = true, HelpText = "Batch File Path")]
        public string BatchListFilePath { get; set; }
        
        [Option('d', "outputDirectory", Required = true, HelpText = "Output Directory Path")]
        public string OutputDirectory { get; set; }
    }
}