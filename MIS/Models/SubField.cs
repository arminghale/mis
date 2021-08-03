using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class SubField
    {
        public SubField()
        {

        }
        public int id { get; set; }
        public int fieldid { get; set; }
        public string value { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Field Field { get; set; }
    }
}
