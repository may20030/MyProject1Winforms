using ECommerceManagement.Application.DTOs.ProductDTOs;
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
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product, int> _repository;

        public ProductService(IGenericRepository<Product, int> repository)
        {
            _repository = repository;
        }

        // Get all products
        public async Task<List<GetProductDTO>> GetAllAsync()
        {
            var products = await _repository.GetAll()
                .Include(p => p.Category)
                .ToListAsync();

            return products.Adapt<List<GetProductDTO>>();
        }

        // Get product by Id
        public async Task<GetProductDTO> GetByIdAsync(int id)
        {
            var product = await _repository.GetAll()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product?.Adapt<GetProductDTO>();
        }

        // Create product
        public async Task CreateAsync(CreateProductDTO dto)
        {
            var product = dto.Adapt<Product>();
            await _repository.AddAsync(product);
        }

        // Update product
        public async Task UpdateAsync(UpdateProductDTO dto)
        {
            var product = await _repository.GetByIdAsync(dto.Id);
            if (product == null) return;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.ImageUrl = dto.ImageUrl;

            _repository.Update(product);
        }

        // Delete product
        public async Task DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return;

            _repository.Delete(product);
        }
    }
}
