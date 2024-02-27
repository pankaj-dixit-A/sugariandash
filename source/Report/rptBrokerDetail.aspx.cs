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

public partial class Report_rptBrokerDetail : System.Web.UI.Page
{
    string Broker_Code = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string from = string.Empty;
    string to = string.Empty;
    string f = "../GSReports/BrokerDetail_.htm";
    string f_Main = "../Report/BrokerDetail_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Broker_Code = Request.QueryString["Broker_Code"].ToString();
        fromDate = Request.QueryString["FromDT"].ToString();
        toDate = Request.QueryString["ToDt"].ToString();
        if (!Page.IsPostBack)
        {
            this.BindList();
            lblCompanyName.Text = Session["Company_Name"].ToString();
        }
    }
    protected void BindList()
    {
        try
        {
            from = Convert.ToDateTime(fromDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            to = Convert.ToDateTime(toDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            if (Broker_Code != string.Empty)
            {
                qry = "Select Convert(varchar(10),d.doc_date,103) as DO_Date,d.tran_type as Do_Type,d.doc_no as Do_No,A.Ac_Name_E as Customer,quantal,(mill_rate-sale_rate*quantal) as Amount,B.Short_Name as Mill " +
                    " from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster A ON A.Ac_Code=d.GETPASSCODE and A.Company_Code=d.Company_Code left outer join " + tblPrefix + "AccountMaster B ON B.Ac_Code=d.mill_code and B.Company_Code=d.Company_Code" +
                    " where d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.tran_type='DO' and d.broker=" + Broker_Code + " and d.doc_date between '" + from + "' AND '" + to + "' order by convert(datetime,d.doc_date,103) asc,A.Ac_Name_E ";

                lblBrokerName.Text = "Broker Name :" + " " + clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + Broker_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            }
            else
            {
                qry = "Select Convert(varchar(10),d.doc_date,103) as DO_Date,d.tran_type as Do_Type,d.doc_no as Do_No,A.Ac_Name_E as Customer,quantal,(mill_rate-sale_rate*quantal) as Amount,B.Short_Name as Mill " +
                                  " from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster A ON A.Ac_Code=d.GETPASSCODE and A.Company_Code=d.Company_Code left outer join " + tblPrefix + "AccountMaster B ON B.Ac_Code=d.mill_code and B.Company_Code=d.Company_Code " +
                                  " where d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.tran_type='DO' and  d.doc_date between '" + from + "' AND '" + to + "' order by convert(datetime,d.doc_date,103) asc,A.Ac_Name_E";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("DO_Date", typeof(string)));
                dt.Columns.Add(new DataColumn("DO_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("DO_No", typeof(string)));
                dt.Columns.Add(new DataColumn("Customer", typeof(string)));
                dt.Columns.Add(new DataColumn("quantal", typeof(double)));
                dt.Columns.Add(new DataColumn("Amount", typeof(double)));
                dt.Columns.Add(new DataColumn("Mill", typeof(string)));
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DO_Date"] = ds.Tables[0].Rows[i]["DO_Date"].ToString();
                        dr["DO_Type"] = ds.Tables[0].Rows[i]["DO_Type"].ToString();
                        dr["DO_No"] = ds.Tables[0].Rows[i]["DO_No"].ToString();
                        dr["Customer"] = ds.Tables[0].Rows[i]["Customer"].ToString();
                        dr["quantal"] = ds.Tables[0].Rows[i]["quantal"].ToString();
                        //string amount = Math.Abs(double.Parse(amount));
                        dr["Amount"] = ds.Tables[0].Rows[i]["Amount"].ToString();
                        dr["Mill"] = ds.Tables[0].Rows[i]["Mill"].ToString();
                        dt.Rows.Add(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {

                        lblDate.Text = "Broker List From :" + fromDate + " To " + toDate;
                        lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(quantal)", string.Empty));
                        lblAmountTotal.Text = Convert.ToString(dt.Compute("SUM(Amount)", string.Empty));
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
        }

        catch
        { }
    }
    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {

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
                msg.Body = "Broker Details";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Broker Details " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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