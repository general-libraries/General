using System;
using General.Helpers;
using Newtonsoft.Json;

namespace General.Web.Api.JsonConverters
{
    public class ObfuscationJsonConverter : JsonConverter
    {
        public static bool Enabled
        {
            get { return Manager.Instance.GlobalSetting.IdObfuscation; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(string).IsAssignableFrom(objectType) ||
                   typeof(long).IsAssignableFrom(objectType) ||
                   typeof(int).IsAssignableFrom(objectType) ||
                   typeof(long[]).IsAssignableFrom(objectType) ||
                   typeof(int[]).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            return Enabled ? HashHelper.Decode(value, objectType) : Convert.ChangeType(value, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Enabled) { value = HashHelper.Encode(value); }
            writer.WriteValue(value);
        }
    }
}
