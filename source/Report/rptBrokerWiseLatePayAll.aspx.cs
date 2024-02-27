using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Net.Mail;

public partial class Report_rptBrokerWiseLatePayAll : System.Web.UI.Page
{
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string Broker_Code = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string f = "../GSReports/BrokerWiseLatePay_.htm";
    string f_Main = "../Report/BrokerWiseLatePay_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Broker_Code = Request.QueryString["Broker_Code"];
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            BindList();
            lblCompanyNameBottom.Text = Session["Company_Name"].ToString();
        }
    }
    private void BindList()
    {
        if (fromDT != string.Empty)
        {
            fromDT = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            fromDT = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (toDT != string.Empty)
        {
            toDT = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            toDT = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (Broker_Code != string.Empty)
        {
            txtEmail.Text = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Broker_Code + "");
            qry = "select CONVERT(varchar(10),doc_date,103) as Date,doc_no as RefNo,Party as Customer_Name,Mill,Sale_Rate,rate_diff,qntl as Qntl,Bill_Amount as amount,Balance as short from" +
                " " + tblPrefix + "qryBrokerWiseShortPayAll where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "' and Brokercode='" + Broker_Code + "' order by Convert(datetime,doc_date,103) asc,Party";
            lblBrokerName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Balance!=0 and Ac_Code=" + Broker_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        }
        else
        {
            qry = "select CONVERT(varchar(10),doc_date,103) as Date,doc_no as RefNo,Party as Customer_Name,Mill,Sale_Rate,rate_diff,qntl as Qntl,Bill_Amount as amount,Balance as short from" +
                           " " + tblPrefix + "qryBrokerWiseShortPayAll where Balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "' order by Convert(datetime,doc_date,103) asc,Party";
            lblBrokerName.Text = "All Broker Details";
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            lblDate.Text = "Late Payment List From <b>" + DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy") + "</b>  TO  <b>" + DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy") + "</b>";
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("RefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
            dt.Columns.Add(new DataColumn("amount", typeof(double)));
            dt.Columns.Add(new DataColumn("short", typeof(double)));
            dt.Columns.Add(new DataColumn("DelayDays", typeof(string)));
            dt.Columns.Add(new DataColumn("Interest", typeof(string)));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string date = ds.Tables[0].Rows[i]["Date"].ToString();
                    dr["Date"] = date;
                    dr["RefNo"] = ds.Tables[0].Rows[i]["RefNo"].ToString();
                    string cust = ds.Tables[0].Rows[i]["Customer_Name"].ToString();
                    string mill = ds.Tables[0].Rows[i]["Mill"].ToString();
                    double salerate = ds.Tables[0].Rows[i]["Sale_Rate"].ToString() != string.Empty ? Convert.ToDouble(ds.Tables[0].Rows[i]["Sale_Rate"].ToString()) : 0.00;
                    string qntl = ds.Tables[0].Rows[i]["qntl"].ToString();
                    string ratediff = ds.Tables[0].Rows[i]["rate_diff"].ToString();
                    dr["Qntl"] = qntl;
                    if (ratediff == "0.00")
                    {
                        dr["Customer_Name"] = cust + " " + mill + " " + Math.Abs(salerate);
                    }
                    else
                    {
                        dr["Customer_Name"] = cust + " " + mill + " " + Math.Abs(salerate) + "+" + ratediff + "*" + qntl;
                    }
                    dr["amount"] = ds.Tables[0].Rows[i]["amount"].ToString();
                    DateTime start = DateTime.Parse(date);
                    string end = System.DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime todaysdate = DateTime.Parse(end);
                    double delaydays = 0.00;
                    if (start < todaysdate)
                    {
                        delaydays = (todaysdate - start).TotalDays;
                    }
                    dr["DelayDays"] = Math.Abs(delaydays);
                    dr["Interest"] = "";
                    double Short = Convert.ToDouble(ds.Tables[0].Rows[i]["short"].ToString());
                    dr["short"] = Short;
                    if (Short != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    lblqntltotal.Text = Convert.ToString(dt.Compute("SUM(QNTL)", string.Empty));
                    lblvoctotal.Text = Convert.ToString(dt.Compute("SUM(amount)", string.Empty));
                    lblShortTotal.Text = Convert.ToString(dt.Compute("SUM(short)", string.Empty));
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
                else
                {
                    dtl.DataSource = null;
                    dtl.DataBind();
                }
            }
        }
        else
        {
            dtl.DataSource = null;
            dtl.DataBind();
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder StrHtmlGenerate = new StringBuilder();
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            pnlMain.RenderControl(tw);
            string sim = sw.ToString();
            StrExport.Append(sim);
            StrExport.Append("</div></body></html>");
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(StrExport.ToString());
            Response.Flush();
            Response.End();
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (txtEmail.Text != string.Empty)
        {
            try
            {
                string email = txtEmail.Text;
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
                msg.Body = "Broker Wise Late Payment";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Broker Wise Late Payment" + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
            catch (Exception e1)
            {
                //Response.Write("mail err:" + e1);
                Response.Write("<script>alert('Error sending Mail');</script>");
                return;
            }
            Response.Write("<script>alert('Mail sent successfully');</script>");
        }
    }
}