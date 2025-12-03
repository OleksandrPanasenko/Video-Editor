using Newtonsoft.Json;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Globalization;

namespace VideoEditor.Infrastructure
{
    public class ColorJsonConverter : JsonConverter<Color> // Note: Inheritance style is different
    {
        // Override the base methods
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            // Converts Color to a standard web hex string (e.g., "#FF0000")
            string hex = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
            writer.WriteValue(hex);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                // Read the hex string and use ColorTranslator to convert it
                return ColorTranslator.FromHtml((string)reader.Value);
            }
            // Handle null or non-string values gracefully (returns an empty color)
            return Color.Empty;
        }

        // Required override for non-generic Newtonsoft.Json.JsonConverter
        /*public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }*/
    }
}
