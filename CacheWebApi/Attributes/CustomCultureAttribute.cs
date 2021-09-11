using System;
using System.Linq;

namespace Cache.WebApi.Attributes
{
    public class CustomCultureAttribute : Attribute
    {
        private readonly string[] _allowedCultures;

        public CustomCultureAttribute(params string[] allowedCultures) 
            => _allowedCultures = allowedCultures;

        public bool IsAuthorized(string requestCulture)
        {
            return _allowedCultures.Contains(requestCulture);
        }
    }
}