using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Domain.Entities
{
    public class User: BaseEntity
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public UserRole Role { get; set; }

        // Navigation
        public List<Order> Orders { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
    }
}
