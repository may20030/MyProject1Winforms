using ECommerceManagement.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<GetCategoryDTO>> GetAllAsync();
        Task<GetCategoryDTO> GetByIdAsync(int id);
        Task CreateAsync(CreateCategoryDTO dto);
        Task UpdateAsync(UpdateCategoryDTO dto);
        Task DeleteAsync(int id);
    }
}
