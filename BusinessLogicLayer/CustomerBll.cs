using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Entity;
using DataAccessLayer;
using System.Data;


namespace BusinessLogicLayer
{
    public class CustomerBll
    {
        CustomerDataAccessLayer customerDal = new CustomerDataAccessLayer();
        public string CreateCustomer(Customer customer) 
        {
            if (customerDal.DoesCustomerExist(customer)) 
            {
                return customerDal.CreateCustomer(customer);
            }
            else { return "کاربری با همین شماره تماس در سیستم ثبت شده است"; }
        }
        public DataTable SearchCustomers(String searchParameter, int index)
        {
           return customerDal.SearchCustomers(searchParameter, index);
        }


        public DataTable GetActiveCustomers()
        {
            
            return customerDal.GetActiveCustomers();
        }

        public Customer GetCustomerById(int id)
        {
           return customerDal.GetCustomerById(id);
        }
    }
}
