using System;
using System.ComponentModel.DataAnnotations;

namespace POSSystem.DAL
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int EmployeeId { get; set; }
        public DateTime SaleDate { get; set; }
        public string ProductIds { get; set; }
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }
        public decimal InvoiceTotal { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
