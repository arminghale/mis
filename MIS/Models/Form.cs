using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Form
    {
        public Form()
        {

        }
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<SubForm> SubForms1 { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<SubForm> SubForms2 { get; set; }
        public virtual List<Field> Fields { get; set; }
        public virtual List<UserForms> UserForms { get; set; }
    }
}
