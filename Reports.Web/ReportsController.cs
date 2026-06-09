namespace Reports.Web.Controllers;

[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("sales")]
    public IActionResult GetSales()
    {
        // Вернуть данные из Postgres через Redis cache  
        return Ok();
    }
}