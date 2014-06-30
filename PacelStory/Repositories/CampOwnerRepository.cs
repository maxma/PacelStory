using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class CampOwnerRepository
    {

        public IEnumerable<CampOwner> GetAllCommunities(int pageNumber)
        {
            return null;
        }

        /// <summary>
        /// GetSpecifiedCampOwnerById
        /// </summary>
        /// <param name="campOwnerId"></param>
        /// <returns></returns>
        public CampOwner GetSpecifiedCampOwnerById(int id)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                CampOwner specifiedCampOwner = new CampOwner();

                specifiedCampOwner = entities.CampOwner.SingleOrDefault<CampOwner>(x => x.campOwnerId == id);

                return specifiedCampOwner;
            }
        }

        /// <summary>
        /// GetSpecifiedCampOwnerByMobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CampOwner GetSpecifiedCampOwnerByMobile(string mobile)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                CampOwner specifiedCampOwner = new CampOwner();

                specifiedCampOwner = entities.CampOwner.SingleOrDefault<CampOwner>(x => x.mobile == mobile);

                return specifiedCampOwner;
            }
        }
        

        public long CreateCampOwner(CampOwner item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.CampOwner.Add(item);
                
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.campOwnerId;
            }
            else
            {
                return 0;
            }
        }

        public long RemoveCampOwner(long id)
        {
            int returnCode = 0;
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                CampOwner specifiedCampOwner = new CampOwner();
                specifiedCampOwner = entities.CampOwner.SingleOrDefault<CampOwner>(x => x.campOwnerId == id);
                entities.CampOwner.Remove(specifiedCampOwner);
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }

        public long UpdateCampOwner(CampOwner item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.CampOwner.Attach(item);
                var entry = entities.Entry(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.campOwnerId;
            }
            else
            {
                return 0;
            }
        }

    }
}