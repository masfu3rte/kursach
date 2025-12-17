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
            using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));
            writer.WriteStartObject();
            writer.WriteString(TypeProperty, type.Name);
            foreach (var property in jsonDoc.RootElement.EnumerateObject())
            {
                property.WriteTo(writer);
            }
            writer.WriteEndObject();
        }
    }
}
