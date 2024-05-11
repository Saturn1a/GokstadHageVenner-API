using FluentValidation;
using GokstadHageVennerAPI.Models.DTOs;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace GokstadHageVennerAPI.Validators;

public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
{
    public RegistrationDTOValidator()
    {
        RuleFor(x => x.Message)
            //.NotEmpty().WithMessage("Registration Message is required")
            //.MinimumLength(2).WithMessage("Registration Message needs to be atleast 2 characters")
            .MaximumLength(50).WithMessage("Registration Message cant be longer than 50 characters");
        



    }
}
