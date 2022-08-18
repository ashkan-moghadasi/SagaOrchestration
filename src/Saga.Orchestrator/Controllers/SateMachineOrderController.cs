using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Proxies;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SateMachineOrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public SateMachineOrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
        }

        [HttpPost]
        public OrderResponse Post([FromBody] Order order)
        {
            var response=_orderManager.CreateOrder(order);
            return response;
        }
    }
}
