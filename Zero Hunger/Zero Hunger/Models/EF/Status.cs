using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.EF
{
    public class Status
    {
        [Key]
        public string Name { get; set; }
    }
}