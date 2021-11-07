using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class WebsiteSecondHandItem : WebsiteItem
{
    [ForeignKey("SecondHandItem")]
    public Guid SecondHandItemId { get; set; }
    public virtual SecondHandItem SecondHandItem { get; set; }
}