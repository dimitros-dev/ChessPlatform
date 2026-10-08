namespace ChessPlatform.ViewModels
{
    public class TournamentCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TournamentType { get; set; }
        public int TimeControlMinutes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
