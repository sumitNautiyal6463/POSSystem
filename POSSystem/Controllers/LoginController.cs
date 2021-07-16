using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POSSystem.Interfaces;
using POSSystem.Models;

namespace POSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //Dependency Injection
        private readonly AppSettings _appSettings;
        private readonly ILogin _login;

        public LoginController(IOptions<AppSettings> appSettings, ILogin login)
        {
            _appSettings = appSettings.Value;
            _login = login;
        }

        #region Login
        /// <summary>
        /// Description: Login to create Token
        /// </summary>
        /// <param name="Model">EmailId,Password</param>
        /// <returns>Token</returns>
        [HttpPost, Route("Token")]
        public async Task<ActionResult> Tokens(TokenRequest Model)
        {
            if (Model == null)
                return new StatusCodeResult(500);
            return await GenerateNewToken(Model, _appSettings);
        }

        /// <summary>
        /// Description: Method to create new jwt token
        /// </summary>
        /// <param name="Model">EmailId,Password</param>
        /// <returns>Token</returns>
        private async Task<ActionResult> GenerateNewToken(TokenRequest Model, AppSettings _appSettings)
        {
            var authToken = await _login.GenerateToken(Model, _appSettings);
            return Ok(new { Result = authToken, Success = authToken.Success });
        }
        #endregion
    }
}