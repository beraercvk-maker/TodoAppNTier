using System.ComponentModel.DataAnnotations;

namespace TodoAppNTier.Dtos.WorkUpdateDtos
{
    public class WorkUpdateDto
    {
  
        [Range(1, int.MaxValue, ErrorMessage = "Geçersiz Id değeri.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Görev tanımı boş geçilemez.")]
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }
    }
}