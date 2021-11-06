using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.User;

namespace MyWatcher.Services
{
    public interface IUserService
    {
        Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto);
    }

    public class UserService : IUserService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbFactory;
        
        public async Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var user = await dbContext.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user != null) return null;
            
            
            //Generate a new salt
            var salt = "";
            //Generate password from salt
            var password = "";
            user = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Salt = salt,
                Password = password,
                LastLogin = DateTimeOffset.Now
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            return await GetUserGetDtoFromUser(user);
        }

        private async Task<UserGetDTO> GetUserGetDtoFromUser(User user)
        {
            return new UserGetDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}