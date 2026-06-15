using System.ComponentModel.DataAnnotations;

namespace AplikacjaWebowa.ViewModels
{
    public class EditSurveyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj pytanie ankiety.")]
        [StringLength(
            200,
            ErrorMessage = "Pytanie może mieć maksymalnie 200 znaków.")]
        [Display(Name = "Pytanie ankiety")]
        public string Question { get; set; } = string.Empty;

        [Display(Name = "Ankieta aktywna")]
        public bool IsActive { get; set; }
    }
}