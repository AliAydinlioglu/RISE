using Rise.Shared.UserPreferences;
using FluentValidation;

namespace Rise.Server.Endpoints.UserPreferences.Validators
{
    public class UpdateValidator : Validator<Dictionary<string, object>>
    {
        public UpdateValidator()
        {

        }
    }
}