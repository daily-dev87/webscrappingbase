using System;
using System.Threading.Tasks;

namespace WebScrappingBase.Service
{
    public sealed class StorageUow : IDisposable
    {
        readonly StorageDbContext _ctx;

        public StorageUow(StorageDbContext ctx) => _ctx = ctx;

        public void Dispose() => _ctx.Dispose();

        public async Task ApplyChanges()
        {
            await _ctx.SaveChangesAsync();
        }

        
        public AccountService AccountService => new AccountService(_ctx);
        
        public ProxyService ProxyService => new ProxyService(_ctx);

        public ImagesService ImagesService => new ImagesService(_ctx);
    }
}
