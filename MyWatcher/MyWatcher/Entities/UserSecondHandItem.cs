using System;
using System.ComponentModel.DataAnnotations.Schema;
using MyWatcher.Models.User;

namespace MyWatcher.Entities;

public class UserSecondHandItem : UserItem
{
    [ForeignKey("SecondHandItem")]
    public Guid SecondHandItemId { get; set; }
    public virtual SecondHandItem SecondHandItem { get; set; }
    
    public UserSecondHandItem() {}

    public UserSecondHandItem(Guid userId, Guid itemId, string name) : base(userId, name)
    {
        SecondHandItemId = itemId;
    }
}