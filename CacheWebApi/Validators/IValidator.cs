namespace CacheWebApi.Validators
{
    public interface IValidator<TModel>
    {
        (bool, string) Validate(TModel model);
    }
}