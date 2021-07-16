using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using POSSystem.Interfaces;
using POSSystem.POSDBContext;
using POSSystem.Models;
using POSSystem.DAL;

namespace POSSystem.Repositories
{
    public class LoginRepository : ILogin
    {
        private readonly POSContext _context;
        public LoginRepository(POSContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Description: Method to create new token
        /// </summary>
        /// <param name="Model">EmailId,Password</param>
        /// <returns>Token</returns>
        public async Task<TokenResponse> GenerateToken(TokenRequest Model, AppSettings _appSettings)
        {
            TokenResponse accessToken = new TokenResponse();
            try
            {
                await Task.Delay(1);
                var UserDetail = _context.Employee.Where(x => x.EmailId == Model.EmailId
                                 && x.Password == Model.Password).FirstOrDefault();
                if (UserDetail != null)
                {
                    accessToken = CreateAccessToken(UserDetail, _appSettings);
                    accessToken.Success = true;
                    accessToken.Message = "Login Successfully";
                }
                else
                {
                    accessToken.Success = false;
                    accessToken.Message = "Invalid Credential";
                }
            }
            catch (Exception ex)
            {
                accessToken.Success = false;
                accessToken.Message = "Invalid Credential";
            }
            return accessToken;
        }

        /// <summary>
        /// Description: Create Access Token
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        private TokenResponse CreateAccessToken(Employee User, AppSettings _appSettings)
        {
            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", User.EmployeeId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, User.EmployeeName),
                    new Claim(ClaimTypes.Role, User.RoleType),
                    new Claim(JwtRegisteredClaimNames.Email, User.EmailId.ToString()),
                    new Claim("LoggedOn", DateTime.UtcNow.ToString()),
                }),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Site,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddDays(tokenExpiryTime)
            };
            // generate token
            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);
            return new TokenResponse()
            {
                Token = encodedToken,
                Expiration = newtoken.ValidTo,
                UserName = User.EmployeeName
            };
        }
    }
}
