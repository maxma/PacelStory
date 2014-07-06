using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacelStory.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private static int pageSize = 7;
        //private static int _nextId = 1;

        public CustomerRepository()
        {
            
        }


        public IEnumerable<Customer> GetAllCustomers(int pageNumber)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                List<Customer> customerList = new List<Customer>();

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
                    customerList = entities.Customer.OrderByDescending(i => i.customerId).Skip(innerRows).Take(pageSize).ToList<Customer>();

                    return customerList;
                }
                else
                {
                    return null;
                }
            }
        }

        public Customer GetSpecifiedCustomerById(long id)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Customer specifiedCustomer = new Customer();

                specifiedCustomer = entities.Customer.SingleOrDefault<Customer>(x => x.customerId == id && x.type == "0");

                return specifiedCustomer;

            }
        }

        public Customer GetSpecifiedCustomerType2ById(long id)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Customer specifiedCustomer = new Customer();

                specifiedCustomer = entities.Customer.SingleOrDefault<Customer>(x => x.customerId == id && x.type == "1");

                return specifiedCustomer;

            }
        }

        public Customer GetSpecifiedCustomerByMoble(string mobile)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Customer specifiedCustomer = new Customer();

                specifiedCustomer = entities.Customer.FirstOrDefault<Customer>(x => x.mobile == mobile && x.type == "0");

                return specifiedCustomer;

            }
        }

        public Customer GetSpecifiedCustomerType2ByMoble(string mobile)
        {
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Customer specifiedCustomer = new Customer();

                specifiedCustomer = entities.Customer.FirstOrDefault<Customer>(x => x.mobile == mobile && x.type == "1");

                return specifiedCustomer;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long CreateCustomer(Customer item)
        {            
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.Customer.Add(item);
                //entities.SignedPacel.Add((SignedPacel)item);
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.customerId;
            }
            else
            {
                return 0;
            }

            
        }

        public int RemoveCustomer(long id)
        {
            int returnCode = 0;
            using (PacelDbEntities entities = new PacelDbEntities())
            {
                Customer specifiedCustomer = new Customer();
                specifiedCustomer = entities.Customer.SingleOrDefault<Customer>(x => x.customerId == id);
                entities.Customer.Remove(specifiedCustomer);
                returnCode = entities.SaveChanges();
            }

            return returnCode;
        }

        public long UpdateCustomer(Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            long insertSuccess = 0;

            using (PacelDbEntities entities = new PacelDbEntities())
            {
                entities.Customer.Attach(item);
                var entry = entities.Entry(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                insertSuccess = entities.SaveChanges();
            }

            if (insertSuccess != 0)
            {
                return item.customerId;
            }
            else
            {
                return 0;
            }

        }               

        
    }
}