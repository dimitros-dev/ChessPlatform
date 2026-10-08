namespace ChessPlatform.ViewModels
{
    public class TournamentStandingsViewModel
    {
        public string PlayerName { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public double Points { get; set; }
    }
}
