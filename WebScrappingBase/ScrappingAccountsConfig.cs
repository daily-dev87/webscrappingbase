using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebScrappingBase
{
    public class ScrappingAccountsConfig
    {
        private const string ConfigFileName = "config.json";

        public AccountInfo[] Accounts { get; set; }

        public static async Task<ScrappingAccountsConfig> Read()
        {
            var json = await File.ReadAllTextAsync(ConfigFileName);

            return JsonConvert.DeserializeObject<ScrappingAccountsConfig>(json);
        }
    }
}
