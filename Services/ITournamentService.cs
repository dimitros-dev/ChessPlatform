using ChessPlatform.Models;
using ChessPlatform.ViewModels;

namespace ChessPlatform.Services
{
    public interface ITournamentService
    {
        Task <bool>CreateTournamentAsync(Tournament tournament);
        Task<bool> JoinTournamentAsync(int tournamentId, string playerId);
        Task<bool> LeaveTournamentAsync(int tournamentId, string playerId);
        Task<bool> DeleteTournamentAsync(int tournamentId, string organizerId);
        Task<bool> StartTournamentAsync(int tournamentId, string organizerId);
        Task<List<TournamentStandingsViewModel>> GetStandingsAsync(int tournamentId);
    }
}
