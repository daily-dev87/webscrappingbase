namespace WebScrappingBase
{
    public class AccountInfo
    {
        public AccountInfo()
        {
            ScrappingCredentials = new ScrappingCredentials();
        }

        public ScrappingCredentials ScrappingCredentials { get; set; }

        public ProxyData Proxy { get; set; }

        public int AccountId { get; set; }

    }
    public class ProxyData
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}