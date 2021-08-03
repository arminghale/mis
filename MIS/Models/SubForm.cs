using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class SubForm
    {
        public SubForm()
        {

        }
        public int id { get; set; }
        public int form1id { get; set; }
        public int form2id { get; set; }
        public int type { get; set; }
        public string title { get; set; }
        public int radif { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public virtual Form Form1 { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public virtual Form Form2 { get; set; }
    }
}
