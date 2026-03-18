using System.ComponentModel.DataAnnotations;

namespace TodoAppNTier.Dtos.WorkDtos
{
    public class WorkCreateDto
    {
       [Required(ErrorMessage = "Tanım alanı boş geçilemez.")]
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }
    }
}