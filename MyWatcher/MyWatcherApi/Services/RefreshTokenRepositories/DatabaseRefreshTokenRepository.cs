using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWatcher.Models;

namespace MyWatcher.Services.RefreshTokenRepositories;

public class DatabaseRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DatabaseContext _context;

    public DatabaseRefreshTokenRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Create(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        RefreshToken refreshToken = await _context.RefreshTokens.FindAsync(id);
        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAll(int userId)
    {
        IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens
            .Where(r => r.UserId == userId)
            .ToListAsync();
        _context.RefreshTokens.RemoveRange(refreshTokens);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetByToken(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }
}