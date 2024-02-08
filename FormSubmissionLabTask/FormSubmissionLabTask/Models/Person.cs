using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FormSubmissionLabTask.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Name is mandatory")]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z\s.-]+$", ErrorMessage = "Name can only contain alphabets, spaces, dots and hyphens.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Username is mandatory")]
        [Display(Name = "Username")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Username can only contain alphabets, numbers and special characters(excluding space).")]
        [StringLength(maximumLength: 10, ErrorMessage = "Username cannot be longer than 10 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "ID is mandatory")]
        [Display(Name = "ID")]
        [RegularExpression(@"^\d{2}-\d{5}-\d$", ErrorMessage = "ID must be in the format X-XXXXX-X. Where X is a digit 0-9")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [Display(Name = "Email")]
        [RegularExpression(@"^\d{2}-\d{5}-\d@student.aiub.edu$", ErrorMessage ="Email must be in \"XX-XXXXX-X@student.aiub.edu\" format. Where X is a digit 0-9")]
        [CustomCompare("Id", ErrorMessage = "Email must be same for ID")]
        public string Email { get; set; }
        

    }
}