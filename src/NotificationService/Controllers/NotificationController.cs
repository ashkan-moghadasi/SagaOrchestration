using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        public int Post([FromBody] Notification notification)
        {
            Console.WriteLine($"Send Notification for {notification.ProductName}");
            return 3;
        }
        [HttpDelete]
        public void Delete(int id)
        {
            Console.WriteLine($"Send Rollback Transaction Notification for {id}");
        }
    }
}
