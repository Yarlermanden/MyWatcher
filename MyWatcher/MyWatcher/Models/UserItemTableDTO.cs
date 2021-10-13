using System;

namespace MyWatcher.Models
{
    public class UserItemTableDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime? LastScan { get; set; }
        public bool Activate { get; set; }
    }
}