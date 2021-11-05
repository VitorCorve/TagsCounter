using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Http;

namespace TagsCounter.Models.Services
{
    public class Parser
    {
        public static int PasreItemsCount(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                using (HttpClientHandler cliendHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    AutomaticDecompression = DecompressionMethods.Deflate |
                    DecompressionMethods.GZip |
                    DecompressionMethods.None
                })
                {
                    using (HttpClient client = new HttpClient(cliendHandler))
                    {
                        using (HttpResponseMessage response = client.GetAsync(url).Result)
                            if (response.IsSuccessStatusCode)
                            {
                                var html = response.Content.ReadAsStringAsync().Result;

                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlDocument document = new HtmlDocument();
                                    document.LoadHtml(html);

                                    return document.DocumentNode.SelectNodes("//a").Count;
                                }
                            }
                    }
                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
