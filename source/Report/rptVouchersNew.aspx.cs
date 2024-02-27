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
using System.Net.Mime;

public partial class Report_rptVouchersNew : System.Web.UI.Page
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

                string DO_No = dsV.Tables[0].Rows[0]["DO_No"].ToString();
                string CarporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no=" + DO_No + " and tran_type='DO'");
                string Broker_CODE = dsV.Tables[0].Rows[0]["Broker_CODE"].ToString();
                string BrokerMail = string.Empty;
                if (Broker_CODE != "2" || !string.IsNullOrWhiteSpace(Broker_CODE))
                {
                    BrokerMail = "," + clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Broker_CODE + "");
                }


                if (CarporateNo != "0")
                {
                    string Ac_Code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                    string Unit_Code = dsV.Tables[0].Rows[0]["Unit_Code"].ToString();
                    string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                    string unitMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Unit_Code);
                    if (!string.IsNullOrWhiteSpace(unitMail))
                    {
                        unitMail = "," + unitMail;
                    }
                    txtEmail.Text = acMail + unitMail + BrokerMail;
                }
                else
                {
                    string Ac_Code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                    string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                    string ccMail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                    if (!string.IsNullOrWhiteSpace(acMail))
                    {
                        acMail = "," + acMail;
                    }
                    txtEmail.Text = ccMail + acMail + BrokerMail;
                }

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
                dsV.Tables[0].Columns.Add(new DataColumn("BrokerShortNew", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("driver_no", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("PO_Details", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("ASN_GRN", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("InWords", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_city", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_state", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("SR_PR", typeof(string)));
                string partyCityCode = dsV.Tables[0].Rows[0]["City_Code"].ToString();
                string partyCity = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string partyState = clsCommon.getString("select state from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                dsV.Tables[0].Rows[0]["party_city"] = partyCity;
                dsV.Tables[0].Rows[0]["party_state"] = partyState;

                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["ASN_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["ASN_No"] = "ASN / GRN No: " + dsV.Tables[0].Rows[0]["ASN_No"].ToString();
                }
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

                string Mill_Rate = dsV.Tables[0].Rows[0]["Mill_Rate"].ToString();
                string Sale_Rate = dsV.Tables[0].Rows[0]["Sale_Rate"].ToString();
                string Purchase_Rate = dsV.Tables[0].Rows[0]["Purchase_Rate"].ToString();

                double quintal = Convert.ToDouble(dsV.Tables[0].Rows[0]["Quantal"].ToString());
                double diff_rate = 0.0;
                double millrate = Mill_Rate != string.Empty ? Convert.ToDouble(Mill_Rate) : 0.0;
                double salerate = 0.0;
                double purchaserate = 0.0;
                if (!string.IsNullOrEmpty(Sale_Rate.Trim()) && Sale_Rate != "0.00")
                {
                    dsV.Tables[0].Rows[0]["SR_PR"] = "Sale Rate: " + Sale_Rate;
                    salerate = Convert.ToDouble(Sale_Rate);
                    diff_rate = (millrate - salerate) * quintal;
                }
                else
                {
                    dsV.Tables[0].Rows[0]["SR_PR"] = "Purchase Rate: " + Purchase_Rate;
                    purchaserate = Convert.ToDouble(Purchase_Rate);
                    diff_rate = (millrate - purchaserate) * quintal;
                }

                string Delivery_Type = dsV.Tables[0].Rows[0]["Delivery_Type"].ToString();


                if (vtype == "LV")
                {
                    dsV.Tables[0].Rows[0]["RATEDIFF"] = dsV.Tables[0].Rows[0]["Resale_Commisson"].ToString().Trim() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Resale_Commisson"].ToString()) : 0.0;
                    dsV.Tables[0].Rows[0]["Diff_Rate"] = Math.Abs(diff_rate); //dsV.Tables[0].Rows[0]["Diff_Amount"].ToString().Trim() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Diff_Amount"].ToString()) : 0.0;
                }
                else
                {
                    dsV.Tables[0].Rows[0]["Diff_Rate"] = Math.Abs(diff_rate);
                }

                if (Delivery_Type == "N")
                {
                    double LESSDIFF = dsV.Tables[0].Rows[0]["Diff_Rate"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Diff_Rate"].ToString()) : 0.00;
                    double BANK_COMMISSION = dsV.Tables[0].Rows[0]["BANK_COMMISSION"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["BANK_COMMISSION"].ToString()) : 0.00;
                    double Brokrage = dsV.Tables[0].Rows[0]["Brokrage"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Brokrage"].ToString()) : 0.00;
                    double RATEDIFF = dsV.Tables[0].Rows[0]["RATEDIFF"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["RATEDIFF"].ToString()) : 0.00;
                    double Commission_Amount = dsV.Tables[0].Rows[0]["Commission_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Commission_Amount"].ToString()) : 0.00;
                    double FREIGHT = dsV.Tables[0].Rows[0]["FREIGHT"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["FREIGHT"].ToString()) : 0.00;
                    double Postage = dsV.Tables[0].Rows[0]["Postage"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Postage"].ToString()) : 0.00;
                    double Interest = dsV.Tables[0].Rows[0]["Interest"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Interest"].ToString()) : 0.00;
                    double Cash_Ac_Amount = dsV.Tables[0].Rows[0]["Cash_Ac_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Cash_Ac_Amount"].ToString()) : 0.00;
                    double OTHER_Expenses = dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString()) : 0.00;
                    double Transport_Amount = dsV.Tables[0].Rows[0]["Transport_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Transport_Amount"].ToString()) : 0.00;

                    double TotalAmount = LESSDIFF + BANK_COMMISSION + Brokrage + RATEDIFF + Commission_Amount + FREIGHT + Postage + Interest + Cash_Ac_Amount + OTHER_Expenses + Transport_Amount;

                    dsV.Tables[0].Rows[0]["FREIGHT"] = TotalAmount;
                    dsV.Tables[0].Rows[0]["Diff_Rate"] = 0.00;
                    dsV.Tables[0].Rows[0]["BANK_COMMISSION"] = 0.00;
                    dsV.Tables[0].Rows[0]["Brokrage"] = 0.00;
                    dsV.Tables[0].Rows[0]["RATEDIFF"] = 0.00;
                    dsV.Tables[0].Rows[0]["Commission_Amount"] = 0.00;
                    dsV.Tables[0].Rows[0]["Postage"] = 0.00;
                    dsV.Tables[0].Rows[0]["Interest"] = 0.00;
                    dsV.Tables[0].Rows[0]["Cash_Ac_Amount"] = 0.00;
                    dsV.Tables[0].Rows[0]["OTHER_Expenses"] = 0.00;

                }
                else
                {
                    double OTHER_Expenses = dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString()) : 0.00;
                    double Transport_Amount = dsV.Tables[0].Rows[0]["Transport_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Transport_Amount"].ToString()) : 0.00;

                    double otherAmount = OTHER_Expenses + Transport_Amount;
                    if (otherAmount != 0)
                    {
                        dsV.Tables[0].Rows[0]["OTHER_Expenses"] = otherAmount;
                    }
                    else
                    {
                        dsV.Tables[0].Rows[0]["OTHER_Expenses"] = 0.00;
                    }
                }


                dsV.Tables[0].Columns.Add(new DataColumn("VoucherNo", typeof(string)));
                double vouchamt = Convert.ToDouble(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());
                dsV.Tables[0].Rows[0]["Voucher_Amount"] = Math.Abs(vouchamt);
                dsV.Tables[0].Rows[0]["InWords"] = clsNoToWord.ctgword(Math.Abs(vouchamt).ToString());

                string millshort = dsV.Tables[0].Rows[0]["millshortname"].ToString();
                string qntl = dsV.Tables[0].Rows[0]["Quantal"].ToString();
                string SR = dsV.Tables[0].Rows[0]["Sale_Rate"].ToString();
                string broker = dsV.Tables[0].Rows[0]["BrokerShort"].ToString();
                if (broker != "Self" && broker != string.Empty)
                {
                    dsV.Tables[0].Rows[0]["BrokerShortNew"] = "Broker: " + broker;
                }

                string narration = dsV.Tables[0].Rows[0]["Narration1"].ToString();
                string finalNarration = "";
                if (broker != "Self")
                {
                    finalNarration = millshort + "-" + qntl + "-" + SR + "-" + broker;
                }
                else
                {
                    finalNarration = millshort + "-" + qntl + "-" + SR;
                }

                dsV.Tables[0].Rows[0]["Narration1"] = finalNarration;

                string ac_code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                string unit_code = dsV.Tables[0].Rows[0]["Unit_Code"].ToString();

                string Do_No = dsV.Tables[0].Rows[0]["DO_No"].ToString();

                string CarporateSaleNo = clsCommon.getString("Select ISNULL(Carporate_Sale_No,0) from " + tblPrefix + "deliveryorder where doc_no=" + Do_No + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                string PODetails = "";
                if (CarporateSaleNo != string.Empty && CarporateSaleNo != "0")
                {
                    PODetails = clsCommon.getString("Select PODETAIL from " + tblPrefix + "CarporateSale where Doc_No=" + CarporateSaleNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                }

                if (!string.IsNullOrWhiteSpace(PODetails))
                {
                    dsV.Tables[0].Rows[0]["PO_Details"] = "<b>PO Details:</b>" + PODetails;
                }

                //string ASN_GRN = dsV.Tables[0].Rows[0]["ASN_No"].ToString();
                //if (!string.IsNullOrEmpty(ASN_GRN))
                //{
                //    dsV.Tables[0].Rows[0]["ASN_No"] = "ASN/GRN No: " + ASN_GRN;
                //}

                string Driver_no = clsCommon.getString("Select driver_no from " + tblPrefix + "deliveryorder where doc_no=" + Do_No + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                if (!string.IsNullOrWhiteSpace(Driver_no))
                {
                    dsV.Tables[0].Rows[0]["driver_no"] = "Driver Mobile:" + Driver_no;
                }

                ViewState["Qntl"] = dsV.Tables[0].Rows[0]["Quantal"].ToString();
                ViewState["lorry"] = dsV.Tables[0].Rows[0]["Lorry_No"].ToString();
                ViewState["PartyName"] = dsV.Tables[0].Rows[0]["PartyName"].ToString();
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

                        ViewState["PartyName"] = PartyNameC;
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

        System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
        string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        string url = Server.MapPath(imgurl);
        imgSign.ImageUrl = imgurl;


    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //this.pnlMain.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();
            string mail = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //StringBuilder sb = new StringBuilder();
                    ////sb.Append("<!DOCTYPE html PUBLIC ?-//W3C//DTD XHTML 1.0 Transitional//EN? ?http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd?>");
                    ////sb.Append("<html xmlns=?http://www.w3.org/1999/xhtml?><head runat=?server?>");
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
                    pnlMain.RenderControl(tw);
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
                    contentType.Name = "Voucher.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(mail);
                    msg.Body = "Voucher";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "V.No:" + Convert.ToString(Request.QueryString["VNO"]) + " " + ViewState["lorry"].ToString() + " Qt:" + ViewState["Qntl"].ToString() + " " + ViewState["PartyName"].ToString();
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
                //pnlMain.RenderControl(tw);

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
                //msg.Body = "Voucher";
                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "V.No:" + Convert.ToString(Request.QueryString["VNO"]) + " " + ViewState["lorry"].ToString() + " Qt:" + ViewState["Qntl"].ToString() + " " + ViewState["PartyName"].ToString();
                //msg.IsBodyHtml = true;
                //if (smtpPort != string.Empty)
                //{
                //    SmtpServer.Port = Convert.ToInt32(smtpPort);
                //}
                //                     SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
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
    protected void btnPriPrinted_Click(object sender, EventArgs e)
    {
    }
}