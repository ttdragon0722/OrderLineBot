using OrderBot.Enum;
using OrderBot.Models;
using OrderBot.Providers;
using OrderBot.Utils;

namespace OrderBot.Services
{
    public class OrderService
    {
        private readonly DBContext _context;
        public OrderService(DBContext context)
        {
            _context = context;
        }

        // 插入資料的方法
        public void InsertData<T>(T entity) where T : class
        {
            if (entity != null)
            {
                // 根據泛型類型檢查並插入資料
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }

        // 可選：根據物件類型，單獨處理 OrderEvent 和 OrderRequest
        public void InsertOrderEvent(OrderEvent orderEvent)
        {
            InsertData(orderEvent); // 使用通用插入方法
        }

        public void InsertOrderRequest(OrderRequest orderRequest)
        {
            InsertData(orderRequest); // 使用通用插入方法
        }


        public void MakeOrderEvent(OrderEvent orderEvent)
        {
            JsonLog.Log(orderEvent);
            InsertOrderEvent(orderEvent);
        }

        public void MakeOrderRequest(OrderRequest orderRequest)
        {
            JsonLog.Log(orderRequest);
            InsertOrderRequest(orderRequest);
        }

        public OrderEvent QueryLatestActiveEventByGroupId(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                Console.WriteLine("GroupId 不能為空");
                return null;
            };

            var latestOrderEvent = _context.OrderEvent
                .Where(e => e.GroupId == groupId && e.Status == "active")
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefault();

            return latestOrderEvent;
        }

        public OrderEvent? LatestEventFinished(string groupId, long timestamp)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                Console.WriteLine("GroupId 不能為空");
                return null;
            }

            // 查找最新的「active」狀態的訂單事件
            var latestOrderEvent = _context.OrderEvent
                .Where(e => e.GroupId == groupId && e.Status == "active")
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefault();

            // 如果找不到符合的訂單事件
            if (latestOrderEvent == null)
            {
                Console.WriteLine("未找到符合的訂單事件");
                return null;
            }

            // 將 Status 改為 "finish"
            latestOrderEvent.Status = Status.Finished;

            // 更新 LastUpdate 為當前時間
            latestOrderEvent.LastUpdate = Support.Timestamp2DateTime(timestamp);

            // 儲存更改
            _context.SaveChanges();

            // 返回更新後的物件
            return latestOrderEvent;
        }

        public OrderEvent? SelectEventFinished(string id, long timestamp)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Id 不能為空");
                return null;
            }

            // 查找指定 id 的訂單事件
            var orderEvent = _context.OrderEvent
                .Where(e => e.Id == id)
                .FirstOrDefault();

            // 如果找不到該事件
            if (orderEvent == null)
            {
                Console.WriteLine($"未找到 Id 為 {id} 的訂單事件");
                return null;
            }

            // 更新狀態和 LastUpdate 時間
            orderEvent.Status = Status.Finished;
            orderEvent.LastUpdate = Support.Timestamp2DateTime(timestamp); 

            // 儲存變更到資料庫
            _context.SaveChanges();

            // 返回更新後的訂單事件
            return orderEvent;
        }



        public OrderEvent? QueryOrderEventById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Id 不能為空");
                return null;
            };

            var orderEvent = _context.OrderEvent
                .FirstOrDefault(e => e.Id == id);

            return orderEvent;
        }

        public List<OrderRequest> GetActiveOrderRequestsByQuoteId(string quoteId)
        {
            if (string.IsNullOrWhiteSpace(quoteId))
            {
                Console.WriteLine("QuoteId 不能為空");
                return [];
            }

            // 查詢符合條件的 OrderRequest 資料
            var activeRequests = _context.OrderRequest
                .Where(r => r.QuoteId == quoteId && r.Status == "active")
                .ToList();

            // 如果找不到符合條件的資料
            if (!activeRequests.Any())
            {
                Console.WriteLine($"未找到符合條件的訂單請求 (QuoteId: {quoteId}, Status: active)");
            }

            return activeRequests;
        }
        public List<(string UserId, int TotalAmount)> GroupAndSumByUserId(List<OrderRequest> orderRequests)
        {
            if (orderRequests == null || !orderRequests.Any())
            {
                Console.WriteLine("訂單列表為空");
                return new List<(string UserId, int TotalAmount)>();
            }

            // 按 UserId 分組，並計算 Amount 的總和
            var groupedResults = orderRequests
                .GroupBy(order => order.UserId)
                .Select(group => (UserId: group.Key, TotalAmount: group.Sum(order => order.Amount)))
                .ToList();

            return groupedResults;
        }

        public List<(string UserId, int TotalAmount)> GetGroupedActiveOrderRequestsByQuoteId(string quoteId)
        {
            if (string.IsNullOrWhiteSpace(quoteId))
            {
                Console.WriteLine("QuoteId 不能為空");
                return new List<(string UserId, int TotalAmount)>();
            }

            // 查詢符合條件的 OrderRequest 資料
            var activeRequests = _context.OrderRequest
                .Where(r => r.QuoteId == quoteId && r.Status == "active")
                .ToList();

            // 如果找不到符合條件的資料
            if (!activeRequests.Any())
            {
                Console.WriteLine($"未找到符合條件的訂單請求 (QuoteId: {quoteId}, Status: active)");
                return new List<(string UserId, int TotalAmount)>();
            }

            // 按 UserId 分組，並計算 Amount 的總和
            var groupedResults = activeRequests
                .GroupBy(order => order.UserId)
                .Select(group => (UserId: group.Key, TotalAmount: group.Sum(order => order.Amount)))
                .ToList();

            return groupedResults;
        }



    }
}
