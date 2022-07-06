using System;

namespace DevTrustApp_NLayer.Entities
{
    public class Person: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
