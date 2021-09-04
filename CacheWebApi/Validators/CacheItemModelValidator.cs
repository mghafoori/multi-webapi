using CacheWebApi.Models;

namespace CacheWebApi.Validators
{
    public class CacheItemModelValidator : IValidator<CacheItemModel>
    {
        public (bool, string) Validate(CacheItemModel model)
        {
            if (model == null)
            {
                return (false, "Null model.");
            }
            if (string.IsNullOrEmpty(model.Key))
            {
                return (false, "'key' is required.");
            }
            if (string.IsNullOrEmpty(model.Value))
            {
                return (false, "'value' is required.");
            }
            return (true, string.Empty);
        }
    }
}