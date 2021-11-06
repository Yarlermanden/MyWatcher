using System;

namespace MyWatcher.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
}