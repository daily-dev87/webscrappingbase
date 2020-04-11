using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebScrappingBase.Api;
using WebScrappingBase.Service;


namespace WebScrappingBase
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var config = await ScrappingAccountsConfig.Read();
            var storageUowProvider = StorageUowProvider.Init();
            // var spotifyServiceGroup = await SpotifyServiceGroup.Create(config);
            var scrappingServiceGroup = await ScrappingServiceGroup.Create(storageUowProvider, config);

            var builder = CreateWebHostBuilder(args, services => services
                .AddSingleton(scrappingServiceGroup)
                .AddSingleton(storageUowProvider));

            await scrappingServiceGroup.GetLogStatus(storageUowProvider);
            builder.Build().Run();
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, Action<IServiceCollection> servicesConfigurator)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
               .ConfigureServices(servicesConfigurator)
               .UseKestrel()
               .UseUrls("http://*:5005")
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseIISIntegration()
               .UseStartup<AspNetCoreStartup>();

            return builder;
        }       

    }
}
