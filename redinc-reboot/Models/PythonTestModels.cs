using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace redinc_reboot.Models
{
    public class CodeModel
    {
        [Required]
        [Display(Name = "Input code")]
        public string InputCode { get; set; }
    }
}
