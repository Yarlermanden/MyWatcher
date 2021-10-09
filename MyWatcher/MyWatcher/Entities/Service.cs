using System.Collections.Generic;

namespace MyWatcher.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual List<Item> Items { get; set; }
    }
}