using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.DTO
{
    public class CollectionRequestDTO
    {
        [Required]
        [Display(Name="Meal Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Good for(hour/s)")]
        [Range(1, int.MaxValue, ErrorMessage = "Hours must be greater than 0")]
        public int ExpireTime { get; set; }
        [Required]
        [Display(Name="Amount(kg/s)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.01")]
        public double Amount { get; set; }
    }
}