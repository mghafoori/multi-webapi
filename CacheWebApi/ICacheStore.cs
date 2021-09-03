namespace Cache.WebApi
{
    public interface ICacheStore
    {
        string Get(string key);
        bool Add(string key, string value);
        bool Update(string key, string value);
        bool Delete(string key);
    }
}