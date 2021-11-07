using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWatcher.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
    
    [ForeignKey("Continent")]
    public Guid ContinentId { get; set; }
    public virtual Continent Continent { get; set; }
}