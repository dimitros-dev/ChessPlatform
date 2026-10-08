using ChessPlatform.Data;
using Microsoft.EntityFrameworkCore;
using ChessPlatform.Models;

namespace ChessPlatform.Services
{
    public class MatchService : IMatchService
    {
        private readonly ChessPlatformContext _context;

        public MatchService(ChessPlatformContext context)
        {
            _context = context;
        }
        public async Task<Match?> GetMatchDetailsAsync(int matchId, string userId)
        {
            var match = await _context.Matches.Include(m => m.Tournament).FirstOrDefaultAsync(m => m.Id == matchId
                                                                                                        && (m.Player1Id == userId || m.Player2Id == userId));
            return match;
        }
        public async Task<bool> SetResultAsync(int matchId, string userId, string result)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if(match == null)
            {
                return false;
            }
            if(match.Player1Id != userId && match.Player2Id != userId)
            {
                return false;
            }
            if (match.IsFinished)
            {
                return false;
            }
            if(result != "WhiteWin" && result != "BlackWin" && result != "Draw")
            {
                return false;
            }
            if(result == "WhiteWin")
            {
                match.WinnerId = match.Player1Id;
            }
            else if(result == "BlackWin")
            {
                match.WinnerId = match.Player2Id;
            }
            else if(result == "Draw")
            {
                match.WinnerId = null;
            }

            match.Result = result;
            match.IsFinished = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
