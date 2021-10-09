
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities
{
    public class UserItem
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        
        //[ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
        //[ForeignKey("Item")]
        public Item Item { get; set; }
    }
}