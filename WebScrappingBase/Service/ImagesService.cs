using System;
using System.Threading.Tasks;
using WebScrappingBase.Models;

namespace WebScrappingBase.Service
{
    public class ImagesService
    {        
        readonly StorageDbContext _db;
        public ImagesService(StorageDbContext db) => _db = db;
        
        public async Task getImages(ImagesModel imageModel)
        {
            //await _db.ImagesInfos;
        }
    }
}
