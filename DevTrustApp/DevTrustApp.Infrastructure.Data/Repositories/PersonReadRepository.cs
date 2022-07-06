using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class PersonReadRepository : ReadRepository<Person>, IPersonReadRepository
    {
        public PersonReadRepository(DevTrustAppDbContext context) : base(context)
        {

        }
    }
}
