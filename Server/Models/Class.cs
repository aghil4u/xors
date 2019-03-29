using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Username  is Required")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password  is Required")]
        public string password { get; set; }
    }
}
