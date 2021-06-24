using System.Text.Json;

namespace TestProject.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions =
            new JsonSerializerOptions(JsonSerializerDefaults.Web);
        
        public static TValue? DeserializeWithWebDefaults<TValue>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
        }
    }
}