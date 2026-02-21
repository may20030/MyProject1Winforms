using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.OrderDTOs
{
    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
