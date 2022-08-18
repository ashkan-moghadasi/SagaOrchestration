using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public int UpdateInventory([FromBody] Inventory inventory)
        {
            //throw new Exception("Update Inventory Failed");
            Console.WriteLine($"Updated inventory for : {inventory.ProductName}");
            return 2;
        }
        [HttpDelete("{id}")]
        public void Delete(int id){
            Console.WriteLine($"Deleted inventory : {id}");
        }
    }
}
