using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class Website
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string BaseUrl { get; set; }
    
    public string Regexes { get; set; } //A Json object containing a list of regexes
    
    [ForeignKey("Country")]
    public Guid? CountryId { get; set; }
    public virtual Country? Country { get; set; }
}