using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    interface IResendRepository
    {
        long CreateResend(long pacelId);

        Resend GetSpecifiedResend(long pacelId);

    }
}