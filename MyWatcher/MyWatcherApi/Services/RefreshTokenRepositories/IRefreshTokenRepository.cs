using System;
using System.Threading.Tasks;
using MyWatcher.Models;

namespace MyWatcher.Services.RefreshTokenRepositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByToken(string token);
    Task Create(RefreshToken refreshToken);
    Task Delete(Guid id);
    Task DeleteAll(int userId);
}