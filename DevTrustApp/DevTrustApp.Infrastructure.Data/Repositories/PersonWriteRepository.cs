using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class PersonWriteRepository : WriteRepository<Person>, IPersonWriteRepository
    {
        public PersonWriteRepository(DevTrustAppDbContext context) : base(context)
        {

        }
    }
}
