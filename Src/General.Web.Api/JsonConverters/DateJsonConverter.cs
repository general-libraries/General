using System;
using Newtonsoft.Json;

namespace General.Web.Api.JsonConverters
{
    /// <summary>
    /// A JsonConverter to convert DateTime to a custom format. Defult format is ISO 8601 date-only format (yyyy-MM-dd);
    /// </summary>
    public class DateJsonConverter : JsonConverter
    {
        /// <summary>
        /// Gets ISO 8601 date format
        /// </summary>
        public const string ISO8601DateFormat = "yyyy-MM-dd";

        private readonly string _dateFormat;

        /// <summary>
        /// Initializes a new <see cref="DateJsonConverter"/> with ISO 8601 date format.
        /// </summary>
        public DateJsonConverter()
            : this(ISO8601DateFormat)
        { }

        /// <summary>
        /// Initializes a new <see cref="DateJsonConverter"/> with custom date format: <paramref name="dateFormat"/>.
        /// </summary>
        /// <param name="dateFormat">Custom date format.</param>
        public DateJsonConverter(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var datetime = value as DateTime?;

            if (datetime.HasValue)
            {
                var str = datetime.Value.ToString(_dateFormat);
                writer.WriteValue(str);
            }
        }

    }
}
