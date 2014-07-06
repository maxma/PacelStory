using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class ResendRepository : IResendRepository
    {
        public Resend GetSpecifiedResendByPacelId(long pacelId)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Resend specifiedResend = new Resend();

                specifiedResend = entities.Resend.SingleOrDefault<Resend>(x => x.pacelId == pacelId);

                return specifiedResend;
            }
        }

        public long CreateResend(Resend item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Resend resend = entities.Resend.Add(item);
                //entities.SignedPacel.Add((SignedPacel)item);
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.pacelId;
            }
            else
            {
                return 0;
            }
        }
    }
}