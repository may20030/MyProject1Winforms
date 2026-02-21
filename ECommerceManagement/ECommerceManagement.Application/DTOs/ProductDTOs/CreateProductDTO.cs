using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }

}
