using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public class Login
    {
        [Required(ErrorMessage ="لطفا نام کاربری را وارد کنید.")]
        public string username { get; set; }
        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید.")]
        public string password { get; set; }
    }
}
