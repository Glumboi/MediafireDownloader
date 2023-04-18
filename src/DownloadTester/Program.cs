// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Net;
using Mediafire;

using (MediaFireDownloader downloader = new MediaFireDownloader())
{
    downloader.DownloadAsync("https://www.mediafire.com/file/9nw5h0335t4gcse/Pokemon_Scarlet_-_8254.zip/", "./DownloadedFile.zip");
}
Console.ReadKey();


