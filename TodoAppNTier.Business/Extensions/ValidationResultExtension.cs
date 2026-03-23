

using TodoAppNTier.Common.ResponseObjects;

namespace TodoAppNTier.Business.Extensions
{
    public static class ValidationResultExtension //extension class'lar static olmak zorundadır.Ayrıca extension method'lar da static olmak zorundadır.
    {
        public static List<CustomValidationError> GetErrorMessages(this FluentValidation.Results.ValidationResult validationResult)
        {
            return validationResult.Errors.Select(e => new CustomValidationError
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage
            }).ToList();
        }
    }
}