using OrderBot.Enum;
namespace OrderBot.Dtos
{
    public class TextMessageDto : BaseMessageDto
    {

        public TextMessageDto()
        {
            Type = MessageTypeEnum.Text;
        }
        public string Text { get; set; } = string.Empty;
        
    }

    public class TextV2MessageDto : TextMessageDto
    {
        public TextV2MessageDto()
        {
            Type = MessageTypeEnum.TextV2;
        }
        public string? QuoteToken { get; set; } = string.Empty;
        public SubstitutionDto? Substitution { get; set; }
    }

    public class QuoteMessageDto : TextMessageDto
    {
        public QuoteMessageDto()
        {

        }
        public string? QuoteToken { get; set; } = string.Empty;
    }
}