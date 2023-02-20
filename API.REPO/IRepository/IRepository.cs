using API.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static API.COMMON.Enum.SelectEnum;

namespace API.REPO.IRepository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(Select select);
        Task<T> GetAsync(int id);
        Task<bool> InsertAsync(T entity);
        Task<bool> InsertRangeAsync(IEnumerable<T> lstEntity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(IEnumerable<T> lstEntity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteRangeAsync(IEnumerable<T> lstEntity);
        Task<bool> DeleteFromTrashAsync(T entity);
        Task<bool> DeleteFromTrashRangeAsync(IEnumerable<T> lstEntity);
        Task SavechangesAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> preicate);
    }
}
