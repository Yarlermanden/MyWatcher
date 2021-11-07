using MyWatcher.Models.Enums;

namespace MyWatcher.Entities;

public class StockItem : Item
{
    public override Service Service { get; set; } = Service.Stock;

    public StockItem() {}
    public StockItem(string url, Service service) : base(url, service)
    {
        
    }
}