using ScoutDAL.Data;
using ScoutDAL.Repository.IRepository;
using ScoutModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoutDAL.Repository
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        private ApplicationDbContext _db;
        public StoreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Store obj)
        {
            _db.Stores.Update(obj);
        }
    }
}
