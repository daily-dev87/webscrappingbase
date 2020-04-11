using System.ComponentModel.DataAnnotations;

namespace WebScrappingBase.Models
{
    public class AccountModel
    {
        public int accountId { get; set; }
        
        [Required]
        public string email { get; set; }
        
        [Required]
        public string password { get; set; }

        public int currentProxyId { get; set; }

        public string country { get; set; }
        
        public int accountTypeId { get; set; }

        public string accountStatus { get; set; }
    }
}
