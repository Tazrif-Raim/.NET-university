using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.EF
{
    public class Benefactor
    {
        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public User User { get; set; }

        [Required]
        public string Name { get; set; }
    }
}