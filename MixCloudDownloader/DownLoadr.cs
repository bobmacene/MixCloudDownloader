using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;

namespace MixCloudDownloader
{
    internal class DownLoadr
    {
        private const string ChromeDriver = "C:\\Users\\bob\\Documents\\visual studio 2015\\Projects\\MixCloudDownloader\\MixCloudDownloader\\bin\\Debug\\chromedriver.exe";
        private const string StreamSavePath = @"C:\Users\bob\Music\Hot Since 82 - Summer Hotcast 2013.m4a";
        private const string RegexPattern = "(https://s.*.m4a)";

        private static void Main(string[] args)
        {
            const string mixCloudUrl = @"https://beta.mixcloud.com/Hot_Since_82/hot-since-82-summer-hotcast-13/";

            var downLoadr = new DownLoadr();
            var pageSource = downLoadr.GetPageSource(ChromeDriver, mixCloudUrl);

            var streamUrl = downLoadr.GetStreamUrl(pageSource, RegexPattern);

            downLoadr.LoadWebStream(streamUrl, StreamSavePath);
         }

        public void LoadWebStream(string streamUrl, string loadPath)
        {
            var webReq = WebRequest.Create(streamUrl);

            using (var webResponse = webReq.GetResponse().GetResponseStream())
            { 
                var memory = new MemoryStream();

                webResponse?.CopyTo(memory);

                File.WriteAllBytes(loadPath, memory.ToArray());
            }
        }

        public string  GetPageSource(string chromeDriverPath, string mixCloudUrl)
        {
            Environment.SetEnvironmentVariable("webdriver.chrome.driver", chromeDriverPath);

            var driver = new ChromeDriver();

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

            driver.Navigate().GoToUrl(mixCloudUrl);
             
            return  driver.PageSource;
        }

        public string GetStreamUrl(string html, string pattern)
        {
            var regex = new Regex(pattern);

            var match = regex.Match(html);

            return match.Groups[1].Value;
        }

    }
}
