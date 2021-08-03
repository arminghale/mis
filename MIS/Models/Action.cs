using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Action
    {
        public Action()
        {

        }
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="لطفا گروه را وارد کنید")]
        public int actiongroupid { get; set; }
        [Required(ErrorMessage ="لطفا عنوان را وارد کنید")]
        public string title { get; set; }
        [Required(ErrorMessage ="لطفا لینک را وارد کنید")]
        public string url { get; set; }
        public virtual ActionGroup ActionGroup { get; set; }
        public virtual List<UACC> UACCs { get; set; }
        public virtual List<RACC> RACCs { get; set; }
    }
}
