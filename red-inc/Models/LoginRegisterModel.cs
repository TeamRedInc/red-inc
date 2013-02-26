using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace red_inc.Models
{
    public class LoginRegisterModel : DbContext
    {
        public class LoginModel
        {
            [Required]
            [Display(Name = "E-mail")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            //[Display(Name = "Remember me?")]
            //public bool RememberMe { get; set; }
         }
    }
}
