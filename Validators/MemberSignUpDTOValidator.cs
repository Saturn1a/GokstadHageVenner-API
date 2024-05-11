using FluentValidation;
using GokstadHageVennerAPI.Models.DTOs;

namespace GokstadHageVennerAPI.Validators;

public class MemberSignUpDTOValidator : AbstractValidator<MemberSignUpDTO>
{
    public MemberSignUpDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be atleast 3 charcaters")
            .MaximumLength(16).WithMessage("Username cant be longer than 16 characters")
            .Must(x => x != "string").WithMessage("Username cannot be ‘string’");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required ")
            .EmailAddress().WithMessage("Need a valid email address");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Firstname is required ")
        .Must(x => x != "string").WithMessage("First cannot be ‘string’");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Lastname is required")
            .Must(x => x != "string").WithMessage("Lastname cannot be ‘string’");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be atleast 8 characters")
            .MaximumLength(16).WithMessage("Password cant be longer than 16 characters")
            .Matches(@"[0-9]+").WithMessage("Password needs atleast one number")
            .Matches(@"[A-z]+").WithMessage("Password needs atleast one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Password needs atleast one lowercase letter")
            .Matches(@"[!?*#_]+").WithMessage("Password needs atleast one special character (! ? * # _)");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MinimumLength(8).WithMessage("Valid norweigan phone number is 8 numeric characters")
            .MaximumLength(8).WithMessage("Valid norweigan phone number is 8 numeric characters")
            .Must(phoneNumber => phoneNumber.All(char.IsDigit)).WithMessage("Phone number must contain only numeric characters");


        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .NotNull().WithMessage("Date of birth cant be null")
            //.Must(date => (int)date.Month > 0).WithMessage("Month cant be 0")
            //.Must(date => (int)date.Month < 13).WithMessage("Month cant be over 12 ")
            //.Must(date => date.Day > 0).WithMessage("Day cant be 0")
            //.Must(date => date.Day < 31).WithMessage("Day cant be over 31");
            .Must(BeAValidDateTime).WithMessage("Invalid date/time")
            .NotEqual(DateTime.Today).WithMessage("Date of birth cant be today");



    }

    private bool BeAValidDateTime(DateTime dateTime)
    {
        return dateTime != default(DateTime);
    }







}
