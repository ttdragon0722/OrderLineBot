using System.Text;

namespace OrderBot.Providers
{
    public class Mention(string userId, int amount)
    {
        public string UserId { get; set; } = userId;
        public int? Amount { get; set; } = amount;
    }

    public class UserMentionsHandler
    {
        private List<Mention> _users;

        public UserMentionsHandler()
        {
            _users = new List<Mention>();
        }

        // 新增一個 Mention
        public void AddMention(string userId, int amount)
        {
            _users.Add(new Mention(userId, amount));
        }

        // 清空 Mention
        public void ClearMentions()
        {
            _users.Clear();
        }

        // 生成 JSON 字串
        public Dictionary<string, object> Mention()
        {
            var result = new Dictionary<string, object>();

            for (int i = 0; i < _users.Count; i++)
            {
                var key = $"user{i}"; // 使用索引值作為名稱，例如 user0, user1, user2
                result[key] = new
                {
                    type = "mention",
                    mentionee = new
                    {
                        type = "user",
                        userId = _users[i].UserId
                    }
                };
            }

            return result;
        }
        public string String()
        {
            if (_users == null || !_users.Any())
            {
                Console.WriteLine("無使用者資料");
                return string.Empty;
            }

            var builder = new StringBuilder();

            for (int i = 0; i < _users.Count; i++)
            {
                builder.Append($"{{user{i}}} x{_users[i].Amount}");
                if (i < _users.Count - 1)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }


    }
}
