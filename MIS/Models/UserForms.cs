using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class UserForms
    {
        public UserForms()
        {

        }
        public int id { get; set; }
        public int userid { get; set; }
        public int formid { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual User User { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Form Form { get; set; }
    }
}
