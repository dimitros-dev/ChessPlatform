using ChessPlatform.Data;
using ChessPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ChessPlatform.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly ChessPlatformContext _context;

        public TournamentRepository(ChessPlatformContext context)
        {
            _context = context;
        }

        public async Task<List<Tournament>> GetAllAsync()
        {
            return await _context.Tournaments.ToListAsync();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            return await _context.Tournaments.FindAsync(id);
        }

        public async Task AddAsync(Tournament tournament)
        {
            await _context.Tournaments.AddAsync(tournament);
        }

        public async Task DeleteAsync(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);

            if (tournament != null)
                _context.Tournaments.Remove(tournament);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
