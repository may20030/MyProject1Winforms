using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.ProductDTOs
{
    public class GetProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
