using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.EF
{
    public class Food
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }

 
        public DateTime? CompleteTime { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        [ForeignKey("Status")]
        public string StatusName { get; set; }
        public Status Status { get; set; }

        [ForeignKey("Employee")]
        public Guid? AssignedTo { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("Benefactor")]
        public Guid BenefactorId { get; set; }
        public Benefactor Benefactor { get; set; }

    }
}