using FluentValidation;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Validation
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("User name is required").Length(3, 100).WithMessage("User name must be between 3-100 characters");
            RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required").MinimumLength(8).WithMessage("Password must exceed 8 characters");  
            RuleFor(u => u.Question).NotEmpty().WithMessage("Security question is required");
            RuleFor(u => u.Answer).NotEmpty().WithMessage("Answer is required");
            RuleFor(u => u.AccessType).NotEmpty().WithMessage("Access type is required");
        }
    }
}
