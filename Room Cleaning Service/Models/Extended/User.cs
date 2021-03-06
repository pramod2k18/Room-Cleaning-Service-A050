using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomCleaning.Models {
   
    [MetadataType(typeof(UserMetadata))]
    public partial class User {
        public string ConfirmPassword { set; get; }
    }

    public class UserMetadata {
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name required")]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }

        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact Number Required")]
        [MinLength(10, ErrorMessage = "Minimum 10 characters")]
        [MaxLength(10, ErrorMessage = "Maximum 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [Display(Name = "Gender")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 character required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password and Password do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "What is the name of your fist pet?")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques1 { get; set; }

        [Display(Name = "Which elementary school did you attend?")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques2 { get; set; }

        [Display(Name = "What is your favorite color?")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques3 { get; set; }
    }
}