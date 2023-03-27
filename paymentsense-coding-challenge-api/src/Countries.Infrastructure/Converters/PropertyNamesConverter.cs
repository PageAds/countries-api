using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Countries.Infrastructure.Converters
{
    public class PropertyNamesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<string>).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = (JObject)JObject.ReadFrom(reader);

            return obj.Properties().Select(x => x.Name);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var values = (IEnumerable<string>)value;
            JObject jo = new JObject();

            foreach (var v in values)
            {
                jo.Add(v, default);
            }

            jo.WriteTo(writer);
        }
    }
}
