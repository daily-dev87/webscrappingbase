using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;
using WebScrappingBase.Models;
using WebScrappingBase.Service;
using WebScrappingBase.PuppeteerPrelude;
using WebScrappingBase.Shared;
using WebScrappingBase.ScrappingInteraction;

namespace WebScrappingBase.Api
{
    public class ScrappingService
    {
        private readonly Page _page;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isPlaylistPlaying;
        private int _accountId;

        private ScrappingService(Page page, int accountId)
        {
            _accountId = accountId;
            _page = page;
        }

        private ScrappingService()
        {
        }

        public static async Task<ScrappingService> Create(AccountInfo config)
        {
            // todo proxy not working , will remove it after getting proper proxy
            // config.Proxy.IpAddress = null;
            var browser = await BrowserProvider.PrepareBrowser(proxy: config.Proxy.IpAddress, port: config.Proxy.Port);
            var pages = await browser.PagesAsync();
            var page = pages.Single();
            string logstatus = "";
            Credentials proxyCredentails = new Credentials
            {
                Username = config.Proxy.UserName,
                Password = config.Proxy.Password
            };
            await page.AuthenticateAsync(proxyCredentails);
            logstatus = await SingIn(page, config.ScrappingCredentials);
            var storageUowProvider = StorageUowProvider.Init();
            using (var uow = storageUowProvider.CreateUow())
            {
                await uow.AccountService.SaveLogStatus(config.AccountId, logstatus);
            }
            return new ScrappingService(page, config.AccountId);
        }

        public static async Task<ScrappingService> CreateMaster()
        {
            return new ScrappingService();
        }

        private static async Task<string> SingIn(Page page, ScrappingCredentials ScrappingCredentials)
        {
            string logStatus = await ScrappingLoginPage.OpenMainPage(page);
            if (logStatus.Equals("Added"))
            {
                await ScrappingLoginPage.ClickSignIn(page);
                bool loginStatus = await ScrappingLoginPage.SignIn(page, ScrappingCredentials.Login, ScrappingCredentials.Password);
                logStatus = "LoggedIn";
                if (!loginStatus)
                {
                    logStatus = "";
                }
            }
            return logStatus;
            //SaveLoginStatus(int accountId, string logStatus);
        }

        public async Task GetLogStatus(StorageUowProvider storageUowProvider, int accountId)
        {
            Console.WriteLine(AppConstants.homeUrl);
            /*
            var logStatus = "Credential";
            try
            {
                var url = _page.Url;
                //var x = await _page.QuerySelectorAsync("button#login-button");
                if (url.IndexOf(AppConstants.homeUrl) >= 0)
                {
                    logStatus = "LoggedIn";
                    var y = await _page.QuerySelectorAsync("div.ConnectBar");
                    if (y != null)
                    {
                        logStatus = "AlreadyInUse";
                    }
                }
                else
                {
                    logStatus = "Credential";
                    var y = await _page.QuerySelectorAllAsync("p.alert");
                    //var y = await _page.QuerySelectorAsync("p.alert");
                    if (y.Length == 0)
                    {
                        logStatus = "Connecting...";
                    }
                    else
                    {
                        var cont = await y[0].EvaluateFunctionAsync<string>("co = > co.innerHTML");
                        if (cont.Contains("Incorrect username or password."))
                        {
                            logStatus = "ValidUserInfo";
                        }
                        else
                        {
                            logStatus = "OopsError";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logStatus = "Credental";
            }

            using (var uow = storageUowProvider.CreateUow())
            {
                await uow.AccountService.SaveLogStatus(accountId, logStatus);
            }
            */
            return;
        }

    }
}
