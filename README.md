<div align="center">
<img src="https://i.imgur.com/S806vLi.png" width="312" height="156" />

[Official NuGet package](https://www.nuget.org/packages/MediafireDownloader/)
<div align="left">


# MediafireDownloader
A small dll that lets you download files from mediafire, it uses HtmlAgilityPack to retrieve the ddl link.

# Code Examples:
___
## Simple console app using this dll
```c#
using MediaFireDownloaderNew;

Console.WriteLine("Please enter a URL: ");

string inputURL = Console.ReadLine();  


using (MediaFireDownloaderNew.MediaFireDownloader downloader = new MediaFireDownloader())
{
    Console.WriteLine("Checking if link is valid...");

    string ddlURL = downloader.ConvertMediaFireToDirectDownload(inputURL);

    Console.WriteLine("Mediafire link converted to DDL: " + ddlURL);
    Console.WriteLine("Starting download, please select an output path: ");

    string outPath = Console.ReadLine().Replace('\"', ' ');

    Console.WriteLine("Now please enter a file name with file ending: ");
    
    string fileName = Console.ReadLine();
    string outFile = Path.Combine(outPath, fileName);
    downloader.DownloadAsync(inputURL, outFile);
}  

Console.ReadKey();
```

