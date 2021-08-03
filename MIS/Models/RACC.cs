using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class RACC
    {
        public RACC()
        {

        }
        [Key]
        public int id { get; set; }
        public int actionid { get; set; }
        public int roleid { get; set; }
        public bool type { get; set; }
        public virtual Role Role { get; set; }
        public virtual Action Action { get; set; }
    }
}
