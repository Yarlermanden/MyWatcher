using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double PriceLastWeek { get; set; }
        public double PriceThisWeek { get; set; }
        public double PriceLowestKnown { get; set; }
        public DateTime? LastWeeklyPriceUpdate { get; set; }
        public string URL { get; set; }
        public bool Active { get; set; }
        public DateTime? LastScan { get; set; }
        
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public Item(){}
        public Item(string url, int serviceId)
        {
            URL = url;
            ServiceId = serviceId;
            Active = true;
            Name = "";
            LastWeeklyPriceUpdate = DateTime.Now;
        }
        
        public double PricePercentageCalculator(double price, double priceLastWeek)
        {
            if (price == 0) return 0;
            return (price - priceLastWeek) / price * 100;
        }
    }
}