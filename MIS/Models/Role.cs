using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Role
    {
        public Role()
        {

        }
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public virtual List<UserRoles> UserRoles { get; set; }
        public virtual List<RACC> RACCs { get; set; }
        public virtual List<RACCDomain> RACCDomains { get; set; }
    }
}
