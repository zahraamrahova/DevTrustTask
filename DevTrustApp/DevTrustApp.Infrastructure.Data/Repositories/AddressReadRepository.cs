using DevTrustApp.Core.DomainService;
using DevTrustApp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp.Infrastructure.Data.Repositories
{
    public class AddressReadRepository : ReadRepository<Address>, IAddressReadRepository
    {
        public AddressReadRepository(DevTrustAppDbContext context) : base(context)
        {

        }
    }
}
