using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomCleaning.Models {
    public class ForgotPassword {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "What is the name of your first pet?")]
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