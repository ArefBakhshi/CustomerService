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


namespace DataAccessLayer
{
    public class CustomerDataAccessLayer
    {
        
        
        public string CreateCustomer(Customer customer)
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
            catch (DbUpdateException e)
            {
                return "ثبت اطلاعات مشتری با مشکل مواجه شد: خطای پایگاه داده\n" + e.Message;
            }
            catch (Exception e)
            {
                return "ثبت اطلاعات مشتری با مشکل مواجه شد: خطای ناشناخته\n" + e.Message;
            }
        }
        public DataTable GetActiveCustomers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;

            DataTable dataTable = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.GetActiveCustomers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

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
                // Handle the exception (e.g., logging, error reporting, etc.)
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return dataTable;
        }// for getting the list of active users in database to be put in data grid.
        public DataTable SearchCustomers(string searchParameter, int index) // For Searching in the existing customers. 
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;


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
                        default:
                            throw new ArgumentException("Invalid index value.");
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", searchParameter);

                    using (SqlDataAdapter adp = new SqlDataAdapter(command))
                    {
                        var ds = new DataSet();
                        adp.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        public bool DoesCustomerExist(Customer newCustomer)
        {
            try
            {
                using (var db = new DB())
                {
                    var existingCustomer = db.Customers.Any(customer => newCustomer.Phone == customer.Phone);
                    return !existingCustomer; // Return true if no matching customer is found, false otherwise
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred while checking customer existence: " + ex.Message);
                return false; // Return false by default in case of any exception
            }
        } // checks if the customer which is being created, exists in the database or not.
        public Customer GetCustomerById(int id) // gets a customer by id to be used for delete and update.
        {
            DB db = new DB();
            return db.Customers.Where(i => i.Id == id).FirstOrDefault();
        }  
        public string UpdateCustomer(Customer customer, int id)
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
            catch (DbUpdateException e)
            {
                return "ویرایش اطلاعات مشتری با مشکل مواجه شد: خطای پایگاه داده\n" + e.Message;
            }
            catch (InvalidOperationException e)
            {
                return "ویرایش اطلاعات مشتری با مشکل مواجه شد: خطای عملیات غیرمجاز\n" + e.Message;
            }
            catch (Exception e)
            {
                return "ویرایش اطلاعات مشتری با مشکل مواجه شد: خطای ناشناخته\n" + e.Message;
            }
        }
        public string DeleteCustomer(int id)
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
            catch (DbUpdateException e)
            {
                return "حذف مشتری با مشکل مواجه شد: خطای پایگاه داده\n" + e.Message;
            }
            catch (Exception e)
            {
                return "حذف مشتری با مشکل مواجه شد: خطای ناشناخته\n" + e.Message;
            }
        }
        
    }
}
