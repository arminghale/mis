using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class UACC
    {
        public UACC()
        {

        }
        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public int actionid { get; set; }
        public bool type { get; set; }
        public virtual User User { get; set; }
        public virtual Action Action { get; set; }

    }
}
