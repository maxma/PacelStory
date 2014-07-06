using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    interface IResendRepository
    {
        long CreateResend(Resend item);

        Resend GetSpecifiedResendByPacelId(long pacelId);

    }
}