using FluentValidation;
using Rise.Shared.StudentActivities;

namespace Rise.Server.Endpoints.StudentActivities.Validators;

public class DetailValidator : Validator<StudentActivityRequest.Detail>
{
    public DetailValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id has to be strictly positive!");
    }
}