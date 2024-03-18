using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.EF
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("Access")]
        public string AccessName { get; set; }
        public Access Access { get; set; }

    }
}