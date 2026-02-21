using ECommerceManagement.Application.DTOs.CartItemDTOs;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IGenericRepository<CartItem, int> _repository;

        public CartItemService(IGenericRepository<CartItem, int> repository)
        {
            _repository = repository;
        }

        // Get all cart items for a user
        public async Task<List<GetCartItemDTO>> GetAllAsync(int userId)
        {
            var items = await _repository.GetAll()
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return items.Adapt<List<GetCartItemDTO>>();
        }

        // Add item to cart
        public async Task AddToCartAsync(CreateCartItemDTO dto)
        {
            var existing = await _repository.GetAll()
                .FirstOrDefaultAsync(ci => ci.UserId == dto.UserId && ci.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                _repository.Update(existing);
            }
            else
            {
                var cartItem = dto.Adapt<CartItem>();
                await _repository.AddAsync(cartItem);
            }
        }

        // Update cart item (quantity)
        public async Task UpdateAsync(UpdateCartItemDTO dto)
        {
            var cartItem = await _repository.GetByIdAsync(dto.Id);
            if (cartItem == null) return;

            cartItem.Quantity = dto.Quantity;
            _repository.Update(cartItem);
        }

        // Remove item from cart
        public async Task RemoveAsync(int id)
        {
            var cartItem = await _repository.GetByIdAsync(id);
            if (cartItem == null) return;

            _repository.Delete(cartItem);
        }
    }
}
