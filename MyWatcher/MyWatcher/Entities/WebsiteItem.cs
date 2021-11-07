using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class WebsiteItem
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string Url { get; set; }
    
    [ForeignKey("Website")]
    public Guid WebsiteId { get; set; }
    public virtual Website Website { get; set; }
}