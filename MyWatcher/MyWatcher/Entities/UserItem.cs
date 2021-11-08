
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities
{
    public class UserItem
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Country")]
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }
        
        [ForeignKey("Continent")]
        public Guid? ContinentId { get; set; }
        public Continent? Continent { get; set; }

        public UserItem(){}
        //public UserItem(int userId, int itemId, string name)
        public UserItem(Guid userId, string name)
        {
            UserId = userId;
            //ItemId = itemId;
            Name = name;
            Active = true;
            Created = DateTime.UtcNow;
        }
    }
}