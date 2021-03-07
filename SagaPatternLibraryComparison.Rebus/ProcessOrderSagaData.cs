using Rebus.Sagas;
using System;

namespace SagaPatternLibraryComparison.Rebus
{
    public class ProcessOrderSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public string Username { get; set; }
        public Guid OrderId { get; set; }
        public bool OrderReceived { get; set; }
        public bool OrderProcessed { get; set; }

        public bool IsComplete => OrderReceived && OrderProcessed;
    }
}
