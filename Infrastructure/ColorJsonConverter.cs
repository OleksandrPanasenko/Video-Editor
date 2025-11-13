using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace VideoEditor.Infrastructure
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        // Deserialization: Reads the hex string from JSON and converts it to a Color object.
        // E.g., reads "#FF0000" and returns Color.Red.
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Use ColorTranslator.FromHtml to handle hex codes (like #RRGGBB)
            return ColorTranslator.FromHtml(reader.GetString());
        }

        // Serialization: Converts the Color object into a standard web hex string for JSON.
        // E.g., takes Color.Red and writes "#FF0000".
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            // Creates a standard #RRGGBB hex string (Alpha is ignored for simplicity)
            writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
        }
    }
}
