using System.Collections.Generic;
using System.Net;

namespace Mediafire
{
    public interface IMediaFireDownloader
    {
        /// <summary>
        /// Downloads a File from MediaFire using the provided Link
        /// </summary>
        /// <param name="url"></param>
        void Download(string url, string destFilename, WebClient webClient = null);
        void DownloadAsync(string url, string destFilename, WebClient webClient = null, bool webClientDenugging = false);

        /// <summary>
        /// Returns the direct download for the specified MediaFire Link
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Direct download Link</returns>
        string ConvertMediaFireToDirectDownload(string sourceURL, string startOfDownloadHref = "https://download");
        
        /// <summary>
        /// Returns all Hrefs from the given MediaFire URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        List<string> GetHrefs(string url);
    }
}