using FluentValidation;
using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Security.Policy;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GokstadHageVennerAPI.Validators;

public class EventDTOValidator : AbstractValidator<EventDTO>
{
    public EventDTOValidator()
    {
        RuleFor(x => x.EventName)
            .NotEmpty().WithMessage("Event name is required")
            .MinimumLength(3).WithMessage("Event name must be atleast 3 charcaters")
            .MaximumLength(50).WithMessage("Event name cant be longer than 50 characters")
            .Must(x => x != "string").WithMessage("Event name cannot be ‘string’");

        RuleFor(x => x.EventDescription)
            .NotEmpty().WithMessage("Event description is required")
            .MinimumLength(10).WithMessage("Event description must be atleast 10 charcaters")
            .MaximumLength(200).WithMessage("Event description cant be longer than 200 characters")
            .Must(x => x != "string").WithMessage("Event name cannot be ‘string’");

        RuleFor(x => x.EventDate)
            .NotEmpty().WithMessage("Event date is required")
            .NotEqual(DateTime.Today).WithMessage("Event date cant be today")
            .LessThanOrEqualTo(DateTime.Today.AddYears(3)).WithMessage("Event date cannot be more than three year ahead")
            .Must(BeAValidDateTime).WithMessage("Invalid date/time ");
            //.Must(BeWithinThreeYears).WithMessage("Event date cannot be more than three years ahead");


    }

    private bool BeAValidDateTime(DateTime dateTime)
    {
        return dateTime != default(DateTime);
    }

    

    //private bool BeWithinThreeYears(DateTime dateTime)
    //{
    //    return dateTime <= DateTime.Today.AddYears(3);
    //}








}









