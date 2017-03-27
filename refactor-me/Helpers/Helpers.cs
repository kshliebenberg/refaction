using System.Data.SqlClient;
using System.Web;
using System.Configuration;

namespace refactor_me.Helpers
{
    public static class Helpers
    {

        public static string ConnectionString()
        {

            return ConfigurationManager.ConnectionStrings["DBConn"].ToString().Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
                        
        }

    }

}