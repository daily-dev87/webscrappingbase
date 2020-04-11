using System;
using System.Collections.Generic;
using System.Text;

namespace WebScrappingBase.Models
{
    public class IpInfo
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
    }
}
