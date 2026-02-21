using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
