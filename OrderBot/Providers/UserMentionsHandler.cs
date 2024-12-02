using Newtonsoft.Json;

namespace OrderBot.Providers
{
    public class Mention(string name, string userId)
    {
        public string Name { get; set; } = name;
        public string UserId { get; set; } = userId;
    }

    public class UserMentionsHandler
    {
        private List<Mention> _users;

        public UserMentionsHandler()
        {
            _users = new List<Mention>();
        }

        // 新增一個 Mention
        public void AddMention(string name, string userId)
        {
            _users.Add(new Mention(name, userId));
        }

        // 清空 Mention
        public void ClearMentions()
        {
            _users.Clear();
        }

        // 生成 JSON 字串
        public string Mention()
        {
            var result = new Dictionary<string, object>();

            foreach (var user in _users)
            {
                result[user.Name] = new
                {
                    type = "mention",
                    mentionee = new
                    {
                        type = "user",
                        userId = user.UserId
                    }
                };
            }

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}
