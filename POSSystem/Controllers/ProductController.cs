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
    public class ProductController : ControllerBase
    {
        //Dependency Injection
        private readonly IProduct _Product;

        public ProductController(IProduct Product)
        {
            _Product = Product;
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

        #region Product
        /// <summary>
        /// Used For : Get Product List
        /// </summary>
        /// <param name="Model">start,length</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost, Route("ProductList")]
        public async Task<ActionResult> ProductList(ViewProductResponse Model)
        {
            try
            {
                var Result = await _Product.ProductList(Model);
                return Ok(new { Result, Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured!");
            }
        }

        /// <summary>
        /// Used For : Save Product Details
        /// </summary>
        /// <param name="Model">Category,ProductName,Description,Quantity,Price</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, Route("ProductSave")]
        public async Task<ActionResult> ProductSave(ProductModel Model)
        {
            try
            {
                var Result = await _Product.ProductSave(Model, UserDetail());
                return Ok(new { Result, Success = Result.Success });
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured!");
            }
        }

        /// <summary>
        /// Used For : Delete Product Details
        /// </summary>
        /// <param name="Model">ProductId</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, Route("ProductDelete")]
        public async Task<ActionResult> ProductDelete(ProductResponse Model)
        {
            try
            {
                var Result = await _Product.ProductDelete(Model, UserDetail());
                return Ok(new { Result, Success = Result.Success });
            }
            catch (Exception ex)
            {
                return BadRequest("An error has occured!");
            }
        }

        /// <summary>
        /// Used For : Edit Product Details
        /// </summary>
        /// <param name="Model">ProductId</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost, Route("ProductGetById")]
        public async Task<ActionResult> ProductGetById(ProductResponse Model)
        {
            try
            {
                var Result = await _Product.ProductGetById(Model);
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