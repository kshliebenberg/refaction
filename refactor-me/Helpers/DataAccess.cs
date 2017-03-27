using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace refactor_me.Helpers
{
    public class DataAccess
    {

        private static string ConnectionString;

        [DebuggerStepThrough()]
        public DataAccess()
        {

            ConnectionString = Helpers.ConnectionString();

        }

        public CommandType SProc
        {

            get { return CommandType.StoredProcedure; }

        }

        /// <summary>
        /// Execute a given query string against the database and return the results in the
        /// form of a dataset.
        /// </summary>
        /// <param name="query">
        /// The query / stored proc to be executed. 
        /// </param>
        /// <param name="cmdType">
        /// The type of query to be executed. 
        /// </param>
        /// <param name="parameters">
        /// An array of SQLParameters to be passed the the SQLCommand object. 
        /// Pass null if there are no parameters required. 
        /// </param>
        /// <returns>
        /// A dataset containing the contents of the query
        /// </returns>
        //[DebuggerStepThrough()]
        public DataSet ExecQueryReturnResults(string query, CommandType cmdType, SqlParameter[] parameters)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {

                        cmd.Parameters.AddRange(parameters);

                    }

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {

                        DataSet returnDS = new DataSet();

                        try
                        {

                            DA.Fill(returnDS);

                        }
                        catch (Exception)
                        {

                            //If I don't do this then any error thrown by DA.Fill() above vanishes :/
                            throw;

                        }

                        return returnDS;

                    }

                }

            }

        }

        /// <summary>
        /// Execute a given query string against the database and return the results in the
        /// form of a dataset.
        /// </summary>
        /// <param name="query">
        /// The query / stored proc to be executed. 
        /// </param>
        /// <param name="cmdType">
        /// The type of query to be executed. 
        /// </param>
        /// <param name="parameters">
        /// An array of SQLParameters to be passed the the SQLCommand object. 
        /// Pass null if there are no parameters required. 
        /// </param>
        /// <returns>
        /// A dataset containing the contents of the query
        /// </returns>
        //[DebuggerStepThrough()]
        public DataSet ExecQueryReturnResults(string query, CommandType cmdType, List<SqlParameter> pars)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    if (pars != null)
                    {

                        cmd.Parameters.AddRange(pars.ToArray());

                    }

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {

                        DataSet returnDS = new DataSet();

                        try
                        {

                            DA.Fill(returnDS);

                        }
                        catch (Exception)
                        {

                            //If I don't do this then any error thrown by DA.Fill() above vanishes :/
                            throw;

                        }

                        return returnDS;

                    }

                }

            }

        }

        /// <summary>
        /// Execute a given query string against the database and return the results in the
        /// form of a dataset.
        /// </summary>
        /// <param name="query">
        /// The query / stored proc to be executed. 
        /// </param>
        /// <param name="cmdType">
        /// The type of query to be executed. 
        /// </param>
        /// <param name="parameters">
        /// An array of SQLParameters to be passed the the SQLCommand object. 
        /// Pass null if there are no parameters required. 
        /// </param>
        /// <returns>
        /// A dataset containing the contents of the query
        /// </returns>
        //[DebuggerStepThrough()]
        public DataSet ExecQueryReturnResults(string query, CommandType cmdType)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {

                        DataSet returnDS = new DataSet();

                        try
                        {

                            DA.Fill(returnDS);

                        }
                        catch (Exception)
                        {

                            //If I don't do this then any error thrown by DA.Fill() above vanishes :/
                            throw;

                        }

                        return returnDS;

                    }

                }

            }

        }

        /// <summary>
        /// Execute a given query string against the database and return the results in the
        /// form of a dataset.
        /// </summary>
        /// <param name="query">
        /// The query / stored proc to be executed. 
        /// </param>
        /// <param name="cmdType">
        /// The type of query to be executed. 
        /// </param>
        /// <param name="parameters">
        /// An array of SQLParameters to be passed the the SQLCommand object. 
        /// Pass null if there are no parameters required. 
        /// </param>
        /// <returns>
        /// A dataset containing the contents of the query
        /// </returns>
        [DebuggerStepThrough()]
        public DataSet ExecQueryReturnResults(string query, CommandType cmdType, SqlParameter[] parameters,
            SqlConnection conn, SqlTransaction tran)
        {

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Transaction = tran;

                cmd.CommandType = cmdType;

                if (parameters != null)
                {

                    cmd.Parameters.AddRange(parameters);

                }

                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {

                    DataSet returnDS = new DataSet();

                    try
                    {

                        DA.Fill(returnDS);

                    }
                    catch (Exception)
                    {

                        //If I don't do this then any error thrown by DA.Fill() above vanishes :/
                        throw;

                    }

                    return returnDS;

                }

            }

        }

        public DataSet ExecQueryReturnResults(string query, CommandType cmdType, List<SqlParameter> pars,
            SqlConnection conn, SqlTransaction tran)
        {

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Transaction = tran;

                cmd.CommandType = cmdType;

                if (pars != null)
                {

                    cmd.Parameters.AddRange(pars.ToArray());

                }

                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {

                    DataSet returnDS = new DataSet();

                    try
                    {

                        DA.Fill(returnDS);

                    }
                    catch (Exception)
                    {

                        //If I don't do this then any error thrown by DA.Fill() above vanishes :/
                        throw;

                    }

                    return returnDS;

                }

            }

        }

        /// <summary>
        /// Execute a given query against a database - but do not return any results. 
        /// </summary>
        /// <param name="query">
        /// The query / stored proc to be executed. 
        /// </param>
        /// <param name="cmdType">
        /// The type of query to be executed. 
        /// </param>
        /// <param name="parameters">
        /// An array of SQLParameters to be passed the the SQLCommand object. 
        /// Pass null if there are no parameters required. 
        /// </param>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
        [DebuggerStepThrough()]
        public int ExecQueryNoResults(string query, CommandType cmdType, SqlParameter[] parameters)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {

                        cmd.Parameters.AddRange(parameters);

                    }

                    return cmd.ExecuteNonQuery();

                }

            }

        }

        [DebuggerStepThrough()]
        public int ExecQueryNoResults(string query, CommandType cmdType, List<SqlParameter> parameters)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {

                        cmd.Parameters.AddRange(parameters.ToArray());

                    }

                    return cmd.ExecuteNonQuery();

                }

            }

        }

        [DebuggerStepThrough()]
        public int ExecQueryNoResults(string query, CommandType cmdType, List<SqlParameter> parameters
            , SqlConnection conn, SqlTransaction Tran)
        {

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Transaction = Tran;

                cmd.CommandType = cmdType;

                cmd.CommandText = query;

                if (parameters != null)
                {

                    cmd.Parameters.AddRange(parameters.ToArray());

                }

                return cmd.ExecuteNonQuery();

            }

        }

        [DebuggerStepThrough()]
        public SqlConnection GetConnection()
        {

            return new SqlConnection(ConnectionString);

        }

        [DebuggerStepThrough()]
        public object ExecuteScalar(string query, CommandType cmdType, List<SqlParameter> parameters)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.CommandType = cmdType;

                    cmd.CommandText = query;

                    if (parameters != null)
                    {

                        cmd.Parameters.AddRange(parameters.ToArray());

                    }

                    return cmd.ExecuteScalar();

                }

            }

        }

        [DebuggerStepThrough()]
        public object ExecuteScalar(string query, CommandType cmdType, List<SqlParameter> parameters, SqlConnection conn, SqlTransaction Tran)
        {

            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = conn;

                cmd.Transaction = Tran;

                cmd.CommandType = cmdType;

                cmd.CommandText = query;

                if (parameters != null)
                {

                    cmd.Parameters.AddRange(parameters.ToArray());

                }

                return cmd.ExecuteScalar();

            }

        }

    }

}