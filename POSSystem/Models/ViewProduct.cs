using POSSystem.Common;
using System.Collections.Generic;

namespace POSSystem.Models
{
    public class ViewProductResponse
    {
        public List<ViewProductByCategory> ViewProductByCategoryList { get; set; } = new List<ViewProductByCategory>();
        public int start { get; set; }
        public int length { get; set; }
        public CommonClass.Search search { get; set; }
        public List<CommonClass.Column> columns { get; set; }
        public List<CommonClass.Order> order { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class ViewProductByCategory
    {
        public string Category { get; set; }
        public List<ViewProduct> ViewProductList { get; set; } = new List<ViewProduct>();
    }
    public class ViewProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
