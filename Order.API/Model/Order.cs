using System;
using System.Collections.Generic;

namespace Order.API.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }
        public Address Address { get; set; }
        public string FailMassage { get; set; }
    }

    public enum OrderStatus
    {
        Suspend,
        Success,
        Fail
    }
}
