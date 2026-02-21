using ECommerceManagement.Application.DTOs.CartItemDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface ICartItemService
    {
        Task<List<GetCartItemDTO>> GetAllAsync(int userId);
        Task AddToCartAsync(CreateCartItemDTO dto);
        Task UpdateAsync(UpdateCartItemDTO dto);
        Task RemoveAsync(int id);
    }
}
