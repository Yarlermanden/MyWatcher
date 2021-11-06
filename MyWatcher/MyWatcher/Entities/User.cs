using System;

namespace MyWatcher.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //encrypted
        public string Salt { get; set; }
        public DateTimeOffset LastLogin { get; set; }
    }
}