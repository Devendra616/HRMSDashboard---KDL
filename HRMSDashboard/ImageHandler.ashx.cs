using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace HRMSDashboard
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        OracleConnection con = null;        

        public void ProcessRequest(HttpContext context) 
        {
            if (context.Request.QueryString["id"] != null)
            {
                string userId = context.Request.QueryString["id"];
                string dbcon = ConfigurationManager.ConnectionStrings["HrmsConnection"].ConnectionString;
                string qEmpPhoto = "Select nvl(emppic,'') from employee where empid = '" + userId + "'";

                if (dbcon == null || string.IsNullOrEmpty(dbcon))
                {
                    throw new Exception("Couldn't find connection string in web.config.");
                }
                con = new OracleConnection(dbcon);
               
                try {
                    con.Open();
                    using (con)
                    {
                        OracleCommand cmd = new OracleCommand(qEmpPhoto, con);
                        byte[] buf = (byte[])cmd.ExecuteScalar();
                        context.Response.BinaryWrite(buf);
                    }
                }
                catch(Exception e) {

                }
                con.Close();
            }
            else
            {
                context.Response.Write("No Image Found");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}