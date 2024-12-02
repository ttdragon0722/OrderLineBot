using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OrderBot.Models;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly DBContext _context;

    public TestController(DBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var result = _context.OrderRequest.ToList(); // 測試 Orders 資料表是否能查詢
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"無法連接資料庫: {ex.Message}");
        }
    }
}
