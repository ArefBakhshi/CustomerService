using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Entity;
using DataAccessLayer;
using System.Data;
using System.Text.RegularExpressions;
using NLog.Fluent;
using NLog;

namespace BusinessLogicLayer
{
    public class CustomerBll
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CustomerDataAccessLayer customerDal = new CustomerDataAccessLayer();


        public string CreateCustomer(Customer customer)
        {

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
               return "نام مشتری نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(customer.Name, @"^[\u0600-\u06FF\s]+$"))
            {
                return "نام مشتری فقط باید شامل حروف فارسی باشد";
            }

            if (string.IsNullOrWhiteSpace(customer.Phone))
            {
                return "شماره تماس نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(customer.Phone, @"^0\d{10}$"))
            {
                return "شماره تماس درست نیست";
            }

            if (!customerDal.DoesCustomerExist(customer))
            {
                try
                {
                    return customerDal.CreateCustomer(customer);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while attempting to create a new customer, Caught in bll", ex.Message);
                    throw;
                }
            }
            else
            {               
                 return "مشتری ای با همین شماره تماس در سیستم ثبت شده است";                
            }


        }
        public DataTable SearchCustomers(String searchParameter, int index)
        {
            try
            {
                return customerDal.SearchCustomers(searchParameter, index);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was a problem in retrieving searched customers from the database Caught in bll.", ex.Message);
                throw;
            }
        }
        public DataTable GetActiveCustomers()
        {
            try
            {
                return customerDal.GetActiveCustomers();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an error in get active customer from database, Caught in bll");
                throw;
            }
        }
        public Customer GetCustomerById(int id)
        {
            try 
            {
                return customerDal.GetCustomerById(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an error from GetCustomerById, Caught in bll");
                throw;
            }
            
        }
        public string UpdateCustomer(Customer customer, int id)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                return "نام مشتری نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(customer.Name, @"^[\u0600-\u06FF\s]+$"))
            {
                return "نام مشتری فقط باید شامل حروف فارسی باشد";
            }

            if (string.IsNullOrWhiteSpace(customer.Phone))
            {
                return "شماره تماس نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(customer.Phone, @"^0\d{10}$"))
            {
                return "شماره تماس درست نیست";
            }

            if (!customerDal.DoesCustomerExist(customer, id))
            {
                try
                {
                   
                    return customerDal.UpdateCustomer(customer,id);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while attempting to updating the customer, Caught in bll", ex.Message);
                    throw;
                }

            }
            else
            {
                return "مشتری ای با همین شماره تماس در سیستم ثبت شده است";
            }
            
        }
        public string DeleteCustomer(int id)
        {
            try
            {
                return customerDal.DeleteCustomer(id);
            }
            
            catch (Exception ex)
            {
                logger.Error(ex, "There was an error from DeleteCustomer, Caught in bll");
                throw;
            }
        }
    }
}
