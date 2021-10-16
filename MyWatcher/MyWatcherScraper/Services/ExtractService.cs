using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyWatcherScraper.Services
{
    public interface IExtractService
    {
        public Task<double> ExtractPriceFromPricerunner(HtmlAgilityPack.HtmlDocument html);
    }
    
    public class ExtractService : IExtractService
    {
        private Regex _pricerunnerPriceRegex;
        
        public ExtractService()
        {
            _pricerunnerPriceRegex = new Regex(@"Sammenlign priser fra (\d+\.?\d*)( (.{1-5})? til)?");
        }

        public async Task<double> ExtractPriceFromPricerunner(HtmlAgilityPack.HtmlDocument html)
        {
            var match = _pricerunnerPriceRegex.Match(html.DocumentNode.InnerText);
            if (match.Success)
            {
                Console.WriteLine(match.Groups[1]);
                var success = double.TryParse(match.Groups[1].Value, out var price);
                if (success) return price;
                else Console.WriteLine("Failed parsing price to double");
                //Console.WriteLine(match.Groups[2]);
            }
            else Console.WriteLine("No match");
            return -1;
        }
    }
}