using System;
using System.Text.Json;

namespace Golden.Common
{
    public static class JsonUtils
    {
        public static string ToJson(this object value)
        {
            return JsonSerializer.Serialize(value);
        }

        public static T JsonTo<T>(this String json)
        {
            return json.JsonTo<T>(propertyPath: String.Empty);
        }

        public static T JsonTo<T>(this String json, string propertyPath)
        {
            if (propertyPath.Length == 0)
                return JsonSerializer.Deserialize<T>(json);

            using var document = JsonDocument.Parse(json);
            return JsonElementTo<T>(document.RootElement, propertyPath);
        }

        private static T JsonElementTo<T>(JsonElement element, string propertyPath)
        {
            var index = propertyPath.IndexOf('.');
            if (index < 0)
                return element.GetProperty(propertyPath).GetRawText().JsonTo<T>();
            else
                return JsonElementTo<T>(
                    element.GetProperty(propertyPath.Substring(0, index)),
                    propertyPath.Substring(index + 1));
        }
    }
}
