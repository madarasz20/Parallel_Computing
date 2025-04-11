
using System.Net;
using System.Text.RegularExpressions;

namespace N_URL_downloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = @"http://users.nik.uni-obuda.hu/prog4/CompetitionPics/2017/";
            string html = new WebClient().DownloadString(url);

            var mathches = Regex.Matches(html, "<a href=\"([^\"]+)\">");

            List<string> urls = mathches.Cast<Match>()
                .Select(m =>
                {
                    string raw = m.ToString();
                    return raw.Substring(9, raw.Length - 11);
                })
                .Where(s => s.EndsWith(".jpg"))
                .Select(s => url + s)
                .ToList();

            Console.WriteLine($"{ urls.Count()} db lesz letöltve" );

            if(!Directory.Exists("Letöltések"))
            {
                Directory.CreateDirectory("Letöltések");
            }

            //legfeljebb 3 letöltés mehet egyszerre
            SemaphoreSlim semaphore = new SemaphoreSlim(3);

            List<Task> ts = urls
                .Select(u => new Task(() => 
                {
                    string fn = u.Replace(url, "");
                    Console.WriteLine("Ready: " + fn);
                    semaphore.Wait();
                    try
                    {
                        Console.WriteLine("Downloading: " + fn);
                        new WebClient().DownloadFile(u, "Letöltések/" + fn);
                        Console.WriteLine("Done" + fn);
                    }
                finally
                    {
                        semaphore.Release();
                    }
                }, TaskCreationOptions.LongRunning)).ToList();

            Task.WhenAll(ts).ContinueWith(x =>
            {
                Console.WriteLine($"{urls.Count()} db fájl letöltve");
            });

            ts.ForEach(t => t.Start());

            Console.ReadLine();

                
        }
    }
}