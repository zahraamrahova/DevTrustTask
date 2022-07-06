using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly DevTrustAppDbContext _context;
        public ReadRepository(DevTrustAppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();


        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetByIdAsync(string Id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == long.Parse(Id));
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }


        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method = null, bool tracking = true)
        {
            IQueryable<T> query = Table;
            if (method != null)
            {
                query = Table.Where(method);
            }
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<bool> IsExistAsync(string Id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.AnyAsync(data => data.Id == long.Parse(Id));
        }

       
    }
}
