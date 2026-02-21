using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.CartItemDTOs
{
    public class CreateCartItemDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
