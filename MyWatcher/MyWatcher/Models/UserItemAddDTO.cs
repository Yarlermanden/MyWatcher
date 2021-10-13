namespace MyWatcher.Models
{
    public class UserItemAddDTO
    {
        public int UserId {get; set; }
        public int ServiceId { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }

        public UserItemAddDTO()
        {
            
        }

        public UserItemAddDTO(string url, string name, int serviceId)
        {
            URL = url;
            Name = name;
            ServiceId = serviceId;
        }
    }
}