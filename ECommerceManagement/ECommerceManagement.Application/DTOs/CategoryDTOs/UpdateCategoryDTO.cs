using ECommerceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
