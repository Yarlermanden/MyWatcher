using System.ComponentModel.DataAnnotations;

namespace MyWatcher.Models;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; }
}