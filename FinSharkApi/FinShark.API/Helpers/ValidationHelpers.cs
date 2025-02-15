using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinShark.API.Helpers;

public static class ValidationHelpers
{
    public static List<string> GetValidationErrors(ModelStateDictionary modelState)
    {
        return modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}
