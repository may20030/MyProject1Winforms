using ECommerceManagement.Application.DTOs.OrderDTOs;
using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<GetOrderDTO>> GetAllAsync();
        Task<GetOrderDTO> GetByIdAsync(int id);
        Task CreateAsync(CreateOrderDTO dto);
        Task UpdateStatusAsync(int orderId, OrderStatus status);
        Task DeleteAsync(int id);
    }
}
