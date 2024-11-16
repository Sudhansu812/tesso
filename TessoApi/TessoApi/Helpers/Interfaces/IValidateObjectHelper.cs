using TessoApi.Models.Helper;

namespace TessoApi.Helpers.Interfaces
{
    public interface IObjectValidationHelper
    {
        public ObjectValidationResult ValidateObject<T>(T obj);
    }
}
