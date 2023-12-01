using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business_Entity;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using NLog.Fluent;


namespace DataAccessLayer
{
    public class CustomerDataAccessLayer
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        

        public string CreateCustomer(Customer customer) // it does as the name suggests.  
        {
            try
            {
                if (customer == null)
                {
                    return "اطلاعات مشتری صحیح نیست!";
                }

                using (var db = new DB())
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return "ثبت اطلاعات مشتری با موفقیت انجام شد";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected exception occurred while creating a new customer. Caught in Dal");
                throw;
            }
            
        }    
        public DataTable GetActiveCustomers() //Gets active customers from database to populate the grid.
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.GetActiveCustomers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 180; // Timeout in seconds

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            var dataSet = new DataSet();
                            adapter.Fill(dataSet);

                            dataTable = dataSet.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex,"An error occurred while retrieving active customers from the database. Caught in Dal");
                throw;
            }

            return dataTable;
        }
        public DataTable SearchCustomers(string searchParameter, int index) // search results
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        switch (index)
                        {
                            case 0:
                                command.CommandText = "dbo.SearchCustomer";
                                break;
                            case 1:
                                command.CommandText = "dbo.SearchCustomerName";
                                break;
                            case 2:
                                command.CommandText = "dbo.SearchCustomerPhone";
                                break;
                        }

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Search", searchParameter);

                        using (SqlDataAdapter adp = new SqlDataAdapter(command))
                        {
                            adp.Fill(dt);
                        }
                        return dt;
                    }
                }
            }
            
            catch (Exception ex)
            {
                logger.Error(ex, "A general exception occurred during the search operation. caught in Dal");     
                throw;
            }

            
        }
        public bool DoesCustomerExist(Customer customer, int? existingCustomerId = null) // this one is a bit complicated lol.checks for existing customer by phone before create and update. Adds an optional existingCustomerId parameter
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.Phone))
            {
                return true;
            }
            try
            {
                using (var db = new DB())
                {
                    
                    return db.Customers.Any(existingCustomer => existingCustomer.Phone == customer.Phone && existingCustomerId != existingCustomer.Id); 
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected error occurred while checking customer existence, caught in DAL.");
                throw;
            }
        }
        public Customer GetCustomerById(int id) // get the entity by id for edit or deletion.
        {
            DB db = new DB();
            try
            {                
                Customer customer = db.Customers.FirstOrDefault(c => c.Id == id);               
                return customer;
            }
            catch (Exception ex)
            {                
                logger.Error(ex, $"An exception occurred in GetCustomerById when attempting to find customer with ID in dal {id}");               
                throw;
            }
        }
        public string UpdateCustomer(Customer customer, int id) //as it does as the name suggests.
        {
            try
            {
                using (var db = new DB())
                {
                    var existingCustomer = db.Customers.FirstOrDefault(c => c.Id == id);

                    if (existingCustomer != null)
                    {
                        existingCustomer.Name = customer.Name;
                        existingCustomer.Phone = customer.Phone;
                        db.SaveChanges();
                        return "ویرایش اطلاعات مشتری با موفقیت انجام شد!";
                    }
                    else
                    {
                        return "مشتری مورد نظر یافت نشد!";
                    }
                }
            
            
            }
            catch (Exception ex)
            {
                logger.Error(ex,"Problem in UpdateCustomer caugth in dal");
                throw;
            }
        }
        public string DeleteCustomer(int id) //Soft Delete. Chaneged the IsDeleted property value of the Customer to True
        {
            try
            {
                using (var db = new DB())
                {
                    var customer = db.Customers.FirstOrDefault(c => c.Id == id);

                    if (customer != null)
                    {
                        customer.IsDeleted = true;
                        db.SaveChanges();
                        return "حذف مشتری با موفقیت انجام شد!";
                    }
                    else
                    {
                        return "مشتری مورد نظر یافت نشد!";
                    }
                }
            }
            
            catch (Exception ex)
            {
                logger.Error(ex,"Problem in DeleteCustomer caugth in dal") ;
                throw;
            }
        }
        
    }
}
