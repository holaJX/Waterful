using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Core.Repository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {

    }
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
       
        public OrderItemRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}

