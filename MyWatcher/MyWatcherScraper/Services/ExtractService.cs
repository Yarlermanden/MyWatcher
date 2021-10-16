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
        private Regex _pricerunnerPriceRangeRegex;
        private Regex _pricerunnerPriceSingleRegex;
        
        public ExtractService()
        {
            _pricerunnerPriceRangeRegex = new Regex(@"Sammenlign priser fra (\d+(\.?\d*)*)( (.{1-5})? til)?");
            _pricerunnerPriceSingleRegex = new Regex(@"Pris (\d+(\.?\d*)*) ?");
        }

        public async Task<double> ExtractPriceFromPricerunner(HtmlAgilityPack.HtmlDocument html)
        {
            var match = _pricerunnerPriceRangeRegex.Match(html.DocumentNode.InnerText);
            if (match.Success)
            {
                Console.WriteLine(match.Groups[1]);
                var success = double.TryParse(match.Groups[1].Value.Replace(".", ","), out var price);
                if (success) return price;
                else Console.WriteLine("Failed parsing price to double");
                //Console.WriteLine(match.Groups[2]);
            }
            else
            {
                match = _pricerunnerPriceSingleRegex.Match(html.DocumentNode.InnerText);
                if (match.Success)
                {
                    Console.WriteLine(match.Groups[1]);
                    var success = double.TryParse(match.Groups[1].Value.Replace(".", ","), out var price);
                    if (success) return price;
                    else Console.WriteLine("Failed parsing price to double");
                }
                Console.WriteLine("No match");
            }
            return -1;
        }
    }
}