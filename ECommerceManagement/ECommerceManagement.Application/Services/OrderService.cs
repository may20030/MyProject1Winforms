using ECommerceManagement.Application.DTOs.OrderDTOs;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using ECommerceManagement.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order, int> _repository;
        private readonly IGenericRepository<OrderItem, int> _orderItemRepository;

        public OrderService(
            IGenericRepository<Order, int> repository,
            IGenericRepository<OrderItem, int> orderItemRepository)
        {
            _repository = repository;
            _orderItemRepository = orderItemRepository;
        }

        // Get all orders
        public async Task<List<GetOrderDTO>> GetAllAsync()
        {
            var orders = await _repository.GetAll()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Adapt<List<GetOrderDTO>>();
        }

        // Get order by Id
        public async Task<GetOrderDTO> GetByIdAsync(int id)
        {
            var order = await _repository.GetAll()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order?.Adapt<GetOrderDTO>();
        }

        // Create order with items
        public async Task CreateAsync(CreateOrderDTO dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                Status = OrderStatus.Processing,
                OrderDate = DateTime.UtcNow
            };

            await _repository.AddAsync(order);

            // Add items
            foreach (var itemDto in dto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = (await _repository.GetAll()
                                 .OfType<Product>()
                                 .FirstOrDefaultAsync(p => p.Id == itemDto.ProductId))?.Price ?? 0
                };

                await _orderItemRepository.AddAsync(orderItem);
            }
        }

        // Update order status
        public async Task UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return;

            order.Status = status;
            _repository.Update(order);
        }

        // Delete order
        public async Task DeleteAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return;

            _repository.Delete(order);
        }
    }
}
