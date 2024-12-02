using OrderBot.Models;
using OrderBot.Providers;

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


        public void MakeOrderEvent(OrderEvent orderEvent) {
            JsonLog.Log(orderEvent);
            InsertOrderEvent(orderEvent);
        }

        public OrderEvent QueryLatestActiveEventByGroupId(string groupId) {
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
    }
}
