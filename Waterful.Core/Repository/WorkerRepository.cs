using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Waterful.Core.Repository
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        int SearchTotal(string name);
        List<Worker> SearchList(int pageIndex, int pageSize, string name);

    }
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {

        public WorkerRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public int SearchTotal(string name)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(name))
            {
                //result = base.Count(i => i.Status > 0 && i.Name.Contains(name));//mysql出bug
                result = Convert.ToInt32(base.ExecuteScalar("SELECT COUNT(1) FROM workers WHERE status>0 && name LIKE  CONCAT('%', @name,'%');",
                   //new MySqlParameter("@name", name)
                   new MySqlParameter() { ParameterName = "@name", Value = name }

                ));
            }
            else
            {
                result = base.Count(i => i.Status > 0);
            }

            return result;
        }
        public List<Worker> SearchList(int pageIndex, int pageSize, string name)
        {
            List<Worker> result = new List<Worker>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                result = base.ExecuteReader<Worker>("SELECT * FROM workers WHERE status>0 && name LIKE CONCAT('%', @name,'%') LIMIT @pageStart,@pageEnd;",
                    new MySqlParameter() { ParameterName = "@pageStart", Value = (pageIndex - 1) * pageSize },
                    new MySqlParameter() { ParameterName = "@pageEnd", Value = pageSize },
                    new MySqlParameter() { ParameterName = "@name", Value = name }
            );
            }
            else
            {
                result = base.ExecuteReader<Worker>("SELECT * FROM workers WHERE status>0 LIMIT @pageStart,@pageEnd;",
                    new MySqlParameter() { ParameterName = "@pageStart", Value = (pageIndex - 1) * pageSize },
                    new MySqlParameter() { ParameterName = "@pageEnd", Value = pageSize }
                );
            }

            return result;
        }
    }
}

