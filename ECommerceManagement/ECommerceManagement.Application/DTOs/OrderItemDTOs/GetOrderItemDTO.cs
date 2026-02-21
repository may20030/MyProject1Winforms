using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.OrderItemDTOs
{
    public class GetOrderItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
