using System;

namespace MyWatcher.Models.UserItem
{
    public class UserItemUpdateDTO
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public Guid UserId { get; set; }
    }
}