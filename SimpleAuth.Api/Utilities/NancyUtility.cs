using Nancy;
using Nancy.ModelBinding;
using System;

namespace SimpleAuth.Api.Utilities
{
    public static class NancyUtility
    {
        public static T BindFromQuery<T>(this NancyModule module)
        {
            var request = module.Bind<T>();
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var fieldName = Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);

                if (module.Request.Query[fieldName] != null)
                {
                    try
                    {
                        prop.SetValue(request, Convert.ChangeType(module.Request.Query[fieldName], prop.PropertyType));
                    }
                    catch (Exception) { }
                }
            }

            return request;
        }
    }
}