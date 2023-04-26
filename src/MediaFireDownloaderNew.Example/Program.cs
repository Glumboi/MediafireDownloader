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