//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PacelStory.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public long customerId { get; set; }
        public string mobile { get; set; }
        public string username { get; set; }
        public Nullable<bool> gender { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string campname { get; set; }
        public string bldNumber { get; set; }
        public string unitNumber { get; set; }
        public string roomNumber { get; set; }
        public string validationCode { get; set; }
        public string deviceinfo { get; set; }
        public string type { get; set; }
        public Nullable<long> communityId { get; set; }
        public Nullable<System.DateTime> validationCodeTime { get; set; }
        public string campCode { get; set; }
        public string groupName { get; set; }
    }
}