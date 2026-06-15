using System.ComponentModel.DataAnnotations;

namespace AplikacjaWebowa.ViewModels
{
    public class CreateSurveyViewModel
    {
        [Required(ErrorMessage = "Podaj pytanie do ankiety.")]
        [StringLength(200, ErrorMessage = "Pytanie może mieć maksymalnie 200 znaków.")]
        [Display(Name = "Pytanie do ankiety")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj pierwszą opcję odpowiedzi.")]
        [Display(Name = "Opcja 1")]
        public string Option1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj drugą opcję odpowiedzi.")]
        [Display(Name = "Opcja 2")]
        public string Option2 { get; set; } = string.Empty;

        [Display(Name = "Opcja 3")]
        public string? Option3 { get; set; }

        [Display(Name = "Opcja 4")]
        public string? Option4 { get; set; }
    }
}