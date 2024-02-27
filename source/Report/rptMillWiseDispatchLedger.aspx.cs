using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Data;

public partial class Report_rptMillWiseDispatchLedger : System.Web.UI.Page
{
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string Mill_Code = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string f = "../GSReports/DispMillWise_.htm";
    string f_Main = "../Report/DispMillWise_";
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Mill_Code = Request.QueryString["Mill_Code"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            if (string.IsNullOrEmpty(Branch_Code))
            {
                if (string.IsNullOrEmpty(Request.QueryString["Mill_Code"]))
                {
                    qry = "select distinct(d.mill_code) as millcode,ISNULL(a.Short_Name,a.Ac_Name_E) as mill from " + tblPrefix + "deliveryorder d" +
                          " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_code=a.Company_Code where d.tran_type NOT IN('LV','MM') and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by mill";
                }
                else
                {
                    qry = "select distinct(d.mill_code) as millcode,ISNULL(a.Short_Name,a.Ac_Name_E) as mill from " + tblPrefix + "deliveryorder d" +
                            " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_code=a.Company_Code where d.GETPASSCODE=" + Mill_Code + " and d.tran_type NOT IN('LV','MM') and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by mill";
                    lblPartyName.Text = "Milwise Dispatch of <b> " + clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster Where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Mill_Code) + "</b>";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["Mill_Code"]))
                {
                    qry = "select distinct(d.mill_code) as millcode,ISNULL(a.Short_Name,a.Ac_Name_E) as mill from " + tblPrefix + "deliveryorder d" +
                          " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_code=a.Company_Code where d.tran_type NOT IN('LV','MM') and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " order by mill";

                }
                else
                {
                    qry = "select distinct(d.mill_code) as millcode,ISNULL(a.Short_Name,a.Ac_Name_E) as mill from " + tblPrefix + "deliveryorder d" +
                            " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code AND d.company_code=a.Company_Code where d.GETPASSCODE=" + Mill_Code + " and d.tran_type NOT IN('LV','MM') and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " order by mill";
                    lblPartyName.Text = "Milwise Dispatch of <b> " + clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster Where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Mill_Code) + "</b>";
                }
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
        catch (Exception)
        {
            throw;
        }
    }
    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblMillCode = (Label)e.Item.FindControl("lblMillCode");
            Label lblqntltotal = (Label)e.Item.FindControl("lblqntltotal");
            Label lblamounttotal = (Label)e.Item.FindControl("lblamounttotal");
            string millcode = lblMillCode.Text;

            if (string.IsNullOrEmpty(Branch_Code))
            {
                if (string.IsNullOrEmpty(Request.QueryString["Mill_Code"]))
                {
                    qry = " select d.doc_no as do_no,CONVERT(VARCHAR(10),d.doc_date,103) as do_date,d.quantal as quantal,d.mill_rate as millrate,(d.quantal*d.mill_rate) as amount,a.Ac_Name_E as getpass,ISNULL(d.narration1,'GSTC.TRANSFER LETTER') as nar1," +
                        " ISNULL(b.Short_Name,b.Ac_Name_E) as do,d.truck_no as truck from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code " +
                        " left outer join " + tblPrefix + "AccountMaster b on d.DO=b.Ac_Code AND d.company_code=b.Company_Code where d.mill_code=" + millcode + " and d.tran_type NOT IN('LV','MM') and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }
                else
                {
                    qry = " select d.doc_no as do_no,CONVERT(VARCHAR(10),d.doc_date,103) as do_date,d.quantal as quantal,d.mill_rate as millrate,(d.quantal*d.mill_rate) as amount,a.Ac_Name_E as getpass,ISNULL(d.narration1,'GSTC.TRANSFER LETTER') as nar1," +
                " ISNULL(b.Short_Name,b.Ac_Name_E) as do,d.truck_no as truck from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code " +
                " left outer join " + tblPrefix + "AccountMaster b on d.DO=b.Ac_Code AND d.company_code=b.Company_Code where d.mill_code=" + millcode + " and d.voucher_by=" + Mill_Code + " and d.tran_type NOT IN('LV','MM') and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }

            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["Mill_Code"]))
                {
                    qry = " select d.doc_no as do_no,CONVERT(VARCHAR(10),d.doc_date,103) as do_date,d.quantal as quantal,d.mill_rate as millrate,(d.quantal*d.mill_rate) as amount,a.Ac_Name_E as getpass,ISNULL(d.narration1,'GSTC.TRANSFER LETTER') as nar1," +
                    " ISNULL(b.Short_Name,b.Ac_Name_E) as do,d.truck_no as truck from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster b on d.DO=b.Ac_Code AND d.company_code=b.Company_Code where d.mill_code=" + millcode + " and d.tran_type NOT IN('LV','MM') and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + "";
                }
                else
                {
                    qry = " select d.doc_no as do_no,CONVERT(VARCHAR(10),d.doc_date,103) as do_date,d.quantal as quantal,d.mill_rate as millrate,(d.quantal*d.mill_rate) as amount,a.Ac_Name_E as getpass,ISNULL(d.narration1,'GSTC.TRANSFER LETTER') as nar1," +
                    " ISNULL(b.Short_Name,b.Ac_Name_E) as do,d.truck_no as truck from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster b on d.DO=b.Ac_Code AND d.company_code=b.Company_Code where d.mill_code=" + millcode + " and d.voucher_by=" + Mill_Code + " and d.tran_type NOT IN('LV','MM') and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + "";
                }
            }

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("do_no", typeof(string)));
                dt.Columns.Add(new DataColumn("do_date", typeof(string)));
                dt.Columns.Add(new DataColumn("quantal", typeof(double)));
                dt.Columns.Add(new DataColumn("millrate", typeof(string)));
                dt.Columns.Add(new DataColumn("amount", typeof(double)));
                dt.Columns.Add(new DataColumn("narration", typeof(string)));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["do_no"] = ds.Tables[0].Rows[i]["do_no"].ToString();
                        dr["do_date"] = ds.Tables[0].Rows[i]["do_date"].ToString();
                        dr["quantal"] = ds.Tables[0].Rows[i]["quantal"].ToString();
                        dr["millrate"] = ds.Tables[0].Rows[i]["millrate"].ToString();
                        dr["amount"] = ds.Tables[0].Rows[i]["amount"].ToString();
                        string getpass = ds.Tables[0].Rows[i]["getpass"].ToString();
                        string nar1 = ds.Tables[0].Rows[i]["nar1"].ToString();
                        string DO = ds.Tables[0].Rows[i]["do"].ToString();
                        string truckno = ds.Tables[0].Rows[i]["truck"].ToString();
                        dr["narration"] = getpass + " " + nar1 + " " + DO + " " + truckno;
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        lblqntltotal.Text = Convert.ToString(dt.Compute("SUM(quantal)", string.Empty));
                        lblamounttotal.Text = Convert.ToString(dt.Compute("SUM(amount)", string.Empty));
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
        catch (Exception)
        {
            throw;
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
                msg.Body = "Dispatch Mill Wise Report";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Dispatch Mill Wise Report " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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