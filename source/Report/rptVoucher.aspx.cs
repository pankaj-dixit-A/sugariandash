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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Report_rptVoucher : System.Web.UI.Page
{
    string qryCommon = string.Empty;
    string tblPrefix = string.Empty;
    string cityMasterTable = string.Empty;
    string prefix = string.Empty;
    string vtype = string.Empty;
    string f = "../GSReports/VoucherPrint_.htm";
    string f_Main = "../Report/rptVoucher";
    string voucher = "Voucher";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["tblPrefix"] != null)
            {
                tblPrefix = Session["tblPrefix"].ToString();
            }
            else
            {
                prefix = clsCommon.getString("Select tblPrefix from tblPrefix");
                tblPrefix = prefix.ToString();
            }
            qryCommon = tblPrefix + "qryVoucherList";
            cityMasterTable = tblPrefix + "CityMaster";
            ViewState["VNO"] = Request.QueryString["VNO"];
            ViewState["mailID"] = Request.QueryString["mailID"];
            ViewState["pageBreak"] = Request.QueryString["pageBreak"];
            vtype = Request.QueryString["type"].ToString();
            this.bindData();
            
        }
    }

    private void bindData()
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (vtype != string.Empty || vtype != null)
            {
                qry = "select * from " + qryCommon + " where Doc_No in (" + ViewState["VNO"].ToString() + ") and Tran_Type='" + vtype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            }
            else
            {
                qry = "select * from " + qryCommon + " where Doc_No in (" + ViewState["VNO"].ToString() + ") and Tran_Type='" + vtype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            }
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataList1.DataSource = dt;
                        DataList1.DataBind();
                    }
                }
            }
        }
        catch (Exception exx)
        {
            Response.Write("voucher Number not present");
        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            Label lblvocno = (Label)e.Item.FindControl("lblvocno");
            Label lblvoctype = (Label)e.Item.FindControl("lblvoctype");
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            string vno = lblvocno.Text;
            string type = lblvoctype.Text;
            string qry = "select * from " + qryCommon + " where Doc_No=" + vno + " and Tran_Type='" + type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsV = new DataSet();
            DataTable dt = new DataTable();
            dsV = clsDAL.SimpleQuery(qry);
            if (dsV.Tables[0].Rows.Count > 0)
            {
                dsV.Tables[0].Columns.Add(new DataColumn("VoucherNo", typeof(string)));
                double vouchamt = Convert.ToDouble(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());
                if (vouchamt < 0)
                {
                    dsV.Tables[0].Rows[0]["VoucherNo"] = "Credit Note No:";
                }
                else
                {
                    dsV.Tables[0].Rows[0]["VoucherNo"] = "Debit Note No:";
                }
                dsV.Tables[0].Rows[0]["Voucher_Amount"] = Math.Abs(vouchamt);

                string narration = dsV.Tables[0].Rows[0]["Narration1"].ToString();

                int startIndex = narration.IndexOf('(');
                int EndIndex = narration.LastIndexOf('P');
                int count = (EndIndex - 1) - startIndex;
                string NWMR = narration.Remove(startIndex + 1, count);
                dsV.Tables[0].Rows[0]["Narration1"] = NWMR;
                dt = dsV.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
            }
        }
        catch (Exception exx1)
        {
            //  Unable to cast object of type 'System.Web.UI.HtmlControls.HtmlTable' to type 'System.Web.UI.WebControls.Table'.
            Response.Write(exx1.Message);
        }
    }
    protected void dtlDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblAddr = (Label)e.Item.FindControl("lblCompanyAddr");
        lblAddr.Text = clsGV.CompanyAddress;

        Label lblPartyCity = (Label)e.Item.FindControl("lblPartyCity");
        lblPartyCity.Text = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + lblPartyCity.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

        Label lblPhone = (Label)e.Item.FindControl("lblCompanyMobile");
        lblPhone.Text = clsGV.CompanyPhone;

        Label lblCompany = (Label)e.Item.FindControl("lblCompany");
        lblCompany.Text = Session["Company_Name"].ToString();

        Label lblCompanyBottom = (Label)e.Item.FindControl("lblCompanyBottom");
        lblCompanyBottom.Text = Session["Company_Name"].ToString();

        //Label lblMillEmail = (Label)e.Item.FindControl("lblMillEmail");
        //lblMillEmail.Text = millEmail;
        //if (ViewState["pageBreak"] != null)
        //{
        //    if (ViewState["pageBreak"].ToString() == "N")
        //    {
        //        System.Web.UI.HtmlControls.HtmlTable tb = (System.Web.UI.HtmlControls.HtmlTable)e.Item.FindControl("tbMain");
        //        tb.Style["page-break-after"] = "avoid";
        //    }
        //    else
        //    {
        //        System.Web.UI.HtmlControls.HtmlTable tb = (System.Web.UI.HtmlControls.HtmlTable)e.Item.FindControl("tbMain");
        //        tb.Style["page-break-after"] = "always";
        //    }
        //}
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["type"].ToString() == "LV")
            {
                voucher = "Local Voucher";
            }
            if (Request.QueryString["type"].ToString() == "OV")
            {
                voucher = "Loading Voucher";
            }
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
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
                msg.Body = "Voucher Print";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = voucher + " " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
    protected void btnExportToPdf_Click1(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            PrintPanel.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }
}