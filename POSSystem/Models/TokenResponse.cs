using System;

namespace POSSystem.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
