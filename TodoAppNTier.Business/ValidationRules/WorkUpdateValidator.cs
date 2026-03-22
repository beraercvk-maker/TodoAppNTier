using FluentValidation;
using TodoAppNTier.Dtos.WorkUpdateDtos;

namespace TodoAppNTier.Business.ValidationRules
{
    public class WorkUpdateDtoValidator : AbstractValidator<WorkUpdateDto>
    {
       public WorkUpdateDtoValidator()
       {
           this.RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı zorunludur.");
           this.RuleFor(x => x.Definition).NotEmpty().WithMessage("Görev tanımı boş geçilemez.");
       }
    }
}