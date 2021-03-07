using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using SagaPatternLibraryComparison.Domain.Orders.Events;
using System;
using System.Threading.Tasks;

namespace SagaPatternLibraryComparison.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBus _bus;

        public OrdersController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost("beta/[controller]")]
        public async Task<IActionResult> NewOrder(string username)
        {
            await _bus.Publish(new OrderCreated(Guid.NewGuid(), username));
            return Ok();
        }
    }
}
