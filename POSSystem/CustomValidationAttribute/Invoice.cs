using System.ComponentModel.DataAnnotations;

namespace POSSystem.CustomValidationAttribute
{
    public class Invoice : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.DisplayName == "InvoiceNumber")
            {
                string name = (string)value;
                if (string.IsNullOrEmpty(name))
                    return new ValidationResult("Invoice Number is required!");
            }
            else if (validationContext.DisplayName == "ProductIds")
            {
                string description = (string)value;
                if (string.IsNullOrEmpty(description))
                    return new ValidationResult("Product is required!");
            }
            return ValidationResult.Success;
        }
    }
}
