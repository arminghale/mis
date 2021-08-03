using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class SubDomainValue
    {
        public SubDomainValue()
        {

        }
        [Key]
        public int id { get; set; }
        [ForeignKey("DomainValue")]
        public int domainvalueid { get; set; }
        public int domainvalue2id { get; set; }
        public virtual DomainValue DomainValue { get; set; }
        public virtual DomainValue DomainValue2 { get; set; }
    }
}
