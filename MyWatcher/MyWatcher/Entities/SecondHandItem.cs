using MyWatcher.Models.Enums;

namespace MyWatcher.Entities;

public class SecondHandItem : Item
{
    public override Service Service { get; set; } = Service.SecondHand;
    
    public SecondHandItem() {}
    public SecondHandItem(string url, Service service) : base(url, service)
    {
        
    }
}