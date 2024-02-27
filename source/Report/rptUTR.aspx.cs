using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;

public partial class Report_rptUTR : System.Web.UI.Page
{
    string accode = string.Empty;
    string utr_no = string.Empty;
    string tblPrefix = string.Empty;
    string tblUtr = string.Empty;
    string qry = string.Empty;
    string AcType = string.Empty;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
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
            lblCompanyName.Text = Session["Company_Name"].ToString();
            BindData();
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();

            if (accode != string.Empty)
            {
                if (utr_no != string.Empty)
                {
                    if (AcType != string.Empty)
                    {
                        if (AcType == "M")
                        {
                            qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,utr_no as UTR_BANK_NUMBER,amount as  UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code='" + accode + "' AND doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                        }
                        else
                        {
                            qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,utr_no as UTR_BANK_NUMBER,amount as UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and bank_ac='" + accode + "' AND doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                        }
                    }
                }
                else
                {
                    if (AcType != string.Empty)
                    {
                        if (AcType == "M")
                        {
                            qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,mill_code,utr_no as UTR_BANK_NUMBER,amount as UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code='" + accode + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                        }
                        else
                        {
                            qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,mill_code,utr_no as UTR_BANK_NUMBER,amount as UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and bank_ac='" + accode + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                        }
                    }
                }
            }
            else
            {
                if (accode == string.Empty && utr_no == string.Empty)
                {
                    qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,mill_code,utr_no as UTR_BANK_NUMBER,amount as UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                }
                if (utr_no != string.Empty)
                {
                    qry = "select doc_no as UTR,CONVERT(varchar(10),doc_date,103) as UTR_Date,bank_ac,mill_code,utr_no as UTR_BANK_NUMBER,amount as UTR_AMOUNT,narration_header from " + tblPrefix + "UTR where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no='" + utr_no + "' and doc_date between '" + FromDt + "' and '" + ToDt + "' and IsSave=1";
                }
            }

            ds = clsDAL.SimpleQuery(qry);
            DataTable dt = new DataTable();
            dt.Columns.Add("UTR_NO", typeof(string));
            dt.Columns.Add("UTR_DATE", typeof(string));
            dt.Columns.Add("bank_ac", typeof(string));
            dt.Columns.Add("MillShort", typeof(string));
            dt.Columns.Add("UTR_BANK_NUMBER", typeof(string));
            dt.Columns.Add("UTR_AMOUNT", typeof(string));
            dt.Columns.Add("Narration", typeof(string));

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();

                        dr["UTR_NO"] = ds.Tables[0].Rows[i]["UTR"].ToString();
                        dr["UTR_DATE"] = ds.Tables[0].Rows[i]["UTR_Date"].ToString();
                        string bank_ac = ds.Tables[0].Rows[i]["bank_ac"].ToString();
                        dr["bank_ac"] = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + bank_ac + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string mill_code = ds.Tables[0].Rows[i]["mill_code"].ToString();
                        dr["MillShort"] = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + mill_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        dr["UTR_BANK_NUMBER"] = ds.Tables[0].Rows[i]["UTR_BANK_NUMBER"].ToString();
                        dr["UTR_AMOUNT"] = ds.Tables[0].Rows[i]["UTR_AMOUNT"].ToString();
                        dr["Narration"] = ds.Tables[0].Rows[i]["narration_header"].ToString();
                        dt.Rows.Add(dr);

                    }
                }
                dtl.DataSource = dt;
                dtl.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtl1 = (DataList)e.Item.FindControl("dtlDetails");

            DataSet ds = new DataSet();
            string utrno = string.Empty;
            Label lblUtrno = (Label)e.Item.FindControl("lblUtrNo");
            utrno = lblUtrno.Text.ToString();
            string qry = "";
            if (accode != string.Empty)
            {
                qry = "select U.doc_no as DO_NO,CONVERT(varchar(10),D.doc_date,103) as DO_DATE,A.Ac_Name_E AS GETPASS,D.truck_no as LORRY,b.Short_Name as Voucher_By,D.quantal AS QNTL,D.mill_rate AS MR, " +
                      " SUM(U.Amount) as USEDAmount from " + tblPrefix + "deliveryorder D LEFT OUTER JOIN " + tblPrefix + "DODetails U " +
                      " on D.doc_no=U.DO_No and D.company_code=U.Company_Code and D.Year_Code=U.Year_Code LEFT OUTER JOIN " + tblPrefix + "AccountMaster A ON D.GETPASSCODE=A.Ac_Code and D.company_code=A.Company_Code" +
                      " LEFT OUTER JOIN " + tblPrefix + "AccountMaster b ON D.voucher_by=b.Ac_Code and D.company_code=b.Company_Code where U.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and U.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and U.Bank_Code='" + accode + "' AND U.UTR_NO='" + utrno + "' AND D.tran_type='DO' group by U.doc_no,D.doc_date,A.Ac_Name_E,b.Short_Name,D.quantal,D.mill_rate,D.UTR_No,D.truck_no";
            }
            else
            {
                qry = "select U.doc_no as DO_NO,CONVERT(varchar(10),D.doc_date,103) as DO_DATE,D.truck_no as LORRY,A.Ac_Name_E AS GETPASS,b.Short_Name as Voucher_By,D.quantal AS QNTL,D.mill_rate AS MR, " +
                      " SUM(U.Amount) as USEDAmount from " + tblPrefix + "deliveryorder D LEFT OUTER JOIN " + tblPrefix + "DODetails U " +
                      " on D.doc_no=U.DO_No and D.company_code=U.Company_Code and D.Year_Code=U.Year_Code LEFT OUTER JOIN " + tblPrefix + "AccountMaster A ON D.GETPASSCODE=A.Ac_Code and D.company_code=A.Company_Code" +
                      " LEFT OUTER JOIN " + tblPrefix + "AccountMaster b ON D.voucher_by=b.Ac_Code and D.company_code=b.Company_Code where U.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and U.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and U.UTR_NO='" + utrno + "' and D.tran_type='DO' group by U.doc_no,D.doc_date,A.Ac_Name_E,b.Short_Name,D.quantal,D.mill_rate,D.UTR_No,D.truck_no";
            }
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DO_NO", typeof(string));
                dt.Columns.Add("DO_DATE", typeof(string));
                dt.Columns.Add("DO_GETPASSCODE", typeof(string));
                dt.Columns.Add("LORRY", typeof(string));
                dt.Columns.Add("DO_VoucherBy", typeof(string));
                dt.Columns.Add("QNTL", typeof(string));
                dt.Columns.Add("MR", typeof(string));
                dt.Columns.Add("USEDAmount", typeof(double));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DO_NO"] = ds.Tables[0].Rows[i]["DO_NO"].ToString();
                            dr["DO_DATE"] = ds.Tables[0].Rows[i]["DO_DATE"].ToString();
                            dr["DO_GETPASSCODE"] = ds.Tables[0].Rows[i]["GETPASS"].ToString();
                            dr["LORRY"] = ds.Tables[0].Rows[i]["LORRY"].ToString();
                            dr["DO_VoucherBy"] = ds.Tables[0].Rows[i]["Voucher_By"].ToString();
                            dr["QNTL"] = ds.Tables[0].Rows[i]["QNTL"].ToString();
                            dr["MR"] = ds.Tables[0].Rows[i]["MR"].ToString();
                            dr["USEDAmount"] = ds.Tables[0].Rows[i]["USEDAmount"].ToString();
                            dt.Rows.Add(dr);

                        }

                        Label utrBalance = (Label)e.Item.FindControl("lblUtrAmount");
                        Label lblUtrAmountTotal = (Label)e.Item.FindControl("lblUtrAmountTotal");
                        Label lblUtrUsedAmount = (Label)e.Item.FindControl("lblUtrUsedAmount");
                        Label lblUtrBal = (Label)e.Item.FindControl("lblUtrBal");


                        double utramount = double.Parse(utrBalance.Text);
                        lblUtrAmountTotal.Text = "UTR AMOUNT=" + utramount.ToString();
                        double sum = Convert.ToDouble(dt.Compute("SUM(USEDAmount)", string.Empty));
                        lblUtrUsedAmount.Text = "Used Amount=" + sum.ToString();
                        double Balance = utramount - sum;
                        lblUtrBal.Text = "UTR Balance=" + Balance.ToString();
                    }
                    dtl1.DataSource = dt;
                    dtl1.DataBind();
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
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            string mail = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter tw = new HtmlTextWriter(sw);
                    pnlMain.RenderControl(tw);
                    string s = sw.ToString();
                    s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                    byte[] array = Encoding.UTF8.GetBytes(s);
                    ms.Write(array, 0, array.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = "DeliveryOrder.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(mail);
                    msg.Body = "UTR Report";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "UTR Report";
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
        }
        catch (Exception e1)
        {
            Response.Write("mail err:" + e1);
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }
}