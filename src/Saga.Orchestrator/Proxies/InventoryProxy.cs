using System.Text;
using System.Text.Json;
using DataModel;

namespace Saga.Orchestrator.Proxies;

public class InventoryProxy : IInventoryProxy
{
    private readonly IHttpClientFactory _clientFactory;

    public InventoryProxy(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    public async Task<(int, bool)> UpdateInventory(Order order)
    {
        try
        {
            //UpdateInventory
            var request = JsonSerializer.Serialize(order);
            var inventoryClient = _clientFactory.CreateClient("InventoryService");
            var inventoryResponse = await inventoryClient.PostAsync("api/Inventory",
                new StringContent(request, Encoding.UTF8, "application/JSON"));
            if (!inventoryResponse.IsSuccessStatusCode)
            {
                throw new Exception(inventoryResponse.ReasonPhrase);
            }
            var inventoryId = await inventoryResponse.Content.ReadAsStringAsync();

            return (Int32.Parse(inventoryId), true);


           
        }
        catch (Exception e)
        {
            return (-1, false);
        }
        
    }

    public void DeleteInventory(int orderId)
    {
       
        var client = _clientFactory.CreateClient("InventoryService");
        client.DeleteAsync($"api/Inventory/{orderId}");
        Console.WriteLine($"Inventory Deleted : {orderId}");
    }
}