using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebScrappingBase.Models;

namespace WebScrappingBase.Service
{
    public class AccountService
    {
        readonly StorageDbContext _db;

        public AccountService(StorageDbContext db) => _db = db;

        public async Task AddAccount(AccountModel accountModel)
        {
            var account = new Account()
            {
                accountId = accountModel.accountId,
                email = accountModel.email,
                password = accountModel.password,
                currentProxyId = accountModel.currentProxyId
            };

            await _db.Accounts.AddAsync(account);
        }

        public async Task AddAccount(Account accountEntity)
        {
            Account existAccount = _db.Accounts.FirstOrDefault(a => a.email == accountEntity.email);
            if (existAccount == null)
                await _db.Accounts.AddAsync(accountEntity);

        }

        public async Task UpdateAccount(AccountModel accountModel)
        {
            var account = await _db.Accounts.Where(a => a.accountId == accountModel.accountId).SingleAsync();
            if (account != null)
            {
                account.email = accountModel.email;
                account.password = accountModel.password;
                account.country = accountModel.country;
                account.accountTypeId = accountModel.accountTypeId;
                account.currentProxyId = accountModel.currentProxyId;
                account.accountStatus = accountModel.accountStatus;

                _db.Update(account);
                await _db.SaveChangesAsync();

            }
            //}            
        }

        public async Task UpdateAccountPlaying(int accountId, string status)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.accountId == accountId);
            if (account != null)
            {
                account.accountStatus = status;
            }
            _db.SaveChanges();
        }

        public async Task<List<AccountModel>> RemoveAccount(int accountId)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.accountId == accountId);
            var proxyId = account.currentProxyId;
            if (account != null)
            {
                _db.Accounts.Remove(account);
                var proxy = await _db.Proxies.FirstOrDefaultAsync(p => p.Id == proxyId);
                if (proxy != null) _db.Proxies.Remove(proxy);
            }
            _db.SaveChangesAsync();
            var accounts = await (from a in _db.Accounts
                                  join at in _db.AccountTypes on a.accountTypeId equals at.id
                                  join p in _db.Proxies on a.currentProxyId equals p.Id
                                  
                                  select new AccountModel
                                  {
                                      currentProxyId = p.Id,
                                      email = a.email,
                                      password = a.password,
                                      country = a.country,
                                      accountId = a.accountId,
                                      accountTypeId = at.id,    
                                      accountStatus = a.accountStatus
                                  })
                                  // .Take(2)
                                  .ToListAsync();
            return accounts;

        }


        public string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = "";
            }
            //GetCountryAbbreviation(ipInfo.Country);
            return ipInfo.Country;
        }

        public async Task<List<AccountModel>> GetProxyAccounts()
        {
            var accounts = await (from a in _db.Accounts
                                  join at in _db.AccountTypes on a.accountTypeId equals at.id
                                  join p in _db.Proxies on a.currentProxyId equals p.Id
                                  select new AccountModel
                                  {
                                      currentProxyId = p.Id,
                                      email = a.email,
                                      password = a.password,
                                      country = a.country,
                                      accountId = a.accountId,
                                      accountTypeId = at.id,
                                      accountStatus = a.accountStatus
                                  })
                                  // .Take(2)
                                  .ToListAsync();
            return accounts;

        }

        public async Task SaveLogStatus(int accountId, string logStatus)
        {
            var account = await _db.Accounts.Where(a => a.accountId == accountId).SingleAsync();
            account.accountStatus= logStatus;
            _db.Update(account);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsAccountByProxyId(int proxyId)
        {
            return await _db.Accounts.AnyAsync(x => x.currentProxyId == proxyId);
        }
    }
}
