using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderBot.Providers
{
    public class CommandHandler
    {
        // 支援的指令集
        private readonly HashSet<string> validCommands = new HashSet<string> { "開單", "查詢", "幫助" };

        /// <summary>
        /// 解析使用者輸入的指令
        /// </summary>
        /// <param name="userInput">使用者的輸入字串</param>
        /// <returns>解析後的指令與參數</returns>
        public (string Command, Dictionary<string, string> Parameters) ParseInput(string userInput)
        {
            var parts = userInput.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = parts[0];
            var parameters = new Dictionary<string, string>();

            // 處理數量相關指令 (+int 或 -int)
            if ((command.StartsWith("+") || command.StartsWith("-")) && 
                int.TryParse(command.TrimStart('+', '-'), out int quantity))
            {
                parameters["數量"] = command.StartsWith("-") ? (-quantity).ToString() : quantity.ToString();
                return ("訂購", parameters); // 統一處理為 "buy" 指令
            }

            switch (command)
            {
                case "開單":
                    if (parts.Length > 1)
                        parameters["產品"] = parts[1];
                    if (parts.Length > 2 && int.TryParse(parts[2], out int price))
                        parameters["金額"] = price.ToString();
                    else
                        parameters["金額"] = "-1"; // 預設金額為 0
                    break;

                case "查詢":
                    // 查詢不需要額外參數
                    break;

                case "幫助":
                    // 幫助不需要額外參數
                    break;
            }

            return (command.ToLower(), parameters); // 使用小寫標準化指令名稱
        }

        /// <summary>
        /// 執行解析後的指令
        /// </summary>
        /// <param name="userInput">使用者的輸入字串</param>
        /// <returns>執行的指令與參數</returns>
        public (string Command, Dictionary<string, string> Parameters) ExecuteCommand(string userInput)
        {
            try
            {
                var (command, parameters) = ParseInput(userInput);
                Console.WriteLine(command);
                Console.WriteLine(parameters);
                switch (command)
                {
                    case "開單":
                        var product = parameters.ContainsKey("產品") ? parameters["產品"] : "未提供";
                        var price = parameters.ContainsKey("金額") ? parameters["金額"] : "未提供";
                        Console.WriteLine($"開單指令執行中...\n產品：{product}\n金額：{price}");
                        break;

                    case "查詢":
                        Console.WriteLine("執行查詢指令...");
                        break;

                    case "幫助":
                        Console.WriteLine("顯示幫助資訊...");
                        break;

                    case "訂購":
                        var amount = parameters.ContainsKey("數量") ? parameters["數量"] : "未提供";
                        Console.WriteLine($"訂購處理中...\n數量：{amount}");
                        break;

                    default:
                        break;
                }

                return (command, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"指令處理失敗：{ex.Message}");
                return (string.Empty, new Dictionary<string, string>());
            }
        }
    }
}
