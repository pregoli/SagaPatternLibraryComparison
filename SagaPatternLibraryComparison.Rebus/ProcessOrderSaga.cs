using Rebus.Handlers;
using Rebus.Sagas;
using Rebus.Bus;
using SagaPatternLibraryComparison.Domain.Orders.Commands;
using SagaPatternLibraryComparison.Domain.Orders.Events;
using System;
using System.Threading.Tasks;

namespace SagaPatternLibraryComparison.Rebus
{
    public class ProcessOrderSaga : Saga<ProcessOrderSagaData>,
      IAmInitiatedBy<OrderCreated>,
      IHandleMessages<OrderProcessed>
    {
        private readonly IBus _bus;

        public ProcessOrderSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<ProcessOrderSagaData> config)
        {
            config.Correlate<OrderCreated>(x => x.OrderId,  d => d.OrderId);
            config.Correlate<OrderProcessed>(x => x.OrderId,  d => d.OrderId);
        }

        public async Task Handle(OrderCreated @event)
        {
             if (!IsNew) return;

             Data.OrderReceived = true;
             Data.Username = @event.Username;
             Data.OrderId = @event.OrderId;

             await _bus.Publish(new ProcessOrder { 
                 OrderId = Data.OrderId,
                 Username = Data.Username
             });
        }

        public async Task Handle(OrderProcessed message)
        {
            Data.OrderProcessed = true;

            await Task.Delay(TimeSpan.FromMilliseconds(2000));
            MarkAsComplete();
        }
    }
}
