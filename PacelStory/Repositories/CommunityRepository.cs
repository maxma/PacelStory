using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private static int pageSize = 7;
        //private static List<Community> pacels = new List<Community>();
        //private static int _nextId = 1;

        public CommunityRepository()
        {

        }


        public IEnumerable<Community> GetAllCommunities(int pageNumber)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Community> communityList = new List<Community>();
                //List<ArticleWithImageList> articleListWithImageList = new List<ArticleWithImageList>();

                int innerRows = -1;
                try
                {
                    innerRows = (pageNumber - 1) * pageSize;
                }
                catch (Exception)
                {
                    throw new Exception("pageNumber or pageSize is not INT");
                }

                if (innerRows != -1)
                {
                    communityList = entities.Community.OrderByDescending(i => i.communityId).Skip(innerRows).Take(pageSize).ToList<Community>();

                    return communityList;
                }
                else
                {
                    return null;
                }
            }
        }

        public Community GetSpecifiedCommunityById(long id)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Community specifiedCommunity = new Community();

                specifiedCommunity = entities.Community.SingleOrDefault<Community>(x => x.communityId == id);

                return specifiedCommunity;

            }
        }

        public List<Community> GetCommunityByCampOwnerMobile(string mobile)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Community> specifiedCommunities = new List<Community>();

                specifiedCommunities = entities.Community.Where(x => x.campOwnerMobile == mobile).OrderByDescending(i => i.communityId).ToList<Community>();

                return specifiedCommunities;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long CreateCommunity(Community item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.Community.Add(item);
                //entities.SignedPacel.Add((SignedPacel)item);
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.communityId;
            }
            else
            {
                return 0;
            }

        }

        public int RemoveCommunity(long id)
        {
            int returnCode = 0;
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Community specifiedCommunity = new Community();
                specifiedCommunity = entities.Community.SingleOrDefault<Community>(x => x.communityId == id);
                entities.Community.Remove(specifiedCommunity);
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }

        public long UpdateCommunity(Community item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.Community.Attach(item);
                var entry = entities.Entry(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.communityId;
            }
            else
            {
                return 0;
            }

        }
    }
}