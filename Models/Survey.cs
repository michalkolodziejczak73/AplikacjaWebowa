using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;

namespace AplikacjaWebowa.Models
{
    public class Survey
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Question { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public string CreatorId { get; set; } = string.Empty;

        public List<Option> Options { get; set; } = new();
    }
}