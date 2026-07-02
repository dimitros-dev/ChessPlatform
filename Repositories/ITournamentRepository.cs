using ChessPlatform.Models;

namespace ChessPlatform.Repositories
{
    public interface ITournamentRepository
    {
        public interface ITournamentRepository
        {
            Task<List<Tournament>> GetAllAsync();
            Task<Tournament?> GetByIdAsync(int id);
            Task AddAsync(Tournament tournament);
            Task DeleteAsync(int id);
            Task SaveAsync();
        }
    }
}
