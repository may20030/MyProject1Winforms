using ECommerceManagement.Application.DTOs.CartItemDTOs;
using ECommerceManagement.Application.DTOs.CategoryDTOs;
using ECommerceManagement.Application.DTOs.OrderDTOs;
using ECommerceManagement.Application.DTOs.OrderItemDTOs;
using ECommerceManagement.Application.DTOs.ProductDTOs;
using ECommerceManagement.Application.DTOs.UserDTOs;
using ECommerceManagement.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Mappers
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // User
            TypeAdapterConfig<CreateUserDTO, User>.NewConfig();
            TypeAdapterConfig<User, UserDTO>.NewConfig();

            // Product
            TypeAdapterConfig<CreateProductDTO, Product>.NewConfig();
            TypeAdapterConfig<UpdateProductDTO, Product>.NewConfig();
            TypeAdapterConfig<Product, GetProductDTO>
                .NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category.Name);

            // Category
            TypeAdapterConfig<CreateCategoryDTO, Category>.NewConfig();
            TypeAdapterConfig<UpdateCategoryDTO, Category>.NewConfig();
            TypeAdapterConfig<Category, GetCategoryDTO>.NewConfig();

            // Order
            TypeAdapterConfig<CreateOrderDTO, Order>.NewConfig();
            TypeAdapterConfig<UpdateOrderDTO, Order>.NewConfig();
            TypeAdapterConfig<Order, GetOrderDTO>.NewConfig();

            TypeAdapterConfig<CreateOrderItemDTO, OrderItem>.NewConfig();
            TypeAdapterConfig<OrderItem, GetOrderItemDTO>
                .NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.Name);

            // CartItem
            TypeAdapterConfig<CreateCartItemDTO, CartItem>.NewConfig();
            TypeAdapterConfig<UpdateCartItemDTO, CartItem>.NewConfig();
            TypeAdapterConfig<CartItem, GetCartItemDTO>
                .NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.Name)
                .Map(dest => dest.Price, src => src.Product.Price);
        }
    }
}
