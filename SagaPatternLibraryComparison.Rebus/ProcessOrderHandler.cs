using Rebus.Bus;
using Rebus.Handlers;
using SagaPatternLibraryComparison.Domain.Orders.Commands;
using SagaPatternLibraryComparison.Domain.Orders.Events;
using System.Threading.Tasks;

namespace SagaPatternLibraryComparison.Rebus
{
    public class ProcessOrderHandler : IHandleMessages<ProcessOrder>
    {
        private readonly IBus _bus;

        public ProcessOrderHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ProcessOrder message)
        {
            await Task.Delay(500);
            await _bus.Reply(new OrderProcessed { OrderId = message.OrderId, Username = message.Username });
        }
    }
}
