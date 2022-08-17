using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpPost]
        public async Task<OrderResponse> Post([FromBody] Order order)
        {
            var request = JsonSerializer.Serialize(order);

            //Create Order
            var orderClient = _httpClientFactory.CreateClient("OrderService");
            var orderResponse = await orderClient.PostAsync("api/Order",
                new StringContent(request,Encoding.UTF8,"application/JSON")
            );
            var orderId =await orderResponse.Content.ReadAsStringAsync();
            string inventoryId = string.Empty;
            try
            {
                //UpdateInventory

                var inventoryClient = _httpClientFactory.CreateClient("InventoryService");
                var inventoryResponse = await inventoryClient.PostAsync("api/Inventory",
                    new StringContent(request, Encoding.UTF8, "application/JSON"));
                if (!inventoryResponse.IsSuccessStatusCode)
                {
                    throw new Exception(inventoryResponse.ReasonPhrase);
                }
                inventoryId = await inventoryResponse.Content.ReadAsStringAsync();


            }
            catch (Exception e)
            {
                await orderClient.DeleteAsync($"api/Order/{orderId}");
                return new OrderResponse() { Success = false, Reson = e.Message };
            }
            
            //Create Notification

            var notificationRequest = new HttpRequestMessage(HttpMethod.Post, "api/Notification");
            notificationRequest.Content = new StringContent(request, Encoding.UTF8, "application/JSON");
            var notificationClient = _httpClientFactory.CreateClient("NotificationService");
            var notificationResponse = await notificationClient.SendAsync(notificationRequest);
               
            var notificationId = await notificationResponse.Content.ReadAsStringAsync();
            
            Console.WriteLine($"OrderId:{orderId}, InventoryId:{inventoryId}, NotificationId:{notificationId}");
            return new OrderResponse() { OrderId = orderId };
        }
    }
}