using DataModel;

namespace Saga.Orchestrator.Proxies;

public interface IOrderProxy
{
    Task<(int, bool)> CreateOrder(Order order);
    void DeleteOrder(int orderId);
}