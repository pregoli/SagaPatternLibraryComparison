using System;
using System.Collections.Generic;
using System.Text;

namespace SagaPatternLibraryComparison.Domain.Orders.Events
{
    public class OrderProcessed
    {
        public Guid OrderId { get; set; }
        public string Username { get; set; }
    }
}
