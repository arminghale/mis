using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Domain
    {
        public Domain()
        {

        }
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="لطفا عنوان را وارد کنید")]
        public string title { get; set; }
        public bool IsAccess { get; set; }
        public virtual List<DomainValue> DomainValues { get; set; }
    }
}
