using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSSystem.Common;
using POSSystem.Interfaces;
using POSSystem.Models;

namespace POSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        //Dependency Injection
        private readonly IInvoice _Invoice;

        public InvoiceController(IInvoice Invoice)
        {
            _Invoice = Invoice;
        }

        #region when user login it find out the information from token
        public CommonClass.UserInfo UserDetail()
        {
            CommonClass.UserInfo userInfo = new CommonClass.UserInfo();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            userInfo.UserId = claim[0].Value;
            userInfo.UserName = claim[1].Value;
            userInfo.UserRole = claim[2].Value;
            return userInfo;
        }
        #endregion

        #region Invoice
        /// <summary>
        /// Used For : Save Invoice Details
        /// </summary>
        /// <param name="Model">InvoiceNumber,ProductIds,VAT,Discount</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, Route("InvoiceSave")]
        public async Task<ActionResult> InvoiceSave(InvoiceModel Model)
        {
            try
            {
                var Result = await _Invoice.InvoiceSave(Model, UserDetail());
                return Ok(new { Result, Success = Result.Success });
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured!");
            }
        }
        #endregion
    }
}