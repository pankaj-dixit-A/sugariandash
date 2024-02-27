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

public partial class Report_rptDuePayment : System.Web.UI.Page
{
    string f = "../GSReports/rptDuePayment_.htm";
    string f_Main = "../Report/rptDuePayment";
    string email = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    int pos;
    PagedDataSource adsource;
    string tblPrefix = string.Empty;
    string uptodate = "";
    string mail = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uptodate = DateTime.Now.ToString("yyyy/MM/dd");
            rptDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            if (!Page.IsPostBack)
            {
                lblCompanyName.Text = Session["Company_Name"].ToString();
                this.ViewState["vs"] = 0;
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            pos = (int)this.ViewState["vs"];
            BindList();
        }
        catch (NullReferenceException ne)
        {
            Response.Write("<script>alert('Please login');</script>");
            Response.Redirect("~/pgeloginForm.aspx", false);
        }
    }

    protected void BindList()
    {
        try
        {
            if (!string.IsNullOrEmpty(uptodate))
            {
                qry = "select CONVERT(varchar(5),Doc_Date,103) as doc_date,Doc_No,Tran_Type as Type,PartyName,a.Short_Name as Broker,Quantal as Qntl,from_station as From_Station,Voucher_Amount as Voc_Amt," +
                      " ISNULL((Select t.amount from " + tblPrefix + "Transact t where t.Tran_Type='BR' and t.Voucher_Type=u.Tran_Type and t.Voucher_No=u.Doc_No and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code),0) as recieved," +
                      " (Voucher_Amount - ISNULL((Select t.amount from " + tblPrefix + "Transact t where t.Tran_Type='BR' and t.Voucher_Type=u.Tran_Type and t.Voucher_No=u.Doc_No and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code),0)) as short" +
                      " from " + tblPrefix + "qryUnionVoucherSale u left outer join " + tblPrefix + "AccountMaster a on u.Broker_CODE=a.Ac_Code AND u.Company_Code=a.Company_Code where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and u.Doc_Date <='" + uptodate + "' order by u.Doc_No";
                DataSet ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("doc_date", typeof(string)));
                    dt.Columns.Add(new DataColumn("Doc_No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("PartyName", typeof(string)));
                    dt.Columns.Add(new DataColumn("Broker", typeof(string)));
                    dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                    dt.Columns.Add(new DataColumn("From_Station", typeof(string)));
                    dt.Columns.Add(new DataColumn("Voc_Amt", typeof(double)));
                    dt.Columns.Add(new DataColumn("recieved", typeof(double)));
                    dt.Columns.Add(new DataColumn("short", typeof(double)));
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["doc_date"] = ds.Tables[0].Rows[i]["doc_date"].ToString();
                            dr["Doc_No"] = ds.Tables[0].Rows[i]["Doc_No"].ToString();
                            dr["Type"] = ds.Tables[0].Rows[i]["Type"].ToString();
                            dr["PartyName"] = ds.Tables[0].Rows[i]["PartyName"].ToString();
                            dr["Broker"] = ds.Tables[0].Rows[i]["Broker"].ToString();
                            dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                            dr["From_Station"] = ds.Tables[0].Rows[i]["From_Station"].ToString();
                            dr["Voc_Amt"] = ds.Tables[0].Rows[i]["Voc_Amt"].ToString();
                            dr["recieved"] = ds.Tables[0].Rows[i]["recieved"].ToString();
                            dr["short"] = ds.Tables[0].Rows[i]["short"].ToString();
                            dt.Rows.Add(dr);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            lblTotalQntl.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                            lblTotalAmt.Text = Convert.ToString(dt.Compute("SUM(Voc_Amt)", string.Empty));
                            lblTotalRecieved.Text = Convert.ToString(dt.Compute("SUM(recieved)", string.Empty));
                            lblTotalBal.Text = Convert.ToString(dt.Compute("SUM(short)", string.Empty));
                            dtlist.DataSource = dt;
                            dtlist.DataBind();
                        }
                        else
                        {
                            dtlist.DataSource = null;
                            dtlist.DataBind();
                        }
                    }
                    else
                    {
                        dtlist.DataSource = null;
                        dtlist.DataBind();
                    }
                }
                else
                {
                    dtlist.DataSource = null;
                    dtlist.DataBind();
                }

            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    protected void dtlist_ItemDataBound(object sender, DataListItemEventArgs e)
    {

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            mail = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
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
                msg.To.Add(mail);
                msg.Body = "Due Payment";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Due Payment ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
}