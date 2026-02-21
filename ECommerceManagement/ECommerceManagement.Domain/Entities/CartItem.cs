using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int Quantity { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }
}
