using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomCleaning.Models {
    public class AdminLogin {
        [Display(Name = "Admin ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Admin ID required")]
        public string AdminID { set; get; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum Length 6 characters")]
        public string Password { set; get; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { set; get; }
    }
}