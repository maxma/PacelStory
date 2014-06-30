using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Models
{
    public class PacelAndCustomer
    {
        public Customer customer { get; set; }
        public Pacel pacel { get; set; }
        public string wuyeMobile { get; set; }
        public string groupName { get; set; }

        public PacelAndCustomer()
        {
            customer = new Customer();
            pacel = new Pacel();
        }
    }
}