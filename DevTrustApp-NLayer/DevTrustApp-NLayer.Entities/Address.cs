using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp_NLayer.Entities
{
   public class Address :BaseEntity
    {
        public string City { get; set; }
        public string AddressLine { get; set; }
    }
}
