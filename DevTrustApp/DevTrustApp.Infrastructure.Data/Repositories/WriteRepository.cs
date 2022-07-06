using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly DevTrustAppDbContext _context;
        public WriteRepository(DevTrustAppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task<T> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return model;
        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }
        public async Task<bool> Remove(string Id)
        {
            T model = await Table.FirstOrDefaultAsync(data => data.Id == long.Parse(Id));
            return Remove(model);
        }
        public T Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return model;
        }

        public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();


    }
}
