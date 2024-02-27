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

public partial class Report_rptDoWiseDispatch : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string Tender_No = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string f = "../GSReports/DOWiseDisp_.htm";
    string f_Main = "../Report/DOWiseDisp_";
    string Branch_Code = string.Empty;
    string qntltotal = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindData();
        }
    }

    private void BindData()
    {
        try
        {

            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select d.doc_no as #,CONVERT(varchar(5),d.doc_date,103) as dodate,d.millShortName as millShortName,a.Short_Name as VocBy,d.GetPassName as GetPass" +
                    ",d.mill_rate as MR,d.quantal as Qntl,d.sale_rate as SR,d.truck_no as lorry,d.Freight_Amount as frt,d.vasuli_amount AS vasuli,d.TransportName as transport," +
                    " d.DOName as do,d.purc_no as tender from " + tblPrefix + "qryDeliveryOrderList d left outer join " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code" +
                    " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.desp_type='DI' and d.tran_type='DO' order by d.doc_no asc";
            }
            else
            {
                qry = "select d.doc_no as #,CONVERT(varchar(5),d.doc_date,103) as dodate,d.millShortName as millShortName,a.Short_Name as VocBy,d.GetPassName as GetPass" +
                    ",d.mill_rate as MR,d.quantal as Qntl,d.sale_rate as SR,d.truck_no as lorry,d.Freight_Amount as frt,d.vasuli_amount AS vasuli,d.TransportName as transport," +
                    " d.DOName as do,d.purc_no as tender from " + tblPrefix + "qryDeliveryOrderList d left outer join " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code" +
                    " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.Branch_Code='" + Branch_Code + "' and d.desp_type='DI' and d.tran_type='DO' order by d.doc_no asc";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("dodate", typeof(string)));
                dt.Columns.Add(new DataColumn("millShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("VocBy", typeof(string)));
                dt.Columns.Add(new DataColumn("GetPass", typeof(string)));
                dt.Columns.Add(new DataColumn("MR", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("SR", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("frt", typeof(string)));
                dt.Columns.Add(new DataColumn("vasuli", typeof(string)));
                dt.Columns.Add(new DataColumn("transport", typeof(string)));
                dt.Columns.Add(new DataColumn("do", typeof(string)));
                dt.Columns.Add(new DataColumn("tender", typeof(string)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["dodate"] = ds.Tables[0].Rows[i]["dodate"].ToString();
                    dr["millShortName"] = ds.Tables[0].Rows[i]["millShortName"].ToString();
                    dr["VocBy"] = ds.Tables[0].Rows[i]["VocBy"].ToString();
                    dr["GetPass"] = ds.Tables[0].Rows[i]["GetPass"].ToString();
                    dr["MR"] = ds.Tables[0].Rows[i]["MR"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["SR"] = ds.Tables[0].Rows[i]["SR"].ToString();
                    dr["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                    dr["frt"] = ds.Tables[0].Rows[i]["frt"].ToString();
                    dr["vasuli"] = ds.Tables[0].Rows[i]["vasuli"].ToString();
                    dr["transport"] = ds.Tables[0].Rows[i]["transport"].ToString();
                    dr["do"] = ds.Tables[0].Rows[i]["do"].ToString();
                    dr["tender"] = ds.Tables[0].Rows[i]["tender"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblfromtodate.Text = "DO Wise Dispatch Register From  <b>" + fromDT + "</b>  To  <b>" + toDT + "</b>";
                    lblQntlTotala.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                    Datalist1.DataSource = dt;
                    Datalist1.DataBind();
                }
                else
                {
                    Datalist1.DataSource = null;
                    Datalist1.DataBind();
                }
            }
            else
            {
                Datalist1.DataSource = null;
                Datalist1.DataBind();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void Datalist1_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
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
        pnlMain.RenderControl(tw);
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
                msg.Body = "Do Wise Dispatch Report";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Do Wise Dispatch Report " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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