using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSDashboard
{
    public partial class LoginPg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName.Focus();
            if (IsPostBack && Session["user"] != null)
            {
                UserName.Text = Session["user"].ToString();
            }
            if (!IsPostBack)
            {
                //UserNameLabel.Text = Google.API.Translate.Translator.Translate(UserNameLabel.Text, Google.API.Translate.Language.English,Google.API.Translate.Language.Hindi);
                DOB.Enabled = false;
                Password.Enabled = false;
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ValidateUser(UserName.Text, Password.Text, DOB.Text.ToUpper()))
            {
                Session["user"] = UserName.Text;
                Response.Redirect("~/Dashboard.aspx");
            }
            else
            {
                FailureText.Text = "Invalid UserId or Password";
                UserName.Text = "";
                Password.Text = "";
                UserName.Focus();
            }
        }

        private bool ValidateUser(string UserName, string Password, string Dob)
        {
            bool boolReturnValue = false;
            OracleConnection con = null;
            string adminPwd = ConfigurationManager.AppSettings["adminPwd"];
            if (UserName.Equals("ADMIN"))
            {
                if (!String.IsNullOrEmpty(adminPwd) && adminPwd.Equals(Password))
                {
                    return true;
                }
                else
                {
                    FailureText.Text = "Invalid Admin Password ...<br>";                    
                    Response.Redirect("~/LoginPg.aspx");
                    return false;
                }
              
            }
            else
            {
                string query = " select  (select  TO_DATE(dob,'DD/MON/YY') - TO_DATE('" + Dob + "','DD/MON/YY') from employee where upper(empid) = '" + UserName + "') udob, (select decryptpass(pwd) from users where userid = '" + UserName + "') password from dual";

                try
                {
                    string strcon = ConfigurationManager.ConnectionStrings["HrmsConnection"].ConnectionString;
                    if (strcon == null || string.IsNullOrEmpty(strcon))
                    {
                        throw new Exception("Couldn't find connection string in web.config.");
                    }
                    con = new OracleConnection(strcon);
                    con.Open();

                    using (con)
                    {
                        OracleCommand cmd = new OracleCommand(query, con);
                        OracleDataReader dr = cmd.ExecuteReader();
                        string field;
                        if (DOB.Enabled)
                        {
                            field = "udob";
                        }
                        else
                        {
                            field = "password";
                        }
                        while (dr.Read())
                        {
                            string temp = dr[field].ToString();
                            if (Password == temp || temp == "0")
                            {
                                boolReturnValue = true;
                                dr.Close();
                                break;
                            }
                        }
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    FailureText.Text = "Cannot get the connection at this moment ...<br>";
                    Logger.log(ex, UserName);
                    Response.Redirect("~/LoginPg.aspx");
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
                return boolReturnValue;
            }

        }

        protected void checkCategory(object sender, EventArgs e)
        {
            UserName.Text = UserName.Text.ToUpper();
            string user = UserName.Text;
            if (user.Equals("ADMIN"))
            {
                Password.Enabled = true;
                DOBReqd.Enabled = false;
                return;
            }
            else
            {

                OracleConnection con = null;
                bool isWorkman = false;
                string query = "select catgcode from employee where upper(empid)='" + user + "'";
                try
                {
                    string strcon = ConfigurationManager.ConnectionStrings["HrmsConnection"].ConnectionString;
                    con = new OracleConnection(strcon);
                    con.Open();
                    using (con)
                    {
                        OracleCommand cmd = new OracleCommand(query, con);
                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string temp = dr["catgcode"].ToString();
                            if (temp == "WM")
                            {
                                isWorkman = true;
                                dr.Close();
                                break;
                            }
                        }
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    FailureText.Text = "Cannot get the connection at this moment ...<br>";
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                    if (isWorkman)
                    {
                        DOB.Enabled = true;
                        DOBReqd.Enabled = true;
                        Password.Enabled = false;
                        PasswordRequired.Enabled = false;
                    }
                    else
                    {
                        DOB.Enabled = false;
                        DOBReqd.Enabled = false;
                        Password.Enabled = true;
                        PasswordRequired.Enabled = true;
                    }

                }
            }
        }
    }
}
