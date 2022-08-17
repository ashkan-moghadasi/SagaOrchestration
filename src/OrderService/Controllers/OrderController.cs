using DataModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
       

        // POST api/<OrderController>
        [HttpPost]
        public int Post([FromBody] Order order)
        {
            Console.WriteLine($"Created New Order: {order.ProductName}");
            return 1;
        }
        

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Deleted Order {id}");
        }
    }
}
