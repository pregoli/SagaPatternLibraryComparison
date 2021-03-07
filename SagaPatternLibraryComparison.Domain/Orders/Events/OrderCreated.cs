using System;
using System.Collections.Generic;
using System.Text;

namespace SagaPatternLibraryComparison.Domain.Orders.Events
{
    public class OrderCreated
    {
        public OrderCreated(Guid orderId, string username)
        {
            OrderId = orderId;
            Username = username;
        }

        public Guid OrderId { get; set; }
        public string Username { get; set; }
    }
}
