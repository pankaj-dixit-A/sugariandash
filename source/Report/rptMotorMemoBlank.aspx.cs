using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mime;

public partial class Report_rptMotorMemoBlank : System.Web.UI.Page
{
    string f = "../GSReports/MotorMemo_.htm";
    string f_Main = "../Sugar/pgeMotorMemo";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string trnType = "MM";
    string GLedgerTable = string.Empty;
    string filterDoNo = string.Empty;
    string partyEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string prefix = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
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
        qryCommon = tblPrefix + "qryDeliveryOrderListReport";
        cityMasterTable = tblPrefix + "CityMaster";
        filterDoNo = Request.QueryString["do_no"];
        partyEmail = Request.QueryString["email"];
        if (!IsPostBack)
        {
            this.BindReport();
        }
    }
    private void BindReport()
    {
        try
        {
            string qry = "select *, Convert(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no in (" + filterDoNo + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            DataList1.DataSource = ds;
            DataList1.DataBind();
            DataList2.DataSource = ds;
            DataList2.DataBind();
        }
        catch
        {

        }
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            imgSign.ImageUrl = imgurl;

            Label lblMillShortName = (Label)e.Item.FindControl("lblMillShortName");
            ViewState["millshort"] = lblMillShortName.Text;

            Label lblName = (Label)e.Item.FindControl("lblCompanyFooter");
            lblName.Text = Session["Company_Name"].ToString();

            Label lblTruck = (Label)e.Item.FindControl("lblTruck");
            ViewState["lorry"] = lblTruck.Text;

            Label lblGetPassName = (Label)e.Item.FindControl("lblGetPassName");
            ViewState["getpass"] = lblGetPassName.Text;

            Label lblGetpassCode = (Label)e.Item.FindControl("lblGetpassCode");

            Label lblgetpasscityCode = (Label)e.Item.FindControl("lblgetpasscityCode");

            Label lblGetPassCity = (Label)e.Item.FindControl("lblGetpassCity");
            lblGetPassCity.Text = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + lblgetpasscityCode.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            Label lblGetpassState = (Label)e.Item.FindControl("lblGetpassState");
            lblGetpassState.Text = clsCommon.getString("select state from " + cityMasterTable + " where city_code=" + lblgetpasscityCode.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            Label lblCSTNo = (Label)e.Item.FindControl("lblCSTNo");
            Label lblSLNo = (Label)e.Item.FindControl("lblSLNo");
            Label lblTinNo = (Label)e.Item.FindControl("lblTinNo");
            //Label lblGSTNo = (Label)e.Item.FindControl("lblGSTNo");
            //Label lblECCNo = (Label)e.Item.FindControl("lblECCNo");
            Label lblPan = (Label)e.Item.FindControl("lblPan");



            string CST = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string SLNo = clsCommon.getString("Select Local_Lic_No from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string TIN = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string GST = clsCommon.getString("Select Gst_No from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string ECC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string PAN = clsCommon.getString("Select CompanyPan from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string emailid = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string ccmail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtEmail.Text = ccmail + "," + emailid;
            Label lblgetpassMobile = (Label)e.Item.FindControl("lblgetpassMobile");
            string getpassofffone = clsCommon.getString("Select OffPhone from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string getpassmobile = clsCommon.getString("Select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code=" + lblGetpassCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string offPh = "";
            string mobile = "";
            if (!string.IsNullOrEmpty(getpassofffone))
            {
                offPh = "Off.Ph:<b>&nbsp;" + getpassofffone + "</b>";
            }
            if (!string.IsNullOrEmpty(getpassmobile))
            {
                mobile = "Mobile.:<b>&nbsp;" + getpassmobile + "</b>";
            }
            lblgetpassMobile.Text = offPh + " &nbsp;" + mobile;
            if (!string.IsNullOrWhiteSpace(CST))
            {
                lblCSTNo.Text = "CST: " + CST;
            }
            if (!string.IsNullOrWhiteSpace(SLNo))
            {
                lblSLNo.Text = "SL No: " + SLNo;
            }
            if (!string.IsNullOrWhiteSpace(TIN))
            {
                lblTinNo.Text = "TIN: " + TIN;
            }
            //if (!string.IsNullOrWhiteSpace(GST))
            //{
            //    lblGSTNo.Text = "GST: " + GST;
            //}
            //if (!string.IsNullOrWhiteSpace(ECC))
            //{
            //    lblECCNo.Text = "ECC: " + ECC;
            //}
            if (!string.IsNullOrWhiteSpace(PAN))
            {
                lblPan.Text = "PAN: " + PAN;
            }

            Label lblFinalAmt = (Label)e.Item.FindControl("lblFinalAmt");
            Label lblInWords = (Label)e.Item.FindControl("lblInWords");
            lblInWords.Text = clsNoToWord.ctgword(lblFinalAmt.Text);


            Label lblCompany = (Label)e.Item.FindControl("lblCompany");
            lblCompany.Text = "M/S. " + Session["Company_Name"].ToString();
            Label lblAl1 = (Label)e.Item.FindControl("lblAl1");
            Label lblAl2 = (Label)e.Item.FindControl("lblAl2");
            Label lblAl3 = (Label)e.Item.FindControl("lblAl3");
            Label lblAl4 = (Label)e.Item.FindControl("lblAl4");
            Label lblOtherDetails = (Label)e.Item.FindControl("lblOtherDetails");

            #region Address
            string qry = "Select * from tblVoucherHeadAddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("LeftAddress", typeof(string)));
                dt.Columns.Add(new DataColumn("MiddlePart", typeof(string)));
                dt.Columns.Add(new DataColumn("RightAddress", typeof(string)));

                string AL1 = ds.Tables[0].Rows[0]["AL1"].ToString();
                string AL2 = ds.Tables[0].Rows[0]["AL2"].ToString();
                string AL3 = ds.Tables[0].Rows[0]["AL3"].ToString();
                string AL4 = ds.Tables[0].Rows[0]["AL4"].ToString();
                string OtherDetails = ds.Tables[0].Rows[0]["Other"].ToString();

                string rnl = AL1.Replace("\n", "<br/>");
                var TabSpace = new String(' ', 4);
                string ab = rnl.Replace("\t", TabSpace);
                string la = ab.Replace(" ", "&nbsp;");
                lblAl1.Text = la;


                string rnl1 = AL2.Replace("\n", "<br/>");
                var TabSpace1 = new String(' ', 4);
                string ab1 = rnl1.Replace("\t", TabSpace1);
                string la1 = ab1.Replace(" ", "&nbsp;");
                lblAl2.Text = la1;

                string rnl2 = AL3.Replace("\n", "<br/>");
                var TabSpace2 = new String(' ', 4);
                string ab2 = rnl2.Replace("\t", TabSpace2);
                string la2 = ab2.Replace(" ", "&nbsp;");
                lblAl3.Text = la2;

                string rnl3 = AL4.Replace("\n", "<br/>");
                var TabSpace3 = new String(' ', 4);
                string ab3 = rnl3.Replace("\t", TabSpace2);
                string la3 = ab3.Replace(" ", "&nbsp;");
                lblAl4.Text = la3;

                string rnl4 = OtherDetails.Replace("\n", "<br/>");
                var TabSpace4 = new String(' ', 4);
                string ab4 = rnl4.Replace("\t", TabSpace2);
                string la4 = ab4.Replace(" ", "&nbsp;");
                lblOtherDetails.Text = la4;

            }
            #endregion

        }
        catch
        {

        }
    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            mail = txtEmail.Text;
            using (MemoryStream ms = new MemoryStream())
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(sw);
                pnl.RenderControl(tw);
                string s = sw.ToString();
                s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                byte[] array = Encoding.UTF8.GetBytes(s);
                ms.Write(array, 0, array.Length);
                ms.Seek(0, SeekOrigin.Begin);
                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Octet;
                contentType.Name = "TransferLetter.htm";
                Attachment attachment = new Attachment(ms, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "MOTOR MEMO";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "Memo No:" + Convert.ToString(Request.QueryString["do_no"]) + "-" + ViewState["lorry"].ToString() + "-" + ViewState["millshort"].ToString() + "-" + ViewState["getpass"].ToString();
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



            //StringWriter sw = new StringWriter();
            //HtmlTextWriter tw = new HtmlTextWriter(sw);
            //pnl.RenderControl(tw);
            //string s = sw.ToString();
            //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
            //try
            //{
            //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            //    {
            //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            //        {
            //            w.WriteLine(s);
            //        }
            //    }
            //}
            //catch (Exception ee)
            //{
            //    f = f_Main + ".htm";
            //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            //    {
            //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            //        {
            //            w.WriteLine(s);
            //        }
            //    }
            //}
            //string mailFrom = Session["EmailId"].ToString();
            //string smtpPort = "587";
            //string emailPassword = Session["EmailPassword"].ToString();
            //MailMessage msg = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            //SmtpServer.Host = clsGV.Email_Address;
            //msg.From = new MailAddress(mailFrom);
            //msg.To.Add(mail);
            //msg.Body = "MOTOR MEMO";

            //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            //msg.IsBodyHtml = true;
            //msg.Subject = "Memo No:" + Convert.ToString(Request.QueryString["do_no"]) + "-" + ViewState["lorry"].ToString() + "-" + ViewState["millshort"].ToString() + "-" + ViewState["getpass"].ToString();

            //msg.IsBodyHtml = true;
            //if (smtpPort != string.Empty)
            //{
            //    SmtpServer.Port = Convert.ToInt32(smtpPort);
            //}
            //                     SmtpServer.EnableSsl = true;
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            //SmtpServer.Send(msg);
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
        pnl.RenderControl(tw);
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