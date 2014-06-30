using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class TextRepository
    {
        public TextRepository()
        {

        }

        //public IEnumerable<tbl_smsmt_send> GetAllCommunities(int pageNumber)
        //{
        //    using (PacelDbEntities entities = new PacelDbEntities())
        //    {
        //        List<Community> communityList = new List<Community>();
        //        //List<ArticleWithImageList> articleListWithImageList = new List<ArticleWithImageList>();

        //        int innerRows = -1;
        //        try
        //        {
        //            innerRows = (pageNumber - 1) * pageSize;
        //        }
        //        catch (Exception)
        //        {
        //            throw new Exception("pageNumber or pageSize is not INT");
        //        }

        //        if (innerRows != -1)
        //        {
        //            communityList = entities.Community.OrderByDescending(i => i.communityId).Skip(innerRows).Take(pageSize).ToList<Community>();

        //            return communityList;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public tbl_smsmt_send GetSpecifiedCommunityById(long id)
        //{
        //    using (PacelDbEntities entities = new PacelDbEntities())
        //    {
        //        Community specifiedCommunity = new Community();

        //        specifiedCommunity = entities.Community.SingleOrDefault<Community>(x => x.communityId == id);

        //        return specifiedCommunity;

        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CreateText(tbl_smsmt_send item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int returnCode = 0;

            using (textDbEntities entities = new textDbEntities())
            {
                entities.tbl_smsmt_send.Add(item);
                //entities.SignedPacel.Add((SignedPacel)item);
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }
    }
}