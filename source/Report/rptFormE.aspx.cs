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
using System.Net.Mime;

public partial class Report_rptFormE : System.Web.UI.Page
{
    string f = "../GSReports/FormE_.htm";
    string f_Main = "../GSReports/FormE";
    string tblPrefix = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    string Doc_No = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        cityMasterTable = tblPrefix + "CityMaster";
        Doc_No = Request.QueryString["Doc_No"];
        if (!Page.IsPostBack)
        {
            this.BindReport();
        }
    }

    private void BindReport()
    {
        try
        {
            if (!string.IsNullOrEmpty(Doc_No))
            {
                qry = "select v.Doc_No as #,v.Doc_Date as dt,v.PartyName as Party,v.PartyAddress as PartyAddress,a.Local_Lic_No as LIC,a.Cst_no as CST,a.Gst_No as GST,a.Tin_No as TIN,a.ECC_No as ECC," +
                      " (ISNULL(c.city_name_e,' ') +','+ISNULL(c.state,' ')) as PartyCity,v.From_Place as Dispatch_From,v.To_Place as Dispatch_To,v.Lorry_No as lorry,(ISNULL(v.Quantal,0)+ISNULL(v.Quantal1,0)) as Qntl," +
                      " (ISNULL(v.BAGS,0)+ISNULL(v.BAGS1,0)) as Bags,v.Grade as Grade,v.MillName as MillName,v.BrokerName as BrokerName,d.city_name_e as BrokerCity, d.state as BrokerState" +
                      " ,v.Voucher_Amount as VoucherAmount,(v.millshortname+' '+Convert(varchar(20),v.Sale_Rate,20)+' '+v.BrokerShort) as millbrokerSR from " + tblPrefix + "qryVoucherList v left outer join " + tblPrefix + "AccountMaster a on v.Ac_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                      " left outer join " + cityMasterTable + " c on a.City_Code=c.city_code and a.Company_Code=c.company_code left outer join " + tblPrefix + "AccountMaster x on v.Broker_CODE=x.Ac_Code and v.Company_Code=x.Company_Code" +
                      " left outer join " + cityMasterTable + " d on x.City_Code=d.city_code and x.Company_Code=d.company_code where v.Tran_Type='OV' and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and v.Doc_No=" + Doc_No + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //string qntl = ds.Tables[0].Rows[0]["Qntl"].ToString();
                    //ds.Tables[0].Rows[0]["Qntl"] = Convert.ToString(qntl + " " + "QT");
                    ds.Tables[0].Columns.Add(new DataColumn("CmpName", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("cmpAddress", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("CmpCity", typeof(string)));
                    ds.Tables[0].Rows[0]["CmpName"] = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    ds.Tables[0].Rows[0]["CmpAddress"] = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())); ;
                    ds.Tables[0].Rows[0]["CmpCity"] = clsCommon.getString("Select (City_E+','+State_E) from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())); ;
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter tw = new HtmlTextWriter(sw);
                    pnlMain.RenderControl(tw);
                    string s = sw.ToString();
                    s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                    byte[] array = Encoding.UTF8.GetBytes(s);
                    ms.Write(array, 0, array.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = "FormE.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(email);
                    msg.Body = "Form E";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "Form E On" + DateTime.Now.ToString("dd/MM/yyyy");
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

                //StringWriter sw = new StringWriter();
                //HtmlTextWriter tw = new HtmlTextWriter(sw);
                //pnlMain.RenderControl(tw);
                //string s = sw.ToString();

                //try
                //{
                //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                //    {
                //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                //        {
                //            w.WriteLine(s);
                //        }
                //    }
                //}
                //catch (Exception)
                //{
                //    f = f_Main + ".htm";
                //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                //    {
                //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                //        {
                //            w.WriteLine(s);
                //        }
                //    }
                //}

                //string mailFrom = Session["EmailId"].ToString();
                //string smtpPort = "587";
                //string emailPassword = Session["EmailPassword"].ToString();
                //MailMessage msg = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                //SmtpServer.Host = clsGV.Email_Address;
                //msg.From = new MailAddress(mailFrom);
                //msg.To.Add(email);
                //msg.Body = "Form E";
                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "Form E " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                //msg.IsBodyHtml = true;
                //if (smtpPort != string.Empty)
                //{
                //    SmtpServer.Port = Convert.ToInt32(smtpPort);
                //}
                //                     SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                //SmtpServer.Send(msg);
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