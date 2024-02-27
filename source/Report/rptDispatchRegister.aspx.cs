using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading;

public partial class Report_rptDispatchRegister : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string trnType = "DO";
    string GLedgerTable = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string partyEmail = string.Empty;
    string f = "../GSReports/DispatchRegister_.htm";
    string f_Main = "../Report/rptDispatchRegister";
    string v = "../GSReports/Voucher";
    string v_main = "../Report/rptVoucher";
    string Branch_Code = string.Empty;
    string qry = "";
    string cityMasterTable = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();//"NT_1_";
        qryCommon = tblPrefix + "qryDeliveryOrderListReport";
        fromDT = Request.QueryString["fromDT"];
        toDT = Request.QueryString["toDT"];
        partyEmail = Request.QueryString["email"];
        Branch_Code = Request.QueryString["Branch_Code"];
        cityMasterTable = tblPrefix + "CityMaster";
        if (!IsPostBack)
        {
            this.BindReport();
        }
    }

    private void BindReport()
    {
        try
        {
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select *,Convert(varchar(10),doc_date,103) as doc_date from " + qryCommon + " where doc_date<'" + toDT + "' and doc_date>'" + fromDT + "' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
            }
            else
            {
                qry = "select *,Convert(varchar(10),doc_date,103) as doc_date from " + qryCommon + " where doc_date<'" + toDT + "' and doc_date>'" + fromDT + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code='" + Branch_Code + "' and tran_type='" + trnType + "'";
            }
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            DataList1.DataSource = ds;
            DataList1.DataBind();
        }
        catch
        {

        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            //Label lblAddr = (Label)e.Item.FindControl("lblCompanyAddr");
            //lblAddr.Text = clsGV.CompanyAddress;

            //Label lblName = (Label)e.Item.FindControl("lblCompanyName");
            //lblName.Text = Session["Company_Name"].ToString();

            //Label lblPhone = (Label)e.Item.FindControl("lblCompanyMobile");
            //lblPhone.Text = clsGV.CompanyPhone;


            //Label lblPartyEmail = (Label)e.Item.FindControl("lblPartyEmail");
            //lblPartyEmail.Text = partyEmail;
            //Label lblCompanyBottom = (Label)e.Item.FindControl("lblCompanyBottom");
            //lblCompanyBottom.Text = "For  " + Session["Company_Name"].ToString();

            Label lblDocNo = (Label)e.Item.FindControl("lblDocNo");
            string doc_no = lblDocNo.Text;


            Label lblFreight = (Label)e.Item.FindControl("lblFreight");
            Label lblRefNo = (Label)e.Item.FindControl("lblRefNo");
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select  Freight_AmountMM,doc_no from " + qryCommon + " where purc_no=" + doc_no + " and tran_type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            else
            {
                qry = "select  Freight_AmountMM,doc_no from " + qryCommon + " where purc_no=" + doc_no + " and tran_type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //lblFreight.Text=dt.Rows[0]["Freight_AmountMM"].ToString();
                        lblRefNo.Text = dt.Rows[0]["doc_no"].ToString();
                    }
                }
            }

            Label lblDispatchTo = (Label)e.Item.FindControl("lblDispatchTo");
            if (lblDispatchTo.Text != string.Empty && lblDispatchTo.Text != "Self")
            {
                lblDispatchTo.Text = "Dispatch To:" + lblDispatchTo.Text;
            }

            Label lblPaymentTo = (Label)e.Item.FindControl("lblPaymentTo");
            if (lblPaymentTo.Text != string.Empty && lblPaymentTo.Text != "Self")
            {
                lblPaymentTo.Text = "Payment To:" + lblPaymentTo.Text;
            }

            Label lblBillTo = (Label)e.Item.FindControl("lblBillTo");
            if (lblBillTo.Text != string.Empty)
            {
                lblBillTo.Text = "Bill To:" + lblBillTo.Text;
            }
        }
        catch
        {

        }
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<div  style='font-size:12px;'>");
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
    protected void btnPrintOrMail_Click(object sender, EventArgs e)
    {
        try
        {
            string dono = "";
            var path = ""; int j = 0;
            foreach (DataListItem li in DataList1.Items)
            {
                v = "../GSReports/Voucher";
                CheckBox chkPrint = li.FindControl("chkPrint") as CheckBox;
                CheckBox chkMail = li.FindControl("chkMail") as CheckBox;
                Label lblDocNo = (Label)li.FindControl("lblDocNo");
                string ins = "a";
                if (chkPrint.Checked == true)
                {
                    dono = lblDocNo.Text;
                    ins += j;
                    string getpassmail = clsCommon.getString("select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=(select GETPASSCODE from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")");
                    string vtype = clsCommon.getString("Select voucher_type from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    string voucherno = clsCommon.getString("Select voucher_no from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "popup" + j, "window.open('../Report/rptVoucher.aspx?VNO='" + voucherno + "'&type='" + vtype + "');", true);
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), ins, "javascript:vp('" + voucherno + "','" + vtype + "');", true);
                    j += 1;
                }
                if (chkMail.Checked == true)
                {
                    dono = lblDocNo.Text;
                    string getpassmail = clsCommon.getString("select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=(select VOUCHER_BY from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")");
                    string vtype = clsCommon.getString("Select voucher_type from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    string voucherno = clsCommon.getString("Select voucher_no from " + tblPrefix + "deliveryorder where doc_no=" + dono + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    path = v;
                    try
                    {
                        string qry = "";
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();

                        qry = "select * from " + tblPrefix + "qryVoucherList where Doc_No=" + voucherno + " and Tran_Type='" + vtype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    dtlVoucher.DataSource = dt;
                                    dtlVoucher.DataBind();
                                    if (getpassmail != string.Empty)
                                    {
                                        StringWriter sw = new StringWriter();
                                        HtmlTextWriter tw = new HtmlTextWriter(sw);
                                        pnlVoucher.RenderControl(tw);
                                        string op = sw.ToString();
                                        try
                                        {
                                            v = path + voucherno + "(" + vtype + ")" + ".htm";

                                            using (FileStream fs = new FileStream(Server.MapPath(v), FileMode.Create))
                                            {
                                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                                {
                                                    w.WriteLine(op);
                                                    //fs.Close();
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                        try
                                        {
                                            string mailFrom = Session["EmailId"].ToString();
                                            string smtpPort = "587";
                                            string emailPassword = Session["EmailPassword"].ToString();
                                            MailMessage msg = new MailMessage();
                                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                                            SmtpServer.Host = clsGV.Email_Address;
                                            msg.From = new MailAddress(mailFrom);
                                            msg.To.Add(getpassmail);
                                            msg.Body = "Voucher Print";
                                            msg.Attachments.Add(new Attachment(Server.MapPath(v)));
                                            //File.Create(path).Close();
                                            msg.IsBodyHtml = true;
                                            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                                            msg.Subject = "Loading Voucher ON " + DateTime.Now.ToString("dd/MM/yyyy");
                                            msg.IsBodyHtml = true;
                                            if (smtpPort != string.Empty)
                                            {
                                                SmtpServer.Port = Convert.ToInt32(smtpPort);
                                            }
                                                                 SmtpServer.EnableSsl = true;
                                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                                            //SmtpServer.Credentials = CredentialCache.DefaultNetworkCredentials;
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
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        Response.Write("voucher Number not present");
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtlVoucher_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            Label lblAddr = (Label)e.Item.FindControl("lblCompanyAddr");
            lblAddr.Text = clsGV.CompanyAddress;

            Label lblPartyCity = (Label)e.Item.FindControl("lblPartyCity");
            lblPartyCity.Text = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + lblPartyCity.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


            Label lblPhone = (Label)e.Item.FindControl("lblCompanyMobile");
            lblPhone.Text = clsGV.CompanyPhone;


            //    Label lblMillEmail = (Label)e.Item.FindControl("lblMillEmail");
            //   lblMillEmail.Text = millEmail;


            if (ViewState["pageBreak"] != null)
            {
                if (ViewState["pageBreak"].ToString() == "N")
                {
                    System.Web.UI.HtmlControls.HtmlTable tb = (System.Web.UI.HtmlControls.HtmlTable)e.Item.FindControl("tbMain");
                    tb.Style["page-break-after"] = "avoid";
                }
                else
                {
                    System.Web.UI.HtmlControls.HtmlTable tb = (System.Web.UI.HtmlControls.HtmlTable)e.Item.FindControl("tbMain");
                    tb.Style["page-break-after"] = "always";
                }
            }
        }
        catch (Exception exx1)
        {
            //  Unable to cast object of type 'System.Web.UI.HtmlControls.HtmlTable' to type 'System.Web.UI.WebControls.Table'.
            Response.Write(exx1.Message);
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
                msg.Body = "Dispatch Register";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Dispatch Register " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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