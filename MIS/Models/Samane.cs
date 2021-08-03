using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Samane
    {
        public Samane()
        {

        }
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="لطفا عنوان را وارد کنید")]
        public string title { get; set; }
        public virtual List<ActionGroup> ActionGroups { get; set; }
    }
}
