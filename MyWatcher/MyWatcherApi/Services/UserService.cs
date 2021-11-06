using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MyWatcher.Services.RefreshTokenRepositories;
using MyWatcher.Services.TokenGenerators;

namespace MyWatcher.Services
{
    public interface IUserService
    {
        Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto);
        Task<UserGetDTO?> LoginUser(UserLoginDTO dto);
        //Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<UserGetDTO> Authenticate(User account);
    }

    public class UserService : IUserService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbFactory;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserService(IDbContextFactory<DatabaseContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        
        public async Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto)
        {
            dto.Email = dto.Email.ToLower();
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var user = await dbContext.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user != null) return null;

            var salt = await GenerateSalt(32);
            var password = await GenerateHashedPassword(salt, dto.Password);
            user = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Salt = salt,
                Password = password,
                LastLogin = DateTimeOffset.UtcNow
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            return await GetUserGetDtoFromUser(user);
        }

        public async Task<UserGetDTO?> LoginUser(UserLoginDTO dto)
        {
            dto.Email = dto.Email.ToLower();
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var user = await dbContext.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null) return null;

            var hashedPassword = await GenerateHashedPassword(user.Salt, dto.Password);
            if (hashedPassword != user.Password) return null;
            
            user.LastLogin = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync();
            return await GetUserGetDtoFromUser(user);
        }

        /*
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            
        }
        */

        public async Task<UserGetDTO> Authenticate(User account)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            string accessToken = _accessTokenGenerator.GenerateToken(account);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = account.Id
            };
            await _refreshTokenRepository.Create(refreshTokenDTO);

            var response = await GetUserGetDtoFromUser(account);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;
            return response;
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

        private async Task<string> GenerateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        private async Task<string> GenerateHashedPassword(string salt, string plainPassword)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var passBytes = Encoding.ASCII.GetBytes(plainPassword);
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] combined = new byte[saltBytes.Length + passBytes.Length];

            for (int i = 0; i < passBytes.Length; i++) combined[i] = passBytes[i];
            for (int i = 0; i < saltBytes.Length; i++) combined[i + passBytes.Length] = saltBytes[i];

            var hashed = algorithm.ComputeHash(combined);
            return Convert.ToBase64String(hashed);
        }
    }
}