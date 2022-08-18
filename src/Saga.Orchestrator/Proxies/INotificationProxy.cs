using DataModel;

namespace Saga.Orchestrator.Proxies;

public interface INotificationProxy
{
    Task<(int, bool)> SendNotification(Order order);
}