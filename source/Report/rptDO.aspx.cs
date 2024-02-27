using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;

public partial class Report_rptDO : System.Web.UI.Page
{
    string f = "../GSReports/rptDO_.htm";
    string f_Main = "../Report/rptDO";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string trnType = "DO";
    string GLedgerTable = string.Empty;
    string filterDoNo = string.Empty;
    string millEmail = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();// "NT_1_";
        qryCommon = tblPrefix + "qryDeliveryOrderList";
        filterDoNo = Request.QueryString["do_no"];
        millEmail = Request.QueryString["email"];
        this.BindReport();
    }

    private void BindReport()
    {
        try
        {

            string qry = "select * from " + qryCommon + " where doc_no in (" + filterDoNo + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            DataList1.DataSource = ds;
            DataList1.DataBind();

            //Label lblName = (Label)pnl.FindControl("lblCompanyAddr");
            //lblName.Text = clsGV.CompanyAddress;
            //Label lblMobile = (Label)pnl.FindControl("lblCompanyMobile");
            //lblMobile.Text = clsGV.CompanyPhone;
        }
        catch
        {

        }
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblAddr = (Label)e.Item.FindControl("lblCompanyAddr");
        lblAddr.Text = clsGV.CompanyAddress;

        Label lblName = (Label)e.Item.FindControl("lblCompanyName");
        lblName.Text = Session["Company_Name"].ToString();

        Label lblPhone = (Label)e.Item.FindControl("lblCompanyMobile");
        lblPhone.Text = clsGV.CompanyPhone;


        Label lblMillEmail = (Label)e.Item.FindControl("lblMillEmail");
        lblMillEmail.Text = millEmail;
    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEmail.Text != string.Empty)
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(sw);
                pnl.RenderControl(tw);
                string s = sw.ToString();
                try
                {
                    using (FileStream fs = new FileStream(f, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(s);
                        }
                    }

                }
                catch
                {
                    f = f_Main + DateTime.Now.ToString("ss") + "htm";
                    using (FileStream fs = new FileStream(f, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(s);
                        }
                    }
                }

                string mailFrom = Session["EmailId"].ToString();// clsGV.Email_Address;
                string smtpPort = clsGV.smtpServerPort;// Session["smtpServerPort"].ToString();
                string emailPassword = Session["EmailPassword"].ToString();// Session["EmailPassword"].ToString();

                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(millEmail);
                //mail.To.Add("toaddress2@gmail.com");
                msg.Body = "Delivery Order Report";
                msg.Attachments.Add(new Attachment(f));
                msg.IsBodyHtml = true;
                msg.Subject = "Delivery Order Report  " + DateTime.Now.ToString("dd/MM/yyyy");
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
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        pnl.RenderControl(tw);
        string sim = sw.ToString();
        StrExport.Append(sim);
        StrExport.Append("</div></body></html>");
        string strFile = "report.xls";
        string strcontentType = "application/excel";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.BufferOutput = true;
        Response.ContentType = strcontentType;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        Response.Write(StrExport.ToString());
        Response.Flush();
        Response.Close();
        Response.End();
    }
}