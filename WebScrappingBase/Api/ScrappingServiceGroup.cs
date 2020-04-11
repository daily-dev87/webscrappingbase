using System.Collections.Generic;
using System.Threading.Tasks;
using WebScrappingBase.Models;
using WebScrappingBase.Service;
using System;
namespace WebScrappingBase.Api
{
    public class ScrappingServiceGroup
    {
        private Dictionary<int, ScrappingService> _ScrappingServices;
        private ScrappingAccountsConfig _config;

        private ScrappingServiceGroup(Dictionary<int, ScrappingService> ScrappingServices, ScrappingAccountsConfig config)
        {
            _config = config;
            _ScrappingServices = ScrappingServices;

        }
               
        public static async Task<ScrappingServiceGroup> Create(ScrappingAccountsConfig config)
        {
            var ScrappingServices = new Dictionary<int, ScrappingService>();
            foreach (var accountInfo in config.Accounts)
            {
                ScrappingServices.Add(accountInfo.AccountId, await ScrappingService.Create(accountInfo));
            }

            // Create a master service
            ScrappingServices.Add(0, await ScrappingService.CreateMaster());

            return new ScrappingServiceGroup(ScrappingServices, config);
        }

        public static async Task<ScrappingServiceGroup> Create(StorageUowProvider storageUowProvider, ScrappingAccountsConfig config)
        {
            var ScrappingServices = new Dictionary<int, ScrappingService>();
            List<AccountModel> allAccounts;
            
            using (var uow = storageUowProvider.CreateUow())
            {
                allAccounts = await uow.AccountService.GetProxyAccounts();// get all account with proxy
            }
            foreach (var accountModel in allAccounts)
            {
                var accountInfo = GetAccountInfoFromAccountModel(accountModel);
                ScrappingServices.Add(accountInfo.AccountId, await ScrappingService.Create(accountInfo));//open web browser and login by users
            }

            // Create a master service with the credentials from the config file
            ScrappingServices.Add(0, await ScrappingService.CreateMaster());//create empty ScrappingService
            
            return new ScrappingServiceGroup(ScrappingServices, config);
        }

        public static async Task<ScrappingServiceGroup> Create(StorageUowProvider storageUowProvider, ScrappingAccountsConfig config, AccountModel[] allAccounts, ScrappingServiceGroup ScrappingServiceGroup)
        {
            var ScrappingServices = ScrappingServiceGroup._ScrappingServices;
            foreach (var accountModel in allAccounts)
            {
                var accountInfo = new AccountInfo();
                accountInfo.ScrappingCredentials.Login = accountModel.email;
                accountInfo.ScrappingCredentials.Password = accountModel.password;
                accountInfo.AccountId = accountModel.accountId;
                accountInfo.Proxy = new ProxyData
                {
                    /*
                    IpAddress = accountModel.currentProxy.IpAddress,
                    Port = accountModel.currentProxy.Port,
                    UserName = accountModel.currentProxy.UserName,
                    Password = accountModel.currentProxy.Password
                    */
                };
                ScrappingServices.Add(accountInfo.AccountId, await ScrappingService.Create(accountInfo));//open web browser and login by users
            }

            // Create a master service with the credentials from the config file
            //ScrappingServices.Add(0, await ScrappingService.CreateMaster());//create empty ScrappingService
            return new ScrappingServiceGroup(ScrappingServices, config);
        }

        public ScrappingService GetService(int accountId)
        {
            return _ScrappingServices[accountId];
        }

        public ScrappingService GetMainService()
        {
            return _ScrappingServices[0];
        }

        private static AccountInfo GetAccountInfoFromAccountModel(AccountModel accountModel)
        {
            var accountInfo = new AccountInfo();
            accountInfo.ScrappingCredentials.Login = accountModel.email;
            accountInfo.ScrappingCredentials.Password = accountModel.password;
            accountInfo.AccountId = accountModel.accountId;
            accountInfo.Proxy = new ProxyData
            {
                /*
                IpAddress = accountModel.currentProxy.IpAddress,
                Port = accountModel.currentProxy.Port,
                UserName = accountModel.currentProxy.UserName,
                Password = accountModel.currentProxy.Password
                */
            };

            return accountInfo;
        }

        public async Task<bool> IsAccountLinkedWithProxyId(StorageUowProvider storageUowProvider, int proxyId)
        {
            var result = false;

            using (var uow = storageUowProvider.CreateUow())
            {
                result = await uow.AccountService.IsAccountByProxyId(proxyId);
            }

            return result;
        }

        public async Task GetLogStatus(StorageUowProvider storageUowProvider)
        {
            List<AccountModel> allAccounts;
            using (var uow = storageUowProvider.CreateUow())
            {
                allAccounts = await uow.AccountService.GetProxyAccounts();
            }

            foreach (var accountModel in allAccounts)
            {
                if (!accountModel.accountStatus.Contains("Proxy"))
                {
                    var ScrappingService = _ScrappingServices[accountModel.accountId];
                    await ScrappingService.GetLogStatus(storageUowProvider, accountModel.accountId);
                }
            }
        }

        public async Task GetAddAccountLogStatus(StorageUowProvider storageUowProvider, AccountModel[] accountList)
        {
            foreach (var accountModel in accountList)
            {
                var ScrappingService = _ScrappingServices[accountModel.accountId];
                await ScrappingService.GetLogStatus(storageUowProvider, accountModel.accountId);
            }

        }
    }
}
