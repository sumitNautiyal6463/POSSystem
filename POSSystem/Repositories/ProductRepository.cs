using POSSystem.Common;
using POSSystem.Interfaces;
using POSSystem.Models;
using POSSystem.POSDBContext;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
namespace POSSystem.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly POSContext _context;
        public ProductRepository(POSContext context)
        {
            _context = context;
        }

        #region ProductList
        /// <summary>
        /// Used For : To Get the Product List
        /// </summary>
        /// <param name="Product">start,length and search</param>
        /// <returns></returns>
        public async Task<ViewProductResponse> ProductList(ViewProductResponse Model)
        {
            ViewProductResponse viewProductResponse = new ViewProductResponse();
            try
            {
                await Task.Delay(1);
                var Search = Model.search != null ? Model.search.value : "";
                string sortBy = "";
                string sortDir = "";
                var ProductList = _context.Product.Where(x => x.Status == true).ToList();
                viewProductResponse.ViewProductByCategoryList = ProductList.Where(x =>
                                    ((!string.IsNullOrEmpty(Search)
                                    && x.ProductName.Trim().ToLower().Contains(Search.Trim().ToLower()))
                                    || string.IsNullOrEmpty(Search)))
                                    .GroupBy(y => y.Category)
                                    .Select(x => new ViewProductByCategory
                                    {
                                        Category = x.First().Category,
                                        ViewProductList = x.Select(y => new ViewProduct
                                        {
                                            ProductId = y.ProductId,
                                            ProductName = y.ProductName,
                                            Description = y.Description,
                                            Price = y.Price
                                        }).ToList()
                                    }).OrderBy(x => x.Category)
                                  .Skip(Model.start).Take(Model.length).ToList();
                if (Model.order != null)
                {
                    sortBy = Model.columns[Model.order[0].column].data;
                    sortDir = Model.order[0].dir.ToLower();
                    viewProductResponse.ViewProductByCategoryList = viewProductResponse.ViewProductByCategoryList
                                            .AsQueryable().OrderBy(sortBy + " " + sortDir).ToList();
                }
                viewProductResponse.recordsTotal = _context.Product.Where(x => x.Status == true).Count();
                viewProductResponse.recordsFiltered = _context.Product.Where(x => x.Status == true
                                        && ((!string.IsNullOrEmpty(Search)
                                        && (x.ProductName.Trim().ToLower().Contains(Search.Trim().ToLower())))
                                        || string.IsNullOrEmpty(Search))).Count();
                viewProductResponse.Success = true;
            }
            catch (Exception ex)
            {
                viewProductResponse.Success = false;
                viewProductResponse.Message = "No Record is Found";
            }
            // Return the ProductList
            return viewProductResponse;
        }
        #endregion

        #region Productave
        /// <summary>
        /// Used For : Save and Update Product Details
        /// </summary>
        /// <param name="Model">ProductId,Name,Description,Price,Quantity</param>
        /// <returns></returns>
        public async Task<ProductModel> ProductSave(ProductModel Model, CommonClass.UserInfo userInfo)
        {
            try
            {
                await Task.Delay(1);
                if (CheckDuplicate(Model, userInfo))
                {
                    var Product = _context.Product.Where(x => x.Status == true
                                  && x.ProductId == Model.ProductId).FirstOrDefault();
                    if (Product == null)
                    {
                        DAL.Product product = new DAL.Product();
                        product.ProductName = Model.ProductName;
                        product.Category = Model.Category;
                        product.Price = Model.Price;
                        product.Quantity = Model.Quantity;
                        product.Description = Model.Description;
                        product.Status = true;
                        product.CreatedBy = Convert.ToInt32(userInfo.UserId);
                        product.CreatedDate = DateTime.UtcNow;
                        await _context.AddAsync(product);
                        await _context.SaveChangesAsync();
                        Model.Success = true;
                        Model.Message = "Product saved successfully!!";
                    }
                    else
                    {
                        Product.ProductName = Model.ProductName;
                        Product.Category = Model.Category;
                        Product.Price = Model.Price;
                        Product.Quantity = Model.Quantity;
                        Product.Description = Model.Description;
                        Product.Status = true;
                        Product.UpdatedBy = Convert.ToInt32(userInfo.UserId);
                        Product.UpdatedDate = DateTime.Now;
                        _context.Product.Update(Product);
                        await _context.SaveChangesAsync();
                        Model.Success = true;
                        Model.Message = "Product updated successfully!!";
                    }
                }
            }
            catch (Exception ex)
            {
                Model.Success = false;
                Model.Message = "Invalid Record";
            }
            return Model;
        }
        #endregion

        #region ProductDelete
        /// <summary>
        /// Used For : Delete Product Details.
        /// </summary>
        /// <param name="Model">ProductId</param>
        /// <returns></returns>
        public async Task<ProductResponse> ProductDelete(ProductResponse Model, CommonClass.UserInfo userInfo)
        {
            try
            {
                await Task.Delay(1);
                var Product = _context.Product.Where(x => x.Status == true
                              && x.ProductId == Model.ProductId).FirstOrDefault();
                if (Product != null)
                {
                    Product.Status = false;
                    Product.UpdatedBy = Convert.ToInt32(userInfo.UserId);
                    Product.UpdatedDate = DateTime.UtcNow;
                    _context.Product.Update(Product);
                    await _context.SaveChangesAsync();
                    Model.Success = true;
                    Model.Message = "Product deleted successfully!!";
                }
                else
                {
                    Model.Success = false;
                    Model.Message = "Invalid Record!!";
                }
            }
            catch (Exception ex)
            {
                Model.Success = false;
                Model.Message = "Invalid Record!!";
            }
            return Model;
        }
        #endregion

        #region  ProductGetById
        /// <summary>
        /// Used For : To get the details for specific Product against the the Product ID
        /// </summary>
        /// <param name="Model">ProductId</param>
        /// <returns></returns>
        public async Task<ProductResponse> ProductGetById(ProductResponse Model)
        {
            ProductResponse Product = new ProductResponse();
            try
            {
                await Task.Delay(1);
                if (!string.IsNullOrEmpty(Model.ProductId.ToString()))
                {
                    Product = _context.Product.Where(x => x.Status == true
                                  && x.ProductId == Model.ProductId)
                                  .Select(x => new ProductResponse
                                  {
                                      ProductId = x.ProductId,
                                      ProductName = x.ProductName,
                                      Category = x.Category,
                                      Description = x.Description,
                                      Price = x.Price,
                                      Quantity = x.Quantity
                                  }).FirstOrDefault();
                    Product.Success = true;
                }
                else
                {
                    Product.Success = false;
                    Product.Message = "Invalid Record!!";
                }
            }
            catch (Exception ex)
            {
                Product.Success = false;
                Product.Message = "Invalid Record!!";
            }
            return Product;
        }
        #endregion

        #region CheckDuplicate
        /// <summary>
        /// Used For : Check Product Name Already Exist or Not
        /// </summary>
        /// <param name="Model">Name</param>
        /// <returns></returns>
        private bool CheckDuplicate(ProductModel Model, CommonClass.UserInfo userInfo)
        {
            var Product = _context.Product.Where(x => x.Category.Trim().ToLower() == Model.Category.Trim().ToLower()
                          && x.ProductName.Trim().ToLower() == Model.ProductName.Trim().ToLower()
                          && x.ProductId != Model.ProductId).FirstOrDefault();
            if (Product != null)
            {
                var UpdateDeleteItem = _context.Product
                                       .Where(x => x.Category.Trim().ToLower() == Model.Category.Trim().ToLower()
                                       && x.ProductName.Trim().ToLower() == Model.ProductName.Trim().ToLower()
                                       && x.Status == false).FirstOrDefault();
                if (UpdateDeleteItem != null)
                {
                    Product.Status = true;
                    Product.UpdatedBy = Convert.ToInt32(userInfo.UserId);
                    _context.Update(Product);
                    _context.SaveChanges();
                    Model.Success = true;
                    Model.Message = "Product saved successfully!!";
                    return false;
                }
                else
                {
                    Model.Success = false;
                    Model.Message = "Product Name already exist!!";
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}