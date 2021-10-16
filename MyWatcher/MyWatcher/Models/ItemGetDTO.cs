using MyWatcher.Entities;

namespace MyWatcher.Models
{
    public class ItemGetDTO
    {
        public int Id { get; set; }
        public string URL { get; set; }

        public ItemGetDTO()
        {
            
        }
        public ItemGetDTO(Item i)
        {
            Id = i.Id;
            URL = i.URL;
        }
    }
}