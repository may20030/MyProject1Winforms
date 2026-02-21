using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
    }
}
