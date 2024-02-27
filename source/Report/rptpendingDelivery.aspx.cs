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

public partial class Report_rptpendingDelivery : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qryCommon = "";
    string f = "../GSReports/PendingDelivery_.htm";
    string f_Main = "../Report/rptpendingDelivery";
    string email = string.Empty;
    string prefix = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        if (Session["tblPrefix"] != null)
        {
            tblPrefix = Session["tblPrefix"].ToString();
        }
        else
        {
            prefix = clsCommon.getString("Select tblPrefix from tblPrefix");
            tblPrefix = prefix.ToString();
        }
        qryCommon = tblPrefix + "qryPendingDelivery";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                lblCompany.Text = Session["Company_Name"].ToString();
                this.bindData();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    private void bindData()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string qry = "";
            qry = "select [Tender_No],[Tender_Date],[millname],[Buyer_Quantal],[salerate],[salevalue],[receivedDate],[received],[balance] from " + qryCommon + " where " +
            " TenderNo_BR>0 and TenderNo_BR!=isnull(purc_no,0)";

            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                    }
                }
            }
        }
        catch (Exception eex)
        {
            Response.Write(eex.Message);
        }
    }

    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            email = txtEmail.Text.ToString();
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            PrintPanel.RenderControl(tw);
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
            catch (Exception ee)
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
            msg.Body = "Pending Delivery";
            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            msg.Subject = "Pending Delivery Report" + " " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
            msg.IsBodyHtml = true;
            if (smtpPort != string.Empty)
            {
                SmtpServer.Port = Convert.ToInt32(smtpPort);
            }
                                 SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
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
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        PrintPanel.RenderControl(tw);
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