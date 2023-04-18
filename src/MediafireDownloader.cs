﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using HtmlAgilityPack;

namespace Mediafire
{
    public class MediaFireDownloader : IMediaFireDownloader
    {
        #region Properties
        
        public static string DownloadingURL => _downloadingUrl;
        public static string DestinationPath => _destinationPath;

        #endregion Properties

        #region Private variables
        
        private static string _downloadingUrl;
        private static string _destinationPath;
        private static WebClient _client = new WebClient();
        private static readonly Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();
        
        #endregion
        
        public MediaFireDownloader(WebClient client)
        {
            if (client != null)
            {
                _client = client;
                return;
            }
            
            client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
            client.DownloadFileCompleted += ClientOnDownloadFileCompleted;
        }

        public virtual void ClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
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
            if (totalBytes.ToString(CultureInfo.InvariantCulture).Contains(","))
            {
                return totalBytes.ToString(CultureInfo.InvariantCulture).Split(',')[0];
            }

            return totalBytes.ToString(CultureInfo.InvariantCulture).Split('.')[0];
        }

        public void Download(string url, string destFilename, WebClient webClient = null)
        {
            AssignDownloadInfo(url, destFilename).DownloadFile(new Uri(ConvertMediaFireToDirectDownload(url)), destFilename);
        }

        public void DownloadAsync(string url, string destFilename, WebClient webClient = null, bool webClientDenugging = false)
        {
            AssignDownloadInfo(url, destFilename).DownloadFileAsync(new Uri(ConvertMediaFireToDirectDownload(url)), destFilename);
        }

        public string ConvertMediaFireToDirectDownload(string sourceURL, string startOfDownloadHref = "https://download")
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

        public List<string> GetHrefs(string url)
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

        private WebClient AssignDownloadInfo(string url, string destFilename)
        {
            _downloadingUrl = url;
            _destinationPath = destFilename;

            _stopWatch.Start();

            return _client;
        }
    }
}