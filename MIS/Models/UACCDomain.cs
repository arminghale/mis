using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class UACCDomain
    {
        public UACCDomain()
        {

        }
        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public int domainvalueid { get; set; }
        public virtual User User { get; set; }
        public virtual DomainValue DomainValue { get; set; }
    }
}
