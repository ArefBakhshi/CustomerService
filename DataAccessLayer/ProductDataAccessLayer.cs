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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public string CreateProduct(Product product) // it does as the name suggests.  
        {
            try
            {
                if (product == null)
                {
                    return "اطلاعات کالا صحیح نیست!";
                }

                using (var db = new DB())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return "ثبت اطلاعات کالا با موفقیت انجام شد";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected exception occurred while creating a new product. Caught in Dal");
                throw;
            }

        }
        public DataTable GetActiveProducts() //Gets active Product from database to populate the grid.
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.GetActiveProducts", connection))
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
                logger.Error(ex, "An error occurred while retrieving active Products from the database. Caught in Dal");
                throw;
            }

            return dataTable;
        }
        public DataTable SearchProducts(string searchParameter) // search results
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    

                    using (SqlCommand command = new SqlCommand("dbo.SearchProducts", connection))
                    {                   
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
        public bool DoesProductExist(Product product, int? existingProductId = null) // this one is a bit complicated lol.checks for existing Product by phone before create and update. Adds an optional existingCustomerId parameter
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                return true;
            }
            try
            {
                using (var db = new DB())
                {

                    return db.Products.Any(existingProduct => existingProduct.Name == product.Name && existingProductId != existingProduct.Id);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected error occurred while checking product existence, caught in DAL.");
                throw;
            }
        }
        public Product GetProductById(int id) // get the entity by id for edit or deletion.
        {
            DB db = new DB();
            try
            {
                Product product = db.Products.FirstOrDefault(c => c.Id == id);
                return product;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An exception occurred in GetProductById when attempting to find product with ID in dal {id}");
                throw;
            }
        }
        public string UpdateProduct(Product product, int id) //as it does as the name suggests.
        {
            try
            {
                using (var db = new DB())
                {
                    var existingProduct = db.Products.FirstOrDefault(c => c.Id == id);

                    if (existingProduct != null)
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.Price = product.Price;
                        existingProduct.Stock = product.Stock;
                        db.SaveChanges();
                        return "ویرایش اطلاعات کالا با موفقیت انجام شد";
                    }
                    else
                    {
                        return "کالا مورد نظر یافت نشد";
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex, "Problem in UpdateCustomer caugth in dal");
                throw;
            }
        }
        public string DeleteProduct(int id) //Soft Delete. Chaneged the IsDeleted property value of the Product to True
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
                        return "حذف کالا با موفقیت انجام شد";
                    }
                    else
                    {
                        return "کالا مورد نظر یافت نشد";
                    }
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex, "Problem in DeleteProduct caugth in dal");
                throw;
            }
        }
    }
}
