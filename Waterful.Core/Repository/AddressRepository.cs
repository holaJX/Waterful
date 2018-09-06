using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Core.Repository
{
    public interface IAddressRepository : IRepository<Address>
    {

    }
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
       
        public AddressRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}

