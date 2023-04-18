using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Mediafire
{
    public class MediafireDownloader_OLD
    {
        #region Properties
        
        public static string DownloadingURL => _downloadingUrl;
        public static string DestinationPath => _destinationPath;
        
        #endregion Properties

        #region Private variables
        
        private static string _downloadingUrl;
        private static string _destinationPath;
        private static WebClient client = new WebClient();
        private static readonly Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();
        
        #endregion


        /// <summary>
        /// Converts a given Mediafire url to a direct download, can be used with a webclient to save the file.
        /// </summary>
        /// <param name="sourceURL">Mediafire url</param>
        /// <param name="startOfDownloadHref">Beginning of the hrefs url, usually its: https://download</param>
        /// <returns></returns>
        public static string GetMediafireDDL(string sourceURL, string startOfDownloadHref = "https://download")
        {
            foreach (string item in GetHrefs(sourceURL))
            {
                if (item.Contains(startOfDownloadHref))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all hrefs in a website
        /// </summary>
        /// <param name="url">Source url</param>
        /// <returns></returns>
        private static List<string> GetHrefs(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = web.Load(url);
            List<string> resultList = new List<string>();

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];

                if (att.Value.Contains("a"))
                {
                    resultList.Add(att.Value);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Downloads a Mediafire link directly to the destPath using a webclient
        /// </summary>
        /// <param name="url">Mediafire url</param>
        /// <param name="destFilename">destination file name</param>
        /// <param name="webClient">put your custom webclient here, leave null to use default webclient</param>
        /// <param name="webClientDenugging">determines if the webclient should do Console.Write/Line on progress changed and download completed</param>
        public static void DownloadMediafireFileAsync(string url, string destFilename, WebClient webClient = null, bool webClientDenugging = false)
        {
            AssignDownloadInfo(url, destFilename, webClient, webClientDenugging).DownloadFileAsync(new Uri(GetMediafireDDL(url)), destFilename);
        }        
        
        public static void DownloadMediafireFile(string url, string destFilename, WebClient webClient = null)
        {
            AssignDownloadInfo(url, destFilename, webClient).DownloadFile(new Uri(GetMediafireDDL(url)), destFilename);
        }

        /// <summary>
        /// Assigns url, destPath and webClient events to the download and returns a new webclient
        /// </summary>
        /// <param name="url">url to download from</param>
        /// <param name="destFilename"><destination file name/param>
        /// <param name="webClient">default/custom webclient</param>
        /// <param name="webClientDenugging">debugging for webclient</param>
        private static WebClient AssignDownloadInfo(string url, string destFilename, WebClient webClient, bool webClientDenugging = false)
        {
            _downloadingUrl = url;
            _destinationPath = destFilename;
            if (webClient == null)
            {
                webClient = client;
            }

            if (webClientDenugging)
            {
                webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
            }
            
            _stopWatch.Start();
            return webClient;
        }
        
        private static void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _stopWatch.Reset();
            Console.WriteLine($"\n{_downloadingUrl} finished downloading and got saved at: {_destinationPath}");
        }

        private static void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            int percentageString = int.Parse(Math.Truncate(percentage).ToString());

            // Calculate progress values
            double fileSize = totalBytes / 1024.0 / 1024.0;
            double downloadSpeed = e.BytesReceived / 1024.0 / 1024.0 / _stopWatch.Elapsed.TotalSeconds;

            //Calculate ETA
            double remainingBytes = totalBytes - bytesIn;
            double remainingTime = remainingBytes / (downloadSpeed * 1024 * 1024);
            string remainingTimeString = TimeSpan.FromSeconds(remainingTime).ToString(@"hh\:mm\:ss");

            string progressBarText = string.Format("{0}% Done | {1} MB/s | {2}: {3} MB | ETA: {4}",
                e.ProgressPercentage,
                downloadSpeed.ToString("0.00"),
                "Filesize",
                GetFileSizeWithoutComma(fileSize),
                remainingTimeString);

            Console.Write($"\r{progressBarText}");
        }
        
        /// <summary>
        /// Used to ensure that the filesize is displayed without a . or , in any language
        /// </summary>
        /// <param name="totalBytes"></param>
        /// <returns></returns>
        private static string GetFileSizeWithoutComma(double totalBytes)
        {
            if (totalBytes.ToString().Contains(","))
            {
                return totalBytes.ToString().Split(',')[0];
            }

            return totalBytes.ToString().Split('.')[0];
        }
    }
}