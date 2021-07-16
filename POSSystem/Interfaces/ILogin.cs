using POSSystem.Models;
using System.Threading.Tasks;

namespace POSSystem.Interfaces
{
    /// <summary>
    /// Used For : Interfaces for Generate Token
    /// </summary>
    /// <param name="ILogin"></param>
    /// <returns></returns>
    public interface ILogin
    {
        Task<TokenResponse> GenerateToken(TokenRequest Model, AppSettings _appSettings);
    }
}
