using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    interface ICampOwnerRepository
    {
        IEnumerable<CampOwner> GetAllCommunities(int pageNumber);

        CampOwner GetSpecifiedCampOwnerById(long campOwnerId);
        CampOwner GetSpecifiedCampOwnerByMobile(string mobile);
        long CreateCampOwner(CampOwner item);
        int RemoveCampOwner(long communityId);
        long UpdateCampOwner(CampOwner item);
    }
}