using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AProjectManager.Persistence.FileData
{
    public class YamlConfigManager : IConfigManager
    {
        public string Serialize<T>(T obj)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            return serializer.Serialize(obj);
        }

        public T Deserialize<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<T>(input);
        }
    }
}