using System.ComponentModel.DataAnnotations;
using System.Text;
using TessoApi.Helpers.Interfaces;
using TessoApi.Models.Helper;

namespace TessoApi.Helpers
{
    public class ObjectValidationHelper : IObjectValidationHelper
    {
        public ObjectValidationResult ValidateObject<T>(T obj)
        {
            List<ValidationResult> validationResults = [];
            StringBuilder errors = new();

            if (!Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true))
            {
                foreach (var err in validationResults)
                {
                    errors.AppendLine($"{err.ErrorMessage}");
                }
                return new ObjectValidationResult
                {
                    IsValid = false,
                    Error = errors.ToString()
                };
            }

            return new ObjectValidationResult
            {
                IsValid = true,
                Error = string.Empty
            };
        }
    }
}
