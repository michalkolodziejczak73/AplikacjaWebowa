using AplikacjaWebowa.Models;
using System.ComponentModel.DataAnnotations;

namespace AplikacjaWebowa.ViewModels
{
    public class VoteViewModel
    {
        public int SurveyId { get; set; }

        public string Question { get; set; } = string.Empty;

        public List<Option> Options { get; set; } = new();

        [Required(ErrorMessage = "Wybierz jedną odpowiedź.")]
        public int? SelectedOptionId { get; set; }

        public bool AlreadyVoted { get; set; }
    }
}