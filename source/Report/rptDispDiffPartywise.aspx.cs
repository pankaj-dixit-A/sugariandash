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

public partial class Report_rptDispDiffPartywise : System.Web.UI.Page
{
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    string f = "../GSReports/DispatchPartyWise_.htm";
    string f_Main = "../Report/DispatchPartyWise_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {

        if (string.IsNullOrEmpty(Branch_Code))
        {
            qry = "select Distinct(d.GETPASSCODE) as partycode,a.Ac_Name_E as Party from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_Code=a.Company_Code" +
              " where d.tran_type='DO' and d.desp_type='DO' and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  order by Party";
        }
        else
        {
            qry = "select Distinct(d.GETPASSCODE) as partycode,a.Ac_Name_E as Party from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_Code=a.Company_Code" +
              " where  d.tran_type='DO' and d.desp_type='DO' and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " order by Party";
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    datalist.DataSource = dt;
                    datalist.DataBind();
                }
                else
                {
                    datalist.DataSource = null;
                    datalist.DataBind();
                }
            }
        }
    }
    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
        Label lblPartyCode = (Label)e.Item.FindControl("lblPartyCode");
        string getpass = lblPartyCode.Text;
        Label lblToPayTotal = (Label)e.Item.FindControl("lblToPayTotal");
        Label lblToRecieveTotal = (Label)e.Item.FindControl("lblToRecieveTotal");
        if (string.IsNullOrEmpty(Branch_Code))
        {
            qry = "select CONVERT(varchar(10),d.doc_date,103) as tdate,d.doc_no as tno,ISNULL(a.Short_Name,a.Ac_Name_E) as mill,d.voucher_no as VN,d.voucher_type as VT," +
                " d.Quantal as quantal,d.mill_rate as millrate,d.sale_rate as salerate from " + tblPrefix + "deliveryorder d " +
                  " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_Code=a.Company_Code where d.GETPASSCODE=" + getpass + " and d.tran_type='DO' and d.desp_type='DO' and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        }
        else
        {
            qry = "select CONVERT(varchar(10),d.doc_date,103) as tdate,d.doc_no as tno,ISNULL(a.Short_Name,a.Ac_Name_E) as mill ,d.voucher_no as VN,d.voucher_type as VT," +
           " d.Quantal as quantal,d.mill_rate as millrate,d.sale_rate as salerate from " + tblPrefix + "deliveryorder d " +
             " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_Code=a.Company_Code where d.GETPASSCODE=" + getpass + " and d.tran_type='DO' and d.desp_type='DO' and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + "";
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("tdate", typeof(string)));
            dt.Columns.Add(new DataColumn("tno", typeof(string)));
            dt.Columns.Add(new DataColumn("mill", typeof(string)));
            dt.Columns.Add(new DataColumn("quantal", typeof(string)));
            dt.Columns.Add(new DataColumn("millrate", typeof(string)));
            dt.Columns.Add(new DataColumn("salerate", typeof(string)));
            dt.Columns.Add(new DataColumn("topay", typeof(double)));
            dt.Columns.Add(new DataColumn("torecieve", typeof(double)));
            dt.Columns.Add(new DataColumn("OV", typeof(string)));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string vtype = ds.Tables[0].Rows[i]["VT"].ToString();
                    if (vtype == "LV")
                    {
                        dr["OV"] = ds.Tables[0].Rows[i]["VN"].ToString();
                    }

                    dr["tdate"] = ds.Tables[0].Rows[i]["tdate"].ToString();
                    dr["tno"] = ds.Tables[0].Rows[i]["tno"].ToString();
                    dr["mill"] = ds.Tables[0].Rows[i]["mill"].ToString();
                    double qntl = Convert.ToDouble(ds.Tables[0].Rows[i]["quantal"].ToString());
                    dr["quantal"] = qntl;
                    double millrate = Convert.ToDouble(ds.Tables[0].Rows[i]["millrate"].ToString());
                    double salerate = Convert.ToDouble(ds.Tables[0].Rows[i]["salerate"].ToString());
                    dr["millrate"] = millrate;
                    dr["salerate"] = salerate;
                    double amount = ((salerate - millrate) * qntl);
                    if (amount > 0)
                    {
                        dr["torecieve"] = Math.Abs(amount);
                    }
                    else
                    {
                        dr["topay"] = Math.Abs(amount);
                    }
                    if (amount != 0)
                    {
                        dt.Rows.Add(dr);
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    lblToPayTotal.Text = Convert.ToString(dt.Compute("SUM(topay)", string.Empty));
                    lblToRecieveTotal.Text = Convert.ToString(dt.Compute("SUM(torecieve)", string.Empty));
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
                else
                {
                    dtlDetails.DataSource = null;
                    dtlDetails.DataBind();
                }
            }
            else
            {
                dtlDetails.DataSource = null;
                dtlDetails.DataBind();
            }
        }
    }

    protected void lnkOV_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkOV = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkOV.NamingContainer;
            string no = lnkOV.Text;
            Session["LV_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:DebitNote();", true);
            lnkOV.Focus();
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
                msg.Body = "Dispatch Party Wise Report";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Dispatch Party Wise " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
