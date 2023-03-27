using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace Countries.Infrastructure.Converters.RestCountries
{
    public class LanguagesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<string>).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = (JObject)JObject.ReadFrom(reader);

            return obj.Properties().Select(x => x.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var values = (IEnumerable<string>)value;
            JObject jo = new JObject();

            foreach (var v in values)
            {
                jo.Add(v, new JValue(v));
            }

            jo.WriteTo(writer);
        }
    }
}
