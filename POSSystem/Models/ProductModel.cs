using POSSystem.CustomValidationAttribute;
using System;

namespace POSSystem.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //Custom Validation
        [Product]
        public string Category { get; set; }
        //Custom Validation
        [Product]
        public string ProductName { get; set; }
        //Custom Validation
        [Product]
        public string Description { get; set; }
        //Custom Validation
        [Product]
        public int Quantity { get; set; }
        //Custom Validation
        [Product]
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
