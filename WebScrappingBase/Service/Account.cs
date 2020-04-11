using WebScrappingBase.Models;
namespace WebScrappingBase.Service
{
    public sealed class Account
    {
        public int accountId { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public int currentProxyId { get; set; }

        public string country { get; set; }

        public int accountTypeId { get; set; }

        public string accountStatus { get; set; }
    }
}
