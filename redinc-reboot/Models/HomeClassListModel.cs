using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.Modules.Class;

namespace redinc_reboot.Models
{
    public class HomeClassListModel
    {
        public int Id {get; set;}

        [Required]
        public String Name {get; set;}

        [Required]
        public String Instructor {get; set;}

        public List<ClassData> ClassItems { get; set; }
        
    }
}
