
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities
{
    public class UserItem
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Item")]
        //Could be a list of items - as a group - like all iPhone 12's
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public UserItem(){}
        public UserItem(int userId, int itemId, string name)
        {
            UserId = userId;
            ItemId = itemId;
            Name = name;
        }
    }
}