//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoomCleaning.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int RatingDusting { get; set; }
        public int RatingVacuuming { get; set; }
        public int OverallImpression { get; set; }
        public string AdditionalInformation { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string Location { get; set; }
    }
}
