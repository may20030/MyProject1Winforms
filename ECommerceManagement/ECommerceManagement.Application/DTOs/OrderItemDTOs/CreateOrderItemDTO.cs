using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.OrderItemDTOs
{
    public class CreateOrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
