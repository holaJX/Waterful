using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Core.Repository
{
    public interface ICouponUseRepository : IRepository<CouponUse>
    {

    }
    public class CouponUseRepository : RepositoryBase<CouponUse>, ICouponUseRepository
    {
       
        public CouponUseRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}

