using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.DTO;
using MySql.Data.MySqlClient;
using System.Text;
using System.Linq.Expressions;

namespace Waterful.Core.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProduct(ProductDto model);
        /// <summary>
        /// 微商城-获取首页产品列表
        /// </summary>
        /// <returns></returns>
        List<Product> GetWechatProductList();
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public Product GetProduct(ProductDto model)
        {
            var result = _dbContext.Products.Where(e => e.Status > -1)
                    .Where(x => x.Level == model.level && x.CategoryId == model.CategoryId && x.PaymentType == model.PaymentType)
                    .FirstOrDefault();

            return result;
        }
        /// <summary>
        /// 微商城-获取首页产品列表
        /// </summary>
        /// <returns></returns>
        public List<Product> GetWechatProductList()
        {
            var result = _dbContext.Products
                    .Where(x =>x.Status == 1)
                    .GroupBy(x => new { x.CategoryId, x.PaymentType })
                    .Select(g => new Product
                    {
                        CategoryId = g.Key.CategoryId,
                        PaymentType = g.Key.PaymentType,
                        Id = g.OrderBy(o => o.Level).Select(x => x.Id).FirstOrDefault()
                    }).Distinct().ToList();

            return result;
        }
    }
}

