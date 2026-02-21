using ECommerceManagement.Application.DTOs.OrderItemDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; }
    }
}
