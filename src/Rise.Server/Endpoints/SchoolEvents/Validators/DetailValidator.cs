using Rise.Shared.SchoolEvents;
using FluentValidation;

namespace Rise.Server.Endpoints.SchoolEvents.Validators;
    public class DetailValidator : Validator<SchoolEventRequest.Detail>
    {
        public DetailValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id has to be strictly positive!");
        }
    }