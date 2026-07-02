namespace ChessPlatform.Models
{
    public class TournamentPlayer
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public string PlayerId { get; set; }
        public ApplicationUser Player { get; set; }
    }
}
