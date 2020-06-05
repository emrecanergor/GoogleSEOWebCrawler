using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSEOCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerasync();
            Console.ReadLine();

        }

        private static async Task startCrawlerasync()
        {
            try
            {
                List<string> urls = new List<string>
                {
                    "https://sutsuz.com/sutsuz-pankek-tarifi/",
                    "https://sutsuz.com/sutsuz-brownie-kek-tarifi/",
                    "https://sutsuz.com/sutsuz-limonlu-kek-tarifi/"
                };

                var httpClient = new HttpClient();
                foreach (var url in urls)
                {
                    if (!string.IsNullOrEmpty(url))
                    {

                        var html = await httpClient.GetStringAsync(url);
                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(html);

                        string recipeName = "defaultName";
                        string fileTexts = url + "\n\n";
                        var nodes = htmlDocument.DocumentNode?.SelectNodes("//meta");
                        foreach (HtmlNode node in nodes ?? Enumerable.Empty<HtmlNode>())
                        {
                            var nodeValue = node.Attributes["itemprop"]?.Value;
                            if (!string.IsNullOrEmpty(nodeValue))
                            {
                                var nodeData = node.Attributes["content"]?.Value;
                                if (!string.IsNullOrEmpty(nodeData))
                                {
                                    //link
                                    //file

                                    if ("mainEntityOfPage" == nodeValue)
                                    {
                                        recipeName = nodeData.Replace(" ", "_");
                                    }

                                    fileTexts += nodeValue + ":\t" + nodeData + "\n";

                                }
                            }
                        }

                        if (nodes != null)
                        {
                            WriteToFileF(fileTexts, recipeName);
                        }

                    }
                }

                Console.WriteLine("Successful....");
                Console.WriteLine("Press Enter to exit the program...");
                ConsoleKeyInfo keyinfor = Console.ReadKey(true);
                if (keyinfor.Key == ConsoleKey.Enter)
                {
                    System.Environment.Exit(0);
                }

            }
            catch (Exception e)
            {

            }
        }

        public static void WriteToFileF(string message, string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Recipes";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filepath = path + "\\" + DateTime.Now.ToShortDateString() + "_" + fileName + ".txt";

            //log.SetEventThings(id: 1035, type: "Error");
            if (!File.Exists(filepath))
            {
                //log.SetEventThings(id: 1036, type: "Error");
                using (StreamWriter sw = File.CreateText(filepath))
                    sw.WriteLine(message);
            }
            else
            {
                //log.SetEventThings(id: 1037, type: "Error");
                using (StreamWriter sw = File.AppendText(filepath))
                    sw.WriteLine(message);
            }
        }
    }
}
