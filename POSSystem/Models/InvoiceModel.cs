using POSSystem.CustomValidationAttribute;
using System;

namespace POSSystem.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        //Custom Validation
        [Invoice]
        public string InvoiceNumber { get; set; }
        public int EmployeeId { get; set; }
        public DateTime SaleDate { get; set; }
        //Custom Validation
        [Invoice]
        public string ProductIds { get; set; }
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }
        public decimal InvoiceTotal { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
