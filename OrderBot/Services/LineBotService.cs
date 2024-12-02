using System;
using System.Net.Http.Headers;
using OrderBot.Providers;
using OrderBot.Dtos;
using OrderBot.Enum;
using System.Text;
using OrderBot.Models;
using Microsoft.IdentityModel.Tokens;

namespace OrderBot.Services
{
    public class LineBotService
    {

        // (將 LineBotController 裡宣告的 ChannelAccessToken & ChannelSecret 移到 LineBotService中)
        // 貼上 messaging api channel 中的 accessToken & secret
        private readonly string channelAccessToken = "wj7lfiQ69voD7ogL6asiiM18YT6Um9DHQ1g9lb9wyWlbDmJfufwVkdAmZS7yzgzOo/j3LIUCMlSharXke18K62SHeiER37aSgwO1dT1EJ/o4GyiWX1IvKL+2hn46sCBHncn28l5gAAu8jdo5gdTF2gdB04t89/1O/w1cDnyilFU=";
        private readonly string channelSecret = "f72cba06bfe3af4c8c1e9e8a409861fc";
        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider();
        private readonly CommandHandler _commandHandler = new CommandHandler();
        private readonly OrderService _orderService;


        /// 初始化
        public LineBotService(DBContext context)
        {
            _orderService = new OrderService(context);
        }
        public static DateTime ConvertUnixTimestampToDateTime(long unixTimestampMilliseconds)
        {
            // Convert the Unix timestamp to DateTime using DateTimeOffset
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).DateTime;

            // Return the DateTime object
            return dateTime.ToLocalTime();
        }

        public void ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            foreach (var eventObject in requestBody.Events)
            {
                if (eventObject.Source.Type == "group")
                {
                    switch (eventObject.Type)
                    {
                        case WebhookEventTypeEnum.Message:
                            Console.WriteLine("收到使用者傳送訊息！");
                            if (eventObject.Message.Type == "text")
                            {
                                Console.WriteLine(eventObject.Message.Text);
                                Console.WriteLine(eventObject.ReplyToken);

                                var (command, parameters) = _commandHandler.ExecuteCommand(eventObject.Message.Text);
                                switch (command)
                                {
                                    case "開單":

                                        if (parameters.ContainsKey("產品"))
                                        {
                                            _orderService.MakeOrderEvent(new OrderEvent()
                                            {
                                                Id = eventObject.Message.Id,
                                                Timestamp = ConvertUnixTimestampToDateTime(eventObject.Timestamp),
                                                Product = parameters["產品"],
                                                Price = parameters["金額"],
                                                GroupId = eventObject.Source.GroupId,
                                                UserId = eventObject.Source.UserId,
                                                QuoteToken = eventObject.Message.QuoteToken,
                                                Status = Status.Active,
                                                LastUpdate = ConvertUnixTimestampToDateTime(eventObject.Timestamp)
                                            });
                                            var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                                            {
                                                ReplyToken = eventObject.ReplyToken,
                                                Messages = new List<TextMessageDto>
                                            {
                                                new TextMessageDto(){Text = $"開單成功"}
                                            }
                                            };
                                            ReplyMessageHandler("text", replyMessage);
                                        }
                                        break;


                                    case "訂購":
                                        Console.WriteLine(_orderService.QueryLatestActiveEventByGroupId(eventObject.Source.GroupId));
                                        break;
                                }
                            }
                            break;
                        case WebhookEventTypeEnum.Unsend:
                            Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息！");
                            break;
                        case WebhookEventTypeEnum.Follow:
                            Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友！");
                            break;
                        case WebhookEventTypeEnum.Unfollow:
                            Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖了我們！");
                            break;
                        case WebhookEventTypeEnum.Join:
                            Console.WriteLine("我們被邀請進入聊天室了！");
                            break;
                        case WebhookEventTypeEnum.Leave:
                            Console.WriteLine("我們被聊天室踢出了");
                            break;
                    }

                } else
                {
                    var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                    {
                        ReplyToken = eventObject.ReplyToken,
                        Messages = new List<TextMessageDto>
                        {
                            new TextMessageDto(){Text = $"不是群組開不了單"}
                        }
                    };
                    ReplyMessageHandler("text", replyMessage);
                }
            }
        }

        /// <summary>
        /// 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public void ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody)
        {
            ReplyMessage(requestBody);
        }

        /// <summary>
        /// 將回覆訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>    
        public async void ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken); //帶入 channel access token
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            Console.WriteLine(json);

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }


}