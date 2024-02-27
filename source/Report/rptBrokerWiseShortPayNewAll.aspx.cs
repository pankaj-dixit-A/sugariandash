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

public partial class Report_rptBrokerWiseShortPayNewAll : System.Web.UI.Page
{
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string Broker_Code = string.Empty;
    DataSet ds;
    DataTable dt;
    string from = "";
    string to = "";
    string tblPrefix = string.Empty;
    string f = "../GSReports/BrokerWiseShortPay_.htm";
    string f_Main = "../Report/BrokerWiseShortPay_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Broker_Code = Request.QueryString["Broker_Code"].ToString();
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            from = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            to = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            lblDate.Text = "From: <b>" + from + "</b> To:<b>" + to + "</b>";
            if (Broker_Code != string.Empty)
            {
                txtEmail.Text = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Broker_Code + "");
                qry = "select DISTINCT(BROKER) as BrokerCode,BrokerName as BrokerName from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and BROKER=" + Broker_Code + " and Bill_Amount>0 and doc_date between '" + from + "' and '" + to + "' order by BROKER desc ";
                lblBrokerName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster Where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Broker_Code) + " Only Balance Report";
            }
            else
            {
                qry = "select DISTINCT(BROKER) as BrokerCode,BrokerName as BrokerName from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Bill_Amount>0 and doc_date between '" + from + "' and '" + to + "' order by BROKER desc ";
                lblBrokerName.Text = "All Broker Details Only Balance Report";
            }

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblBrokerCode = (Label)e.Item.FindControl("lblBrokerCode");
            string Broker = lblBrokerCode.Text;
            Label lblAmountTotal = (Label)e.Item.FindControl("lblAmountTotal");
            Label lblRecievedTotal = (Label)e.Item.FindControl("lblRecievedTotal");
            Label lblShortTotal = (Label)e.Item.FindControl("lblShortTotal");
            Label lblQntlTotal = (Label)e.Item.FindControl("lblQntlTotal");
            qry = "select u.doc_date as dt,u.Tran_Type as ttype,u.doc_no as #,u.Unit_Name as Party,u.NETQNTL as Qntl,a.Short_Name as Mill,u.Bill_Amount as VocAmount," +
                " ISNULL((Select SUM(t.amount) as Recieved from " + tblPrefix + "Transact t where t.Tran_Type IN('BR','CR') and t.Voucher_No=u.doc_no and t.Voucher_Type=u.Tran_Type and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code),0) as Recieved," +
                " (u.Bill_Amount-ISNULL((Select SUM(t.amount) as Recieved from " + tblPrefix + "Transact t where t.Tran_Type IN('BR','CR') and t.Voucher_No=u.doc_no and t.Voucher_Type=u.Tran_Type and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code),0)) as Short,u.BROKER from " + tblPrefix + "qryVoucherSaleUnion u" +
                " left outer join " + tblPrefix + "AccountMaster a on u.mill_code=a.Ac_Code and u.Company_Code=a.Company_Code where u.BROKER='" + Broker + "' and u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and u.doc_date between '" + from + "' and '" + to + "' order by convert(datetime,u.doc_date,103) asc";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("dt", typeof(string)));
                dt.Columns.Add(new DataColumn("ttype", typeof(string)));
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("Party", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("Mill", typeof(string)));
                dt.Columns.Add(new DataColumn("VocAmount", typeof(double)));
                dt.Columns.Add(new DataColumn("Recieved", typeof(double)));
                dt.Columns.Add(new DataColumn("Short", typeof(double)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["dt"] = ds.Tables[0].Rows[i]["dt"].ToString();
                    dr["ttype"] = ds.Tables[0].Rows[i]["ttype"].ToString();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["Party"] = ds.Tables[0].Rows[i]["Party"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["Mill"] = ds.Tables[0].Rows[i]["Mill"].ToString();
                    dr["VocAmount"] = ds.Tables[0].Rows[i]["VocAmount"].ToString();
                    dr["Recieved"] = ds.Tables[0].Rows[i]["Recieved"].ToString();
                    double Short = Convert.ToDouble(ds.Tables[0].Rows[i]["short"].ToString());
                    dr["short"] = Short;
                    if (Short != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {

                    lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                    lblAmountTotal.Text = Convert.ToString(dt.Compute("SUM(VocAmount)", string.Empty));
                    lblRecievedTotal.Text = Convert.ToString(dt.Compute("SUM(Recieved)", string.Empty));
                    lblShortTotal.Text = Convert.ToString(dt.Compute("SUM(Short)", string.Empty));
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
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
                msg.Body = "Broker Wise Short Payment Report";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Broker Wise Short Payment " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
}