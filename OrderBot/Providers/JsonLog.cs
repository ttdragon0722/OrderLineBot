using System.Text.Json;

namespace OrderBot.Providers {
    public class  JsonLog {
        public static void Log(object obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // 啟用縮排
            };
            Console.WriteLine(JsonSerializer.Serialize(obj, options));
        }
    }
}