using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace redinc_reboot.Models
{
    public class HomeClassListModel
    {
        public int Id { get; }

        [Required]
        public String Name {get; set;}

        [Required]
        public String Instructor {get; set;}

        public List<ClassModel> ClassItems { get; set; }
        
    }
}
