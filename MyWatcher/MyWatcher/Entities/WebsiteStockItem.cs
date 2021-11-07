using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class WebsiteStockItem : WebsiteItem
{
    [ForeignKey("StockItem")]
    public Guid StockItemId { get; set; }
    public virtual StockItem StockItem { get; set; }
}