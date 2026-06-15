using System.ComponentModel.DataAnnotations;

namespace AplikacjaWebowa.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; } = string.Empty;

        public int SurveyId { get; set; }

        public Survey Survey { get; set; } = null!;

        public List<Vote> Votes { get; set; } = new();
    }
}