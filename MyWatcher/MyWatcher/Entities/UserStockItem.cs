using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class UserStockItem : UserItem
{
    [ForeignKey("StockItem")]
    public Guid StockItemId { get; set; }
    public virtual StockItem StockItem { get; set; }
    
    public UserStockItem() {}

    public UserStockItem(Guid userId, Guid stockItemId, string name) : base(userId, name)
    {
        StockItemId = stockItemId;
    }
}