using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Waterful.Core.Repository;

namespace Waterful.Core
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        int SaveChange();
        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
    }
}
