using System;

namespace MyWatcher.Models.UserItem
{
    public class UserItemTableDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double PriceComparedToLastWeek { get; set; } //Percentage 
        public double LowestPriceKnown { get; set; }
        public DateTime? LastScan { get; set; }
        public bool Active { get; set; }
    }
}