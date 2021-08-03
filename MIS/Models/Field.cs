using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Field
    {
        public Field()
        {

        }
        public int id { get; set; }
        public int formid { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public bool isnull { get; set; }
        public int radif { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Form Form { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public virtual List<SubField> SubFields { get; set; }

    }
}
