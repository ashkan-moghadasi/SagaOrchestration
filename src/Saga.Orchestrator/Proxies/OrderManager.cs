using DataModel;

namespace Saga.Orchestrator.Proxies
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderProxy _orderProxy;
        private readonly IInventoryProxy _inventoryProxy;
        private readonly INotificationProxy _notificationProxy;

        enum OrderState
        {
            NotStarted,
            OrderCreated,
            OrderCanceled,
            OrderCreateFailed,
            InventoryUpdated,
            InventoryUpdatedFailed,
            InventoryRolledBack,
            NotificationSent,
            NotificationSendFailed
        }

        enum OrderAction
        {
            CreateOrder,
            CancelOrder,
            UpdateInventory,
            RollbackInventory,
            SendNotification
        }

        public OrderManager(IOrderProxy orderProxy, IInventoryProxy inventoryProxy,
            INotificationProxy notificationProxy)
        {
            _orderProxy = orderProxy ?? throw new ArgumentNullException(nameof(orderProxy));
            _inventoryProxy = inventoryProxy ?? throw new ArgumentNullException(nameof(inventoryProxy));
            _notificationProxy = notificationProxy ?? throw new ArgumentNullException(nameof(notificationProxy));
        }

        public  OrderResponse CreateOrder(Order order)
        {
            var orderStateMachine = new Stateless.StateMachine<OrderState, OrderAction>(OrderState.NotStarted);
            int orderId = -1;
            bool success = false;
            orderStateMachine.Configure(OrderState.NotStarted)
                .PermitDynamic(OrderAction.CreateOrder, () =>
                {
                     (orderId, success) = _orderProxy.CreateOrder(order).Result;
                    return success ? OrderState.OrderCreated : OrderState.OrderCreateFailed;
                });
            
            orderStateMachine.Configure(OrderState.OrderCreated)
                .PermitDynamic(OrderAction.UpdateInventory, () =>
                {
                    var (inventoryId, success) = _inventoryProxy.UpdateInventory(order).Result;
                    return success ? OrderState.InventoryUpdated : OrderState.InventoryUpdatedFailed;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.UpdateInventory));

            orderStateMachine.Configure(OrderState.InventoryUpdated)
                .PermitDynamic(OrderAction.SendNotification, () =>
                {
                    var (notificationId, success) = _notificationProxy.SendNotification(order).Result;
                    return success ? OrderState.NotificationSent : OrderState.NotificationSendFailed;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.SendNotification));

            orderStateMachine.Configure(OrderState.InventoryUpdatedFailed)
                .PermitDynamic(OrderAction.RollbackInventory, () =>
                {
                     _inventoryProxy.DeleteInventory(orderId);
                     return OrderState.InventoryRolledBack;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.RollbackInventory));

            orderStateMachine.Configure(OrderState.InventoryRolledBack)
                .PermitDynamic(OrderAction.CancelOrder, () =>
                {
                    _orderProxy.DeleteOrder(orderId);
                    return OrderState.OrderCanceled;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.CancelOrder));

            orderStateMachine.Fire(OrderAction.CreateOrder);
            return new OrderResponse() { OrderId = orderId.ToString(), Success = orderStateMachine.State == OrderState.NotificationSent };
            
        }
    }
}