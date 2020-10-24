using System;
using BigBlueButton_Video_Downloader.BigBlueButton;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.Webdriver;
using Colorful;
using CommandLine;
using Console = System.Console;

namespace BigBlueButton_Video_Downloader.UI
{
    public class CommandLineManager
    {
        public static void Parse(string[] args)
        {
            Parser.Default.ParseArguments<SingleDownloadOptions, BatchDownloadOptions>(args)
                .MapResult(
                    (SingleDownloadOptions opts) =>
                    {
                        try
                        {
                            var bigBlueButtonFacade = new BigBlueButtonFacade(
                                    new FileDownloader(),
                                    new VideoService(),
                                    new PresentationService(
                                        new VideoService()
                                    )
                                )
                                .SetUrl(opts.DownloadUrl)
                                .SetDriverType(WebDriverType.Chrome)
                                .SetOutputFileName(opts.OutputFile)
                                .SetOutputDirectory(opts.OutputDirectory);

                            if (opts.UseMultiThread)
                            {
                                bigBlueButtonFacade.EnableMultiThread();
                            }
                            else
                            {
                                bigBlueButtonFacade.DisableMultiThread();
                            }

                            if (opts.DownloadPresentation)
                            {
                                bigBlueButtonFacade.EnableDownloadPresentation();
                            }
                            else
                            {
                                bigBlueButtonFacade.DisableDownloadPresentation();
                            }

                            if (opts.DownloadWebcam)
                            {
                                bigBlueButtonFacade.EnableDownloadWebcamVideo();
                            }
                            else
                            {
                                bigBlueButtonFacade.DisableDownloadWebcamVideo();
                            }

                            if (opts.DownloadDeskShare)
                            {
                                bigBlueButtonFacade.EnableDownloadDeskshareVideo();
                            }
                            else
                            {
                                bigBlueButtonFacade.DisableDownloadDeskshareVideo();
                            }


                            bigBlueButtonFacade
                                .StartAsync().GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Occured : {e.Message}");
                            return -1;
                        }

                        return 0;
                    },
                    (BatchDownloadOptions batch) => { return 0; },
                    errs => 1
                );
        }
    }

    [Verb("batch-download", HelpText = "Batch Download")]
    public class BatchDownloadOptions
    {
    }

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