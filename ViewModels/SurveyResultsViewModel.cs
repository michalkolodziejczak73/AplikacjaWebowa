namespace AplikacjaWebowa.ViewModels
{
    public class SurveyResultsViewModel
    {
        public int SurveyId { get; set; }

        public string Question { get; set; } = string.Empty;

        public int TotalVotes { get; set; }

        public List<OptionResultViewModel> Options { get; set; } = new();
    }

    public class OptionResultViewModel
    {
        public string Text { get; set; } = string.Empty;

        public int VoteCount { get; set; }

        public double Percentage { get; set; }
    }
}