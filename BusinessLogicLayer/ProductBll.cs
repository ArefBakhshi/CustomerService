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
using System.Globalization;
using System.Collections;

namespace BusinessLogicLayer
{
    public class ProductBll
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ProductDataAccessLayer productDal = new ProductDataAccessLayer();


        public string CreateProduct(Product product)
        {

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return "نام کالا نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(product.Name, @"^[\u0600-\u06FF\s]+$"))
            {
                return "نام کالا فقط باید شامل حروف فارسی باشد";
            }



            string pricePattern = @"^[0-9]+$";
            string priceAsString = product.Price.ToString("F0", CultureInfo.InvariantCulture);

            if (!Regex.IsMatch(priceAsString, pricePattern))
            {
                return "قیمت کالا باید به صورت عدد صحیح مثبت و بدون اعشار وارد شود.";
            }

            // Validate the stock
            if (product.Stock <= 0 || !Regex.IsMatch(product.Stock.ToString(), @"^\d+$"))
{
                return "موجودی کالا باید به صورت عدد صحیح مثبت بزرگتر از صفر وارد شود.";
            }

            if (!productDal.DoesProductExist(product))
            {
                try
                {
                    return productDal.CreateProduct(product);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while attempting to create a new product, Caught in bll", ex.Message);
                    throw;
                }
            }
            else
            {
                return "کالایی با همین نام در سیستم ثبت شده است";
            }


        }
        public DataTable SearchProducts(String searchParameter)
        {
            try
            {
                return productDal.SearchProducts(searchParameter);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was a problem in retrieving searched Products from the database Caught in bll.", ex.Message);
                throw;
            }
        }
        public DataTable GetActiveProducts()
        {
            try
            {
                return productDal.GetActiveProducts();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an error in get active Products from database, Caught in bll");
                throw;
            }
        }
        public Product GetProductById(int id)
        {
            try
            {
                return productDal.GetProductById(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "There was an error from GetProductById, Caught in bll");
                throw;
            }

        }
        public string UpdateProduct(Product product, int id)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return "نام مشتری نمیتواند خالی باشد";
            }

            if (!Regex.IsMatch(product.Name, @"^[\u0600-\u06FF\s]+$"))
            {
                return "نام مشتری فقط باید شامل حروف فارسی باشد";
            }
            string pricePattern = @"^\d+$";
            string priceAsString = product.Price.ToString("F0", CultureInfo.InvariantCulture);

            if (!Regex.IsMatch(priceAsString, pricePattern))
            {
                return "قیمت کالا باید به صورت عدد صحیح مثبت و بدون اعشار وارد شود.";
            }

            if (product.Stock <= 0)
            {
                return "موجودی کالا باید به صورت عدد صحیح مثبت بزرگتر از صفر وارد شود.";
            }


            if (!productDal.DoesProductExist(product, id))
            {
                try
                {

                    return productDal.UpdateProduct(product, id);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while attempting to updating the Product, Caught in bll", ex.Message);
                    throw;
                }

            }
            else
            {
                return "مشتری ای با همین شماره تماس در سیستم ثبت شده است";
            }

        }
        public string DeleteProduct(int id)
        {
            try
            {
                return productDal.DeleteProduct(id);
            }

            catch (Exception ex)
            {
                logger.Error(ex, "There was an error from DeleteProduct, Caught in bll");
                throw;
            }
        }
    }
}
