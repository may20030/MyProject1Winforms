using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }

        // Audit Fields
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        // Soft Delete
        public bool IsDeleted { get; set; } = false;
    }
}
