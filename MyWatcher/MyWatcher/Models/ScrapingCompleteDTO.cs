using System;
using MyWatcher.Models.Enums;

namespace MyWatcher.Models;

public class ScrapingCompleteDTO
{
    //Type of service
    
    public Guid? UserId { get; set; }
    public Service Service { get; set; }
}