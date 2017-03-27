using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{

    /// <summary>
    /// An API controller class that handles all the Web API methods for CRUD operations on Products and Product Options.
    /// </summary>
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {

        /// <summary>
        /// Get a list of all products currently in the database.
        /// </summary>
        /// <returns>
        /// A list of all products in the database.
        /// </returns>
        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {

            try
            {

                return Ok(new Products());

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
            

        }

        /// <summary>
        /// Searches for products in the database by name
        /// </summary>
        /// <param name="name">The string on which you want to search</param>
        /// <returns>
        /// A list of all products that contain the search string in their name. 
        /// </returns>
        [Route]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            
            try
            {

                return Ok(new Products(name));

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }

        }

        /// <summary>
        /// Get a specific product by it's Id value. 
        /// </summary>
        /// <param name="id">The id value of the product that you wish to see.</param>
        /// <returns>
        /// The product that corresponds to the supplied id. 
        /// </returns>
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {

            try
            {

                Product product = new Product(id);

                if (!product.ProductFound)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return Ok(product);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
            
        }

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="product">The product that needs to be added to the database.</param>
        /// <returns>
        /// An HTTP response code indicating whether the creation was successful or not. 
        /// </returns>
        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            
            try
            {

                product.Save();

                return Ok(product);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }

        }

        /// <summary>
        /// Updates a given product. 
        /// </summary>
        /// <param name="id">The id of the product to be updated.</param>
        /// <param name="product">The product containing the updated data</param>
        /// <returns>
        /// An HTTP response code indicating whether the operation was successful or not. 
        /// </returns>
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
                       
            try
            {

                if (id != product.Id)
                {

                    return BadRequest("The supplied id and product.id values did not match...");

                }

                Product orig = new Product(product.Id, product.Name, product.Description, product.Price, product.DeliveryPrice);
                
                if (!orig.IsNew)
                {
                    orig.Save();
                    return Ok(orig);
                }
                else
                {

                    return NotFound();

                }

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
            
        }

        /// <summary>
        /// Deletes a specific product from the database based on its id value. 
        /// </summary>
        /// <param name="id">The id of the product that you wish to delete</param>
        /// <returns>
        /// An HTTP response code indicating whether the operation was successful or not.
        /// </returns>
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {

                Product product = new Product(id);

                if (!product.ProductFound)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                product.Delete();

                return Ok(product);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }

        }

        /// <summary>
        /// Get all the product options for a specific product id. 
        /// </summary>
        /// <param name="productId">The product id for which options have been requested.</param>
        /// <returns>
        /// A list of product options linked to the specified Id.
        /// </returns>
        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            
            try
            {

                return Ok(new ProductOptions(productId));

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
        }

        /// <summary>
        /// Get a specific product option for a specific product. 
        /// </summary>
        /// <param name="productId">The specific product id</param>
        /// <param name="id">The specific product option id</param>
        /// <returns>
        /// The requested product option
        /// </returns>
        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {

            try
            {

                ProductOption option = new ProductOption(id);

                if (!option.ProductOptionFound)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return Ok(option);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
        }

        /// <summary>
        /// Creates a new product option for the specified product. 
        /// </summary>
        /// <param name="productId">The id of the product</param>
        /// <param name="option">The product option object that needs to be added</param>
        /// <returns></returns>
        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
                        
            try
            {
                option.Save();

                return Ok(option);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }

        }

        /// <summary>
        /// Updates the values of a specific product option for a specific product. 
        /// </summary>
        /// <param name="id">The specific product id</param>
        /// <param name="option">The product option object with updated values</param>
        /// <returns>
        /// An HTTP response code indicating whether the operation was successful or not. 
        /// </returns>
        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid id, ProductOption option)
        {
            try
            {

                ProductOption orig = new ProductOption(option.Id, option.ProductId, option.Name, option.Description);

                if (id != option.Id)
                {

                    return BadRequest("The supplied id and option.id values do not match.");

                }

                if (!orig.IsNew)
                {

                    orig.Save();

                    return Ok(option);

                }
                else
                {

                    return NotFound();

                }            

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
            
        }

        /// <summary>
        /// Deletes a specific product option from a specific product
        /// </summary>
        /// <param name="id">The id of the product option to be deleted</param>
        /// <returns></returns>
        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {

            try
            {

                ProductOption opt = new ProductOption(id);

                opt.Delete();

                return Ok(opt);

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }
                        
        }

    }

}
