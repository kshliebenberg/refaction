using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using refactor_me.Helpers;
using System.Data;

namespace refactor_me.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Refactored - changed the parameter passed to the <see cref="LoadProducts(string)"/> function to an empty string instead of a <see cref="null"/>.
        /// </remarks>
        public Products()
        {
            Items = new List<Product>();

            LoadProducts("");
        }

        public Products(string name)
        {
            Items = new List<Product>();

            LoadProducts(name);
        }

        /// <summary>
        /// Load all products based that contain a specific string in their name field. 
        /// </summary>
        /// <remarks>
        /// Refactored - Moved sql query to a stored proc, added proper handling of query parameters and moved the <see cref="Product"/> object creation
        /// to this method - reducing the need to call the database multiple times to instantiate one <see cref="Product"/> object. 
        /// </remarks>
        private void LoadProducts(string name)
        {
                      
            DataAccess DA = new DataAccess();

            string query = "getProductLikeName";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@Name", name));

            using (DataSet ds = DA.ExecQueryReturnResults(query, CommandType.StoredProcedure, sqlparams))
            {

                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {

                    Items.Add(
                        new Product(Guid.Parse(dr["id"].ToString()), dr["name"].ToString(), dr["Description"].ToString(), Decimal.Parse(dr["Price"].ToString()),
                                    Decimal.Parse(dr["DeliveryPrice"].ToString()))
                    );

                }

            }
            
        }

    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        [JsonIgnore]
        public bool ProductFound { get; private set; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        /// <summary>
        /// Instantiate a product based on its ID value.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Refactored - moved SQL query to a stored proc and implemented proper handling of parameters.</remarks>
        public Product(Guid id)
        {
            DataAccess _DA = new DataAccess();
            
            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("id", id));

            string query = "getProductById";

            using (DataSet ds = _DA.ExecQueryReturnResults(query, CommandType.StoredProcedure, sqlparams))
            {

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {

                    DataRow dr = dt.Rows[0];

                    IsNew = false;
                    ProductFound = true;
                    Id = Guid.Parse(dr["Id"].ToString());
                    Name = dr["Name"].ToString();
                    Description = (DBNull.Value == dr["Description"]) ? null : dr["Description"].ToString();
                    Price = decimal.Parse(dr["Price"].ToString());
                    DeliveryPrice = decimal.Parse(dr["DeliveryPrice"].ToString());

                }
                else
                {

                    ProductFound = false;

                    IsNew = true;

                    return;

                }
                
            }
            
        }

        public Product(Guid _Id, string _Name, string _Description, decimal _Price, decimal _DeliveryPrice)
        {

            IsNew = false; 

            Id = _Id;

            Name = _Name;

            Description = _Description;

            Price = _Price;

            DeliveryPrice = _DeliveryPrice;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Refactored - Removed ternary operator to increase readability - split inserting and updating out into separate functions - 
        /// and implemented better handling of query parameters. 
        /// </remarks>
        public void Save()
        {

            DataAccess _DA = new DataAccess();

            if (IsNew)
            {

                Insert(_DA);

            }
            else
            {

                Update(_DA);

            }

        }

        private void Insert(DataAccess DA)
        {

            string query = "insertProduct";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@Id", Id));
            sqlparams.Add(new SqlParameter("@Name", Name));
            sqlparams.Add(new SqlParameter("@Description", Description));
            sqlparams.Add(new SqlParameter("@Price", Price));
            sqlparams.Add(new SqlParameter("@DeliveryPrice", DeliveryPrice));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlparams);

        }

        private void Update(DataAccess DA)
        {

            string query = "updateProduct";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@Id", Id));
            sqlparams.Add(new SqlParameter("@Name", Name));
            sqlparams.Add(new SqlParameter("@Description", Description));
            sqlparams.Add(new SqlParameter("@Price", Price));
            sqlparams.Add(new SqlParameter("@DeliveryPrice", DeliveryPrice));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlparams);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Refactored - moved sql query into a stored proc - and added better handling of query parameters. 
        /// </remarks>
        public void Delete()
        {

            List<ProductOption> Options = new ProductOptions(Id).Items;

            foreach (ProductOption option in Options )
                option.Delete();

            DataAccess DA = new DataAccess();

            string query = "deleteProduct";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@id", Id));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlparams);
            
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        //Not needed - a product Id is required to get production options...
        //public ProductOptions()
        //{

        //    //Do nothing.
        //    //LoadProductOptions("");
        //}

        public ProductOptions(Guid productId)
        {
            Items = new List<ProductOption>();

            LoadProductOptions(productId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <remarks>
        /// Refactored - Moved sql query to a stored proc, added proper handling of query parameters and moved the <see cref="ProductOption"/> object creation
        /// to this method - reducing the need to call the database multiple times to instantiate one <see cref="ProductOption"/> object. 
        /// </remarks>
        private void LoadProductOptions(Guid productId)
        {

            DataAccess DA = new DataAccess();

            string query = "getProductOptionByProduct";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@ProductId", productId));

            using (DataSet ds = DA.ExecQueryReturnResults(query, CommandType.StoredProcedure, sqlparams))
            {

                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {

                    Items.Add(new ProductOption(Guid.Parse(dr["iD"].ToString()), Guid.Parse(dr["ProductId"].ToString()),
                                                 dr["Name"].ToString(), dr["Description"].ToString()
                                                ));

                }

            }

         }

    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        [JsonIgnore]
        public bool ProductOptionFound { get; private set; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid _Id, Guid _ProductId, string _Name, string _Description)
        {

            IsNew = false;

            Id = _Id;

            ProductId = _ProductId;

            Name = _Name;

            Description = _Description;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// <remarks>Refactored - moved SQL query to a stored proc and implemented proper handling of parameters.</remarks>
        /// </remarks>
        public ProductOption(Guid id)
        {

            IsNew = true;

            DataAccess DA = new DataAccess();

            string query = "getProductOptionById";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            sqlparams.Add(new SqlParameter("@Id", id));

            using (DataSet ds = DA.ExecQueryReturnResults(query, CommandType.StoredProcedure, sqlparams))
            {

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {

                    DataRow dr = dt.Rows[0];

                    IsNew = false;

                    Id = Guid.Parse(dr["Id"].ToString());
                    ProductId = Guid.Parse(dr["ProductId"].ToString());
                    Name = dr["Name"].ToString();
                    Description = (DBNull.Value == dr["Description"]) ? null : dr["Description"].ToString();

                    ProductOptionFound = true;

                }
                else
                {

                    IsNew = true;
                    ProductOptionFound = false;

                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Refactored - Removed ternary operator to increase readability - split inserting and updating out into separate functions - 
        /// and implemented better handling of query parameters. 
        /// </remarks>
        public void Save()
        {

            DataAccess DA = new DataAccess();

            if (IsNew)
            {

                Insert(DA);

            }else
            {

                Update(DA);

            }
            
        }

        public void Insert(DataAccess DA)
        {

            string query = "insertProductOption";

            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@Id",Id));
            sqlParams.Add(new SqlParameter("@ProductId", ProductId));
            sqlParams.Add(new SqlParameter("@Name", Name));
            sqlParams.Add(new SqlParameter("@Description", Description));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlParams);

        }

        public void Update(DataAccess DA)
        {

            string query = "UpdateProductOption";

            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@Id", Id));
            sqlParams.Add(new SqlParameter("@ProductId", ProductId));
            sqlParams.Add(new SqlParameter("@Name", Name));
            sqlParams.Add(new SqlParameter("@Description", Description));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlParams);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Refactored - moved sql query into a stored proc - and added better handling of query parameters. 
        /// </remarks>
        public void Delete()
        {

            DataAccess DA = new DataAccess();

            string query = "deleteProductOption";

            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@Id", Id));

            DA.ExecQueryNoResults(query, CommandType.StoredProcedure, sqlParams);
            
        }

    }

}