using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Persistence.Infrastructure;

public class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (!property.Writable)
        {
            var propertyInfo = member as PropertyInfo;

            bool hasPrivateSetter = propertyInfo?.GetSetMethod(true) != null;

            property.Writable = hasPrivateSetter;
        }

        return property;
    }
}