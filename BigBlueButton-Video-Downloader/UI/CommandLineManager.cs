using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.BigBlueButton;
using BigBlueButton_Video_Downloader.Downloader;
using BigBlueButton_Video_Downloader.Media;
using BigBlueButton_Video_Downloader.Models;
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
                            SingleDownload(opts);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Occured : {e.Message}");
                            return -1;
                        }

                        return 0;
                    },
                    (BatchDownloadOptions batch) =>
                    {
                        try
                        {
                            var batchItems = ParseBatchItems(batch.BatchListFilePath).ToList();
                            var items = batchItems.Select(i =>
                            {
                                var outputNameCount = batchItems.Count(t => t.OutputName.Equals(i.OutputName));
                                if (outputNameCount > 1)
                                {
                                    i.OutputName =
                                        $"{i.OutputName}_{outputNameCount - 1}";
                                }

                                return i;
                            }).ToList();
                            Parallel.ForEach(items, (batchItem, state, index) =>
                            {
                                SingleDownload(new SingleDownloadOptions()
                                {
                                    DownloadPresentation = batchItem.DownloadPresentation,
                                    DownloadUrl = batchItem.Url,
                                    DownloadWebcam = batchItem.DownloadWebcam,
                                    DownloadDeskShare = batchItem.DownloadDeskShare,
                                    UseMultiThread = batch.UseMultiThread,
                                    OutputDirectory = batch.OutputDirectory,
                                    OutputFile = batchItem.OutputName
                                });
                            });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Occured : {e.Message}");
                            return -1;
                        }

                        return 0;
                    },
                    errs => 1
                );
        }

        private static void SingleDownload(SingleDownloadOptions opts)
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

        public static IEnumerable<BatchItem> ParseBatchItems(string path)
        {
            using var streamReader = new StreamReader(path);
            var line = "";
            while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
            {
                var items = line.Split("|");
                var url = items.ElementAt(0);
                var outputName = items.ElementAt(1);
                var downloadDeskshare = bool.Parse(items.ElementAtOrDefault(2) ?? "true");
                var downloadPresentation = bool.Parse(items.ElementAtOrDefault(3) ?? "true");
                var downloadWebcam = bool.Parse(items.ElementAtOrDefault(4) ?? "true");
                yield return new BatchItem()
                {
                    Url = url,
                    OutputName = outputName,
                    DownloadDeskShare = downloadDeskshare,
                    DownloadPresentation = downloadPresentation,
                    DownloadWebcam = downloadWebcam
                };
            }
        }
    }
}