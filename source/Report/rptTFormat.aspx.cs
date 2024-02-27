using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;


public partial class Report_rptTFormat : System.Web.UI.Page
{
    string tblPrefix =string.Empty; 
    string tblHead = "";
    string qryCommon = "";
    string email = string.Empty;
    string f = "../GSReports/TFormat_.htm";
    string f_Main = "../Report/rptTFormat";
    IFormatProvider ifrDT = CultureInfo.CreateSpecificCulture("en-GB");
    string prefix = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        qryCommon = "qryGledgernew";

        if (Session["tblPrefix"] != null)
        {
            tblPrefix = Session["tblPrefix"].ToString();
        }
        else
        {
            prefix = clsCommon.getString("Select tblPrefix from tblPrefix");
            tblPrefix = prefix.ToString();
        }

       // ViewState["fromDt"] = Request.QueryString["fromDt"];
        ViewState["ToDt"] = Request.QueryString["ToDt"];
        ViewState["whr1"] = "";

        if (!Page.IsPostBack)
        {
            lblCompany.Text = Session["Company_Name"].ToString();
            this.bindData();
        }
    }

    private void bindData()
    {
        try
        {
            DataTable dtCredit = new DataTable();
            DataTable dtDebit = new DataTable();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string qry = "";
            dtCredit.Columns.Add("AC_Code",typeof(Int32));
            dtCredit.Columns.Add("Ac_Name",typeof(string));
            dtCredit.Columns.Add("Balace", typeof(double));

            dtDebit.Columns.Add("AC_Code", typeof(Int32));
            dtDebit.Columns.Add("Ac_Name", typeof(string));
            dtDebit.Columns.Add("Balace", typeof(double));


            if (ViewState["ToDt"] != null)
            {
                string ToDt = DateTime.Parse(ViewState["ToDt"].ToString(), ifrDT).ToString("yyyy/MM/dd");


                qry = "select AC_CODE,Ac_Name_E,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance "
                        + " from qryGledgernew  "
                        + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and DOC_DATE<'" + ToDt + "'"
                        + " group by AC_CODE,Ac_Name_E  having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end)!=0";
                ds = clsDAL.SimpleQuery(qry);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                double bal = 0.00;
                                if (dt.Rows[i]["Balance"].ToString() != string.Empty)
                                {
                                    bal = Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                }

                                if (bal > 0)
                                {
                                    DataRow dr = dtDebit.NewRow();
                                    dr["AC_Code"] = dt.Rows[i]["AC_CODE"].ToString();
                                    dr["Ac_Name"] = dt.Rows[i]["Ac_Name_E"].ToString();
                                    dr["Balace"] = dt.Rows[i]["Balance"].ToString();

                                    dtDebit.Rows.Add(dr);
                                }
                                else
                                {
                                    DataRow dr = dtCredit.NewRow();
                                    dr["AC_Code"] = dt.Rows[i]["AC_CODE"].ToString();
                                    dr["Ac_Name"] = dt.Rows[i]["Ac_Name_E"].ToString();
                                    dr["Balace"] = dt.Rows[i]["Balance"].ToString();

                                    dtCredit.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }

                dtl_Debit.DataSource = dtDebit;
                dtl_Debit.DataBind();

                dtl_Credit.DataSource = dtCredit;
                dtl_Credit.DataBind();
                

            }
        }
        catch(Exception eex)
        {
            Response.Write(eex.Message);
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
            msg.Body = "T-Fomat Report";
            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            msg.Subject = "T-Format Balance Report " + " " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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