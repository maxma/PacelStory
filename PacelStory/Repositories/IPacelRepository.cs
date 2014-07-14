using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacelStory.Repositories
{
    interface IPacelRepository
    {
        IEnumerable<Pacel> GetAll(int pageNumber);
        IEnumerable<Pacel> GetPacelsByCustomerId(long customerId, int pageNumber);
        IEnumerable<Pacel> GetUnSignedPacelsByCustomerId(long customerId, int pageNumber);
        IEnumerable<Pacel> GetSignedPacelsByCustomerId(long customerId, int pageNumber);
        IEnumerable<Pacel> GetUnSignedPacelsByCustomerType2Id(long customerId, int pageNumber);
        Pacel GetSpecifiedPacel(long id);
        Pacel GetPacelByLogisticsId(string logisticsId, long customerId);
        long CreatePacel(Pacel item);
        int RemovePacel(long id);
        int UpdatePacel(Pacel item);
    }
}
