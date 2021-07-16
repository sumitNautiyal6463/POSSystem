using POSSystem.Common;
using POSSystem.Models;
using System.Threading.Tasks;

namespace POSSystem.Interfaces
{
    /// <summary>
    /// Used For : Interfaces for List, Save, Delete, Get By Id of Product
    /// </summary>
    /// <param name="IProduct"></param>
    /// <returns></returns>
    public interface IProduct
    {
        Task<ViewProductResponse> ProductList(ViewProductResponse Model);
        Task<ProductModel> ProductSave(ProductModel Model, CommonClass.UserInfo userInfo);
        Task<ProductResponse> ProductDelete(ProductResponse Model, CommonClass.UserInfo userInfo);
        Task<ProductResponse> ProductGetById(ProductResponse Model);
    }
}
