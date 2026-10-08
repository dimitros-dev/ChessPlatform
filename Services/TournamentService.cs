using ChessPlatform.Data;
using ChessPlatform.Models;
using ChessPlatform.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace ChessPlatform.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ChessPlatformContext _context;
        public TournamentService(ChessPlatformContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateTournamentAsync(Tournament tournament)
        {
            if(tournament.EndDate <= tournament.StartDate)
            {
                return false;
            }
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> JoinTournamentAsync(int tournamentId, string playerId)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);

            if(tournament == null || tournament.IsStarted)
            {
                return false;
            }

            var alreadyJoined = await _context.TournamentPlayers.AnyAsync(tp => tp.TournamentId == tournamentId
                                                                           && tp.PlayerId == playerId);
            if (alreadyJoined)
            {
                return false;
            }
            var tournamentPlayer = new TournamentPlayer 
            {
                TournamentId = tournamentId,
                PlayerId = playerId
            };

            _context.TournamentPlayers.Add(tournamentPlayer);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> LeaveTournamentAsync(int tournamentId, string playerId)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);

            if(tournament == null || tournament.IsStarted)
            {
                return false;
            }

            var record = await _context.TournamentPlayers.FirstOrDefaultAsync(tp => tp.TournamentId == tournamentId
                                                                                && tp.PlayerId == playerId);
            if(record == null)
            {
                return false;
            }

            _context.TournamentPlayers.Remove(record);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteTournamentAsync(int tournamentId, string organizerId)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId
                                                                            && t.CreatedById == organizerId);
            if (tournament == null)
            {
                return false;
            }

            var players = _context.TournamentPlayers.Where(x => x.TournamentId == tournamentId);

            _context.TournamentPlayers.RemoveRange(players);

            var matches = _context.Matches.Where(x => x.TournamentId == tournamentId);

            _context.Matches.RemoveRange(matches);
            _context.Tournaments.Remove(tournament);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> StartTournamentAsync(int tournamentId, string organizerId)
        {
            
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
                return false;

            if (tournament.CreatedById != organizerId)
                return false;

            if (tournament.IsStarted)
                return false;

            var playerCount = await _context.TournamentPlayers.CountAsync(tp => tp.TournamentId == tournamentId);

            if (playerCount < 2)
                return false;

            if (playerCount % 2 != 0)
                return false;

            var players = await _context.TournamentPlayers
                .Where(tp => tp.TournamentId == tournamentId)
                .Select(tp => tp.PlayerId)
                .ToListAsync();

            tournament.IsStarted = true;

            for (int i = 0; i < players.Count; i += 2)
            {
                var match = new Match
                {
                    TournamentId = tournamentId,
                    Player1Id = players[i],
                    Player2Id = players[i + 1],
                    IsFinished = false
                };

                _context.Matches.Add(match);
            }

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<TournamentStandingsViewModel>> GetStandingsAsync(int tournamentId)
        {
            var players = await _context.TournamentPlayers
                .Where(tp => tp.TournamentId == tournamentId)
                .Include(tp => tp.Player)
                .ToListAsync();

            var matches = await _context.Matches
                .Where(m => m.TournamentId == tournamentId && m.IsFinished)
                .ToListAsync();

            var standings = new List<TournamentStandingsViewModel>();

            foreach (var player in players)
            {
                var playerMatches = matches
                    .Where(m => m.Player1Id == player.PlayerId ||
                                m.Player2Id == player.PlayerId)
                    .ToList();

                var wins = playerMatches.Count(m => m.WinnerId == player.PlayerId);

                var draws = playerMatches.Count(m => m.Result == "Draw");

                var gamesPlayed = playerMatches.Count;

                var losses = gamesPlayed - wins - draws;

                var points = wins + (draws * 0.5);

                standings.Add(new TournamentStandingsViewModel
                {
                    PlayerName = player.Player.UserName,
                    GamesPlayed = gamesPlayed,
                    Wins = wins,
                    Draws = draws,
                    Losses = losses,
                    Points = points
                });
            }

            return standings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.Wins)
                .ToList();
        }

    }
   
}
