using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class ActionGroup
    {
        public ActionGroup()
        {

        }
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="لطفا سامانه را وارد کنید")]
        public int samaneid { get; set; }
        [Required(ErrorMessage ="لطفا عنوان را وارد کنید")]
        public string title { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Samane Samane { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<Action> Actions { get; set; }

    }
}
