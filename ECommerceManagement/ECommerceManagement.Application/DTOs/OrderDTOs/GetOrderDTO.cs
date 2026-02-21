using ECommerceManagement.Application.DTOs.OrderItemDTOs;
using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.OrderDTOs
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<GetOrderItemDTO> Items { get; set; }
    }
}
