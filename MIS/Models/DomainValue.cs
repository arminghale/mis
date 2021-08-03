using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class DomainValue
    {
        public DomainValue()
        {

        }
        [Key]
        public int id { get; set; }
        public int domainid { get; set; }
        [Required(ErrorMessage ="لطفا مقدار را وارد کنید")]
        public string value { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Domain Domain { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<UACCDomain> UACCDomains { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<RACCDomain> RACCDomains { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<SubDomainValue> SubDomainValue { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<SubDomainValue> SubDomainValue2 { get; set; }
    }
}
