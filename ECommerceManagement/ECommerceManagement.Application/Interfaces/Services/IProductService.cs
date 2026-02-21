using ECommerceManagement.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<GetProductDTO>> GetAllAsync();
        Task<GetProductDTO> GetByIdAsync(int id);
        Task CreateAsync(CreateProductDTO dto);
        Task UpdateAsync(UpdateProductDTO dto);
        Task DeleteAsync(int id);
    }
}
