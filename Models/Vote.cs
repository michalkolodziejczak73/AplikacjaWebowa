namespace AplikacjaWebowa.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int OptionId { get; set; }

        public Option Option { get; set; } = null!;

        public string RespondentId { get; set; } = string.Empty;

        public DateTime VotedAt { get; set; } = DateTime.Now;
    }
}