using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class RACCDomain
    {
        public RACCDomain()
        {

        }
        [Key]
        public int id { get; set; }
        public int roleid { get; set; }
        public int domainvalueid { get; set; }
        public virtual Role Role { get; set; }
        public virtual DomainValue DomainValue { get; set; }
    }
}
