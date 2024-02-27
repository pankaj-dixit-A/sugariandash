using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class Sugar_pgeGenerateCustomerLogin : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string qry = string.Empty;
    int company_code = 0;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        company_code = Convert.ToInt32(Session["Company_Code"].ToString());
        user = Session["user"].ToString();
        if (!Page.IsPostBack)
        {
            btnGenerate.Text = "Create Login";
            btnShowGeneratedAccounts.Text = "Show Generated";
            pnlAccounts.Visible = true;
            pnlGeneratedAccounts.Visible = false;

            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                this.BindGrid();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void BindGrid()
    {
        qry = "Select Ac_Code,Ac_Name_E,Email_Id,Mobile_No from " + tblPrefix + "AccountMaster where Company_Code=" + company_code + " AND Ac_Code NOT IN(1,2,3) AND Ac_Code NOT IN(Select User_Name from tblUser WHERE User_Type='C' and Company_Code=" + company_code + ") and (Ac_Code like '%" + txtSearch.Text + "%' or Ac_Name_E like '%" + txtSearch.Text + "%') order by Ac_Code";
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                grdAccounts.DataSource = dt;
                grdAccounts.DataBind();
            }
            else
            {
                grdAccounts.DataSource = null;
                grdAccounts.DataBind();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            this.BindGrid();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void grdAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAccounts.PageIndex = e.NewPageIndex;
        this.BindGrid();
        //grdAccounts.DataBind();
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        string[] arr;
        string accountcodes = string.Empty;
        string str = "";
        string Ac_Code = string.Empty;
        string Email = string.Empty;
        string Mobile = string.Empty;
        string password = string.Empty;
        if (btnGenerate.Text == "Create Login")
        {
            if (grdAccounts.Rows.Count > 0)
            {
                for (int i = 0; i < grdAccounts.Rows.Count; i++)
                {
                    CheckBox chk = grdAccounts.Rows[i].FindControl("grdAccountsCB") as CheckBox;
                    if (chk != null && chk.Checked == true)
                    {
                        Ac_Code = grdAccounts.Rows[i].Cells[0].Text.ToString();
                        Email = Server.HtmlDecode(grdAccounts.Rows[i].Cells[2].Text.ToString());
                        Mobile = Server.HtmlDecode(grdAccounts.Rows[i].Cells[3].Text.ToString());

                        if (string.IsNullOrWhiteSpace(Email))
                        {
                        }
                        else
                        {
                            GeneratePassword gp = new GeneratePassword();
                            password = gp.NewPassword();
                            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                            {
                                obj.flag = 1;
                                obj.tableName = "tblUser";
                                obj.columnNm = "User_Name,Password,User_Type,Company_Code";
                                obj.values = "'" + Ac_Code + "','" + password + "','C','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                                obj.insertAccountMaster(ref str);
                                list.Add(Ac_Code);
                            }
                            SendEmail(Email, Ac_Code, password);

                            arr = list.ToArray();
                            accountcodes = string.Join(",", arr);
                            string qry = "update " + tblPrefix + "AccountMaster SET Is_Login=1 WHERE Ac_Code IN (" + accountcodes + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                            DataSet ds = new DataSet();
                            ds = clsDAL.SimpleQuery(qry);
                        }
                    }
                }

            }
        }
        if (btnGenerate.Text == "Delete Login")
        {
            if (grdGeneratedAccounts.Rows.Count > 0)
            {
                for (int i = 0; i < grdGeneratedAccounts.Rows.Count; i++)
                {
                    CheckBox chk = grdGeneratedAccounts.Rows[i].FindControl("grdAccountsCB") as CheckBox;
                    if (chk != null && chk.Checked == true)
                    {
                        Ac_Code = grdGeneratedAccounts.Rows[i].Cells[0].Text.ToString();
                        list.Add("'" + Ac_Code + "'");
                    }
                }
                arr = list.ToArray();
                accountcodes = string.Join(",", arr);
                DataSet ds = new DataSet();
                qry = "delete from tblUser where User_Name IN(" + accountcodes + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                qry = "update " + tblPrefix + "AccountMaster SET Is_Login=NULL WHERE Ac_Code IN(" + accountcodes + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                ds = clsDAL.SimpleQuery(qry);
            }
        }
        BindGrid();
        Response.Redirect("../Sugar/pgeGenerateCustomerLogin.aspx", false);
    }
    protected void SendEmail(string Email, string Ac_Code, string Password)
    {
        try
        {
            string mailFrom = Session["EmailId"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["EmailPassword"].ToString();
            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Host = clsGV.Email_Address;
            msg.From = new MailAddress(mailFrom);
            msg.To.Add(Email);
            string username = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + Ac_Code + "");
            string company = "http://" + clsGV.Website + "";
            string body = "Hello <strong>" + username + "</strong> , ";
            body += "<br /><br />Your Account is Generated in Our Company";
            body += "<br />Account Code is=" + Ac_Code + " And Password is=" + Password + "";
            body += "<br />Please Visit: <a href=" + company + ">Gautam Sugar Trading Company</a>";
            msg.Body = body;
            msg.IsBodyHtml = true;
            msg.Subject = "Account Passsword";
            if (smtpPort != string.Empty)
            {
                SmtpServer.Port = Convert.ToInt32(smtpPort);
            }
                                 SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            SmtpServer.Send(msg);
        }

        catch (Exception e1)
        {
            Response.Write("mail err:" + e1);
            //Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }
    protected void grdAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("80px");
            e.Row.Cells[1].Width = new Unit("380px");
            e.Row.Cells[2].Width = new Unit("280px");
            e.Row.Cells[3].Width = new Unit("180px");
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            i++;
            foreach (TableCell cell in e.Row.Cells)
            {
                string s = cell.Text.ToString();
                if (cell.Text.Length > 40)
                {
                    cell.Text = cell.Text.Substring(0, 40) + "..";
                    cell.ToolTip = s;
                }
            }
            //e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtSearch.Text.Trim(), delegate(Match match)
            //{
            //    return string.Format("<span style = 'background-color:#FFFF00'>{0}</span>", match.Value);
            //}, RegexOptions.IgnoreCase);

            //e.Row.Cells[1].Text = Regex.Replace(e.Row.Cells[1].Text, txtSearch.Text.Trim(), delegate(Match match)
            //{
            //    return string.Format("<span style = 'background-color:#FFFF00'>{0}</span>", match.Value);
            //}, RegexOptions.IgnoreCase);
        }
    }
    protected void btnShowGeneratedAccounts_Click(object sender, EventArgs e)
    {
        if (btnShowGeneratedAccounts.Text == "Show Generated")
        {
            pnlGeneratedAccounts.Visible = true;
            pnlAccounts.Visible = false;
            btnGenerate.Text = "Delete Login";
            btnShowGeneratedAccounts.Text = "Show Not Generated";
            qry = "Select Ac_Code,Ac_Name_E,Email_Id,Mobile_No from " + tblPrefix + "AccountMaster where Company_Code=" + company_code + " AND Is_Login=1";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdGeneratedAccounts.DataSource = dt;
                        grdGeneratedAccounts.DataBind();
                    }
                    else
                    {
                        grdGeneratedAccounts.DataSource = null;
                        grdGeneratedAccounts.DataBind();
                    }
                }
            }
        }
        else
        {
            pnlGeneratedAccounts.Visible = false;
            pnlAccounts.Visible = true;
            btnGenerate.Text = "Create Login";
            btnShowGeneratedAccounts.Text = "Show Generated";
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    #region searching gridview records
    //[WebMethod]
    //public static string GetCustomers(string searchTerm, int pageIndex)
    //{
    //    string query = "[GetCustomers_Pager]";
    //    SqlCommand cmd = new SqlCommand(query);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
    //    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
    //    cmd.Parameters.AddWithValue("@PageSize", "10");
    //    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
    //    return GetData(cmd, pageIndex).GetXml();
    //}
    //private static DataSet GetData(SqlCommand cmd, int pageIndex)
    //{
    //    //string pre = tblPrefix;
    //    string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(strConnString))
    //    {
    //        using (SqlDataAdapter sda = new SqlDataAdapter())
    //        {
    //            cmd.Connection = con;
    //            sda.SelectCommand = cmd;
    //            using (DataSet ds = new DataSet())
    //            {
    //                sda.Fill(ds, "NT_1_AccountMaster");
    //                DataTable dt = new DataTable("Pager");
    //                dt.Columns.Add("PageIndex");
    //                dt.Columns.Add("PageSize");
    //                dt.Columns.Add("RecordCount");
    //                dt.Rows.Add();
    //                dt.Rows[0]["PageIndex"] = pageIndex;
    //                dt.Rows[0]["PageSize"] = "10";
    //                dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
    //                ds.Tables.Add(dt);
    //                return ds;
    //            }
    //        }
    //    }
    //}
    #endregion


}