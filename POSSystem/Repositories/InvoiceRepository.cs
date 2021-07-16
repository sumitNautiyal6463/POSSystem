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
    public class InvoiceRepository : IInvoice
    {
        private readonly POSContext _context;
        public InvoiceRepository(POSContext context)
        {
            _context = context;
        }

        #region Invoiceave
        /// <summary>
        /// Used For : Save and Update Invoice Details
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<InvoiceModel> InvoiceSave(InvoiceModel Model, CommonClass.UserInfo userInfo)
        {
            try
            {
                await Task.Delay(1);
                var productList = Model.ProductIds.Split(",").ToList();
                var InvoiceDetail = _context.Invoice.Where(x => x.Status == true
                              && x.InvoiceId == Model.InvoiceId).FirstOrDefault();
                //IQuerable
                IQueryable<DAL.Product> ProductList = _context.Product.Where(x => x.Status == true
                                                      && productList.Contains(x.ProductId.ToString()));
                if (InvoiceDetail == null)
                {
                    DAL.Invoice Invoice = new DAL.Invoice();
                    Invoice.InvoiceNumber = Model.InvoiceNumber;
                    Invoice.VAT = Model.VAT;
                    Invoice.SaleDate = DateTime.UtcNow;
                    Invoice.ProductIds = Model.ProductIds;
                    Invoice.Discount = Model.Discount;
                    Invoice.EmployeeId = Convert.ToInt32(userInfo.UserId);
                    Invoice.InvoiceTotal = ProductList.Select(x => x.Quantity * x.Price).Sum() + Model.VAT - Model.Discount;
                    Invoice.Status = true;
                    //unboxing
                    Invoice.CreatedBy = Convert.ToInt32(userInfo.UserId);
                    Invoice.CreatedDate = DateTime.UtcNow;
                    await _context.AddAsync(Invoice);
                    await _context.SaveChangesAsync();

                    ProductList.ToList().ForEach(x =>
                    {
                        x.Quantity = x.Quantity - 1;
                    });
                    await _context.SaveChangesAsync();
                    Model.Success = true;
                    Model.Message = "Invoice saved successfully!!";
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
    }
}