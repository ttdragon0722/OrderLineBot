using System.ComponentModel.DataAnnotations;
using OrderBot.Enum;

namespace OrderBot.Models
{
    public class OrderEvent
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Product { get; set; }
        public string Price { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }
        public string QuoteToken { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdate { get; set; }

        public bool IsPriceExist() {
            return int.TryParse(Price, out int price) && price > 0;
        }
        public int PriceInt() {
            return int.Parse(Price);
        }

    }

    public class OrderRequest
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string QuoteId { get; set; }
        public int Amount { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Status { get; set; }
    }

}
