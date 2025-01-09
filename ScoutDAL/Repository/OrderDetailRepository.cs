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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
         private ApplicationDbContext _db;
            public OrderDetailRepository(ApplicationDbContext db) : base(db)
            {
                _db = db;
            }

            public void Update(OrderDetail obj)
            {
                _db.OrderDetails.Update(obj);
            }
        }
    }
