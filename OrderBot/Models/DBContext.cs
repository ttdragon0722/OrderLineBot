using Microsoft.EntityFrameworkCore;
using OrderBot.Models;

namespace OrderBot.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        // 定義資料表
        public DbSet<OrderRequest> OrderRequest { get; set; }
        public DbSet<OrderEvent> OrderEvent { get; set; }
    }
}
