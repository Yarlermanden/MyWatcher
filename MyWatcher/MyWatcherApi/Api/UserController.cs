using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Models;
using MyWatcher.Models.User;
using MyWatcher.Services;

namespace MyWatcherApi.Api;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class UserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO dto)
    {
        var user = await _userService.RegisterUser(dto);
        if (user == null) return new ConflictResult();
        return new OkObjectResult(user);
    }
}