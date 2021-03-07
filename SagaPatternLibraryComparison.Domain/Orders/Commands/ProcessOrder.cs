using System;
using System.Collections.Generic;
using System.Text;

namespace SagaPatternLibraryComparison.Domain.Orders.Commands
{
    public class ProcessOrder
    {
        public Guid OrderId { get; set; }
        public string Username { get; set; }
    }
}
