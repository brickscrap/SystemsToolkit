using FluentValidation;
using System.Net;

namespace SysTk.WebAPI.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> IsValidIP<T> (this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(x => IPAddress.TryParse(x, out _)).WithMessage("Invalid IP address provided.");
    }
}
