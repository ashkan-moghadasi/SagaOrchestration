using DataModel;

namespace Saga.Orchestrator.Proxies;

public interface IOrderManager
{
    OrderResponse CreateOrder(Order order);
}