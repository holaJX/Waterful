using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Core.Repository
{
    /// <summary>
    /// 仓储接口定义
    /// </summary>
    public interface IRepository
    {

    }


    /// <summary>
    /// 定义泛型仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>, new()
    {
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, bool asNoTracking = true);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, bool asNoTracking = true);
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);


        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        void Insert(IEnumerable<TEntity> list);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        void Update(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        //TEntity InsertOrUpdate(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        void Delete(TEntity entity, bool autoSave = true);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        void Delete(TPrimaryKey id, bool autoSave = true);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否自动保存</param>
        void Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true);
        /// <summary>
        /// 根据条件获取实体总数(分页用)
        /// </summary>
        /// <returns></returns>
        int Count();
        int Count(Expression<Func<TEntity, bool>> where);
        bool Any();
        bool Any(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="startPage">起始页</param>
        /// <param name="pageSize">页面条目</param>
        /// <param name="rowCount">数据总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        IQueryable<TEntity> LoadPageList(int startPage, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> order = null);

        void Save();

        int ExecuteNonQuery(string sql, params object[] parameters);
        object ExecuteScalar(string sql, params object[] parameters);
        /// <summary>
        /// 执行sql查询对应实体list
        /// </summary>
        /// <param name="sql">select p.id, p.name, a.fullAddress, a.lat, a.lon from ( select * from {nameof(Person)} where {nameof(name)}=@{nameof(name)}</param>
        /// <param name="parameters"> new[] { new SqlParameter(nameof(name), name) }</param>
        /// <returns></returns>
        List<TEntity> ExecuteReader(string sql, params object[] parameters);
        List<T> ExecuteReader<T>(string sql, params object[] parameters) where T : class, new();

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);




        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> where);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null);

        decimal Sum(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> where = null);
        Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> where = null);
    }

    /// <summary>
    /// 默认int主键类型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : Entity, new()
    {
    }

}
