using System;

namespace MyWatcher.Models.User;

public class UserGetDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    //Cookie
    public UserGetDTO()
    {
        
    }
}