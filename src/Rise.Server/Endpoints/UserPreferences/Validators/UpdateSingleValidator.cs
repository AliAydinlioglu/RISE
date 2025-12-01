using Rise.Shared.UserPreferences;
using FluentValidation;

namespace Rise.Server.Endpoints.UserPreferences.Validators
{
    public class UpdateSingleValidator : Validator<UserPreferenceRequest.UpdateSingle>
    {
        public UpdateSingleValidator()
        {

        }
    }
}