using OrderBot.Dtos;
using OrderBot.Services;
using Microsoft.AspNetCore.Mvc;
using OrderBot.Providers;
using OrderBot.Models;


namespace OrderBot.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        // 宣告 service
        private readonly LineBotService _lineBotService;
        private readonly JsonProvider _jsonProvider;
        private readonly DBContext _context;
        // constructor
        public LineBotController(DBContext context)
        {
            _lineBotService = new LineBotService(context);
            _jsonProvider = new JsonProvider();
        }

        [HttpPost("Webhook")]
        public IActionResult Webhook(WebhookRequestBodyDto body)
        {
            _lineBotService.ReceiveWebhook(body); // 呼叫 Service
            return Ok();
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage(string ReplyToken, string Text)
        {
            var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
            {
                ReplyToken = ReplyToken,
                Messages = new List<TextMessageDto>
                {
                    new TextMessageDto(){
                        Text = Text
                    }
                }
            };
            _lineBotService.ReplyMessageHandler("text", replyMessage);
            return Ok();
        }
        [HttpPost("QuoteMessage")]
        public IActionResult SendQuoteMessage(string ReplyToken, string Text, string QuoteToken)
        {
            var replyMessage = new ReplyMessageRequestDto<QuoteMessageDto>()
            {
                ReplyToken = ReplyToken,
                Messages = new List<QuoteMessageDto>
                {
                    new QuoteMessageDto(){
                        Text = Text ,
                        QuoteToken = QuoteToken,
                    }
                }
            };
            _lineBotService.ReplyMessageHandler("text", replyMessage);
            return Ok();
        }

    }


}