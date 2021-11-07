using MyWatcher.Models.Enums;

namespace MyWatcher.Models;

public class ScrapingCompleteDTO
{
    //Type of service
    
    public int? UserId { get; set; }
    public Service Service { get; set; }
}