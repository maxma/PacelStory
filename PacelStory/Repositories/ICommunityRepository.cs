using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    interface ICommunityRepository
    {
        IEnumerable<Community> GetAllCommunities(int pageNumber);

        Community GetSpecifiedCommunityById(long communityId);
        List<Community> GetCommunityByCampOwnerMobile(string mobile);
        long CreateCommunity(Community item);
        int RemoveCommunity(long communityId);
        long UpdateCommunity(Community item);

    }
}