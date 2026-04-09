using System.ComponentModel.DataAnnotations;
using TodoAppNTier.Business.Mappings.AutoMapper;

namespace TodoAppNTier.Dtos.WorkUpdateDtos
{
    public class WorkUpdateDto : IDto
    {
  
        [Range(1, int.MaxValue, ErrorMessage = "Geçersiz Id değeri.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Görev tanımı boş geçilemez.")]
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }


        public int AppUserId { get; set; }
    }
}