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
using System.Net.Mime;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;

public partial class Report_rptSellBillForGST : System.Web.UI.Page
{

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string email = string.Empty;
    string billno = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        billno = Request.QueryString["billno"];
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        cityMasterTable = tblPrefix + "CityMaster";
        qryCommon = tblPrefix + "qrySugarSaleList";
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            if (billno != string.Empty)
            {
                qry = "select s.newsbno,s.doc_no as #,CONVERT(VARCHAR(10),s.doc_date,103) as dt,ISNULL(DO_No,0) as DO,s.FROM_STATION as From_Place,a.Pincode as Party_Pin,a.GSTStateCode as PartyGSTStateCode,s.TO_STATION as To_Place,s.LORRYNO as lorry,s.wearhouse,s.subTotal as Sub_Total,ISNULL(s.CGSTRate,0.00) as CGSTRate,ISNULL(s.CGSTAmount,0.00) as CGSTAmount,ISNULL(s.SGSTRate,0.00) as SGSTRate,ISNULL(s.SGSTAmount,0.00) as SGSTAmount,ISNULL(s.IGSTRate,0.00) as IGSTRate,ISNULL(s.IGSTAmount,0.00) as IGSTAmount,ISNULL(s.freight,0.00) as Less_Frieght,s.ASN_No,s.Ac_Code as PartyCode,s.Unit_Code as UnitCode," +
                    " ISNULL(s.cash_advance,0.00) as Cash_Advance,s.TaxableAmount,s.RateDiff,ISNULL(s.bank_commission,0.00) as Bank_Commission,ISNULL(s.OTHER_AMT,0.00) as Other_Expenses,s.Bill_Amount as Bill_Amount,ISNULL(s.TCS_Rate,0.000) AS TCS_Rate,ISNULL(s.TCS_Amt,0.00) as TCS_Amt,s.TCS_Net_Payable as TCS_Net_Payable,s.einvoiceno,s.ackno,s.newsbno,a.Ac_Name_E as Party_Name,a.Address_E as Party_Address," +
                    " a.Ac_Name_E as PartyName,a.Address_E as PartyAddress,a.Local_Lic_No as Party_SLN,a.Tin_No as Party_TIN,a.ECC_No as Party_Ecc,a.Cst_no as Party_Cst,a.Gst_No as Party_Gst,a.CompanyPan as Party_PAN,a.FSSAI as Party_FSSAI,c.city_name_e as Party_City,c.state as Party_State,a.GSTStateCode as PartyGSTStateCode,b.Short_Name as millshort,b.Ac_Name_E as Mill_Name,b.FSSAI as millfssai,a.Email_Id,a.Email_Id_cc,('Off.Phone: <b>'+a.OffPhone+'</b> &nbsp;&nbsp;Mobile: <b>'+a.Mobile_No+'</b>') as Party_Phone ,s.Eway_Bill_No from " + tblPrefix + "SugarSale s" +
                    " left outer join " + tblPrefix + "AccountMaster a on s.Ac_Code=a.Ac_Code and s.Company_Code=a.Company_Code left outer join " + tblPrefix + "AccountMaster b on s.mill_code=b.Ac_Code and s.Company_Code=b.Company_Code" +
                    " left outer join " + tblPrefix + "CityMaster c on a.City_Code=c.city_code and a.Company_Code=c.company_code where s.doc_no IN(" + billno + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add(new DataColumn("PODetails", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("DriverMobile", typeof(string)));

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["ASN_No"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["ASN_No"] = "ASN/GRN No: " + ds.Tables[0].Rows[i]["ASN_No"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_TIN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_TIN"] = "&nbsp;&nbsp;TIN: " + ds.Tables[0].Rows[i]["Party_TIN"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Ecc"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Ecc"] = "&nbsp;&nbsp;ECC: " + ds.Tables[0].Rows[i]["Party_Ecc"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Cst"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Cst"] = "&nbsp;&nbsp;CST: " + ds.Tables[0].Rows[i]["Party_Cst"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Gst"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Gst"] = "&nbsp;&nbsp;GST: " + ds.Tables[0].Rows[i]["Party_Gst"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_SLN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_SLN"] = "&nbsp;&nbsp;Sugar Lic No: " + ds.Tables[0].Rows[i]["Party_SLN"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_PAN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_PAN"] = "&nbsp;&nbsp;PAN: " + ds.Tables[0].Rows[i]["Party_PAN"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_FSSAI"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_FSSAI"] = "&nbsp;&nbsp;FSSAI_No: " + ds.Tables[0].Rows[i]["Party_FSSAI"].ToString();
                        }

                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["lorry"].ToString()))
                        {
                            ViewState["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["millshort"].ToString()))
                        {
                            ViewState["millshort"] = ds.Tables[0].Rows[i]["millshort"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Name"].ToString()))
                        {
                            ViewState["Party_Name"] = ds.Tables[0].Rows[i]["Party_Name"].ToString();
                        }
                    }

                    string DO_no = clsCommon.getString("Select DO_No from " + tblPrefix + "SugarSale where doc_no=" + billno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                    string carporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where doc_no=" + DO_no + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                    string delivery_type = clsCommon.getString("Select Delivery_Type from " + tblPrefix + "deliveryorder where doc_no=" + DO_no + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                    string DriverMobile = clsCommon.getString("Select driver_no from " + tblPrefix + "deliveryorder where doc_no=" + DO_no + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");

                    if (!string.IsNullOrEmpty(DriverMobile.Trim().ToString()))
                    {
                        ds.Tables[0].Rows[0]["DriverMobile"] = "Driver Mobile: " + DriverMobile;
                    }
                    if (!string.IsNullOrEmpty(carporateNo))
                    {
                        string SellingType = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + carporateNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        string PODetails = clsCommon.getString("Select PODETAIL from " + tblPrefix + "CarporateSale where Doc_No=" + carporateNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        if (PODetails != string.Empty)
                        {
                            ds.Tables[0].Rows[0]["PODetails"] = "PO Details: <b>" + PODetails + "</b>";
                        }
                        if (SellingType == "P")
                        {
                            ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                            ds.Tables[0].Rows[0]["Cash_Advance"] = 0.00;
                        }
                        else
                        {
                            if (delivery_type == "C")
                            {
                                ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                            }
                        }
                    }
                    else
                    {
                        if (delivery_type == "C")
                        {
                            ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                        }
                    }

                    double TaxableAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["TaxableAmount"].ToString());
                    double Less_Frieght = Convert.ToDouble(ds.Tables[0].Rows[0]["Less_Frieght"].ToString());
                    double subtotal = Math.Round((TaxableAmount - Less_Frieght));
                    ds.Tables[0].Rows[0]["Sub_Total"] = subtotal.ToString();


                    ds.Tables[0].Columns.Add(new DataColumn("CompanyState", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("CompanyGSTStateCode", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("DateOfSupply", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("CompanyGST", typeof(string)));
                    string companyGST = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    ds.Tables[0].Rows[0]["CompanyGST"] = companyGST;
                    ds.Tables[0].Rows[0]["CompanyState"] = Session["CompanyState"].ToString();
                    ds.Tables[0].Rows[0]["CompanyGSTStateCode"] = Session["CompanyGSTStateCode"].ToString();
                    ds.Tables[0].Rows[0]["DateOfSupply"] = DateTime.Now.ToString("dd/MM/yyyy");


                    ds.Tables[0].Columns.Add(new DataColumn("CT", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitName", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitAddress", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitCity", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitGSTStateCode", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitGST", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitPan", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitFSSAI", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("UnitLicNo", typeof(string)));
                    string partyCode = ds.Tables[0].Rows[0]["PartyCode"].ToString();
                    string unitCode = ds.Tables[0].Rows[0]["UnitCode"].ToString();
                    //if (partyCode != unitCode)
                    //{
                    using (clsDataProvider objDataProvider = new clsDataProvider())
                    {
                        DataSet dsUnitDetails = new DataSet();
                        string qryUnitDetails = "select Ac_Name_E as UnitName,Address_E as UnitAddress,CityName as UnitCity,GSTStateCode as UnitGSTStateCode,GSTStateName as UnitState " +
                            " ,ISNULL('GST:'+NULLIF(Gst_No,''),'') as UnitGST,ISNULL('PAN:'+NULLIF(CompanyPan,''),'') as  UnitPan,ISNULL('FSSAI_No:'+NULLIF(FSSAI,''),'') as  UnitFSSAI,ISNULL('LIC:'+NULLIF(Local_Lic_No,''),'') as UnitLicNo" +
                            " from " + tblPrefix + "qryAccountsList where Ac_Code=" + unitCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                        dsUnitDetails = objDataProvider.GetDataSet(qryUnitDetails);
                        if (dsUnitDetails.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].Rows[0]["CT"] = "Consigned To,";
                            ds.Tables[0].Rows[0]["UnitName"] = dsUnitDetails.Tables[0].Rows[0]["UnitName"].ToString();
                            ds.Tables[0].Rows[0]["UnitAddress"] = dsUnitDetails.Tables[0].Rows[0]["UnitAddress"].ToString();
                            ds.Tables[0].Rows[0]["UnitCity"] = "City:<b>" + dsUnitDetails.Tables[0].Rows[0]["UnitCity"].ToString() + "</b>&nbsp;&nbsp;&nbsp;State:<b>" + dsUnitDetails.Tables[0].Rows[0]["UnitState"].ToString() + "</b>";
                            ds.Tables[0].Rows[0]["UnitGSTStateCode"] = "State Code:&nbsp;" + dsUnitDetails.Tables[0].Rows[0]["UnitGSTStateCode"].ToString();
                            ds.Tables[0].Rows[0]["UnitGST"] = dsUnitDetails.Tables[0].Rows[0]["UnitGST"].ToString();
                            ds.Tables[0].Rows[0]["UnitPan"] = dsUnitDetails.Tables[0].Rows[0]["UnitPan"].ToString();
                            ds.Tables[0].Rows[0]["UnitFSSAI"] = dsUnitDetails.Tables[0].Rows[0]["UnitFSSAI"].ToString();
                            ds.Tables[0].Rows[0]["UnitLicNo"] = dsUnitDetails.Tables[0].Rows[0]["UnitLicNo"].ToString();
                        }
                    }
                    //}

                    dt = new DataTable();
                    dt = ds.Tables[0];
                    DataTable dt1 = new DataTable();
                    dt1 = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtEmail.Text = dt.Rows[0]["Email_Id_cc"].ToString() + "," + dt.Rows[0]["Email_Id"].ToString();
                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                        dtlist1.DataSource = dt1;
                        dtlist1.DataBind();
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            Label lblSB_No = (Label)e.Item.FindControl("lblSB_No");
            Label lblBillAmount = (Label)e.Item.FindControl("lblBillAmount");
            Label lblCmptinNo = (Label)e.Item.FindControl("lblCmptinNo");
            Label lblCmpMobile = (Label)e.Item.FindControl("lblCmpMobile");
            lblCmptinNo.Text = clsCommon.getString("select Pan_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            Label lblCompnayFSSAI_No = (Label)e.Item.FindControl("lblCompnayFSSAI_No");
            lblCompnayFSSAI_No.Text = clsCommon.getString("select FSSAI_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            Label lblCompany = (Label)e.Item.FindControl("lblCompany");
            lblCompany.Text = "M/S. " + Session["Company_Name"].ToString();
            Label lblAl1 = (Label)e.Item.FindControl("lblAl1");
            Label lblAl2 = (Label)e.Item.FindControl("lblAl2");
            Label lblAl3 = (Label)e.Item.FindControl("lblAl3");
            Label lblAl4 = (Label)e.Item.FindControl("lblAl4");
            Label lblOtherDetails = (Label)e.Item.FindControl("lblOtherDetails");

            Label lblTCSNet_Payable = (Label)e.Item.FindControl("lblTCSNet_Payable");

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

            Label lblInwords = (Label)e.Item.FindControl("lblInwords");
            Label lblNameCmp = (Label)e.Item.FindControl("lblNameCmp");
            DataList dtItemDetails = (DataList)e.Item.FindControl("dtItemDetails");
            string sbNo = lblSB_No.Text;
            //lblInwords.Text = clsNoToWord.ctgword(lblBillAmount.Text);
            lblInwords.Text = clsNoToWord.ctgword(lblTCSNet_Payable.Text);

            Label lblSubTotal = (Label)e.Item.FindControl("lblSubTotal");
            string city = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string Pin = clsCommon.getString("Select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("Select State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            //lblCityStatePin.Text = city + " (" + Pin + ") " + state;
            //lblCmpMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //lblCompanyAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            qry = " select s.System_Name_E+' '+ISNULL(d.narration,'') as Item,s.HSN,d.bags as Bags,d.packing as Packing,d.Quantal as Qntl,d.rate as Rate,d.item_Amount as Value from " + tblPrefix + "sugarsaleDetails d" +
                " left outer join " + tblPrefix + "SystemMaster s on d.item_code=s.System_Code and d.Company_Code=s.Company_Code and s.System_Type='I' where d.doc_no=" + sbNo + " and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by d.detail_id";

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    double subtotal = Convert.ToDouble(lblSubTotal.Text.ToString());
                    dt.Rows[0]["Value"] = subtotal;
                    double quintal = Convert.ToDouble(dt.Rows[0]["Qntl"].ToString());
                    double rate = Math.Round((subtotal / quintal), 2);
                    dt.Rows[0]["Rate"] = rate.ToString();
                    lblNameCmp.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    dtItemDetails.DataSource = dt;
                    dtItemDetails.DataBind();
                }
            }


            System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            imgSign.ImageUrl = "~/Images/mysign.JPG";

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            email = txtEmail.Text.ToString();
            Label lblSB_No = (Label)dtlist.Items[0].FindControl("lblSB_No");

            //StringWriter sw = new StringWriter();
            //HtmlTextWriter tw = new HtmlTextWriter(sw);
            //pnlMain.RenderControl(tw);
            //string s = sw.ToString();
            //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");

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
                contentType.Name = "Invoice.htm";
                Attachment attachment = new Attachment(ms, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(email);
                msg.Body = "Sale Bill";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                string lorry = string.Empty;
                string millshort = string.Empty;
                string party = string.Empty;
                if (ViewState["lorry"] != null)
                {
                    lorry = "Lorry:" + ViewState["lorry"].ToString();
                }
                if (ViewState["millshort"] != null)
                {
                    millshort = "Mill:" + ViewState["millshort"].ToString();
                }
                if (ViewState["Party_Name"] != null)
                {
                    party = "Getpass:" + ViewState["Party_Name"].ToString();
                }
                msg.Subject = "Bill No:" + lblSB_No.Text + " " + lorry + " " + millshort + " " + party;
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

    protected void btnPriPrinted_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }
    protected void btnPDFMail_Click(object sender, EventArgs e)
    {
        #region[Pdf comment]
        //try
        //{
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    tblmn.RenderControl(hw);
        //    string s1 = sw.ToString().Replace("font-size: medium", "font-size: xx-small");
        //    //StringReader sr = new StringReader(s1.ToString());
        //    StringReader sr = new StringReader(s1.ToString().Replace("../Images", Server.MapPath("~/Images")));

        //    Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
        //    var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        //    pdfDoc.Open();
        //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //    pdfDoc.Close();
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}
        //catch
        //{
        //}
        #endregion

        string filepath = "";
        Attachment attachment = null;
        try
        {
            email = txtEmail.Text.ToString();
            Label lblSB_No = (Label)dtlist.Items[0].FindControl("lblSB_No");
            string fileName = "SellBill_" + lblSB_No.Text + ".pdf";
            filepath = "~/PAN/" + fileName;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw1);
            tblmn.RenderControl(hw);
            string s1 = sw1.ToString().Replace("font-size: medium", "font-size: xx-small");
            StringReader sr = new StringReader(s1.ToString().Replace("../Images", Server.MapPath("~/Images")));
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4, 30f, 30f, 30f, 250f);
            var writer = PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath(filepath), FileMode.Create));
            Font tblfont = new Font();
            pdfDoc.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            pdfDoc.Close();
            ContentType contentType = new ContentType();
            contentType.MediaType = MediaTypeNames.Application.Pdf;
            contentType.Name = fileName;
            attachment = new Attachment(Server.MapPath(filepath), contentType);

            string mailFrom = Session["EmailId"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["EmailPassword"].ToString();
            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Host = clsGV.Email_Address;
            msg.From = new MailAddress(mailFrom);
            msg.To.Add(email);
            msg.Body = "Sale Bill";
            msg.Attachments.Add(attachment);
            msg.IsBodyHtml = true;
            string lorry = string.Empty;
            string millshort = string.Empty;
            string party = string.Empty;
            if (ViewState["lorry"] != null)
            {
                lorry = "Lorry:" + ViewState["lorry"].ToString();
            }
            if (ViewState["millshort"] != null)
            {
                millshort = "Mill:" + ViewState["millshort"].ToString();
            }
            if (ViewState["Party_Name"] != null)
            {
                party = "Getpass:" + ViewState["Party_Name"].ToString();
            }
            msg.Subject = "Bill No:" + lblSB_No.Text + " " + lorry + " " + millshort + " " + party;
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
            attachment.Dispose();
            if (File.Exists(Server.MapPath(filepath)))
            {
                File.Delete(Server.MapPath(filepath));
            }
        }
        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        finally
        {
            attachment.Dispose();
            if (File.Exists(Server.MapPath(filepath)))
            {
                File.Delete(Server.MapPath(filepath));
            }
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");


    }
}