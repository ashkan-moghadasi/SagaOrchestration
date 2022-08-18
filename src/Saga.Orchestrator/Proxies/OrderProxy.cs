using System.Text;
using System.Text.Json;
using DataModel;

namespace Saga.Orchestrator.Proxies
{
    public class OrderProxy : IOrderProxy
    {
        private readonly IHttpClientFactory _clientFactory;

        public OrderProxy(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<(int, bool)> CreateOrder(Order order)
        {
            var request = JsonSerializer.Serialize(order);
            try
            {
                //Create Order
                var orderClient = _clientFactory.CreateClient("OrderService");
                var orderResponse = await orderClient.PostAsync("api/Order",
                    new StringContent(request, Encoding.UTF8, "application/JSON")
                );
                var orderId = await orderResponse.Content.ReadAsStringAsync();
                return (Int32.Parse(orderId), true);
            }
            catch (Exception e)
            {
                return (-1, false);
            }
            

            
        }

        public void DeleteOrder(int orderId)
        {
            var orderClient = _clientFactory.CreateClient("OrderService");
            orderClient.DeleteAsync($"api/Order/{orderId}");
            Console.WriteLine($"Order Deleted {orderId}");
        }
    }
}
