using FluentValidation;
using TodoAppNTier.Dtos.WorkDtos;

namespace TodoAppNTier.Business.ValidationRules
{
   public class WorkCreateDtoValidator : AbstractValidator<WorkCreateDto>
    {
            public WorkCreateDtoValidator()
            {
               this.RuleFor(x => x.Definition).NotEmpty().WithMessage("Tanım alanı boş geçilemez");
            }
    }
}