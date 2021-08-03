using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class User
    {
        public User()
        {

        }
        [Key]
        public int id { get; set; }
        public int parentid { get; set; }
        [Required(ErrorMessage ="لطفا نام کاربری را وارد کنید")]
        public string username { get; set; }
        [Required(ErrorMessage ="لطفا رمز عبور را وارد کنید")]
        public string password { get; set; }
        [Required(ErrorMessage ="لطفا نام و نام خانوادگی را وارد کنید")]
        public string name { get; set; }
        public virtual User ParentUser { get; set; }
        public virtual List<UserRoles> UserRoles { get; set; }
        public virtual List<UACC> UACCs { get; set; }
        public virtual List<UACCDomain> UACCDomains { get; set; }
        public virtual List<UserForms> UserForms { get; set; }
    }
}
