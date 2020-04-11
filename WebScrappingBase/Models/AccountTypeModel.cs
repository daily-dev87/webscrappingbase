using System.ComponentModel.DataAnnotations;

namespace WebScrappingBase.Models
{
    public class AccountTypeModel
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
