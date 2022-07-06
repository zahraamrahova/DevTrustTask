using DevTrustApp_NLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevTrustApp_NLayer.WebAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        data.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        data.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }


            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
