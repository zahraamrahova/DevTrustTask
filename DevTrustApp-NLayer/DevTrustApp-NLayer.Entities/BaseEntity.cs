using System;
using System.Collections.Generic;
using System.Text;

namespace DevTrustApp_NLayer.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
