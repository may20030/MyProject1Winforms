using ECommerceManagement.Application.DTOs.CategoryDTOs;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category, int> _repository;

        public CategoryService(IGenericRepository<Category, int> repository)
        {
            _repository = repository;
        }

        // Get all categories
        public async Task<List<GetCategoryDTO>> GetAllAsync()
        {
            var categories = await _repository.GetAll().ToListAsync();
            return categories.Adapt<List<GetCategoryDTO>>();
        }

        // Get category by Id
        public async Task<GetCategoryDTO> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category?.Adapt<GetCategoryDTO>();
        }

        // Create category
        public async Task CreateAsync(CreateCategoryDTO dto)
        {
            var category = dto.Adapt<Category>();
            await _repository.AddAsync(category);
        }

        // Update category
        public async Task UpdateAsync(UpdateCategoryDTO dto)
        {
            var category = await _repository.GetByIdAsync(dto.Id);
            if (category == null) return;

            category.Name = dto.Name;
            _repository.Update(category);
        }

        // Delete category
        public async Task DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return;

            _repository.Delete(category);
        }
    }
}
