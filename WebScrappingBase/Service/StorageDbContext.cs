using Microsoft.EntityFrameworkCore;

namespace WebScrappingBase.Service
{
    public sealed class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Account>()
           .HasAlternateKey(a => new { a.email });

            modelBuilder.Entity<AccountType>().HasData(new AccountType { id = 1, type = "Premium" },
                new AccountType { id = 2, type = "Family Member" },
                new AccountType { id = 3, type = "Family Owner" },
                new AccountType { id = 4, type = "Free" });

            modelBuilder.Entity<Proxy>().HasData(new Proxy { Id = 1, IpAddress = "p.webshare.io", Port = 20001 });

            modelBuilder.Entity<Account>().HasData(new Account
            {
                accountId = 1,
                email = "chh@hjorth-hansen.dk",
                country = "Germany",
                password = "casperhjorth3005",
                //currentProxy = ;
                accountTypeId = 1,
                accountStatus = "use"
            }, new Account
            {
                accountId = 2,
                email = "athenaeydis@gmail.com",
                country = "Germany",
                password = "grimmurkisi",
                accountTypeId = 4,
                accountStatus = "not"
            }
            );
        }

        public DbSet<ImagesInfo> ImagesInfos { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountType> AccountTypes { get; set; }

        public DbSet<Proxy> Proxies { get; set; }

    }
}
