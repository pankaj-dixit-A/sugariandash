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

public partial class Report_rptUtrOnlyBalance : System.Web.UI.Page
{
    string accode = string.Empty;
    string utr_no = string.Empty;
    string tblPrefix = string.Empty;
    string tblUtr = string.Empty;
    string qry = string.Empty;
    string AcType = string.Empty;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
    DataSet ds;
    DataTable dt;
    string f = "../GSReports/Utr_.htm";
    string f_Main = "../GSReports/Utr_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        tblUtr = tblPrefix + "UTR";
        AcType = Request.QueryString["AcType"];
        FromDt = Request.QueryString["FromDt"];
        ToDt = Request.QueryString["ToDt"];
        accode = Request.QueryString["accode"];
        utr_no = Request.QueryString["utr_no"];
        if (!Page.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        try
        {
            ds = new DataSet();
            FromDt = DateTime.Parse(FromDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            ToDt = DateTime.Parse(ToDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            if (accode != string.Empty)
            {
                if (utr_no != string.Empty)
                {
                    if (AcType != string.Empty)
                    {
                        if (AcType == "M")
                        {
                            qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u" +
                                  "  where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code='" + accode + "' AND doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                        }
                        else
                        {
                            qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and bank_ac='" + accode + "' AND doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                        }
                    }
                }
                else
                {
                    if (AcType != string.Empty)
                    {
                        if (AcType == "M")
                        {
                            qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code='" + accode + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                        }
                        else
                        {
                            qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and bank_ac='" + accode + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                        }
                    }
                }
            }
            else
            {
                if (accode == string.Empty && utr_no == string.Empty)
                {
                    qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                }
                if (utr_no != string.Empty)
                {
                    qry = "select u.doc_no as #,CONVERT(varchar(10),u.doc_date,103) as dt,u.utr_no as UTRNo,u.millname as Mill,LEFT(u.bankname,20) as Bank,u.balance as balance from " + tblPrefix + "qryUTRBalanceForDO u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and u.balance!=0 and u.IsSave=1";
                }
            }

            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    lblAcName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + accode + "");
                    lblFromTo.Text = "From :<b>" + DateTime.Parse(FromDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy") + "</b> To: <b>" + DateTime.Parse(ToDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
            }

            //dt.Columns.Add("UTR_NO", typeof(string));
            //dt.Columns.Add("UTR_DATE", typeof(string));
            //dt.Columns.Add("UTR_BANK_NUMBER", typeof(string));
            //dt.Columns.Add("UTR_AMOUNT", typeof(string));

            //if (ds != null)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            DataRow dr = dt.NewRow();

            //            dr["UTR_NO"] = ds.Tables[0].Rows[i]["UTR"].ToString();
            //            dr["UTR_DATE"] = ds.Tables[0].Rows[i]["UTR_Date"].ToString();
            //            dr["UTR_BANK_NUMBER"] = ds.Tables[0].Rows[i]["UTR_BANK_NUMBER"].ToString();
            //            dr["UTR_AMOUNT"] = ds.Tables[0].Rows[i]["UTR_AMOUNT"].ToString();
            //            dt.Rows.Add(dr);
            //        }
            //    }
            //    dtl.DataSource = dt;
            //    dtl.DataBind();
            //}
        }
        catch (Exception)
        {
            throw;
        }
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
        string email = txtEmail.Text;
        if (txtEmail.Text != string.Empty)
        {
            try
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
                msg.Body = "Utr Report";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Utr Report " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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