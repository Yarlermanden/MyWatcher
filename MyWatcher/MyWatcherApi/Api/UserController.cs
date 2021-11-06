using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Models;
using MyWatcher.Models.User;
using MyWatcher.Services;

namespace MyWatcherApi.Api;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO dto)
    {
        Console.WriteLine("Registering user");
        var user = await _userService.RegisterUser(dto);
        if (user == null) return new ConflictResult();
        return new OkObjectResult(user);
    }

    [HttpPost("loginUser")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO dto)
    {
        Console.WriteLine("Login user in");
        var user = await _userService.LoginUser(dto);
        if (user == null) return new BadRequestResult();
        return new OkObjectResult(user);
    }
}