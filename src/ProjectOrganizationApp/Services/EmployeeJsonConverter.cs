using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectOrganizationApp.Models;

namespace ProjectOrganizationApp.Services
{
    public class EmployeeJsonConverter : JsonConverter<Employee>
    {
        private const string TypeProperty = "$type";

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Employee).IsAssignableFrom(typeToConvert);
        }

        public override Employee? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            if (!jsonDoc.RootElement.TryGetProperty(TypeProperty, out var typeElement))
            {
                return null;
            }

            var typeName = typeElement.GetString();
            var json = jsonDoc.RootElement.GetRawText();
            return typeName switch
            {
                nameof(Constructor) => JsonSerializer.Deserialize<Constructor>(json, options),
                nameof(Engineer) => JsonSerializer.Deserialize<Engineer>(json, options),
                nameof(Technician) => JsonSerializer.Deserialize<Technician>(json, options),
                nameof(LaboratoryAssistant) => JsonSerializer.Deserialize<LaboratoryAssistant>(json, options),
                nameof(SupportStaff) => JsonSerializer.Deserialize<SupportStaff>(json, options),
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, Employee value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var safeOptions = CreateSafeOptions(options);

            using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, type, safeOptions));
            writer.WriteStartObject();
            writer.WriteString(TypeProperty, type.Name);
            foreach (var property in jsonDoc.RootElement.EnumerateObject())
            {
                property.WriteTo(writer);
            }
            writer.WriteEndObject();
        }

        private static JsonSerializerOptions CreateSafeOptions(JsonSerializerOptions options)
        {
            var safeOptions = new JsonSerializerOptions(options);
            for (var i = safeOptions.Converters.Count - 1; i >= 0; i--)
            {
                if (safeOptions.Converters[i] is EmployeeJsonConverter)
                {
                    safeOptions.Converters.RemoveAt(i);
                }
            }

            return safeOptions;
        }
    }
}
