using PacelStory.Repositories;
using PacelStory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PacelStory.Controllers
{
    public class ResendController : ApiController
    {

        ResendRepository resendRepositoy = new ResendRepository();
        PacelRepository pacelRepository = new PacelRepository();

        PacelResponseString prs = new PacelResponseString();

        public int ResendText(long pacelId)
        {
            if(pacelId)

            return 0;
        }

    }
}
