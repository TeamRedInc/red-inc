using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace redinc_reboot.Models
{
    public class ClassModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public String Name {get; set;}

        [Required]
        public String Instructor {get; set;}
    }
}
