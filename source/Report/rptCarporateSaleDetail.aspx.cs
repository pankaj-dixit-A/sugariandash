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

public partial class Report_rptCarporateSaleDetail : System.Web.UI.Page
{
    DataSet ds = null;
    DataTable dt = null;
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    string Branch_Code = string.Empty;
    string fromDt = string.Empty;
    string toDt = string.Empty;
    string f = "../GSReports/CarporateSaleDetail_.htm";
    string f_Main = "../Report/CarporateSaleDetail_";
    string PDS = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        if (string.IsNullOrEmpty(Convert.ToString(Request.QueryString["Branch_Code"])))
        {
            Branch_Code = "";
        }
        else
        {
            Branch_Code = Convert.ToString(Request.QueryString["Branch_Code"]);
        }
        fromDt = Request.QueryString["fromDt"];
        toDt = Request.QueryString["toDt"];
        PDS = Request.QueryString["PDS"];
        if (!IsPostBack)
        {
            BindList();
            lblCompany.Text = Session["Company_Name"].ToString();
        }
    }

    private void BindList()
    {
        try
        {
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select DISTINCT(c.Doc_No) as CSNO,x.Short_Name as CSName,CONVERT(varchar(10),c.Doc_Date,103) as CSDate ,b.Ac_Name_E as CSUnitName,c.SELL_RATE as CSSaleRate,c.Quantal as CSQntl," +
                       " ISNULL((SELECT SUM(d.quantal) as desp FROM " + tblPrefix + "deliveryorder d where d.Carporate_Sale_No=c.Doc_No And d.company_code=c.Company_Code And d.Year_Code=c.Year_Code group by d.Carporate_Sale_No),0) as CSDesp" +
                        " ,c.PODETAIL as CSPodetails from " + tblPrefix + "CarporateSale c" +
                        " left outer join " + tblPrefix + "AccountMaster b on c.Unit_Code=b.Ac_Code and c.Company_Code=b.Company_Code" +
                        " left outer join " + tblPrefix + "AccountMaster x on c.Ac_Code=x.Ac_Code and c.Company_Code=x.Company_Code" +
                        " where c.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " And c.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and c.Doc_Date between '" + fromDt + "' and '" + toDt + "' and c.SellingType='" + PDS + "' order by CSName asc";
            }
            else
            {
                qry = "select distinct(c.Doc_No) as CSNO,x.Short_Name as CSName,CONVERT(varchar(10),c.Doc_Date,103) as CSDate ,b.Ac_Name_E as CSUnitName,c.SELL_RATE as CSSaleRate,c.Quantal as CSQntl," +
                      " ISNULL((SELECT SUM(d.quantal) as desp FROM " + tblPrefix + "deliveryorder d where d.Carporate_Sale_No=c.Doc_No And d.company_code=c.Company_Code And d.Year_Code=c.Year_Code group by d.Carporate_Sale_No),0) as CSDesp" +
                      " ,c.PODETAIL as CSPodetails from " + tblPrefix + "CarporateSale c" +
                      " left outer join " + tblPrefix + "AccountMaster b on c.Unit_Code=b.Ac_Code and c.Company_Code=b.Company_Code" +
                      " left outer join " + tblPrefix + "AccountMaster x on c.Ac_Code=x.Ac_Code and c.Company_Code=x.Company_Code" +
                      " where c.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " And c.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and c.Branch_Code=" + Branch_Code + " and c.Doc_Date between '" + fromDt + "' and '" + toDt + "' and c.SellingType='" + PDS + "' order by CSName asc";

            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("CSNO", typeof(string)));
                dt.Columns.Add(new DataColumn("CSName", typeof(string)));
                dt.Columns.Add(new DataColumn("CSDate", typeof(string)));
                dt.Columns.Add(new DataColumn("CSUnitName", typeof(string)));
                dt.Columns.Add(new DataColumn("CSSaleRate", typeof(double)));
                dt.Columns.Add(new DataColumn("CSQntl", typeof(double)));
                dt.Columns.Add(new DataColumn("CSDesp", typeof(double)));
                dt.Columns.Add(new DataColumn("CSBalance", typeof(double)));
                dt.Columns.Add(new DataColumn("CSPodetails", typeof(string)));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["CSNO"] = ds.Tables[0].Rows[i]["CSNO"].ToString();
                        dr["CSName"] = ds.Tables[0].Rows[i]["CSName"].ToString();
                        dr["CSDate"] = ds.Tables[0].Rows[i]["CSDate"].ToString();
                        dr["CSUnitName"] = ds.Tables[0].Rows[i]["CSUnitName"].ToString();
                        dr["CSSaleRate"] = ds.Tables[0].Rows[i]["CSSaleRate"].ToString();
                        double Qntl = Convert.ToDouble(ds.Tables[0].Rows[i]["CSQntl"].ToString());
                        dr["CSQntl"] = Qntl;
                        double desp = Convert.ToDouble(ds.Tables[0].Rows[i]["CSDesp"].ToString());
                        dr["CSDesp"] = desp;
                        double balance = Qntl - desp;
                        dr["CSBalance"] = balance;
                        dr["CSPodetails"] = ds.Tables[0].Rows[i]["CSPodetails"].ToString();
                        //if (balance != 0)
                        //{
                            dt.Rows.Add(dr);
                        //}
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                    }
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
        try
        {
            DataList dtlDetails = e.Item.FindControl("dtlDetails") as DataList;
            Label lblCSNo = e.Item.FindControl("lblCSNo") as Label;
            string carporatesaleno = lblCSNo.Text.ToString();
            if (!string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select d.doc_no as DONo,CONVERT(varchar(10),d.doc_date,103) as DODate,d.quantal as DODesp,a.Short_Name as DOMill,d.narration4,s.PartyName," +
                    " d.truck_no as DOLorryNo,d.Freight_Amount as DOFrt,b.Short_Name as DOTransport,d.driver_no as DOMobile,c.Ac_Name_E as DOGetpass,d.voucher_no as VN,d.voucher_type as VT,d.memo_no as MM,d.SB_No as SB from " + tblPrefix + "deliveryorder d" +
                    " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster b on d.transport=b.Ac_Code and d.company_code=b.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster c on d.GETPASSCODE=c.Ac_Code and d.company_code=c.Company_Code" +
                    " left outer join " + tblPrefix + "qrySugarSaleList s on d.SB_No=s.doc_no and d.company_code=s.Company_Code and d.Year_Code=s.Year_Code" +
                    " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " And d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.Carporate_Sale_No=" + carporatesaleno + " and d.Branch_Code=" + Branch_Code + "";
            }
            else
            {
                qry = "select d.doc_no as DONo,CONVERT(varchar(10),d.doc_date,103) as DODate,d.quantal as DODesp,a.Short_Name as DOMill,d.narration4,s.PartyName," +
                      " d.truck_no as DOLorryNo,d.Freight_Amount as DOFrt,b.Short_Name as DOTransport,d.driver_no as DOMobile,c.Ac_Name_E as DOGetpass,d.voucher_no as VN,d.voucher_type as VT,d.memo_no as MM,d.SB_No as SB from " + tblPrefix + "deliveryorder d" +
                      " left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code" +
                      " left outer join " + tblPrefix + "AccountMaster b on d.transport=b.Ac_Code and d.company_code=b.Company_Code" +
                      " left outer join " + tblPrefix + "AccountMaster c on d.GETPASSCODE=c.Ac_Code and d.company_code=c.Company_Code" +
                      " left outer join " + tblPrefix + "qrySugarSaleList s on d.SB_No=s.doc_no and d.company_code=s.Company_Code and d.Year_Code=s.Year_Code" +
                      " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " And d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.Carporate_Sale_No=" + carporatesaleno + "";
            }

            DataSet ds1 = new DataSet();
            ds1 = clsDAL.SimpleQuery(qry);
            if (ds1 != null)
            {
                ds1.Tables[0].Columns.Add(new DataColumn("PS", typeof(string)));
                ds1.Tables[0].Columns.Add(new DataColumn("VO", typeof(string)));

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string VN = ds1.Tables[0].Rows[i]["VN"].ToString();
                    string VT = ds1.Tables[0].Rows[i]["VT"].ToString();
                    string SB = ds1.Tables[0].Rows[i]["SB"].ToString();

                    if (VT == "PS")
                    {
                        ds1.Tables[0].Rows[i]["PS"] = VN;
                    }
                    else
                    {
                        ds1.Tables[0].Rows[i]["VO"] = VN;

                    }
                    if (!string.IsNullOrEmpty(SB))
                    {
                        ds1.Tables[0].Rows[i]["DOGetpass"] = ds1.Tables[0].Rows[i]["PartyName"].ToString();
                        ds1.Tables[0].Rows[i]["SB"] = SB;

                    }
                }
                dt = new DataTable();
                dt = ds1.Tables[0];
                if (dt.Rows.Count > 0)
                {
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
        catch (Exception)
        {

            throw;
        }

    }
    protected void lnkPS_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkPS = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkPS.NamingContainer;
            Label Label8 = (Label)item.FindControl("Label8");
            string ps = lnkPS.Text;
            Session["PURC_NO"] = ps;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:sugarpurchase();", true);
            lnkPS.Focus();
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void lnkSB_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSB = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkSB.NamingContainer;
            string no = lnkSB.Text;
            Session["SB_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsdsd", "javascript:salebill();", true);
            lnkSB.Focus();
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void lnkOV_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkOV = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkOV.NamingContainer;
            string no = lnkOV.Text;
            Session["VOUC_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:loadingvoucher();", true);
            lnkOV.Focus();
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void lnkMM_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkMM = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkMM.NamingContainer;
            string no = lnkMM.Text;
            Session["MEMO_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjs", "javascript:memo();", true);
            lnkMM.Focus();
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
                msg.Body = "Carporate Sell Details";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Carporate Sell Details " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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