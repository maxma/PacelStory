using PacelStory.Models;
using PacelStory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class PacelAndCustomerRepository
    {
        CustomerRepository cr = new CustomerRepository();
        PacelRepository pr = new PacelRepository();

        public int CreatePacelAndCustomer(PacelAndCustomer item)
        {
            // try to retrieve customer
            cr.GetSpecifiedCustomerByMoble(item.customer.mobile);

            // create new or update existing customer

            // create pacel
            item.pacel.customerId = item.customer.customerId;  // set the customerId in Pacel object by customer object
            pr.CreatePacel(item.pacel);

            // create qrcode
            string encodingString = item.pacel.pacelId.ToString() + item.customer.customerId.ToString();

            QrCodeUtility arCode = new QrCodeUtility();
            arCode.GenerateQrCode("", "pacelIdCustomerMobile");

            
            return 0;
        }

    }
}