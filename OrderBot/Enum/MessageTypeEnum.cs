namespace OrderBot.Enum
{
    /// <summary>
    /// Message Type of Line Message
    /// 
    /// This enum contains the various message types supported by the LINE Messaging API.
    /// <br />
    /// Reference:
    /// https://developers.line.biz/en/docs/messaging-api/message-types/
    /// </summary>
    public static class MessageTypeEnum
    {
        public const string Text = "text";
        public const string TextV2 = "textV2";
        public const string Sticker = "sticker";
        public const string Image = "image";
        public const string Video = "video";
        public const string Audio = "audio";
        public const string Location = "location";
        public const string Imagemap = "imagemap";
        public const string Template = "template";
        public const string Flex = "flex";
    }
}
