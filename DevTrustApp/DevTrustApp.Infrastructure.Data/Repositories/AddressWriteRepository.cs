using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class AddressWriteRepository : WriteRepository<Address>, IAddressWriteRepository
    {
        public AddressWriteRepository(DevTrustAppDbContext context) : base(context)
        {

        }
    }
}
