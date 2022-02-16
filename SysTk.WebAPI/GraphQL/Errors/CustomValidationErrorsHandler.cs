using FairyBread;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate.Resolvers;

namespace SysTk.WebAPI.GraphQL.Errors
{
    public class CustomValidationErrorsHandler : DefaultValidationErrorsHandler
    {
        protected override IErrorBuilder CreateErrorBuilder(IMiddlewareContext context, string argumentName,
                                                            IValidator validator, ValidationFailure failure)
        {
            return base.CreateErrorBuilder(context, argumentName, validator, failure).SetCode("Validation_Failure").ClearExtensions();
        }
    }
}
