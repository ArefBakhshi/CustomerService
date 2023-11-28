using Business_Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductDataAccessLayer
    {
        public string CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    return "اطلاعات مشتری صحیح نیست!";
                }

                using (var db = new DB())
                {
                    db.Products.Add(product);
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
        public DataTable GetActiveProducts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;

            DataTable dataTable = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.GetActiveProduct", connection))
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
        }// for getting the list of active Products in database to be put in data grid.
        public DataTable SearchProducts(string searchParameter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.SearchProducts";
                        command.Parameters.AddWithValue("@Search", searchParameter);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            var dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            return dataSet.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
                return null; // or throw an exception, depending on your requirements
            }
        }// For Searching in the existing products.
        public bool DoesProductExist(Product product) // Checking if new Product already exists in database .
        {
            try
            {
                using (var db = new DB())
                {
                    var existingProduct = db.Products.FirstOrDefault(c => c.Name == product.Name);

                    if (existingProduct == null)
                    {
                        return true; // Customer does not exist in the database
                    }
                    else
                    {
                        return false; // Customer with the same phone number already exists
                    }
                }
            }
            catch (Exception)
            {
                // Handle or log the exception if necessary
                return false; // Return false by default in case of any exception
            }
        }
        public Customer GetCustomerById(int id) // gets a Product by id to be used for delete and update.
        {
            DB db = new DB();
            return db.Customers.Where(i => i.Id == id).FirstOrDefault();
        }
        public string UpdateProduct(Product product, int id)
        {
            try
            {
                using (var db = new DB())
                {
                    var existingProducts = db.Products.FirstOrDefault(c => c.Id == id);

                    if (existingProducts != null)
                    {
                        existingProducts.Name = product.Name;
                        existingProducts.Price = product.Price;
                        existingProducts.Stock = product.Stock;

                        db.SaveChanges();
                        return "ثبت اطلاعات مشتری با موفقیت انجام شد!";
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
        public string DeleteProduct(int id)
        {
            try
            {
                using (var db = new DB())
                {
                    var product = db.Products.FirstOrDefault(c => c.Id == id);

                    if (product != null)
                    {
                        product.IsDeleted = true;
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
