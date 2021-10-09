using System.Collections.Generic;

namespace MyWatcher.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Item> Items { get; set; }
    }
}