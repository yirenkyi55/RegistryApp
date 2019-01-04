using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryAppUI.ViewModels
{
    public class UserMailsValidator : AbstractValidator<UserEmail>
    {
        public UserMailsValidator()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("Invalid email address");
        }
    }
}
