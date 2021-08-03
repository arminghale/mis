using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class UserRoles
    {
        public UserRoles()
        {

        }
        [Key]
        public int id { get; set; }
        public int roleid { get; set; }
        public int userid { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
