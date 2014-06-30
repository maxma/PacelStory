using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Models
{
    public class CampOwnerAndCommunity
    {
        public CampOwner campOwner { get; set; }
        public Community community { get; set; }
        public List<string> campStaffMobileList { get; set; }

        public CampOwnerAndCommunity()
        {
            campOwner = new CampOwner();
            community = new Community();
            campStaffMobileList = new List<string>();
        }
    }
}