using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Drawing;

public partial class Report_rptVoucher_s : System.Web.UI.Page
{
    string qryCommon = string.Empty;
    string tblPrefix = string.Empty;
    string cityMasterTable = string.Empty;
    string prefix = string.Empty;
    string vtype = string.Empty;
    string f = "../GSReports/VoucherPrint_.htm";
    string f_Main = "../Report/rptVoucher";
    string voucher = "Voucher";
    string doc_no = string.Empty;
    string type = string.Empty;
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
            doc_no = Request.QueryString["VNO"];
            type = Request.QueryString["type"];
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
            string qry = "select * from " + qryCommon + " where Doc_No in(" + doc_no + ") and Tran_Type='" + type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsV = new DataSet();
            DataTable dt = new DataTable();
            dsV = clsDAL.SimpleQuery(qry);
            if (dsV.Tables[0].Rows.Count > 0)
            {
                dt = dsV.Tables[0];
                DataTable dt2 = new DataTable();
                dt2 = dsV.Tables[0];
                if (dt.Rows.Count > 0)
                {

                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                    dtlDetails2.DataSource = dt2;
                    dtlDetails2.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dtlDetails_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtl = (DataList)e.Item.FindControl("dtl");
            Label lblDocno = (Label)e.Item.FindControl("lblDocno");


            string vno = lblDocno.Text;
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            string vtype = lbltype.Text;

            string qry = "select * from " + qryCommon + " where Doc_No=" + vno + " and Tran_Type='" + vtype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsV = new DataSet();
            DataTable dt = new DataTable();
            dsV = clsDAL.SimpleQuery(qry);
            if (dsV.Tables[0].Rows.Count > 0)
            {
                dsV.Tables[0].Columns.Add(new DataColumn("CT", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("PartyNameC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("PartyAddressC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_cityC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Cst_noC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Gst_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Tin_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Local_Lic_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("ECC_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("CompanyPanC", typeof(string)));


                dsV.Tables[0].Columns.Add(new DataColumn("InWords", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_city", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_state", typeof(string)));
                string partyCityCode = dsV.Tables[0].Rows[0]["City_Code"].ToString();
                string partyCity = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string partyState = clsCommon.getString("select state from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                dsV.Tables[0].Rows[0]["party_city"] = partyCity;
                dsV.Tables[0].Rows[0]["party_state"] = partyState;

                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Local_Lic_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Local_Lic_No"] = "LIC No: " + dsV.Tables[0].Rows[0]["Local_Lic_No"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Tin_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Tin_No"] = "TIN: " + dsV.Tables[0].Rows[0]["Tin_No"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Cst_no"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Cst_no"] = "CST: " + dsV.Tables[0].Rows[0]["Cst_no"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Gst_no"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Gst_no"] = "GST: " + dsV.Tables[0].Rows[0]["Gst_no"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["CompanyPan"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["CompanyPan"] = "PAN: " + dsV.Tables[0].Rows[0]["CompanyPan"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["ECC_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["ECC_No"] = "ECC: " + dsV.Tables[0].Rows[0]["ECC_No"].ToString();
                }

                dsV.Tables[0].Columns.Add(new DataColumn("VoucherNo", typeof(string)));
                double vouchamt = Convert.ToDouble(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());
                dsV.Tables[0].Rows[0]["InWords"] = clsNoToWord.ctgword(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());

                string millshort = dsV.Tables[0].Rows[0]["millshortname"].ToString();
                string qntl = dsV.Tables[0].Rows[0]["Quantal"].ToString();
                string SR = dsV.Tables[0].Rows[0]["Sale_Rate"].ToString();
                string broker = dsV.Tables[0].Rows[0]["BrokerShort"].ToString();
                string narration = dsV.Tables[0].Rows[0]["Narration1"].ToString();
                string finalNarration = millshort + "-" + qntl + "-" + SR + "-" + broker;
                dsV.Tables[0].Rows[0]["Narration1"] = finalNarration;

                string ac_code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                string unit_code = dsV.Tables[0].Rows[0]["Unit_Code"].ToString();

                if (ac_code != unit_code)
                {
                    if (unit_code != "0")
                    {
                        

                        dsV.Tables[0].Rows[0]["CT"] = "Consigned To,";
                        string PartyNameC = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string PartyAddressC = clsCommon.getString("Select Address_E from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string city_code = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string party_cityC = clsCommon.getString("Select 'City:<b>'+city_name_e+'</b>&nbsp;&nbsp;&nbsp;State:<b>'+state+'</b>' from " + tblPrefix + "CityMaster where city_code=" + city_code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string Cst_noC = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string Gst_NoC = clsCommon.getString("Select Gst_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string Tin_NoC = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string Local_Lic_NoC = clsCommon.getString("Select Local_Lic_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string ECC_NoC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string CompanyPanC = clsCommon.getString("Select CompanyPan from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        dsV.Tables[0].Rows[0]["PartyNameC"] = PartyNameC;
                        dsV.Tables[0].Rows[0]["PartyAddressC"] = PartyAddressC;
                        dsV.Tables[0].Rows[0]["party_cityC"] = party_cityC;

                        if (!string.IsNullOrWhiteSpace(Local_Lic_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Local_Lic_NoC"] = "LIC No: " + Local_Lic_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Local_Lic_NoC"] = Local_Lic_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(Tin_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Tin_NoC"] = "TIN: " + Tin_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Tin_NoC"] = Tin_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(Cst_noC))
                        {
                            dsV.Tables[0].Rows[0]["Cst_noC"] = "CST: " + Cst_noC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Cst_noC"] = Cst_noC;
                        }
                        if (!string.IsNullOrWhiteSpace(Gst_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Gst_NoC"] = "GST: " + Gst_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Gst_NoC"] = Gst_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(CompanyPanC))
                        {
                            dsV.Tables[0].Rows[0]["CompanyPanC"] = "PAN: " + CompanyPanC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["CompanyPanC"] = CompanyPanC;
                        }
                        if (!string.IsNullOrWhiteSpace(ECC_NoC))
                        {
                            dsV.Tables[0].Rows[0]["ECC_NoC"] = "ECC: " + ECC_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["ECC_NoC"] = ECC_NoC;
                        }
                    }
                }
                dt = dsV.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtl_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblSignCmpName = (Label)e.Item.FindControl("lblSignCmpName");
        lblSignCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        Label lblCompany = (Label)e.Item.FindControl("lblCompany");
        lblCompany.Text = Session["Company_Name"].ToString();
        Label lblLeftAddress = (Label)e.Item.FindControl("lblLeftAddress");
        Label lblMiddlePart = (Label)e.Item.FindControl("lblMiddlePart");
        Label lblRightAddress = (Label)e.Item.FindControl("lblRightAddress");

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

            string LeftAddress = ds.Tables[0].Rows[0]["LeftAddress"].ToString();
            string MiddlePart = ds.Tables[0].Rows[0]["MiddlePart"].ToString();
            string RightAddress = ds.Tables[0].Rows[0]["RightAddress"].ToString();

            string rnl = LeftAddress.Replace("\n", "<br/>");
            var TabSpace = new String(' ', 4);
            string ab = rnl.Replace("\t", TabSpace);
            string la = ab.Replace(" ", "&nbsp;");
            lblLeftAddress.Text = la;


            string rnl1 = MiddlePart.Replace("\n", "<br/>");
            var TabSpace1 = new String(' ', 4);
            string ab1 = rnl1.Replace("\t", TabSpace1);
            string la1 = ab1.Replace(" ", "&nbsp;");
            lblMiddlePart.Text = la1;

            string rnl2 = RightAddress.Replace("\n", "<br/>");
            var TabSpace2 = new String(' ', 4);
            string ab2 = rnl2.Replace("\t", TabSpace2);
            string la2 = ab2.Replace(" ", "&nbsp;");
            lblRightAddress.Text = la2;
        }
        #endregion

        System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
        string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        imgSign.ImageUrl = imgurl;


    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string mail = txtEmail.Text;
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
                msg.Body = "Voucher";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Voucher ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
    protected void btnPriPrinted_Click(object sender, EventArgs e)
    {
        //btnPriPrinted.Attributes.Add("onclick", "return PrintPanel();");
        //foreach (DataListItem item in dtlDetails.Items)
        //{
        //    DataList dtl = (DataList)item.FindControl("dtl");
        //    foreach (DataListItem it in dtl.Items)
        //    {
        //        HtmlControl tbl = (HtmlControl)it.FindControl("tbMain");
        //        HtmlTableCell tdcmpname = (HtmlTableCell)tbl.FindControl("tdcmpname");
        //        HtmlTableCell tdcmpMob = (HtmlTableCell)tbl.FindControl("tdcmpMob");
        //        HtmlTableCell tdcmpaddr = (HtmlTableCell)tbl.FindControl("tdcmpaddr");

        //        tdcmpname.Attributes.Add("class", "noprint2");
        //        tdcmpaddr.Attributes.Add("class", "noprint2");
        //        tdcmpMob.Attributes.Add("class", "noprint2");
        //        tdcmpMob.Attributes.Add("Style", "height:100px");
        //    }
        //}
        //ClientScript.RegisterStartupScript(this.GetType(), "a", "return PrintPanel()", true);
        //Page.ClientScript.RegisterClientScriptBlock(GetType(), "kl", "return PrintPanel();", true);

        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "jh", "javascript:return PrintPanel();", true);
    }
}