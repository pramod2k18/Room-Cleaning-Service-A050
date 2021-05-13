using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RoomCleaning.Models
{
    public class ResetPassword
    {
        [Display(Name = "Type Your New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "No Password Entered")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum Length shoule be 6 characters")]
        public string NewPassword { get; set; }
    }
}