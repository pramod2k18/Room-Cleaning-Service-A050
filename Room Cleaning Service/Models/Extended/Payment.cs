using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomCleaning.Models {
 
    [MetadataType(typeof(PaymentMetadata))]
    public partial class Payment {
    }

    public class PaymentMetadata {
        public int Id { get; set; }

        [Display(Name = "Card Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Number Required")]
        [MinLength(16, ErrorMessage = "Minimum 16 characters")]
        [DataType(DataType.CreditCard)]
        public string cardNumber { get; set; }

        [Display(Name = "Month Expire")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Month Expiry Required")]
        [Range(1, 12, ErrorMessage = "Invalid Month")]
        public Nullable<int> ExpMonth { get; set; }

        [Display(Name = "Year Expire")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Year Expiry Required")]
        [Range(2020, 9999, ErrorMessage = "Invalid Year")]
        public Nullable<int> ExpYear { get; set; }

        [Display(Name = "CVV")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "CVV Required")]
        [Range(1, 999, ErrorMessage = "Invalid CVV")]
        public Nullable<int> cvv { get; set; }

        [Display(Name = "Name on Card")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Name Required")]
        public string name { get; set; }
        public Nullable<double> amount { get; set; }
        public string Method { get; set; }

    }
}