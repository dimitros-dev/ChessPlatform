using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ChessPlatform.Models;

namespace ChessPlatform.Services
{
    public interface IMatchService
    {
        Task<Match?> GetMatchDetailsAsync(int matchId, string userId);
        Task<bool> SetResultAsync(int matchId, string userId, string result);
    }
}
