using FluentValidation;
using RegistryLibrary.Models;

namespace RegistryLibrary.Validation
{
    public class RegistryInfoValidator : AbstractValidator<RegistryInfoModel>
    {
        public RegistryInfoValidator()
        {
            RuleFor(reg => reg.MunicipalName).NotEmpty().WithMessage("Assembly name is required");
            RuleFor(reg => reg.RegistryName).NotEmpty().WithMessage("Registry name is required");
            RuleFor(reg => reg.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(reg => reg.Email).EmailAddress().WithMessage("Invalid Email Address");
        }

    }
}
