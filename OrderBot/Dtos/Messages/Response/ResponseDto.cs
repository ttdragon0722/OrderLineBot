namespace OrderBot.Dtos {
    public class SentMessage
    {
        public string Id { get; set; }
        public string QuoteToken { get; set; }
    }

    public class ReplyMessageResponse
    {
        public List<SentMessage> SentMessages { get; set; }
    }
}