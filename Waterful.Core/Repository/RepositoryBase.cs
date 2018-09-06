using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Core.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>, new()
    {

        //定义数据访问上下文对象
        protected readonly PomeloMySqlDbContext _dbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryBase(PomeloMySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, bool asNoTracking = true)
        {
            return asNoTracking ? orderBy(_dbContext.Set<TEntity>().AsNoTracking()).FirstOrDefault(where) : orderBy(_dbContext.Set<TEntity>()).FirstOrDefault(where);
        }
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, bool asNoTracking = true)
        {
            return asNoTracking ? orderBy(_dbContext.Set<TEntity>().AsNoTracking()).FirstOrDefaultAsync(where) : orderBy(_dbContext.Set<TEntity>()).FirstOrDefaultAsync(where);
        }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllList()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).ToList();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity Get(TPrimaryKey id)
        {
            return _dbContext.Set<TEntity>().Find(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Add(entity);
            if (autoSave)
                Save();
            return entity;
        }

        public void Insert(IEnumerable<TEntity> list)
        {
            _dbContext.Set<TEntity>().AddRange(list);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Update(TEntity entity, bool autoSave = true)
        {
            //var obj = Get(entity.Id);
            //EntityToEntity(entity, obj);
            //if (autoSave)
            //    Save();
            //return entity;

            if (autoSave)
            {
                _dbContext.SaveChanges();
            }
            else
            {
                //_dbContext.Set<TEntity>().Update(entity);
                // _dbContext.Entry(entity).State = EntityState.Modified;
            }

        }
        private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        {
            foreach (var mItem in typeof(T).GetProperties())
            {
                mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
            }
        }
        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        //public TEntity InsertOrUpdate(TEntity entity, bool autoSave = true)
        //{
        //    if (Get(entity.Id) != null)
        //        return Update(entity, autoSave);
        //    return Insert(entity, autoSave);
        //}

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TPrimaryKey id, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Remove(Get(id));
            if (autoSave)
                Save();
        }


        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否自动保存</param>
        public void Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Where(where).ToList().ForEach(it => _dbContext.Set<TEntity>().Remove(it));
            if (autoSave)
                Save();
        }
        public int Count()
        {
            return _dbContext.Set<TEntity>().Count();
        }
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return _dbContext.Set<TEntity>().Count(where);
        }
        public bool Any(Expression<Func<TEntity, bool>> where)
        {
            return _dbContext.Set<TEntity>().Any(where);
        }
        public bool Any()
        {
            return _dbContext.Set<TEntity>().Any();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public IQueryable<TEntity> LoadPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> order = null)
        {
            var result = from p in _dbContext.Set<TEntity>()
                         select p;
            if (where != null)
                result = result.Where(where);
           
            result = order != null ? result.OrderByDescending(order) : result.OrderByDescending(m => m.Id);

            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        /// <summary>
        /// 执行sql查询对应实体list
        /// </summary>
        /// <param name="sql">select p.id, p.name, a.fullAddress, a.lat, a.lon from ( select * from {nameof(Person)} where {nameof(name)}=@{nameof(name)}</param>
        /// <param name="parameters"> new[] { new SqlParameter(nameof(name), name) }</param>
        /// <returns></returns>

        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = _dbContext.Database.GetDbConnection();
            int returnValue = -1;

            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    returnValue = command.ExecuteNonQuery();

                    return returnValue;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                throw ex;
            }
#endif
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行sql查询对应实体list
        /// </summary>
        /// <param name="sql">select p.id, p.name, a.fullAddress, a.lat, a.lon from ( select * from {nameof(Person)} where {nameof(name)}=@{nameof(name)}</param>
        /// <param name="parameters"> new[] { new SqlParameter(nameof(name), name) }</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = _dbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    //command.CommandType = System.Data.CommandType.StoredProcedure;

                    return command.ExecuteScalar();
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                throw ex;
            }
#endif
            finally
            {
                conn.Close();
            }
        }
        public List<TEntity> ExecuteReader(string sql, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = _dbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(TEntity).GetProperties();
                    var rtnList = new List<TEntity>();
                    TEntity model;
                    object val;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new TEntity();
                            foreach (var l in propts)
                            {
                                val = reader[l.Name];
                                if (val == DBNull.Value)
                                {
                                    l.SetValue(model, null);
                                }
                                else
                                {
                                    l.SetValue(model, val);
                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                throw ex;
            }
#endif
            finally
            {
                conn.Close();
            }
        }

        public List<T> ExecuteReader<T>(string sql, params object[] parameters) where T : class, new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = _dbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(T).GetProperties();
                    var rtnList = new List<T>();
                    T model;
                    object val;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new T();
                            foreach (var l in propts)
                            {
                                val = reader[l.Name];
                                if (val == DBNull.Value)
                                {
                                    l.SetValue(model, null);
                                }
                                else
                                {
                                    l.SetValue(model, val);
                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                throw ex;
            }
#endif
            finally
            {
                conn.Close();
            }
        }


        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression);
        }

        public IQueryable<TEntity> All()
        {
            return _dbContext.Set<TEntity>();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(predicate);
        }





        public virtual Task<bool> ExistAsync(Expression<Func<TEntity, bool>> where)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return dbSet.AsNoTracking().AnyAsync(where);
        }
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return where == null ? dbSet.AsNoTracking().CountAsync() : dbSet.AsNoTracking().CountAsync(where);
        }
        #region Sum
        public virtual decimal Sum(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> where = null)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return where == null ? dbSet.AsNoTracking().Sum(selector) : dbSet.AsNoTracking().Where(where).Sum(selector);
        }

        public virtual Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> where = null)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return where == null ? dbSet.AsNoTracking().SumAsync(selector) : dbSet.AsNoTracking().Where(where).SumAsync(selector);
        }

        #endregion
    }

    /// <summary>
    /// 主键为int类型的仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int> where TEntity : Entity, new()
    {
        public RepositoryBase(PomeloMySqlDbContext dbContext) : base(dbContext)
        {
        }

    }

}
