using POSSystem.Common;
using POSSystem.Models;
using System.Threading.Tasks;

namespace POSSystem.Interfaces
{
    /// <summary>
    /// Used For : Interfaces Save of Invoice
    /// </summary>
    /// <param name="IInvoice"></param>
    /// <returns></returns>
    public interface IInvoice
    {
        Task<InvoiceModel> InvoiceSave(InvoiceModel Model, CommonClass.UserInfo userInfo);
    }
}
