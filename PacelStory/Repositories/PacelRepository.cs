using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class PacelRepository : IPacelRepository
    {
        private static int pageSize = 7;
        //private static List<Pacel> pacels = new List<Pacel>();
        //private static int _nextId = 1;

        public PacelRepository()
        {
            
        }

        
        public IEnumerable<Pacel> GetUnSignedPacelsByCustomerId(long customerId, int pageNumber)
        {            
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Pacel> pacelList = new List<Pacel>();
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
                    pacelList = entities.Pacel.Where(x => x.customerId == customerId && x.type == "0").OrderByDescending(i => i.arrivedDate).Skip(innerRows).Take(pageSize).ToList<Pacel>();

                    return pacelList;
                }
                else
                {
                    return null;
                }
            }
        }

        public Pacel GetPacelByLogisticsId(string logisticsId, long customerId)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Pacel specifiedPacel = new Pacel();

                specifiedPacel = entities.Pacel.SingleOrDefault<Pacel>(x => x.logisticsId == logisticsId && x.customerId == customerId);

                return specifiedPacel;
            }
        }

        public IEnumerable<Pacel> GetSignedPacelsByCustomerId(long customerId, int pageNumber)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Pacel> pacelList = new List<Pacel>();
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
                    pacelList = entities.Pacel.Where(x => x.customerId == customerId && x.type == "1").OrderByDescending(i => i.arrivedDate).Skip(innerRows).Take(pageSize).ToList<Pacel>();

                    return pacelList;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<Pacel> GetPacelsByCustomerId(long customerId, int pageNumber)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Pacel> pacelList = new List<Pacel>();
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
                    pacelList = entities.Pacel.Where(x => x.customerId == customerId).OrderByDescending(i => i.arrivedDate).Skip(innerRows).Take(pageSize).ToList<Pacel>();

                    return pacelList;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<Pacel> GetAll(int pageNumber)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Pacel> pacelList = new List<Pacel>();
                
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
                    pacelList = entities.Pacel.OrderByDescending(i => i.pacelId).Skip(innerRows).Take(pageSize).ToList<Pacel>();

                    return pacelList;
                }
                else
                {
                    return null;
                }
            }
        }

        //public Pacel Get(long id)
        //{
        //    using (PacelDbEntities entities = new PacelDbEntities())
        //    {
        //        Pacel specifiedPacel = new Pacel();
        //        specifiedPacel = entities.Pacel.SingleOrDefault<Pacel>(x => x.pacelId == id);
        //        return specifiedPacel;
        //    }

        //}

        public Pacel GetSpecifiedPacel(long id)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Pacel specifiedPacel = new Pacel();

                specifiedPacel = entities.Pacel.SingleOrDefault<Pacel>(x => x.pacelId == id);

                return specifiedPacel;


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long CreatePacel(Pacel item)
        {            
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Pacel pacel = entities.Pacel.Add(item);
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

        public int RemovePacel(long id)
        {
            int returnCode = 0;
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Pacel specifiedPacel = new Pacel();
                specifiedPacel = entities.Pacel.SingleOrDefault<Pacel>(x => x.pacelId == id);
                entities.Pacel.Remove(specifiedPacel);
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }

        public int UpdatePacel(Pacel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int returnCode = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {                
                entities.Pacel.Attach(item);
                var entry = entities.Entry(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }

        //public bool Update(Product item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }
        //    int index = products.FindIndex(p => p.Id == item.Id);
        //    if (index == -1)
        //    {
        //        return false;
        //    }
        //    products.RemoveAt(index);
        //    products.Add(item);
        //    return true;
        //}

      
    }
}