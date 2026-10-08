namespace ChessPlatform.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string? WinnerId { get; set; }
        public bool IsFinished { get; set; }
        public string? Result { get; set; }
        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }
        public Tournament Tournament { get; set; }
    }
}
