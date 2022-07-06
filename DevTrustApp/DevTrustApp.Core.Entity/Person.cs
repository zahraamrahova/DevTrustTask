using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp.Core.Entity
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
