using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderBot.Providers
{
    /// <summary>
    /// c# classes to JSON 
    /// </summary>
    public class JsonProvider
    {
        private readonly JsonSerializerOptions serializeOption = new()
        {   
            // 駝峰式命名
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // 在處理屬性名稱時忽略大小寫
            PropertyNameCaseInsensitive = true,
            // 如果屬性值為 null，則不包含在序列化結果中。
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            // 使用較寬鬆的編碼規則來處理特殊字符，例如不轉義 / 或 Unicode 字符，生成更「人類可讀」的 JSON。
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static readonly JsonSerializerOptions deserializeOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, serializeOption);
        }

        public T Deserialize<T>(string str)
        {
            var result = JsonSerializer.Deserialize<T>(str, deserializeOptions) ?? throw new InvalidOperationException("Deserialization failed, result is null.");
            return result;
        }
    }
}
