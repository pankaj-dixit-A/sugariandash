using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Text;

public partial class Report_rptTransferLetter : System.Web.UI.Page
{
    string f = "../GSReports/transferletter_.htm";
    string f_Main = "../GSReports/TL_";
    string tblPrefix = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    string Do_Number = string.Empty;
    string DOCODE = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        cityMasterTable = tblPrefix + "CityMaster";
        Do_Number = Request.QueryString["DONO"];
        DOCODE = Request.QueryString["DOCODE"];
        if (!Page.IsPostBack)
        {
            this.BindReport();
        }
    }
    private void BindReport()
    {
        try
        {
            qry = "select d.doc_no as DoNo,CONVERT(varchar(10),d.doc_date,103) as Do_Date,a.Ac_Name_E as MillName,d.quantal as Qntl,b.Ac_Name_E as Getpass,b.Cst_no as GetPassCST,d.grade as Grade," +
                  " b.Address_E as GetPassAddress,b.Mobile_No as GetPassMobile,d.truck_no as Truck,b.Tin_No as GetPassTin," +
                  " d.mill_rate as MR,c.city_name_e as GetPassCity,c.state as GetPassState,d.amount as Amount from " + tblPrefix + "deliveryorder d" +
                  " LEFT OUTER JOIN " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code" +
                  " LEFT OUTER JOIN " + tblPrefix + "AccountMaster b on d.GETPASSCODE=b.Ac_Code and d.company_code=b.Company_Code" +
                  " LEFT OUTER JOIN " + tblPrefix + "CityMaster c on b.City_Code=c.city_code and b.Company_Code=c.company_code" +
                  " where d.tran_type='DO' and desp_type='DI' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_no=" + Do_Number + "";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string cmpcity = "";
                    if (DOCODE != "2")
                    {
                        lblCompanyName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + DOCODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblCmpAddress.Text = clsCommon.getString("Select Address_E from " + tblPrefix + "AccountMaster where Ac_Code=" + DOCODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string citycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + DOCODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        cmpcity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + citycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string state = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code=" + citycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblCmpCityName.Text = cmpcity + "," + state;
                        lblCompanyMobile.Text = clsCommon.getString("Select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code=" + DOCODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    }
                    else
                    {
                        lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblCmpAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        cmpcity = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string state = clsCommon.getString("Select State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblCmpCityName.Text = cmpcity + "," + state;
                        lblCompanyMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    }

                    lblSubjToCity.Text = "Subject to <b>" + cmpcity + "</b> Jurisdiction";
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
            }
        }
        catch (Exception)
        {
            Response.Write("Number Not Present");
            //throw;
        }
    }
    protected void dtl_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblForCompany = (Label)e.Item.FindControl("lblForCompany");
        lblForCompany.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(sw);
                pnlMain.RenderControl(tw);
                string s = sw.ToString();

                try
                {
                    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(s);
                        }
                    }
                }
                catch (Exception)
                {
                    f = f_Main + ".htm";
                    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(s);
                        }
                    }
                }

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(email);
                msg.Body = "Transfer Letter";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Transfer Letter " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                msg.IsBodyHtml = true;
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
        }
        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");

    }
}