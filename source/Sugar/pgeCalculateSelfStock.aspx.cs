using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;

public partial class Sugar_pgeCalculateSelfStock : System.Web.UI.Page
{
    public DataSet ds = null;
    DataSet ds1 = null;
    DataSet dsentry = null;
    public DataTable dt = null;
    string tblHeadVoucher = string.Empty;
    string GLedgerTable = string.Empty;
    public string tableName { get; set; }
    public string code { get; set; }
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string strTextbox = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;

    string millShortName = string.Empty;
    string DOShortname = string.Empty;
    string voucherbyshortname = string.Empty;
    string AUTO_VOUCHER = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
    }
    protected void btnCalculateSelfbal_Click(object sender, EventArgs e)
    {
        try
        {
            qry = "Select Tender_No,Quantal from " + tblPrefix + "Tender where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Tender_No = ds.Tables[0].Rows[i]["Tender_No"].ToString();
                    double Quintal = Convert.ToDouble(ds.Tables[0].Rows[i]["Quantal"].ToString());

                    string qryDetail = "select SUM(Buyer_Quantal) as SumQntl from " + tblPrefix + "Tenderdetails where Tender_No=" + Tender_No + " and Buyer!=2  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString());
                    string sum = clsCommon.getString(qryDetail);

                    double SumQntl = sum != string.Empty ? Convert.ToDouble(sum) : 0.00;

                    double selfBal = Quintal - SumQntl;

                    string qryUpdate = "Update " + tblPrefix + "Tenderdetails set Buyer_Quantal=" + selfBal + " where Buyer=2 and Tender_No=" + Tender_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString());
                    DataSet dsv = new DataSet();
                    dsv = clsDAL.SimpleQuery(qryUpdate);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Saved!');", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnPostingPurchaseEntries_Click(object sender, EventArgs e)
    {
        try
        {
            string strRef = "";
            string Tran_Type = "PS";
            qry = "delete from " + tblPrefix + "GLEDGER where TRAN_TYPE='PS' and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            qry = "";
            qry = "Select doc_no from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsupdate = new DataSet();
            dsupdate = clsDAL.SimpleQuery(qry);

            if (dsupdate.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsupdate.Tables[0].Rows.Count; i++)
                {
                    string PSNo = dsupdate.Tables[0].Rows[i]["doc_no"].ToString();
                    ds = new DataSet();
                    DataSet MyDs = new DataSet();
                    MyDs = clsDAL.SimpleQuery("Select * from " + tblPrefix + "qrySugarPurcList where doc_no=" + PSNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                    if (MyDs.Tables[0].Rows.Count > 0)
                    {
                        string MyDoc_No = MyDs.Tables[0].Rows[0]["doc_no"].ToString();
                        string MyDoc_Date = DateTime.Parse(MyDs.Tables[0].Rows[0]["doc_date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                        string MyDrCrHead = MyDs.Tables[0].Rows[0]["Ac_Code"].ToString();
                        int Unit_Code = MyDs.Tables[0].Rows[0]["Unit_Code"].ToString() != string.Empty ? Convert.ToInt32(MyDs.Tables[0].Rows[0]["Unit_Code"].ToString()) : 0;
                        string itemCode = "";
                        itemCode = MyDs.Tables[0].Rows[0]["item_code"].ToString();
                        string Purchase_Account = clsCommon.getString("select Purchase_AC from " + tblPrefix + "SystemMaster where System_Type='I' and System_Code=" + itemCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        double Bill_Amount = MyDs.Tables[0].Rows[0]["Bill_Amount"].ToString() != string.Empty ? Convert.ToDouble(MyDs.Tables[0].Rows[0]["Bill_Amount"].ToString()) : 0.00;
                        string Qntl = MyDs.Tables[0].Rows[0]["NETQNTL"].ToString();
                        string TransportAmount = MyDs.Tables[0].Rows[0]["cash_advance"].ToString();
                        string MillShortName = MyDs.Tables[0].Rows[0]["millshortname"].ToString();
                        //string TransportShort = MyDs.Tables[0].Rows[0]["TransportShort"].ToString();
                        string lorryNo = MyDs.Tables[0].Rows[0]["LORRYNO"].ToString();
                        string PartyShort = MyDs.Tables[0].Rows[0]["PartyShortname"].ToString();
                        string BrokerShort = MyDs.Tables[0].Rows[0]["BrokerShort"].ToString();
                        string Subtotal = MyDs.Tables[0].Rows[0]["subTotal"].ToString();
                        string DebitNarration = MillShortName + " " + Qntl + " Lorry: " + lorryNo + " " + BrokerShort;
                        string CreditNarration = PartyShort + " " + Qntl + " " + " Lorry: " + Subtotal + "/" + Qntl;
                        int MyOrderCode = 1;

                        DataSet MyGL = new DataSet();
                        MyGL = clsDAL.SimpleQuery("Select * from " + tblPrefix + "GLEDGER where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + PSNo + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString()) + "");
                        int mygledgercount = MyGL.Tables[0].Rows.Count;
                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                        {
                            //Credit Effect for Party
                            string rev = "";

                            if (mygledgercount >= MyOrderCode)
                            {
                                obj.flag = 2;
                                obj.tableName = tblPrefix + "GLEDGER";
                                obj.columnNm = "TRAN_TYPE='" + Tran_Type + "',DOC_NO='" + MyDoc_No + "',DOC_DATE='" + MyDoc_Date + "',AC_CODE='" + MyDrCrHead + "',UNIT_CODE='" + Unit_Code + "',NARRATION='" + CreditNarration + "',AMOUNT='" + Bill_Amount + "',DRCR='C',DRCR_HEAD='" + Purchase_Account + "',SORT_TYPE='" + Tran_Type + "',SORT_NO='" + PSNo + "' where COMPANY_CODE='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' AND YEAR_CODE='" + Convert.ToInt32(Session["year"].ToString()) + "' AND ORDER_CODE='" + MyOrderCode + "' AND TRAN_TYPE='" + Tran_Type + "' AND DOC_NO='" + MyDoc_No + "'";
                                obj.values = "none";
                                ds = obj.insertAccountMaster(ref strRef);
                                rev = strRef;
                                MyOrderCode = MyOrderCode + 1;
                            }
                            else
                            {
                                obj.flag = 1;
                                obj.tableName = tblPrefix + "GLEDGER";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,Branch_Code,ORDER_CODE,DRCR,DRCR_HEAD,SORT_TYPE,SORT_NO";
                                obj.values = "'" + Tran_Type + "','" + MyDoc_No + "','" + MyDoc_Date + "','" + MyDrCrHead + "','" + Unit_Code + "','" + CreditNarration + "','" + Bill_Amount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(System.Web.HttpContext.Current.Session["Branch_Code"].ToString()) + "','" + MyOrderCode + "','C','" + Purchase_Account + "','" + Tran_Type + "','" + PSNo + "'";
                                ds = obj.insertAccountMaster(ref strRef);
                                rev = strRef;
                                MyOrderCode = MyOrderCode + 1;
                            }
                            if (mygledgercount >= MyOrderCode)
                            {
                                obj.flag = 2;
                                obj.tableName = tblPrefix + "GLEDGER";
                                obj.columnNm = "TRAN_TYPE='" + Tran_Type + "',DOC_NO='" + MyDoc_No + "',DOC_DATE='" + MyDoc_Date + "',AC_CODE='" + Purchase_Account + "',NARRATION='" + DebitNarration + "',AMOUNT='" + Bill_Amount + "',DRCR='D',DRCR_HEAD='" + MyDrCrHead + "',SORT_TYPE='" + Tran_Type + "',SORT_NO='" + PSNo + "' where COMPANY_CODE='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' AND YEAR_CODE='" + Convert.ToInt32(Session["year"].ToString()) + "' AND ORDER_CODE='" + MyOrderCode + "' AND TRAN_TYPE='" + Tran_Type + "' AND DOC_NO='" + MyDoc_No + "'";
                                obj.values = "none";
                                ds = obj.insertAccountMaster(ref strRef);
                                rev = strRef;
                                MyOrderCode = MyOrderCode + 1;
                            }
                            else
                            {
                                //Debit Effect for Purchase Account
                                obj.flag = 1;
                                obj.tableName = tblPrefix + "GLEDGER";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,Branch_Code,ORDER_CODE,DRCR,DRCR_HEAD,SORT_TYPE,SORT_NO";
                                obj.values = "'" + Tran_Type + "','" + MyDoc_No + "','" + MyDoc_Date + "','" + Purchase_Account + "','" + DebitNarration + "','" + Bill_Amount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(System.Web.HttpContext.Current.Session["Branch_Code"].ToString()) + "','" + MyOrderCode + "','D','" + MyDrCrHead + "','" + Tran_Type + "','" + PSNo + "'";
                                ds = obj.insertAccountMaster(ref strRef);
                                rev = strRef;
                                MyOrderCode = MyOrderCode + 1;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnCreatePSandSB_Click(object sender, EventArgs e)
    {
        try
        {
            int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string voucher_type = "";
            int voucherno = 0;
            string strRev = "";
            string retValue = "";
            clsGledgerupdations gleder = new clsGledgerupdations();
            qry = "Select * from " + tblPrefix + "qryDeliveryOrderList where tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  order by doc_no";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string DOC_NO = ds.Tables[0].Rows[i]["doc_no"].ToString();
                    string date = ds.Tables[0].Rows[i]["doc_date"].ToString();
                    string DOC_DATE = DateTime.Parse(date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    string MILL_CODE = ds.Tables[0].Rows[i]["mill_code"].ToString();
                    string TRUCK_NO = ds.Tables[0].Rows[i]["truck_no"].ToString();
                    Int32 BROKER_CODE = Convert.ToInt32(ds.Tables[0].Rows[i]["broker"].ToString());
                    double QUANTAL = Convert.ToDouble(ds.Tables[0].Rows[i]["quantal"].ToString());
                    double mill_rate = Convert.ToDouble(ds.Tables[0].Rows[i]["mill_rate"].ToString());
                    string PACKING = ds.Tables[0].Rows[i]["packing"].ToString();
                    string BAGS = ds.Tables[0].Rows[i]["bags"].ToString();
                    string Delivery_Type = ds.Tables[0].Rows[i]["DT"].ToString();
                    double SALE_RATE = Convert.ToDouble(ds.Tables[0].Rows[i]["sale_rate"].ToString());
                    double FRIEGHT_RATE = Convert.ToDouble(ds.Tables[0].Rows[i]["FreightPerQtl"].ToString());
                    double FRIEGHT_AMOUNT = Convert.ToDouble(ds.Tables[0].Rows[i]["Freight_Amount"].ToString());
                    double MEMO_ADVANCE = Convert.ToDouble(ds.Tables[0].Rows[i]["Memo_Advance"].ToString());
                    string MM_CC = ds.Tables[0].Rows[i]["MM_CC"].ToString();
                    string Transport_Code = ds.Tables[0].Rows[i]["transport"].ToString();
                    string GRADE = ds.Tables[0].Rows[i]["grade"].ToString();
                    string Ac_Code = ds.Tables[0].Rows[i]["voucher_by"].ToString();
                    string Utr_No = ds.Tables[0].Rows[i]["UTR_NO"].ToString();
                    string NARRATION2 = ds.Tables[0].Rows[i]["narration2"].ToString();
                    string NARRATION3 = ds.Tables[0].Rows[i]["narration3"].ToString();
                    string NARRATION4 = ds.Tables[0].Rows[i]["narration4"].ToString();
                    double Diff_Rate = Convert.ToDouble(ds.Tables[0].Rows[i]["diff_rate"].ToString());

                    double VoucherBrokrage = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_Brokrage"].ToString());
                    double VoucherServiceCharge = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_Service_Charge"].ToString());
                    double VoucherRateDiffRate = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_RateDiffRate"].ToString());
                    double VoucherRateDiffAmt = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_RateDiffAmt"].ToString());
                    double VoucherBankCommRate = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_BankCommRate"].ToString());
                    double VoucherBankCommAmt = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_BankCommAmt"].ToString());
                    double VoucherInterest = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_Interest"].ToString());
                    double VoucherTransport = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_TransportAmt"].ToString());
                    double VoucherOtherExpenses = Convert.ToDouble(ds.Tables[0].Rows[i]["Voucher_OtherExpenses"].ToString());

                    double LESSDIFFOV = Diff_Rate * QUANTAL;
                    double MILL_AMOUNT = 0.00;
                    MILL_AMOUNT = QUANTAL * mill_rate;

                    double VOUCHER_AMOUNT = 0.00;
                    string Rate_Type = string.Empty;
                    if (Delivery_Type == "N")
                    {
                        Diff_Rate = ((SALE_RATE - FRIEGHT_RATE) - mill_rate) * QUANTAL;
                        VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                        Rate_Type = "A";
                    }
                    else
                    {
                        Diff_Rate = ((SALE_RATE) - mill_rate) * QUANTAL;
                        VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                        Rate_Type = "L";
                    }

                    string nar = clsCommon.getString("select 'dt:'+Convert(varchar(10),doc_date,103)+'  amt:'+CONVERT(nvarchar(255),amount) from " + tblPrefix + "UTR where doc_no=" + Utr_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    string myNarration = Utr_No + " " + nar;
                    string desp_type = ds.Tables[0].Rows[i]["desp_type"].ToString();
                    string SaleBillTo = ds.Tables[0].Rows[i]["SaleBillTo"].ToString();
                    string Carporate_Sale_No = ds.Tables[0].Rows[i]["Carporate_Sale_No"].ToString();
                    string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    string PDS = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    string GETPASSCODE = ds.Tables[0].Rows[i]["GETPASSCODE"].ToString();


                    Int32 OVTransportCode = Transport_Code != string.Empty ? Convert.ToInt32(Transport_Code) : 0;
                    if (MM_CC == "Cash")
                    {
                        OVTransportCode = 1;
                    }

                    string city_code = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + MILL_CODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string From_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string city_code2 = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + GETPASSCODE + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string To_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code2 + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    double SUBTOTAL = 0.00;
                    SUBTOTAL = QUANTAL * mill_rate;

                    string vouchnarration = millShortName + " (" + "S.R." + SALE_RATE + "-" + FRIEGHT_RATE + "- M.R." + mill_rate + ")*" + QUANTAL;

                    Int32 SaleBillTransport = 0;
                    if (MM_CC == "Credit")
                    {
                        SaleBillTransport = Transport_Code != string.Empty ? Convert.ToInt32(Transport_Code) : 0;
                    }
                    else
                    {
                        SaleBillTransport = 1;
                    }

                    if (desp_type == "DI")
                    {
                        if (GETPASSCODE == selfac)
                        {
                            voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(doc_no),0)+1 from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                            voucher_type = "PS";
                        }
                        else
                        {
                            voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher Where Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                            voucher_type = "OV";
                        }
                    }
                    if (desp_type == "DI")
                    {
                        Int32 Payment_To = Convert.ToInt32(ds.Tables[0].Rows[i]["Bank_Code"].ToString());
                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                        {
                            if (GETPASSCODE == selfac || PDS == "P")
                            {
                                Int32 voucher_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(doc_no),0)+1 from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                string purchaseNo = Convert.ToString(voucher_no);
                                if (purchaseNo != string.Empty)
                                {
                                    string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarPurchase where doc_no=" + purchaseNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                    if (str != string.Empty)
                                    {
                                        getvoucherscode(tblPrefix + "SugarPurchase", "doc_no", "NULL", "Tran_Type");
                                        purchaseNo = ViewState["maxval"].ToString();
                                    }
                                }
                                string pdsParty = clsCommon.getString("Select Ac_Code from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                                string pdsunit = clsCommon.getString("Select Unit_Code from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");

                                obj.flag = 1;
                                obj.tableName = tblPrefix + "SugarPurchase";
                                obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By";
                                obj.values = "'" + purchaseNo + "','PS','" + DOC_NO + "','" + DOC_DATE + "','" + Payment_To + "','" + MILL_CODE + "','" + From_Place + "','" + To_Place + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + SUBTOTAL + "','" + (QUANTAL * mill_rate) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                dsentry = new DataSet();
                                dsentry = obj.insertAccountMaster(ref strRev);
                                retValue = strRev;

                                obj.flag = 1;
                                obj.tableName = tblPrefix + "SugarPurchaseDetails";
                                obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                obj.values = "'" + purchaseNo + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + mill_rate + "','" + (QUANTAL * mill_rate) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                DataSet dsA = new DataSet();
                                dsA = obj.insertAccountMaster(ref strRev);

                                gleder.SugarPurchaseGledgerEffect("PS", Convert.ToInt32(purchaseNo), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                qry = "";
                                qry = "update " + tblPrefix + "deliveryorder set voucher_no='" + purchaseNo + "' , voucher_type='" + "PS" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                ds1 = new DataSet();
                                ds1 = clsDAL.SimpleQuery(qry);

                                if (PDS == "P")
                                {
                                    #region Entry In Sugar Sale
                                    string unitcity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + pdsunit + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                                    int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));

                                    string saleNumber = Convert.ToString(saleno);
                                    if (saleNumber != string.Empty)
                                    {
                                        string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                        if (str != string.Empty)
                                        {
                                            getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                            saleNumber = ViewState["maxval"].ToString();
                                        }
                                    }
                                    string saleParty = pdsParty;
                                    //entry in main table
                                    obj.flag = 1;
                                    obj.tableName = tblPrefix + "SugarSale";
                                    obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                    if (Delivery_Type == "C")
                                    {
                                        obj.values = "'" + saleNumber + "','SB','" + voucherno + "','" + DOC_DATE + "','" + pdsunit + "','" + pdsunit + "','" + MILL_CODE + "','" + From_Place + "','" + unitcity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                    }
                                    else
                                    {
                                        obj.values = "'" + saleNumber + "','SB','" + voucherno + "','" + DOC_DATE + "','" + pdsunit + "','" + pdsunit + "','" + MILL_CODE + "','" + From_Place + "','" + unitcity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                    }
                                    DataSet dsB = new DataSet();
                                    dsB = obj.insertAccountMaster(ref strRev);


                                    //entry in detail table
                                    obj.flag = 1;
                                    obj.tableName = tblPrefix + "sugarsaleDetails";
                                    obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                    obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                    DataSet dsC = new DataSet();
                                    dsC = obj.insertAccountMaster(ref strRev);


                                    qry = "";
                                    qry = "update " + tblPrefix + "deliveryorder set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                    DataSet ds2 = new DataSet();
                                    ds2 = clsDAL.SimpleQuery(qry);
                                    gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                    #endregion
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(SaleBillTo))
                                    {
                                        string salebillto = SaleBillTo;
                                        string salebilltocity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + salebillto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                        int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                        string saleNumber = Convert.ToString(saleno);
                                        if (saleNumber != string.Empty)
                                        {
                                            string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                            if (str != string.Empty)
                                            {
                                                getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                                saleNumber = ViewState["maxval"].ToString();
                                            }
                                        }
                                        //entry in main table
                                        obj.flag = 1;
                                        obj.tableName = tblPrefix + "SugarSale";
                                        obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                        if (Delivery_Type == "C")
                                        {
                                            obj.values = "'" + saleNumber + "','SB','" + voucherno + "','" + DOC_DATE + "','" + salebillto + "','" + salebillto + "','" + MILL_CODE + "','" + From_Place + "','" + salebilltocity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                        }
                                        else
                                        {
                                            obj.values = "'" + saleNumber + "','SB','" + voucherno + "','" + DOC_DATE + "','" + salebillto + "','" + salebillto + "','" + MILL_CODE + "','" + From_Place + "','" + salebilltocity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                        }
                                        DataSet dsD = new DataSet();
                                        dsD = obj.insertAccountMaster(ref strRev);


                                        //entry in detail table
                                        obj.flag = 1;
                                        obj.tableName = tblPrefix + "sugarsaleDetails";
                                        obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                        obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                        DataSet dsE = new DataSet();
                                        dsE = obj.insertAccountMaster(ref strRev);


                                        qry = "";
                                        qry = "update " + tblPrefix + "deliveryorder set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                        DataSet ds3 = new DataSet();
                                        ds3 = clsDAL.SimpleQuery(qry);
                                        gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                    }
                                }
                            }
                            else
                            {
                                obj.flag = 1;
                                obj.tableName = "" + tblPrefix + "Voucher";
                                obj.columnNm = "Tran_Type, DOC_NO , SUFFIX , DO_No ,Lorry_No, DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                                " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate," +
                                " FreightPerQtl, NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ,From_Place,To_Place," +
                                " Mill_Amount,TRANSPORT_CODE,LESSDIFF,Diff_Rate,VOUCHER_AMOUNT,CASH_ACCOUNT,CASH_AMOUNT_RATE,CASH_AC_AMOUNT," +
                                " Company_Code, Year_Code , Branch_Code,Delivery_Type,Created_By,Rate_Type," +
                                " BROKRAGE,SERVICE_CHARGE,L_RATE_DIFF,RATEDIFF,Commission_Rate,Commission_Amount,INTEREST,TRANSPORT_AMOUNT,OTHER_EXPENSES";

                                string voucherNumber = Convert.ToString(voucherno);
                                if (voucherNumber != string.Empty)
                                {
                                    string str = clsCommon.getString("select Doc_No from " + tblPrefix + "Voucher where Doc_No=" + voucherNumber + " and Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                    if (str != string.Empty)
                                    {
                                        getvoucherscode(tblPrefix + "Voucher", "Doc_No", "OV", "Tran_Type");
                                        voucherNumber = ViewState["maxval"].ToString();
                                    }
                                }
                                if (Delivery_Type == "N")
                                {
                                    obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASSCODE + "','" + BROKER_CODE + "'," +
                                                 "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                 " '" + FRIEGHT_RATE + "','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + "','" + NARRATION3 + " " + NARRATION4 + "','" + From_Place + "','" + To_Place + "'," +
                                                 " '" + MILL_AMOUNT + "','" + Transport_Code + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                 " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                }
                                else
                                {
                                    obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASSCODE + "','" + BROKER_CODE + "'," +
                                                 "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                 " '0.00','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + "','" + NARRATION3 + " " + NARRATION4 + "','" + From_Place + "','" + To_Place + "'," +
                                                 " '" + MILL_AMOUNT + "','" + Transport_Code + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                 " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                }
                                DataSet dsF = new DataSet();
                                dsF = obj.insertAccountMaster(ref strRev);

                                retValue = strRev;

                                qry = "";
                                qry = "update " + tblPrefix + "deliveryorder set voucher_no='" + voucherNumber + "' , voucher_type='" + "OV" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                DataSet ds4 = new DataSet();
                                ds4 = clsDAL.SimpleQuery(qry);
                                gleder.LoadingVoucherGlederEffect("OV", Convert.ToInt32(voucherNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                            }
                        }
                    }
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('Successfully Created')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('error " + ex + "')", true);
        }
    }


    #region [getMaxCodeofvouchers]
    private void getvoucherscode(string tblName, string objCode, string trantype, string tblColumnType)
    {
        try
        {
            DataSet ds = null;
            string docno = "0";
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                if (trantype == "NULL")
                {
                    obj.tableName = tblName + " where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                else
                {
                    obj.tableName = tblName + " where  " + tblColumnType + "='" + trantype + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                obj.code = objCode;

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    docno = ds.Tables[0].Rows[0][0].ToString();
                                    ViewState["maxval"] = docno;
                                }
                            }
                        }
                    }
                }
            }
            //return docno;
        }
        catch
        {
        }
    }
    #endregion
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string source = ConfigurationManager.AppSettings["SourcePath"].ToString();
            string destination = ConfigurationManager.AppSettings["DestinationPath"].ToString();
            var SourcePath = @"" + source + "";
            var DestinationPath = @"" + destination + "";

            foreach (string directory in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(directory.Replace(SourcePath, DestinationPath));
            }

            foreach (string file in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                //for copying file
                if (!File.Exists(file.Replace(SourcePath, DestinationPath)))
                {
                    File.Copy(file, file.Replace(SourcePath, DestinationPath), true);
                }
                else
                {
                    //For copying/Replace only modified file
                    DateTime dtDestFileTime = File.GetLastWriteTime(file.Replace(SourcePath, DestinationPath));
                    DateTime dtSourcefiletime = File.GetLastWriteTime(file);
                    if (dtDestFileTime != dtSourcefiletime)
                    {
                        File.Copy(file, file.Replace(SourcePath, DestinationPath), true);
                        File.SetLastWriteTime(file, dtDestFileTime);
                    }
                }
            }

            /*
            DataTable dt = new DataTable();
            //string qry = "Select Mobile from tblUser where User_Name='ankush'";
            string conStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string datetime = DateTime.Now.ToString();
            datetime = datetime.Replace('/', ':');
            datetime = datetime.Replace(':', ' ');
            string finaldate = Regex.Replace(datetime, @"\s+", "");

            string BackupFolder = ConfigurationManager.AppSettings["BackupFolder"];
            if (!System.IO.Directory.Exists(BackupFolder))
            {
                System.IO.Directory.CreateDirectory(BackupFolder);
            }
            var sqlConnStringBuilder = new SqlConnectionStringBuilder(conStr);
            var backupFileName = String.Format("{0}{1}{2}.bak",
                                 BackupFolder, sqlConnStringBuilder.InitialCatalog,
                                finaldate);
            using (var connection = new SqlConnection(sqlConnStringBuilder.ConnectionString))
            {
                var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                    sqlConnStringBuilder.InitialCatalog, backupFileName);

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }*/
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnPdsPartyCodePosting_Click(object sender, EventArgs e)
    {
        try
        {
            clsGledgerupdations gledger = new clsGledgerupdations();
            qry = "select d.SB_No,d.Carporate_Sale_No from " + tblPrefix + "deliveryorder d left join " + tblPrefix + "CarporateSale c on d.Carporate_Sale_No=c.Doc_No and d.company_code=c.Company_Code and d.Year_Code=c.Year_Code" +
                  " where d.tran_type='DO' and d.SB_No!=0 and d.Carporate_Sale_No!=0 and c.SellingType='P' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int Carporate_Sale_No = Convert.ToInt32(ds.Tables[0].Rows[i]["Carporate_Sale_No"].ToString());
                    int SB_No = Convert.ToInt32(ds.Tables[0].Rows[i]["SB_No"].ToString());
                    string PDSPartyCode = clsCommon.getString("select Ac_Code from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");

                    //updating party code in sale bill
                    string updateSB = "Update " + tblPrefix + "SugarSale set Ac_Code='" + PDSPartyCode + "' where doc_no=" + SB_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    DataSet dsUpdate = new DataSet();
                    dsUpdate = clsDAL.SimpleQuery(updateSB);

                    //gledger effects
                    gledger.SugarSaleGledgerEffect("SB", SB_No, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('Successfully Saved')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('Error:+" + ex + "')", true);
        }
    }
    protected void btnDoCorrection_Click(object sender, EventArgs e)
    {
        try
        {
            /* string qry = "";
             #region [Validation Part]
             bool isValidated = true;
             if (txtdoc_no.Text != string.Empty)
             {
                 if (ViewState["mode"] != null)
                 {
                     if (ViewState["mode"].ToString() == "I")
                     {
                         string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");
                         if (str != string.Empty)
                         {
                             lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                             this.getMaxCode();
                             isValidated = true;
                         }
                         else
                         {
                             isValidated = true;
                         }
                     }
                 }
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtdoc_no);
                 return;
             }
             if (txtPurcNo.Text != string.Empty)
             {
                 string tenderNo = clsCommon.getString("Select Tender_No from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                 if (txtPurcNo.Text != tenderNo)
                 {
                     isValidated = false;
                     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Purchase Number Does Not Belongs to This Mill!');", true);
                     setFocusControl(txtMILL_CODE);
                     return;
                 }
                 else
                 {
                     isValidated = true;
                 }
             }
             if (txtDOC_DATE.Text != string.Empty)
             {
                 if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                 {
                     isValidated = true;
                 }
                 else
                 {
                     isValidated = false;
                     setFocusControl(txtDOC_DATE);
                     return;
                 }
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtDOC_DATE);
                 return;
             }
             if (drpDOType.SelectedValue != "0")
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(drpDOType);
                 return;
             }
             if (txtMILL_CODE.Text != string.Empty)
             {
                 string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                 if (str != string.Empty)
                 {
                     isValidated = true;
                 }
                 else
                 {
                     isValidated = false;
                     setFocusControl(txtMILL_CODE);
                     return;
                 }
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtMILL_CODE);
                 return;
             }
             if (txtGETPASS_CODE.Text != string.Empty && txtGETPASS_CODE.Text != "2")
             {
                 string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                 if (str != string.Empty)
                 {
                     isValidated = true;
                 }
                 else
                 {
                     isValidated = false;
                     setFocusControl(txtGETPASS_CODE);
                     return;
                 }
             }
             else
             {
                 isValidated = false;
                 txtGETPASS_CODE.Text = string.Empty;
                 LBLGETPASS_NAME.Text = string.Empty;
                 setFocusControl(txtGETPASS_CODE);
                 return;
             }
             if (txtvoucher_by.Text != string.Empty && txtvoucher_by.Text != "2")
             {
                 string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                 if (str != string.Empty)
                 {
                     isValidated = true;
                 }
                 else
                 {
                     isValidated = false;
                     setFocusControl(txtvoucher_by);
                     return;
                 }
             }
             else
             {
                 isValidated = false;
                 txtvoucher_by.Text = string.Empty;
                 lblvoucherbyname.Text = string.Empty;
                 setFocusControl(txtvoucher_by);
                 return;
             }
             if (txtGRADE.Text != string.Empty)
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtGRADE);
                 return;
             }
             if (txtquantal.Text != string.Empty)
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtquantal);
                 return;
             }
             if (txtPACKING.Text != string.Empty)
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtPACKING);
                 return;
             }
             if (txtexcise_rate.Text != string.Empty)
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtexcise_rate);
                 return;
             }
             if (txtPurcNo.Text != string.Empty)
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtPurcNo);
                 return;
             }
             if (txtcarporateSale.Text != string.Empty)
             {
                 qry = "select Year_Code from " + qrycarporateSalebalance + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Doc_No=" + txtcarporateSale.Text;
                 string s = clsCommon.getString(qry);
                 if (s != string.Empty)
                 {
                     isValidated = true;
                 }
                 else
                 {
                     isValidated = false;
                     txtcarporateSale.Text = "";
                     lblCSYearCode.Text = "";
                     setFocusControl(txtcarporateSale);
                     return;
                 }
             }
             int count = 0;
             if (grdDetail.Rows.Count > 1)
             {
                 for (int i = 0; i < grdDetail.Rows.Count; i++)
                 {
                     if (grdDetail.Rows[i].Cells[9].Text.ToString() == "D")
                     {
                         count++;
                     }
                 }
                 if (grdDetail.Rows.Count == count)
                 {
                     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Add Dispatch Details!');", true);
                     isValidated = false;
                     setFocusControl(btnOpenDetailsPopup);
                     return;
                 }
             }
             if (grdDetail.Rows.Count > 0)
             {
                 double total = 0.00;
                 for (int i = 0; i < grdDetail.Rows.Count; i++)
                 {
                     if (grdDetail.Rows[i].Cells[9].Text.ToString() != "D")
                     {
                         double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                         total += amount;
                     }
                 }

                 if (total == Convert.ToDouble(lblMillAmount.Text))
                 {
                     isValidated = true;
                 }
                 else
                 {
                     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Mill Amount Does Not match with detail amount!');", true);
                     isValidated = false;
                     setFocusControl(btnOpenDetailsPopup);
                     return;
                 }
             }
             if (drpDOType.SelectedValue == "DI")
             {
                 if (grdDetail.Rows.Count == 0)
                 {
                     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Dispatch Details!');", true);
                     isValidated = false;
                     setFocusControl(btnOpenDetailsPopup);
                     return;
                 }
             }
             if (txtSALE_RATE.Text != "0")
             {
                 isValidated = true;
             }
             else
             {
                 isValidated = false;
                 setFocusControl(txtSALE_RATE);
                 return;
             }
             #endregion

             #region -Head part declearation
             Int32 DONumber = 0;
             Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
             string DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
             string DESP_TYPE = drpDOType.SelectedValue;
             string Delivery_Type = string.Empty;
             if (DESP_TYPE == "DI")
             {
                 if (drpDOType.SelectedValue == "DI")
                 {
                     Delivery_Type = drpDeliveryType.SelectedValue;
                 }
             }
             string MILL_CODE = txtMILL_CODE.Text;
             string GETPASS_CODE = txtGETPASS_CODE.Text;
             string VOUCHER_BY = txtvoucher_by.Text;
             double FRIEGHT_RATE = txtFrieght.Text != string.Empty ? Convert.ToDouble(txtFrieght.Text) : 0;
             double FRIEGHT_AMOUNT = txtFrieghtAmount.Text != string.Empty ? Convert.ToDouble(txtFrieghtAmount.Text) : 0;
             double VASULI_AMOUNT = txtVasuliAmount.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount.Text) : 0;
             double VASULI_RATE = txtVasuliRate.Text != string.Empty ? Convert.ToDouble(txtVasuliRate.Text) : 0;
             double MEMO_ADVANCE = txtMemoAdvance.Text != string.Empty ? Convert.ToDouble(txtMemoAdvance.Text) : 0;
             string Ac_Code = txtvoucher_by.Text;
             string GRADE = txtGRADE.Text;
             double QUANTAL = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
             Int32 PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
             Int32 BAGS = txtBAGS.Text != string.Empty ? Convert.ToInt32(txtBAGS.Text) : 0;
             double mill_rate = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
             double EXCISE_RATE = txtexcise_rate.Text != string.Empty ? Convert.ToDouble(txtexcise_rate.Text) : 0.00;
             double SALE_RATE = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.00;
             double MILL_AMOUNT = 0.00;// double.Parse(lblMillAmount.Text.ToString());
             MILL_AMOUNT = QUANTAL * mill_rate;
             lblMillAmount.Text = MILL_AMOUNT.ToString();
             double DIFF_RATE = lblDiffrate.Text != string.Empty ? Convert.ToDouble(lblDiffrate.Text) : 0.00;
             double DIFF_AMOUNT = txtDIFF_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtDIFF_AMOUNT.Text) : 0.00;
             double VASULI_RATE_1 = txtVasuliRate1.Text != string.Empty ? Convert.ToDouble(txtVasuliRate1.Text) : 0.00;
             double VASULI_AMOUNT_1 = txtVasuliAmount1.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount1.Text) : 0.00;
             string SaleBillTo = txtSaleBillTo.Text;
             string MM_CC = drpCC.SelectedValue.ToString();
             //double Party_Commission_Rate = txtPartyCommission.Text != string.Empty ? Convert.ToDouble(txtPartyCommission.Text) : 0.00;
             double MM_Rate = txtMemoAdvanceRate.Text != string.Empty ? Convert.ToDouble(txtMemoAdvanceRate.Text) : 0.00;

             Int32 DO_CODE = txtDO_CODE.Text != string.Empty ? Convert.ToInt32(txtDO_CODE.Text) : 2;
             Int32 BROKER_CODE = txtBroker_CODE.Text != string.Empty ? Convert.ToInt32(txtBroker_CODE.Text) : 2;
             string TRUCK_NO = txtTruck_NO.Text;
             Int32 TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
             Int32 SaleBillTransport = 0;
             if (drpCC.SelectedValue == "Credit")
             {
                 SaleBillTransport = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
             }
             else
             {
                 SaleBillTransport = 1;
             }
             Int32 OVTransportCode = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0; ;
             if (drpCC.SelectedValue == "Cash")
             {
                 OVTransportCode = 1;
             }

             string NARRATION1 = txtNARRATION1.Text;
             string NARRATION2 = txtNARRATION2.Text;
             string NARRATION3 = txtNARRATION3.Text;
             string NARRATION4 = txtNARRATION4.Text;
             string INVOICE_NO = txtINVOICE_NO.Text;
             string CheckPost = txtCheckPostName.Text;
             int purc_no = txtPurcNo.Text != string.Empty ? Convert.ToInt32(txtPurcNo.Text) : 0;
             int purc_order = txtPurcOrder.Text != string.Empty ? Convert.ToInt32(txtPurcOrder.Text) : 0;
             //double final_amout = mill_rate * QUANTAL;
             #region other voucher amount
             double VoucherBrokrage = txtVoucherBrokrage.Text != string.Empty ? Convert.ToDouble(txtVoucherBrokrage.Text) : 0.00;
             double VoucherServiceCharge = txtVoucherServiceCharge.Text != string.Empty ? Convert.ToDouble(txtVoucherServiceCharge.Text) : 0.00;
             double VoucherRateDiffRate = txtVoucherL_Rate_Diff.Text != string.Empty ? Convert.ToDouble(txtVoucherL_Rate_Diff.Text) : 0.00;
             double VoucherRateDiffAmt = txtVoucherRATEDIFFAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherRATEDIFFAmt.Text) : 0.00;
             double VoucherBankCommRate = txtVoucherCommission_Rate.Text != string.Empty ? Convert.ToDouble(txtVoucherCommission_Rate.Text) : 0.00;
             double VoucherBankCommAmt = txtVoucherBANK_COMMISSIONAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherBANK_COMMISSIONAmt.Text) : 0.00;
             double VoucherInterest = txtVoucherInterest.Text != string.Empty ? Convert.ToDouble(txtVoucherInterest.Text) : 0.00;
             double VoucherTransport = txtVoucherTransport_Amount.Text != string.Empty ? Convert.ToDouble(txtVoucherTransport_Amount.Text) : 0.00;
             double VoucherOtherExpenses = txtVoucherOTHER_Expenses.Text != string.Empty ? Convert.ToDouble(txtVoucherOTHER_Expenses.Text) : 0.00;

             #endregion

             double FINAL_AMOUNT = FRIEGHT_AMOUNT - MEMO_ADVANCE;
             string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
             string retValue = string.Empty;
             string strRev = string.Empty;
             int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
             int Year_Code = Convert.ToInt32(Session["year"].ToString());
             int year_Code = Convert.ToInt32(Session["year"].ToString());
             int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
             float DIFF = float.Parse(lblDiffrate.Text);
             double LESS_DIFF = Math.Round(((SALE_RATE - FRIEGHT_RATE) - (mill_rate)) * QUANTAL);
             double LESSDIFFOV = DIFF_RATE * QUANTAL;
             string Driver_Mobile = txtDriverMobile.Text;
             double Diff_Rate = 0.00;
             double VOUCHER_AMOUNT = 0.00;
             string Rate_Type = string.Empty;
             if (drpDeliveryType.SelectedValue == "N")
             {
                 Diff_Rate = ((SALE_RATE - FRIEGHT_RATE) - mill_rate) * QUANTAL;
                 VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                 Rate_Type = "A";
             }
             else
             {
                 Diff_Rate = ((SALE_RATE) - mill_rate) * QUANTAL;
                 VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                 Rate_Type = "L";
             }
             //Int32 UTR_No = txtUTRNo.Text != string.Empty ? Convert.ToInt32(txtUTRNo.Text) : 0;
             Int32 Carporate_Sale_No = txtcarporateSale.Text != string.Empty ? Convert.ToInt32(txtcarporateSale.Text) : 0;
             string WhoseFrieght = ddlFrieghtType.SelectedValue.ToString();
             int UTR_Year_Code = lblUTRYearCode.Text != string.Empty ? Convert.ToInt32(lblUTRYearCode.Text) : 0;
             int Carporate_Sale_Year_Code = lblCSYearCode.Text != string.Empty ? Convert.ToInt32(lblCSYearCode.Text) : 0;
             Int32 voucher_no = 0;
             string PDS = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
             AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
             int memo_no = 0;
             if (ViewState["mode"].ToString() == "I")
             {
                 if (AUTO_VOUCHER == "YES")
                 {
                     if (DESP_TYPE == "DI")
                     {
                         memo_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblHead + " Where Tran_Type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                     }
                     if (DESP_TYPE == "DO")
                     {
                         if (DIFF_AMOUNT != 0)
                         {
                             MaxVoucher();
                             if (hdnvouchernumber.Value != string.Empty)
                             {
                                 voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                             }
                         }
                         else
                         {
                             hdnvouchernumber.Value = "0";
                             voucher_no = 0;
                         }
                     }
                     else
                     {
                         MaxVoucher();
                         if (hdnvouchernumber.Value != string.Empty)
                         {
                             voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                         }
                     }
                 }
             }
             string voucher_type = lblVoucherType.Text;
             //Int32 memo_no = lblMemoNo.Text != string.Empty ? Convert.ToInt32(lblMemoNo.Text) : 0;
             Int32 voucherlbl = lblVoucherNo.Text != string.Empty ? Convert.ToInt32(lblVoucherNo.Text) : 0;
             //double Freight_Amount = lblFreight.Text != string.Empty ? Convert.ToDouble(lblFreight.Text) : 0.00;
             string myNarration = string.Empty;
             string myNarration2 = string.Empty;
             string myNarration3 = string.Empty;
             string myNarration4 = string.Empty;
             string vouchnarration = string.Empty;
             millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + MILL_CODE + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
             if (ddlFrieghtType.SelectedValue == "O")
             {
                 vouchnarration = millShortName + " (" + "S.R." + SALE_RATE + "-" + FRIEGHT_RATE + "- M.R." + mill_rate + ")*" + QUANTAL;
             }
             else
             {
                 vouchnarration = "Qntl " + QUANTAL + "  " + millShortName + "(M.R." + mill_rate + " P.R." + SALE_RATE + ")";
             }
             if (grdDetail.Rows.Count > 0)
             {
                 for (int i = 0; i < grdDetail.Rows.Count; i++)
                 {
                     string utrno = Server.HtmlEncode(grdDetail.Rows[i].Cells[6].Text.ToString());
                     string Utr_No = Server.HtmlEncode(grdDetail.Rows[i].Cells[8].Text.ToString());
                     string nar = clsCommon.getString("select 'dt:'+Convert(varchar(10),doc_date,103)+'  amt:'+CONVERT(nvarchar(255),amount) from " + tblPrefix + "UTR where doc_no=" + Utr_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                     if (i == 0)
                     {
                         if (utrno != "Transfer Letter")
                         {
                             myNarration = utrno + " " + nar;
                         }
                         else
                         {
                             myNarration = "Please Debit The Same Amount in our A/c";
                         }

                     }
                     if (i == 1)
                     {
                         myNarration2 = utrno;
                     }
                     if (i == 2)
                     {
                         myNarration3 = utrno;
                     }
                     if (i == 3)
                     {
                         myNarration4 = utrno;
                     }
                 }
             }

             int VOUCHERAMOUNT = Convert.ToInt32(Math.Ceiling(DIFF_AMOUNT));
             //double MILL_AMOUNT = mill_rate * QUANTAL;
             string city_code = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
             string From_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
             string city_code2 = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
             string To_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code2 + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
             string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
             double BILL_AMOUNT = 0.00;
             double SUBTOTAL = 0.00;
             SUBTOTAL = QUANTAL * mill_rate;
             BILL_AMOUNT = SUBTOTAL;
             int ID1 = 0;
             #endregion-End of Head part declearation
             Int32 pdsparty = hdnfPDSPartyCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSPartyCode.Value) : 0; ;
             Int32 pdsunit = hdnfPDSUnitCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSUnitCode.Value) : 0;
             #region save Head Master
             if (isValidated == true)
             {
                 using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                 {
                     clsGledgerupdations gleder = new clsGledgerupdations();
                     if (ViewState["mode"] != null)
                     {
                         DataSet ds = new DataSet();
                         if (ViewState["mode"].ToString() == "I")
                         {
                             obj.flag = 1;
                             obj.tableName = tblHead;
                             obj.columnNm = "tran_type,DOC_NO,DOC_DATE,DESP_TYPE,MILL_CODE,GETPASSCODE,VOUCHER_BY,GRADE,QUANTAL,PACKING,BAGS,mill_rate,amount,EXCISE_RATE,SALE_RATE,DIFF_RATE,DIFF_AMOUNT,DO,broker,TRUCK_NO,transport,NARRATION1,NARRATION2,NARRATION3,NARRATION4,company_code,Year_Code,Branch_Code,purc_no,purc_order,Created_By,UTR_Year_Code,Carporate_Sale_No,Carporate_Sale_Year_Code,final_amout,memo_no,Ac_Code,FreightPerQtl,Freight_Amount,vasuli_rate,vasuli_amount,Memo_Advance,Delivery_Type,driver_no,WhoseFrieght,Invoice_No,vasuli_rate1,vasuli_amount1," +
                                            " MM_CC,MM_Rate,Voucher_Brokrage,Voucher_Service_Charge,Voucher_RateDiffRate,Voucher_RateDiffAmt,Voucher_BankCommRate,Voucher_BankCommAmt,Voucher_Interest,Voucher_TransportAmt,Voucher_OtherExpenses,CheckPost,SaleBillTo";
                             obj.values = "'" + trnType + "','" + DOC_NO + "','" + DOC_DATE + "','" + DESP_TYPE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + VOUCHER_BY + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + mill_rate + "','" + MILL_AMOUNT + "','" + EXCISE_RATE + "','" + SALE_RATE + "','" + DIFF_RATE + "','" + DIFF_AMOUNT + "','" + DO_CODE + "','" + BROKER_CODE + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + myNarration + "','" + NARRATION2 + myNarration2 + "','" + NARRATION3 + myNarration3 + "','" + NARRATION4 + myNarration4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + purc_order + "','" + user + "','" + UTR_Year_Code + "','" + Carporate_Sale_No + "','" + Carporate_Sale_Year_Code + "','" + MILL_AMOUNT + "','" + memo_no + "','" + Ac_Code + "'," +
                                 "'" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + Delivery_Type + "','" + Driver_Mobile + "','" + WhoseFrieght + "','" + INVOICE_NO + "','" + VASULI_RATE_1 + "','" + VASULI_AMOUNT_1 + "', " +
                                 "'" + MM_CC + "','" + MM_Rate + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "','" + CheckPost + "','" + SaleBillTo + "'";
                             string nn = obj.insertDO(ref strRev);
                             retValue = strRev;
                             txtNARRATION4.Text = nn;

                             #region entry in tender details
                             if (purc_order == 1)
                             {
                                 string id = clsCommon.getString("select AutoID from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + purc_order + "'");
                                 if (id != string.Empty)
                                 {
                                     //this id is already inserted Get max id
                                     string newId = clsCommon.getString("select max(ID) from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                     ID1 = Convert.ToInt32(newId) + 1;
                                 }
                                 if (drpDOType.SelectedValue != "DI")
                                 {
                                     Delivery_Type = "C";
                                 }
                                 obj.flag = 1;
                                 obj.tableName = tblPrefix + "Tenderdetails";
                                 obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                 if (PDS == "P")
                                 {
                                     obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + pdsunit + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + pdsparty + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                 }
                                 else
                                 {
                                     obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + GETPASS_CODE + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + VOUCHER_BY + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                 }
                                 ds = obj.insertAccountMaster(ref strRev);
                                 retValue = strRev;

                                 string buyerQntl = clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                 double buyerTotalQntl = Convert.ToDouble(buyerQntl) - QUANTAL;

                                 qry = "";
                                 qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + buyerTotalQntl + "' where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                 ds = clsDAL.SimpleQuery(qry);

                                 qry = "";
                                 qry = "update " + tblHead + " set purc_order='" + ID1 + "' where DOC_NO='" + DOC_NO + "' and tran_type='" + trnType + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                 ds = clsDAL.SimpleQuery(qry);
                             }
                             #endregion

                             #region VoucherEntries
                             if (AUTO_VOUCHER == "YES")
                             {
                                 #region Code to use later if Customer wants
                                 if (drpDOType.SelectedValue == "DI")
                                 {
                                     if (txtGETPASS_CODE.Text == selfac || PDS == "P")
                                     {
                                         string purchaseNo = Convert.ToString(voucher_no);
                                         if (purchaseNo != string.Empty)
                                         {
                                             string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarPurchase where doc_no=" + purchaseNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                             if (str != string.Empty)
                                             {
                                                 getvoucherscode(tblPrefix + "SugarPurchase", "doc_no", "NULL", "Tran_Type");
                                                 purchaseNo = ViewState["maxval"].ToString();
                                             }
                                         }
                                         //Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                                         Int32 Payment_To = Convert.ToInt32(grdDetail.Rows[0].Cells[4].Text.ToString());
                                         obj.flag = 1;
                                         obj.tableName = tblPrefix + "SugarPurchase";
                                         obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By";
                                         obj.values = "'" + purchaseNo + "','PS','" + DOC_NO + "','" + DOC_DATE + "','" + Payment_To + "','" + MILL_CODE + "','" + From_Place + "','" + To_Place + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + SUBTOTAL + "','" + (QUANTAL * mill_rate) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                         ds = obj.insertAccountMaster(ref strRev);
                                         retValue = strRev;

                                         obj.flag = 1;
                                         obj.tableName = tblPrefix + "SugarPurchaseDetails";
                                         obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                         obj.values = "'" + purchaseNo + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + mill_rate + "','" + (QUANTAL * mill_rate) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                         ds = new DataSet();
                                         ds = obj.insertAccountMaster(ref strRev);

                                         gleder.SugarPurchaseGledgerEffect("PS", Convert.ToInt32(purchaseNo), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                         qry = "";
                                         qry = "update " + tblHead + " set voucher_no='" + purchaseNo + "' , voucher_type='" + "PS" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                         ds = clsDAL.SimpleQuery(qry);

                                         if (PDS == "P")
                                         {
                                             #region Entry In Sugar Sale
                                             string unitcity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + pdsunit + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                                             int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));

                                             string saleNumber = Convert.ToString(saleno);
                                             if (saleNumber != string.Empty)
                                             {
                                                 string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                                 if (str != string.Empty)
                                                 {
                                                     getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                                     saleNumber = ViewState["maxval"].ToString();
                                                 }
                                             }
                                             hdnfSB_No.Value = saleNumber;
                                             lblSB_No.Text = "Sale Bill No: " + hdnfSB_No.Value;
                                             string saleParty = hdnfPDSPartyCode.Value;
                                             //entry in main table
                                             obj.flag = 1;
                                             obj.tableName = tblPrefix + "SugarSale";
                                             obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                             if (drpDeliveryType.SelectedValue == "C")
                                             {
                                                 obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + saleParty + "','" + pdsunit + "','" + MILL_CODE + "','" + From_Place + "','" + unitcity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                             }
                                             else
                                             {
                                                 obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + saleParty + "','" + pdsunit + "','" + MILL_CODE + "','" + From_Place + "','" + unitcity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                             }


                                             ds = obj.insertAccountMaster(ref strRev);

                                             //entry in detail table
                                             obj.flag = 1;
                                             obj.tableName = tblPrefix + "sugarsaleDetails";
                                             obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                             obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                             ds = new DataSet();
                                             ds = obj.insertAccountMaster(ref strRev);

                                             qry = "";
                                             qry = "update " + tblHead + " set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                             ds = clsDAL.SimpleQuery(qry);
                                             gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                             #endregion
                                         }
                                         else
                                         {
                                             if (!string.IsNullOrEmpty(txtSaleBillTo.Text))
                                             {
                                                 string salebillto = txtSaleBillTo.Text;
                                                 string salebilltocity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + salebillto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                                 int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                                 string saleNumber = Convert.ToString(saleno);
                                                 if (saleNumber != string.Empty)
                                                 {
                                                     string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                                     if (str != string.Empty)
                                                     {
                                                         getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                                         saleNumber = ViewState["maxval"].ToString();
                                                     }
                                                 }
                                                 hdnfSB_No.Value = saleNumber;
                                                 lblSB_No.Text = "Sale Bill No: " + hdnfSB_No.Value;
                                                 //entry in main table
                                                 obj.flag = 1;
                                                 obj.tableName = tblPrefix + "SugarSale";
                                                 obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                                 if (drpDeliveryType.SelectedValue == "C")
                                                 {
                                                     obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + salebillto + "','" + salebillto + "','" + MILL_CODE + "','" + From_Place + "','" + salebilltocity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                                 }
                                                 else
                                                 {
                                                     obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + salebillto + "','" + salebillto + "','" + MILL_CODE + "','" + From_Place + "','" + salebilltocity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + SaleBillTransport + "','" + MEMO_ADVANCE + "'";
                                                 }
                                                 ds = obj.insertAccountMaster(ref strRev);

                                                 //entry in detail table
                                                 obj.flag = 1;
                                                 obj.tableName = tblPrefix + "sugarsaleDetails";
                                                 obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                                 obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                                 ds = new DataSet();
                                                 ds = obj.insertAccountMaster(ref strRev);

                                                 qry = "";
                                                 qry = "update " + tblHead + " set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                                 ds = clsDAL.SimpleQuery(qry);
                                                 gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                             }
                                         }
                                     }
                                     else
                                     {
                                         #region Entry in Loading Voucher
                                         int voucherno = voucher_no;
                                         obj.flag = 1;
                                         obj.tableName = "" + tblPrefix + "Voucher";
                                         obj.columnNm = "Tran_Type, DOC_NO , SUFFIX , DO_No ,Lorry_No, DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                                         " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate," +
                                         " FreightPerQtl, NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ,From_Place,To_Place," +
                                         " Mill_Amount,TRANSPORT_CODE,LESSDIFF,Diff_Rate,VOUCHER_AMOUNT,CASH_ACCOUNT,CASH_AMOUNT_RATE,CASH_AC_AMOUNT," +
                                         " Company_Code, Year_Code , Branch_Code,Delivery_Type,Created_By,Rate_Type," +
                                         " BROKRAGE,SERVICE_CHARGE,L_RATE_DIFF,RATEDIFF,Commission_Rate,Commission_Amount,INTEREST,TRANSPORT_AMOUNT,OTHER_EXPENSES";

                                         string voucherNumber = Convert.ToString(voucherno);
                                         if (voucherNumber != string.Empty)
                                         {
                                             string str = clsCommon.getString("select Doc_No from " + tblPrefix + "Voucher where Doc_No=" + voucherNumber + " and Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                             if (str != string.Empty)
                                             {
                                                 getvoucherscode(tblPrefix + "Voucher", "Doc_No", "OV", "Tran_Type");
                                                 voucherNumber = ViewState["maxval"].ToString();
                                             }
                                         }
                                         if (drpDeliveryType.SelectedValue == "N")
                                         {
                                             obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                                          "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                          " '" + FRIEGHT_RATE + "','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + " " + myNarration2 + "','" + NARRATION3 + " " + myNarration3 + " " + NARRATION4 + " " + myNarration4 + "','" + From_Place + "','" + To_Place + "'," +
                                                          " '" + MILL_AMOUNT + "','" + TRANSPORT_CODE + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                          " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                         }
                                         else
                                         {
                                             obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                                          "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                          " '0.00','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + " " + myNarration2 + "','" + NARRATION3 + " " + myNarration3 + " " + NARRATION4 + " " + myNarration4 + "','" + From_Place + "','" + To_Place + "'," +
                                                          " '" + MILL_AMOUNT + "','" + TRANSPORT_CODE + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                          " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                         }
                                         ds = obj.insertAccountMaster(ref strRev);
                                         retValue = strRev;

                                         qry = "";
                                         qry = "update " + tblHead + " set voucher_no='" + voucherNumber + "' , voucher_type='" + "OV" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                         ds = clsDAL.SimpleQuery(qry);
                                         gleder.LoadingVoucherGlederEffect("OV", Convert.ToInt32(voucherNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                     }
                                     #region Entry in Motor Memo

                                     //if (AUTO_VOUCHER == "YES")
                                     //{
                                     //    if (DESP_TYPE == "DI")
                                     //    {
                                     //        memo_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblHead + " Where Tran_Type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                     //    }
                                     //}
                                     obj.flag = 1;
                                     obj.tableName = tblHead;
                                     obj.columnNm = "tran_type,DESP_TYPE,DOC_NO,DOC_DATE,MILL_CODE,GETPASSCODE,GRADE,QUANTAL,PACKING,BAGS,TRUCK_NO,transport,NARRATION1,NARRATION2,NARRATION3,NARRATION4,company_code,Year_Code,Branch_Code,purc_no,purc_order,Ac_Code,FreightPerQtl,Freight_Amount,vasuli_rate,vasuli_amount,less,Created_By,final_amout,driver_no";
                                     string MemoNumber = Convert.ToString(memo_no);
                                     if (MemoNumber != string.Empty)
                                     {
                                         string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no=" + MemoNumber + " and tran_type='MM' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                         if (str != string.Empty)
                                         {
                                             getvoucherscode(tblHead, "doc_no", "MM", "tran_type");
                                             MemoNumber = ViewState["maxval"].ToString();
                                         }
                                     }
                                     if (purc_order == 1)
                                     {
                                         if (PDS == "P")
                                         {
                                             obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + pdsunit + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + pdsparty + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                         }
                                         else
                                         {
                                             obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + VOUCHER_BY + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                         }
                                     }
                                     else
                                     {
                                         if (PDS == "P")
                                         {
                                             obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + pdsunit + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + pdsparty + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                         }
                                         else
                                         {
                                             obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + purc_order + "','" + VOUCHER_BY + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                         }
                                     }
                                     string mm = obj.insertDO(ref strRev);
                                     retValue = strRev;
                                     #endregion
                                 }

                              
                                         #endregion

                                 #region Creating Local Voucher
                                 if (drpDOType.SelectedValue == "DO")
                                 {
                                     if (DIFF_AMOUNT != 0)
                                     {
                                         int voucherno = voucher_no;
                                         obj.flag = 1;
                                         obj.tableName = "" + tblPrefix + "Voucher";
                                         obj.columnNm = " Tran_Type, DOC_NO , SUFFIX , DO_No , DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                                         " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate,Mill_Amount," +
                                         " FREIGHT, NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ,TRANSPORT_CODE," +
                                         " VOUCHER_AMOUNT ,Diff_Amount," +
                                         " Company_Code, Year_Code , Branch_Code,Created_By";

                                         string LVNumber = Convert.ToString(voucherno);
                                         if (LVNumber != string.Empty)
                                         {
                                             string str = clsCommon.getString("select Doc_No from " + tblPrefix + "Voucher where Doc_No=" + LVNumber + " and Tran_Type='LV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                             if (str != string.Empty)
                                             {
                                                 getvoucherscode(tblPrefix + "Voucher", "Doc_No", "LV", "Tran_Type");
                                                 LVNumber = ViewState["maxval"].ToString();
                                             }
                                         }

                                         obj.values = "'" + "LV" + "','" + LVNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                         "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "','" + MILL_AMOUNT + "'," +
                                         " '" + FRIEGHT_AMOUNT + "','" + vouchnarration + "','" + NARRATION2 + " Lorry No:" + TRUCK_NO + "','" + NARRATION3 + "','" + NARRATION4 + TRUCK_NO + "','" + TRANSPORT_CODE + "'," +
                                         " '" + DIFF_AMOUNT + "','" + DIFF_RATE + "'," +
                                            "'" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + user + "'";
                                         ds = obj.insertAccountMaster(ref strRev);
                                         retValue = strRev;

                                         //Gledger Effect for Local Voucher
                                         qry = "";
                                         qry = "delete from " + GLedgerTable + " where TRAN_TYPE='LV' and DOC_NO=" + LVNumber + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                         ds = clsDAL.SimpleQuery(qry);
                                         Int32 GID = 0;

                                         double LVVoucherAmount = ((SALE_RATE - mill_rate) * QUANTAL);
                                         if (LVVoucherAmount > 0)
                                         {
                                             GID = GID + 1;
                                             obj.flag = 1;
                                             obj.tableName = GLedgerTable;
                                             obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                             obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + Math.Abs(LVVoucherAmount) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                             ds = obj.insertAccountMaster(ref strRev);
                                         }
                                         else
                                         {
                                             GID = GID + 1;
                                             obj.flag = 1;
                                             obj.tableName = GLedgerTable;
                                             obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                             obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + Math.Abs(LVVoucherAmount) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                             ds = obj.insertAccountMaster(ref strRev);
                                         }
                                         // diffrance amount effect
                                         if (LVVoucherAmount > 0)
                                         {
                                             //------------Credit effect
                                             GID = GID + 1;
                                             obj.flag = 1;
                                             obj.tableName = GLedgerTable;
                                             obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                             obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + 0 + "','" + vouchnarration + "','" + Math.Abs(LVVoucherAmount) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                             ds = obj.insertAccountMaster(ref strRev);
                                         }
                                         else
                                         {
                                             //------------Credit effect
                                             GID = GID + 1;
                                             obj.flag = 1;
                                             obj.tableName = GLedgerTable;
                                             obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                             obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + 0 + "','" + vouchnarration + "','" + Math.Abs(LVVoucherAmount) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                             ds = obj.insertAccountMaster(ref strRev);
                                         }


                                         qry = "";
                                         if (LVNumber != "0")
                                         {
                                             qry = "update " + tblHead + " set voucher_no='" + LVNumber + "' , voucher_type='" + "LV" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                         }
                                         else
                                         {
                                             qry = "update " + tblHead + " set voucher_no='" + LVNumber + "' , voucher_type='' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                         }
                                         ds = clsDAL.SimpleQuery(qry);
                                     }
                                     else
                                     {
                                         string LVNumber = Convert.ToString(voucher_no);
                                         qry = "";
                                         qry = "update " + tblHead + " set voucher_no='" + LVNumber + "' , voucher_type='' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                         ds = clsDAL.SimpleQuery(qry);
                                     }
                                 #endregion
                                 }
                                 #endregion
                             }
         */
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnGstPurchasePosting_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            int companyGSTStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
            string qry = "select mill_rate,QUANTAL,MillGSTStateCode,voucher_no,purc_no,mill_code from " + tblPrefix + "deliveryorder where  doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and desp_type='DI' and voucher_type='PS'";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double cgstrate = 2.5;
                        double sgstrate = 2.5;
                        double igstrate = 5;

                        double CGSTAmountForPS = 0.0;
                        double SGSTAmountForPS = 0.0;
                        double IGSTAmountForPS = 0.0;

                        double CGSTRateForPS = 0.00;
                        double SGSTRateForPS = 0.00;
                        double IGSTRateForPS = 0.00;

                        double millrate = Convert.ToDouble(dt.Rows[i]["mill_rate"].ToString());
                        double quintal = Convert.ToDouble(dt.Rows[i]["QUANTAL"].ToString());
                        int MillGSTStateCode = Convert.ToInt32(dt.Rows[i]["MillGSTStateCode"].ToString());
                        int PS_No = Convert.ToInt32(dt.Rows[i]["voucher_no"].ToString());
                        int tenderNo = Convert.ToInt32(dt.Rows[i]["purc_no"].ToString());
                        int mill_code = Convert.ToInt32(dt.Rows[i]["mill_code"].ToString());
                        string paymentto = clsCommon.getString("Select ISNULL(Payment_To,0) from " + tblPrefix + "Tender where Tender_No=" + tenderNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");


                        int gstSateCodeForPurchaseBill = MillGSTStateCode;
                        if (paymentto.Trim() != string.Empty)
                        {
                            if (Convert.ToInt32(mill_code) != Convert.ToInt32(paymentto))
                            {
                                int paymenttogststatecode = Convert.ToInt32(clsCommon.getString("Select ISNULL(GSTStateCode,0) from " + tblPrefix + "AccountMaster where Ac_Code=" + paymentto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
                                gstSateCodeForPurchaseBill = paymenttogststatecode;
                            }
                        }


                        if (companyGSTStateCode == gstSateCodeForPurchaseBill)
                        {
                            CGSTRateForPS = cgstrate;
                            double millamount = millrate * quintal;
                            double cgsttaxAmountOnMR = Math.Round((millamount * cgstrate / 100), 2);
                            //double cgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + cgsttaxAmountOnMR) * mill_rate)), 2);
                            //double cgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - cgstExMillRate), 2);
                            CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

                            SGSTRateForPS = sgstrate;
                            double sgsttaxAmountOnMR = Math.Round((millamount * sgstrate / 100), 2);
                            //double sgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + sgsttaxAmountOnMR) * mill_rate)), 2);
                            //double sgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - sgstExMillRate), 2);
                            SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);
                        }
                        else
                        {
                            IGSTRateForPS = igstrate;
                            double igsttaxAmountOnMR = ((millrate * quintal) * igstrate / 100);
                            //double igstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + igsttaxAmountOnMR) * mill_rate)), 2);
                            //double igstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - igstExMillRate), 2);
                            IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
                        }

                        double TotalGstPurchaseAmount = Math.Round((quintal * millrate) + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS, 2);
                        double SUBTOTAL = 0.00;
                        SUBTOTAL = quintal * millrate;

                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                        {
                            clsGledgerupdations gleder = new clsGledgerupdations();

                            string strRev = "";
                            obj.flag = 2;
                            obj.tableName = tblPrefix + "SugarPurchase";
                            obj.columnNm = "SUBTOTAL='" + SUBTOTAL + "',BILL_AMOUNT='" + TotalGstPurchaseAmount + "'," +
                                            " CGSTRate='" + CGSTRateForPS + "',CGSTAmount='" + CGSTAmountForPS + "',SGSTRate='" + SGSTRateForPS + "',SGSTAmount='" + SGSTAmountForPS + "',IGSTRate='" + IGSTRateForPS + "',IGSTAmount='" + IGSTAmountForPS + "'" +
                                            " where Tran_Type='PS' and doc_no=" + PS_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                            obj.values = "none";
                            ds = new DataSet();
                            ds = obj.insertAccountMaster(ref strRev);

                            gleder.SugarPurchaseGledgerEffectForGST("PS", PS_No, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                        }
                    }

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aasdas", "javascript:alert('Success!')", true);
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aaasdsadsdas", "javascript:alert('" + ex.ToString() + "!')", true);
        }
    }

    protected void btnRoundOFFSB_Click(object sender, EventArgs e)
    {
        try
        {
            string strRef = "";
            string Tran_Type = "SB";


            string Roundoff_ac = clsCommon.getString("select RoundOff from NT_1_CompanyParameters where Company_Code=" +
               Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));



            qry = "delete from " + tblPrefix + "GLEDGER where Ac_Code=" + Roundoff_ac + " and TRAN_TYPE='"
              + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);



            qry = "";
            qry = "Select * from CHECKMYDIFF where diff <> 0 and [TRAN_TYPE] ='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsupdate = new DataSet();
            dsupdate = clsDAL.SimpleQuery(qry);

            if (dsupdate.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < dsupdate.Tables[0].Rows.Count; i++)
                {
                    string PSNo = dsupdate.Tables[0].Rows[i]["DOC_NO"].ToString();
                    // string MyDoc_Date = dsupdate.Tables[0].Rows[i]["DOC_DATE"].ToString();
                    string MyDoc_Date = DateTime.Parse(dsupdate.Tables[0].Rows[i]["DOC_DATE"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    double Bill_Amount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["diff"]));

                    //  string drcr=if(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["diff"]) > 0 ,"C","D");

                    string drcr = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["diff"]) > 0 ? "C" : "D";
                    string companycode = dsupdate.Tables[0].Rows[i]["Company_Code"].ToString();
                    string yearcode = dsupdate.Tables[0].Rows[i]["Year_Code"].ToString();


                    int MyOrderCode = 8;


                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        //Credit Effect for Party
                        string rev = "";





                        obj.flag = 1;
                        obj.tableName = tblPrefix + "GLEDGER";
                        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,Branch_Code,ORDER_CODE," +
                            "DRCR,DRCR_HEAD,SORT_TYPE,SORT_NO";

                        obj.values = "'" + Tran_Type + "','" + PSNo + "','" + MyDoc_Date + "','" + Roundoff_ac + "','','"
                            + Bill_Amount + "','" + companycode + "','" + yearcode + "','1','" + MyOrderCode + "','" + drcr + "','" + Roundoff_ac
                            + "','" + Tran_Type + "','" + PSNo + "'";


                        ds = obj.insertAccountMaster(ref strRef);
                        rev = strRef;

                        obj.flag = 2;
                        obj.tableName = tblPrefix + "SugarSale";
                        obj.columnNm = "RoundOff='" + Convert.ToDouble(dsupdate.Tables[0].Rows[i]["diff"]) + "'  where doc_no=" + PSNo + " and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref strRef);
                        rev = strRef;




                    }

                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


}