using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomCleaning.Models {
    public class ForgotUserid {
        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact Number Required")]
        [MinLength(10, ErrorMessage = "Minimum 10 characters")]
        [MaxLength(10, ErrorMessage = "Maximun 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

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