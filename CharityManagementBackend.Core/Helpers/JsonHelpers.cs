using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace CharityManagementBackend.Core.Helpers
{
    public class JsonLowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }

    public static class ObjectStringSerilizer
    {
        public static string ToSerilizedString(this Object obj)
        {

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonLowercaseContractResolver();
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }
    }


}
