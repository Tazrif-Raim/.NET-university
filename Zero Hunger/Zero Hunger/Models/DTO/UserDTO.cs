using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Name must be 3 to 255 characters long")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "Username must be 4 to 255 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain alphabets and numbers")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        public string AccessName { get; set; }
    }
}