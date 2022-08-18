using DataModel;

namespace Saga.Orchestrator.Proxies;

public interface IInventoryProxy
{
    Task<(int, bool)> UpdateInventory(Order order);
    void DeleteInventory(int orderId);
}