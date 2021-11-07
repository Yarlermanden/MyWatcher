using System;
using MyWatcher.Models.Enums;

namespace MyWatcher.Models.UserItem
{
    public class UserItemAddDTO
    {
        public Guid UserId {get; set; }
        public Service Service { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }

        public UserItemAddDTO()
        {
            
        }

        public UserItemAddDTO(string url, string name, Service service)
        {
            URL = url;
            Name = name;
            Service = service;
        }
    }
}