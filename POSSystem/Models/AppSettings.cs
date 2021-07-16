namespace POSSystem.Models
{
    public class AppSettings
    {
        //properties for jwt token signature
        public string Site { get; set; }
        public string Secret { get; set; }
        public string ExpireTime { get; set; }
        public string Audience { get; set; }
    }
}
