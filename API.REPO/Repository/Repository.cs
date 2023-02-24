using API.COMMON.Enum;
using API.DATA;
using API.REPO.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.REPO.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly LearnWordBookContext context;
        private readonly DbSet<T> entities;
        public Repository(LearnWordBookContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }
        private void SetDefaultInset(T entity)
        {
            entity.CreateAt = DateTime.Now;
            entity.IsTrash = false;
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                return false;
            //đính kèm thay đổi
            entities.Attach(entity);
            entity.IsTrash = true;
            //báo hiệu trường istrash sẽ update
            context.Entry(entity).Property(x => x.IsTrash).IsModified = true;
            await SavechangesAsync();
            return true;
        }

        public async Task<bool> DeleteFromTrashAsync(T entity)
        {
            if (entity == null)
                return false;
            entities.Remove(entity);
            await SavechangesAsync();
            return true;
        }

        public async Task<bool> DeleteFromTrashRangeAsync(IEnumerable<T> lstEntity)
        {
            if (!lstEntity.Any())
                return false;
            entities.RemoveRange(lstEntity);
            await SavechangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> lstEntity)
        {
            if (!lstEntity.Any())
                return false;
            foreach (var entity in lstEntity)
            {
                await DeleteAsync(entity);
            }
            return true;
        }

        public IQueryable<T> GetAll(SelectEnum.Select select)
        {
            switch (select)
            {
                case SelectEnum.Select.NONTRASH:
                    return entities.Where(x => x.IsTrash == false);
                case SelectEnum.Select.TRASH:
                    return entities.Where(x => x.IsTrash == true);
                default: return entities.AsQueryable();
            }
        }

        public async Task<T> GetAsync(int id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<bool> InsertAsync(T entity)
        {
            SetDefaultInset(entity);
            await entities.AddAsync(entity);
            await SavechangesAsync();
            return true;
        }


        public async Task<bool> InsertRangeAsync(IEnumerable<T> lstEntity)
        {
            foreach (var entity in lstEntity)
            {
                SetDefaultInset(entity);
            }
            await entities.AddRangeAsync(lstEntity);
            await SavechangesAsync();
            return true;
        }

        public async Task SavechangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await SavechangesAsync();
            return true;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<T> lstEntity)
        {
            //context.Entry(lstEntity).State = EntityState.Modified;
            context.UpdateRange(lstEntity);
            await SavechangesAsync();
            return true;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> preicate)
        {
            return entities.Where(preicate);
        }
    }
}
