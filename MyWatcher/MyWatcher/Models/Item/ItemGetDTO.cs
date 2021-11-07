using System;
using MyWatcher.Entities;

namespace MyWatcher.Models.Item
{
    public class ItemGetDTO
    {
        public Guid Id { get; set; }
        public string URL { get; set; }

        public ItemGetDTO()
        {
            
        }
        public ItemGetDTO(Entities.Item i)
        {
            Id = i.Id;
            URL = i.URL;
        }
    }
}