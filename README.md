<div align="center">
<img src="https://i.imgur.com/S806vLi.png" width="312" height="156" />
<div align="left">

# MediafireDownloader
A small dll that lets you download files from mediafire, it uses HtmlAgilityPack to retrieve the ddl link.

<!-- Place this tag where you want the button to render. -->
<a class="github-button" href="https://github.com/Glumboi/MediafireDownloader/packages" data-color-scheme="no-preference: dark; light: dark; dark: dark;" data-icon="octicon-package" aria-label="Install this package Glumboi/MediafireDownloader on GitHub">Install this package</a>

# Code Examples:
___
## Simple console app using this dll
```c#
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConsoleExt.WriteLineColored(
                "Welcome to EZMediafireDownloaderCLI, " +
                "please enter the link you want to download from below: ",
                ConsoleColor.Magenta);

            string downloadLink = Console.ReadLine();
            
            ConsoleExt.WriteLineColored(
                "\nEnter the path + name + file ending of the file now please (example C:\\Folder\\File.zip): ", 
                ConsoleColor.Magenta);

            string destination = Console.ReadLine();
            
            ConsoleExt.WriteLineColored("\nDownloading, please wait...", ConsoleColor.Magenta);
            
            Mediafire.MediafireDownloader.DownloadMediafireFileAsync(
                downloadLink, destination,
                null, true);
            
            Console.ReadKey();
        }
    }

    internal class ConsoleExt
    {
        public static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    } 

```
