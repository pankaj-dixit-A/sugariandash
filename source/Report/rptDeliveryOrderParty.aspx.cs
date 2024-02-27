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
using System.Web.UI.HtmlControls;
using System.Net.Mime;

public partial class Report_rptDeliveryOrderParty : System.Web.UI.Page
{
    string f = "../GSReports/doprint_.htm";
    string f_Main = "../GSReports/DO_";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string trnType = "DO";
    string GLedgerTable = string.Empty;
    string filterDoNo = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string prefix = string.Empty;
    string PO = string.Empty;
    string Email_Subject = "";
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
        millEmail = Request.QueryString["email"];
        mail = txtEmail.Text;
        PO = Request.QueryString["PO"];

        if (!Page.IsPostBack)
        {
            this.BindReport();
        }
    }

    private void BindReport()
    {
        try
        {
            txtEmail.Text = millEmail;
            //Response.Write("1");
            string qry = "select distinct(tran_type),*,Convert(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no in (" + filterDoNo + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataTable dtBind = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            //  Response.Write("2");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    string purNo = ds.Tables[0].Rows[0]["purc_no"].ToString();
                    string Sell_Note_No = clsCommon.getString("Select Sell_Note_No from " + tblPrefix + "Tender where Tender_No=" + purNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    dt = ds.Tables[0];
                    DataTable dt2 = new DataTable();
                    dt2 = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Local_Lic_No"].ToString()))
                        {
                            ds.Tables[0].Rows[0]["Local_Lic_No"] = "LIC: " + ds.Tables[0].Rows[0]["Local_Lic_No"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Cst_no"].ToString()))
                        {
                            ds.Tables[0].Rows[0]["Cst_no"] = "CST: " + ds.Tables[0].Rows[0]["Cst_no"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ECC_No"].ToString()))
                        {
                            ds.Tables[0].Rows[0]["ECC_No"] = "ECC: " + ds.Tables[0].Rows[0]["ECC_No"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Tin_No"].ToString()))
                        {
                            ds.Tables[0].Rows[0]["Tin_No"] = "TIN: " + ds.Tables[0].Rows[0]["Tin_No"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["PAN_No"].ToString()))
                        {
                            ds.Tables[0].Rows[0]["PAN_No"] = "PAN: " + ds.Tables[0].Rows[0]["PAN_No"].ToString();
                        }
                        string final_amount = ds.Tables[0].Rows[0]["final_amount"].ToString();
                        dt.Columns.Add(new DataColumn("InWords", typeof(string)));
                        string inwords = clsNoToWord.ctgword(final_amount);
                        ds.Tables[0].Rows[0]["InWords"] = inwords;
                        dt.Columns.Add(new DataColumn("Sell_Note_No", typeof(string)));
                        dt.Columns.Add(new DataColumn("SaleNoteHead", typeof(string)));
                        if (!string.IsNullOrWhiteSpace(Sell_Note_No))
                        {
                            dt.Rows[0]["SaleNoteHead"] = "Sell Note:";
                            dt.Rows[0]["Sell_Note_No"] = "No_ " + Sell_Note_No;
                        }
                        DataList1.DataSource = dt;
                        DataList1.DataBind();
                        DataList2.DataSource = dt2;
                        DataList2.DataBind();
                    }
                }
            }
        }
        catch (Exception ess)
        {
            Response.Write("exx1:" + ess.Message);
        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            string DOName = "";
            string docode = "";
            string getpasscode = "";
            Image imgSign = (Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            imgSign.ImageUrl = imgurl;

            Label lblDocNo = (Label)e.Item.FindControl("lblDocNo");
            Label lblCity = (Label)e.Item.FindControl("lblCity");
            string docno = lblDocNo.Text.ToString();
            string qry = "select distinct *,Convert(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no=" + docno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    getpasscode = ds.Tables[0].Rows[0]["GETPASSCODE"].ToString();
                    DOName = ds.Tables[0].Rows[0]["DOName"].ToString();
                    if (!string.IsNullOrEmpty(DOName))
                    {
                        docode = clsCommon.getString("Select Ac_Code from " + tblPrefix + "AccountMaster where Ac_Name_E='" + DOName + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    }
                }
            }
            Label lblName = (Label)e.Item.FindControl("lblCompanyName");
            Label lblAl1 = (Label)e.Item.FindControl("lblAl1");
            Label lblAl2 = (Label)e.Item.FindControl("lblAl2");
            Label lblAl3 = (Label)e.Item.FindControl("lblAl3");
            Label lblAl4 = (Label)e.Item.FindControl("lblAl4");
            if (DOName == "Self" || string.IsNullOrEmpty(DOName) || PO == "O")
            {
                #region Address
                qry = "Select * from tblVoucherHeadAddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                DataSet ds1 = new DataSet();
                ds1 = clsDAL.SimpleQuery(qry);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("LeftAddress", typeof(string)));
                    dt.Columns.Add(new DataColumn("MiddlePart", typeof(string)));
                    dt.Columns.Add(new DataColumn("RightAddress", typeof(string)));

                    string AL1 = ds1.Tables[0].Rows[0]["AL1"].ToString();
                    string AL2 = ds1.Tables[0].Rows[0]["AL2"].ToString();
                    string AL3 = ds1.Tables[0].Rows[0]["AL3"].ToString();
                    string AL4 = ds1.Tables[0].Rows[0]["AL4"].ToString();
                    string OtherDetails = ds1.Tables[0].Rows[0]["Other"].ToString();

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
                    // lblOtherDetails.Text = la4;

                }
                #endregion
                lblName.Text = Session["Company_Name"].ToString();
            }
            else
            {
                lblName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string address = clsCommon.getString("Select Address_E from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string citycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string docity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code='" + citycode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string dostate = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code='" + citycode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string parcity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code='" + citycode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string pincode = clsCommon.getString("Select Pincode from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                string phone = clsCommon.getString("Select OffPhone from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string mobile = clsCommon.getString("Select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string offPhone = string.Empty;
                string mob = string.Empty;
                string TIN = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string ECC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string SLN = clsCommon.getString("Select Local_Lic_No from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string GST = clsCommon.getString("Select Gst_No from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string CST = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code='" + docode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                string TINNo = string.Empty;
                string ECCNo = string.Empty;
                string SLNNo = string.Empty;
                string GSTNo = string.Empty;
                string CSTNo = string.Empty;

                if (!string.IsNullOrWhiteSpace(phone))
                {
                    offPhone = "Office Ph.No:-" + phone;
                }
                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    mob = "Mobile:-" + mobile;
                }
                if (!string.IsNullOrEmpty(TIN.Trim().ToString()))
                {
                    TINNo = "TIN: " + TIN;
                }
                if (!string.IsNullOrEmpty(ECC.Trim().ToString()))
                {
                    ECCNo = "ECC: " + ECC;
                }
                if (!string.IsNullOrEmpty(SLN.Trim().ToString()))
                {
                    SLNNo = "SLN: " + SLN;
                }
                if (!string.IsNullOrEmpty(GST.Trim().ToString()))
                {
                    GSTNo = "GST: " + GST;
                }
                if (!string.IsNullOrEmpty(CST.Trim().ToString()))
                {
                    CSTNo = "CST: " + CST;
                }

                lblAl1.Text = address + "<br/>" + docity + " " + "City: " + docity + " State: " + dostate + " Pin:&nbsp; " + pincode + " " + offPhone + "  " + mob;
                lblAl2.Text = TINNo + " " + ECCNo + " " + SLNNo;
                lblAl3.Text = GSTNo + " " + CSTNo;
            }
            Label lblWithExciseRate = (Label)e.Item.FindControl("lblWithExciseRate");
            Label lblExciseRate = (Label)e.Item.FindControl("lblExciseRate");
            Label lblGetpassCity = (Label)e.Item.FindControl("lblGetpassCity");
            string city = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + lblGetpassCity.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("select state from " + cityMasterTable + " where city_code=" + lblGetpassCity.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string getpasspincode = clsCommon.getString("Select Pincode from " + tblPrefix + "AccountMaster where Ac_Code='" + getpasscode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            lblGetpassCity.Text = "City: <b>" + city + "</b>&nbsp;&nbsp;&nbsp;State:<b>" + state + "</b>&nbsp;&nbsp;&nbsp;Pincode: <b>" + getpasspincode + "</b>";

            Label lblWithoutExcise = (Label)e.Item.FindControl("lblWithoutExcise");
            lblWithoutExcise.Text = (Convert.ToDouble(lblWithExciseRate.Text) - Convert.ToDouble(lblExciseRate.Text)).ToString();

            //Label lblMillEmail = (Label)e.Item.FindControl("lblMillEmail");
            //lblMillEmail.Text = millEmail;
            Label lblDOfrom = (Label)e.Item.FindControl("lblDOfrom");
            if (PO == "O")
            {
                lblDOfrom.Text = "<b>" + Session["Company_Name"].ToString() + "</b>";
            }
            else
            {
                lblDOfrom.Text = "<b>" + DOName + "</b>";
            }
            Label lblCompanyBottom = (Label)e.Item.FindControl("lblCompanyBottom");
            lblCompanyBottom.Text = "( " + Session["Company_Name"].ToString() + " )";
        }
        catch (Exception ess1)
        {
            Response.Write("exx1:" + ess1.Message + " line=");
        }
    }

    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            Label DoNo = (Label)DataList1.Items[0].FindControl("lblDocNo");
            Label Truck = (Label)DataList1.Items[0].FindControl("lblTruckNo");
            Label Qntl = (Label)DataList1.Items[0].FindControl("lblQntl");
            Label lblGetPassName = (Label)DataList1.Items[0].FindControl("lblGetPassName");
            string Do = Convert.ToString(DoNo.Text);
            string Truck_No = Convert.ToString(Truck.Text);
            string DOQntl = Convert.ToString(Qntl.Text);
            mail = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("<!DOCTYPE html PUBLIC ?-//W3C//DTD XHTML 1.0 Transitional//EN? ?http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd?>");
                    //sb.Append("<html xmlns=?http://www.w3.org/1999/xhtml?><head runat=?server?>");
                    //sb.Append(clsGV.printcss);
                    //sb.Append("<script type=?text/javascript?>function print_invoice() {var printContents = document.getElementById(?pnl?).innerHTML;");
                    //sb.Append("var originalContents = document.body.innerHTML;document.body.innerHTML = printContents;window.print();");
                    //sb.Append("document.body.innerHTML = originalContents; }</script>");
                    //sb.Append("</head><body class=?printhalf?><form id=?form1? runat=?server?> <div align=?left?><input type=?button? onclick=?print_invoice();? id=?input? value=?Click Here For Print? /></div>");
                    //sb.Append("<div align=?center? style=?width:100%;?>");
                    //sb.Replace('?', '"');
                    //sb.Replace("../", "http://" + clsGV.Website + "/");
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter tw = new HtmlTextWriter(sw);
                    pnl.RenderControl(tw);
                    string s = sw.ToString();
                    s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                    //sb.Append(s);
                    //sb.Append("</div>");
                    //sb.Append("</form></body></html>");
                    //string a = sb.ToString();

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
                    msg.Body = "DO";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "DO.No:" + Do + " " + Truck_No + " Qt:" + DOQntl + " " + lblGetPassName.Text;
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

                ////StringBuilder sb = new StringBuilder();
                ////sb.Append("<!DOCTYPE html PUBLIC ?-//W3C//DTD XHTML 1.0 Transitional//EN? ?http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd?>");
                ////sb.Append("<html xmlns=?http://www.w3.org/1999/xhtml?><head runat=?server?>");
                ////sb.Append(clsGV.printcss);
                ////sb.Append("<script type=?text/javascript?>function print_invoice() {var printContents = document.getElementById(?pnl?).innerHTML;");
                ////sb.Append("var originalContents = document.body.innerHTML;document.body.innerHTML = printContents;window.print();");
                ////sb.Append("document.body.innerHTML = originalContents; }</script>");
                ////sb.Append("</head><body class=?printhalf?><form id=?form1? runat=?server?> <div align=?left?><input type=?button? onclick=?print_invoice();? id=?input? value=?Click Here For Print? /></div>");
                ////sb.Append("<div align=?center? style=?width:100%;?>");
                ////sb.Replace('?', '"');
                ////sb.Replace("../", "http://" + clsGV.Website + "/");
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter tw = new HtmlTextWriter(sw);
                //pnl.RenderControl(tw);
                //string s = sw.ToString();
                //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                ////sb.Append(s);
                ////sb.Append("</div>");
                ////sb.Append("</form></body></html>");
                ////string a = sb.ToString();

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
                //catch (Exception)
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
                //msg.Body = "<b>Delivery Order Of #:" + Do + " " + Truck_No + "  Quintal: " + DOQntl + "</b>";
                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "DO.No:" + Do + " " + Truck_No + " Qt:" + DOQntl + " " + lblGetPassName.Text;
                //msg.IsBodyHtml = true;
                //if (smtpPort != string.Empty)
                //{
                //    SmtpServer.Port = Convert.ToInt32(smtpPort);
                //}
                //                    SmtpServer.EnableSsl = true;
                ////SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                ////System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                ////    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                ////    System.Security.Cryptography.X509Certificates.X509Chain chain,
                ////    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                ////{
                ////    return true;
                ////};
                //SmtpServer.Send(msg);
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

    private void CreateHtml()
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
    protected void btnPrePrinted_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception)
        {
            throw;
        }
    }
}