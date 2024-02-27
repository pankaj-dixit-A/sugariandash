using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
public partial class pgeEwayBill : System.Web.UI.Page
{
    #region data section
    string qry = string.Empty;
    string qry1 = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    string Trans_Date;
    string CGST_Rate;
    string SGST_Rate;
    string IGST_Rate;
    string Dono;
    string NewSBNumber;
    string State_Code_BillTo;
    string State_Code_Mill;
    int Rowaction = 2;
    int Srno = 3;
    string doc_no = string.Empty;
    string selectedyear = string.Empty;
    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        Dono = Request.QueryString["dono"];
        NewSBNumber = Request.QueryString["SBNo"];
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "EwayBill";
            tblDetails = "";
            qryCommon = "EwayBill";
            user = Session["user"].ToString();
            pnlPopup.Style["display"] = "none";
            selectedyear = Session["selectedyear"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
                if (Company_Code == 1)
                {
                    if (isAuthenticate == "1" || User_Type == "A")
                    {
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        BindEwayBill();
                        //this.showLastRecord();
                    }
                    else
                    {
                        Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                    }
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
                if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
                {
                    pnlPopup.Style["display"] = "none";
                }
                else
                {
                    pnlPopup.Style["display"] = "block";
                    objAsp = btnSearch;
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where ";
                obj.code = "Doc_no";
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
                                    txtDoc_No.Text = ds.Tables[0].Rows[0][0].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion


    private void BindEwayBill()
    {
        try
        {


            //string qryelement = "select doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
            //         + " UPPER(BillToName) as BillToName,UPPER(BillToGst) as BillToGst ,UPPER(Ac_Name_E) as ShippTo,UPPER(Address_E) as Address_E," +
            //         "UPPER(city_name_e) as city_name_e,(case Pincode when 0 then 999999  else pincode end) as pincode,upper(BillToStateCode) AS BillToStateCode," +
            //         " State_Name as State_Name,NETQNTL,TaxableAmount,"
            //         + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0'+'+'+'0') as Taxrate,CGSTAmount,"
            //         + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,DO_No,millstatename as millstatename,CGSTRate,SGSTRate,IGSTRate,state_code_billto,millstatecode "
            //         + " from NT_1_qryNameEwayBillJK where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
            //         + " and DO_No=" + Dono + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            string qryelement = "select  newsbno as doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
                 + " UPPER(BillToName) as BillToName,UPPER(BillToGst) as BillToGst ,UPPER(Ac_Name_E) as ShippTo,UPPER(Address_E) as Address_E," +
                 "UPPER(city_name_e) as city_name_e,(case Pincode when 0 then 999999  else pincode end) as pincode,upper(BillToStateCode) AS BillToStateCode," +
                 " State_Name as State_Name,NETQNTL,TaxableAmount,"
                 + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0'+'+'+'0') as Taxrate,CGSTAmount,"
                 + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,upper(milladdress) as milladdress,(case millpincode when 0 then 999999  else millpincode end) as millpincode," +
                 " millcityname,DO_No,millstatename as millstatename,convert(varchar,TransDate,103)as TransDate,CGSTRate,SGSTRate,IGSTRate,state_code_billto,millstatecode,grade,GSTStateCode,mill_code,Unit_Code "
                 + " from NT_1_qryNameEwayBill_Hinglaj where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                 + " and DO_No=" + Dono + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            #region[from query]
            string qrynm = clsCommon.getString("select UPPER(Company_Name_E) as Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            
            string comnm = qrynm.ToUpper();
           
            string gstno = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string address = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string city = clsCommon.getString("select UPPER(City_E) as City_E  from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string pin = clsCommon.getString("select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("select LOWER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string DODate = clsCommon.getString("select doc_date from NT_1_deliveryorder ");
            #endregion

            double taxamount;
            string taxvalue;
            double CGST;
            double SGST;
            double IGST;
            double CessAmt = 0.00;
            double CessNontAdvol = 0.00;
            double Other = 0.00;
            string Bill_To;
            string Ship_To;
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qryelement);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = null;
                        drpSupply_Type.SelectedValue = "0";
                        drpSub_Type.SelectedValue = "1";
                        drpTran_Type.SelectedValue = "INV";

                        doc_no = dt.Rows[0]["Doc_No"].ToString();
                        txtDoc_No.Text = selectedyear + '-' + doc_no;
                        txtDoc_Date.Text = dt.Rows[0]["doc_date"].ToString();
                        Bill_To = dt.Rows[0]["BillToName"].ToString();
                        Ship_To = dt.Rows[0]["ShippTo"].ToString();
                        if (Bill_To != Ship_To)
                        {
                            drpTran_Type.SelectedValue = "4";

                        }
                        else
                        {
                            drpTran_Type.SelectedValue = "3";

                        }
                        txtBill_From_Name.Text = comnm;
                        txtBill_From_Address.Text = dt.Rows[0]["millname"].ToString();
                        //txtBill_From_Address.Text = address;
                        txtBill_From_Address2.Text = dt.Rows[0]["milladdress"].ToString();
                        txtBill_FromGST_No.Text = gstno;
                        txtBill_From_Place.Text = dt.Rows[0]["millcityname"].ToString();
                        txtBill_From_State.Text = state;
                        txtBill_From_PinCode.Text = dt.Rows[0]["millpincode"].ToString();
                        txtBill_From_State2.Text = dt.Rows[0]["millstatename"].ToString();
                        txtBill_To_Name.Text = dt.Rows[0]["BillToName"].ToString();
                        txtBill_To_Add.Text = dt.Rows[0]["ShippTo"].ToString();
                        txtBill_To_Add2.Text = dt.Rows[0]["Address_E"].ToString();
                        txtBill_TO_GSTNo.Text = dt.Rows[0]["BillToGst"].ToString();
                        txtBill_To_Place.Text = dt.Rows[0]["city_name_e"].ToString();
                        txtBill_To_State.Text = dt.Rows[0]["State_Name"].ToString();
                        txtBill_To_PinCode.Text = dt.Rows[0]["pincode"].ToString();
                        txtItem_Name.Text = "SUGAR";
                        txtItem_Description.Text = dt.Rows[0]["grade"].ToString();
                        txtHSN.Text = "1701";
                        txtQty.Text = dt.Rows[0]["NETQNTL"].ToString();
                        txtUnit.Text = "QTL";
                        txtTaxable_Value.Text = dt.Rows[0]["TaxableAmount"].ToString();


                        //if(CGST!='')
                        //drpCGST_SGST_Rate.SelectedValue = dt.Rows[0]["CGST_SGST_Rate"].ToString();
                        //drpIGST_Rate.SelectedValue = dt.Rows[0]["IGST_Rate"].ToString();
                        //drpCESS_Advol_Rate.SelectedValue = dt.Rows[0]["CESS_Advol_Rate"].ToString();
                        //drpCESS_NonAdvol_Rate.SelectedValue = dt.Rows[0]["CESS_NonAdvol_Rate"].ToString();
                        txtTaxable_Amt.Text = dt.Rows[0]["TaxableAmount"].ToString();
                        txtCGST_Amt.Text = dt.Rows[0]["CGSTAmount"].ToString();
                        txtSGST_Amt.Text = dt.Rows[0]["SGSTAmount"].ToString();
                        txtIGST_Amt.Text = dt.Rows[0]["IGSTAmount"].ToString();
                        txtCESS_Advol_Amt.Text = "0";
                        txtCESS_non_Advol_Amt.Text = "0.00";
                        txtOther_Amt.Text = "0.00";

                        CGST = Convert.ToDouble(dt.Rows[0]["CGSTAmount"].ToString());
                        SGST = Convert.ToDouble(dt.Rows[0]["SGSTAmount"].ToString());
                        IGST = Convert.ToDouble(dt.Rows[0]["IGSTAmount"].ToString());
                        taxamount = Convert.ToDouble(dt.Rows[0]["TaxableAmount"].ToString());
                        double TotalInvoice = taxamount + CGST + SGST + IGST + CessAmt + CessNontAdvol + Other;
                        txtTotal_Bill_Amt.Text = TotalInvoice.ToString();

                        txtTransporter_Name.Text = "";
                        txtTransporter_ID.Text = "";
                        txtApproximate_Distance.Text = dt.Rows[0]["Distance"].ToString();
                        drpTrance_Mode.SelectedValue = "1";
                        drpVehicle_Type.SelectedValue = "R";
                        txtVehicle_No.Text = dt.Rows[0]["LORRYNO"].ToString();
                        //Trans_Date = dt.Rows[0]["TransDate"].ToString(); doc_date
                        Trans_Date = dt.Rows[0]["doc_date"].ToString();

                        hdnfCGST_Rate.Value = dt.Rows[0]["CGSTRate"].ToString();
                        hdnfSGST_Rate.Value = dt.Rows[0]["SGSTRate"].ToString();
                        hdnfIGST_Rate.Value = dt.Rows[0]["IGSTRate"].ToString();
                        hdnfState_Code_BillTo.Value = dt.Rows[0]["state_code_billto"].ToString();
                        hdnfMillState_Code.Value = dt.Rows[0]["millstatecode"].ToString();
                        hdnfState_Code.Value = dt.Rows[0]["GSTStateCode"].ToString();
                        hdnfmillCode.Value = dt.Rows[0]["mill_code"].ToString();
                        hdnfUnitCode.Value = dt.Rows[0]["Unit_Code"].ToString();
                        ViewState["currentTable"] = dt;



                    }
                    else
                    {

                        ViewState["currentTable"] = null;
                    }
                }
                else
                {

                    ViewState["currentTable"] = null;
                }
            }
            else
            {

                ViewState["currentTable"] = null;
            }
        }
        catch
        {
        }
    }
    #region [makeEmptyForm]
    private void makeEmptyForm(string dAction)
    {
        try
        {
            if (dAction == "N")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }
                }
                pnlPopup.Style["display"] = "none";
                //btnAdddetails.Enabled = false;
                //btnClosedetails.Enabled = false;
                ViewState["currentTable"] = null;
                btnSave.Text = "Save";
                btntxtDoc_No.Text = "Choose No";
                btntxtDoc_No.Enabled = false;
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;
                drpDoc_Type.Enabled = false;
                txtDoc_Date.Enabled = false;
                CalendarExtenderDatetxtDoc_Date.Enabled = false;
                drpTran_Type.Enabled = false;
                drpSupply_Type.Enabled = false;
                drpSub_Type.Enabled = false;
                txtBill_From_Name.Enabled = false;
                txtBill_From_Address.Enabled = false;
                txtBill_From_Address2.Enabled = false;
                txtBill_FromGST_No.Enabled = false;
                txtBill_From_Place.Enabled = false;
                txtBill_From_State.Enabled = false;
                txtBill_From_State2.Enabled = false;
                txtBill_From_PinCode.Enabled = false;
                txtBill_To_Name.Enabled = false;
                txtBill_To_Add.Enabled = false;
                txtBill_To_Add2.Enabled = false;
                txtBill_TO_GSTNo.Enabled = false;
                txtBill_To_Place.Enabled = false;
                txtBill_To_State.Enabled = false;
                txtBill_To_PinCode.Enabled = false;
                txtItem_Name.Enabled = false;
                txtItem_Description.Enabled = false;
                txtHSN.Enabled = false;
                txtQty.Enabled = false;
                txtUnit.Enabled = false;
                txtTaxable_Value.Enabled = false;
                drpCGST_SGST_Rate.Enabled = false;
                drpIGST_Rate.Enabled = false;
                drpCESS_Advol_Rate.Enabled = false;
                drpCESS_NonAdvol_Rate.Enabled = false;
                txtTaxable_Amt.Enabled = false;
                txtCGST_Amt.Enabled = false;
                txtSGST_Amt.Enabled = false;
                txtIGST_Amt.Enabled = false;
                txtCESS_Advol_Amt.Enabled = false;
                txtCESS_non_Advol_Amt.Enabled = false;
                txtOther_Amt.Enabled = false;
                txtTotal_Bill_Amt.Enabled = false;
                txtTransporter_Name.Enabled = false;
                txtTransporter_ID.Enabled = false;
                txtApproximate_Distance.Enabled = false;
                drpTrance_Mode.Enabled = false;
                drpVehicle_Type.Enabled = false;
                txtVehicle_No.Enabled = false;
            }
            if (dAction == "A")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btnSave.Text = "Save";
                btntxtDoc_No.Text = "Change No";
                btntxtDoc_No.Enabled = true;
                txtEditDoc_No.Enabled = false;
                txtDoc_No.Enabled = false;
                //btnAdddetails.Enabled = true;
                //btnClosedetails.Enabled = true;
                ViewState["currentTable"] = null;
                drpDoc_Type.Enabled = true;
                txtDoc_Date.Enabled = true;
                txtDoc_Date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                CalendarExtenderDatetxtDoc_Date.Enabled = true;
                drpTran_Type.Enabled = true;
                drpSupply_Type.Enabled = true;
                drpSub_Type.Enabled = true;
                txtBill_From_Name.Enabled = true;
                txtBill_From_Address.Enabled = true;
                txtBill_From_Address2.Enabled = true;
                txtBill_FromGST_No.Enabled = true;
                txtBill_From_Place.Enabled = true;
                txtBill_From_State.Enabled = true;
                txtBill_From_State2.Enabled = true;
                txtBill_From_PinCode.Enabled = true;
                txtBill_To_Name.Enabled = true;
                txtBill_To_Add.Enabled = true;
                txtBill_To_Add2.Enabled = true;
                txtBill_TO_GSTNo.Enabled = true;
                txtBill_To_Place.Enabled = true;
                txtBill_To_State.Enabled = true;
                txtBill_To_PinCode.Enabled = true;
                txtItem_Name.Enabled = true;
                txtItem_Description.Enabled = true;
                txtHSN.Enabled = true;
                txtQty.Enabled = true;
                txtUnit.Enabled = true;
                txtTaxable_Value.Enabled = true;
                drpCGST_SGST_Rate.Enabled = true;
                drpIGST_Rate.Enabled = true;
                drpCESS_Advol_Rate.Enabled = true;
                drpCESS_NonAdvol_Rate.Enabled = true;
                txtTaxable_Amt.Enabled = true;
                txtCGST_Amt.Enabled = true;
                txtSGST_Amt.Enabled = true;
                txtIGST_Amt.Enabled = true;
                txtCESS_Advol_Amt.Enabled = true;
                txtCESS_non_Advol_Amt.Enabled = true;
                txtOther_Amt.Enabled = true;
                txtTotal_Bill_Amt.Enabled = true;
                txtTransporter_Name.Enabled = true;
                txtTransporter_ID.Enabled = true;
                txtApproximate_Distance.Enabled = true;
                drpTrance_Mode.Enabled = true;
                drpVehicle_Type.Enabled = true;
                txtVehicle_No.Enabled = true;
                #region set Business logic for save
                #endregion
            }
            if (dAction == "S")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                btntxtDoc_No.Text = "Choose No";
                btntxtDoc_No.Enabled = false;
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;
                drpDoc_Type.Enabled = false;
                txtDoc_Date.Enabled = false;
                CalendarExtenderDatetxtDoc_Date.Enabled = false;
                drpTran_Type.Enabled = false;
                drpSupply_Type.Enabled = false;
                drpSub_Type.Enabled = false;
                txtBill_From_Name.Enabled = false;
                txtBill_From_Address.Enabled = false;
                txtBill_From_Address2.Enabled = false;
                txtBill_FromGST_No.Enabled = false;
                txtBill_From_Place.Enabled = false;
                txtBill_From_State.Enabled = false;
                txtBill_From_State2.Enabled = false;
                txtBill_From_PinCode.Enabled = false;
                txtBill_To_Name.Enabled = false;
                txtBill_To_Add.Enabled = false;
                txtBill_To_Add2.Enabled = false;
                txtBill_TO_GSTNo.Enabled = false;
                txtBill_To_Place.Enabled = false;
                txtBill_To_State.Enabled = false;
                txtBill_To_PinCode.Enabled = false;
                txtItem_Name.Enabled = false;
                txtItem_Description.Enabled = false;
                txtHSN.Enabled = false;
                txtQty.Enabled = false;
                txtUnit.Enabled = false;
                txtTaxable_Value.Enabled = false;
                drpCGST_SGST_Rate.Enabled = false;
                drpIGST_Rate.Enabled = false;
                drpCESS_Advol_Rate.Enabled = false;
                drpCESS_NonAdvol_Rate.Enabled = false;
                txtTaxable_Amt.Enabled = false;
                txtCGST_Amt.Enabled = false;
                txtSGST_Amt.Enabled = false;
                txtIGST_Amt.Enabled = false;
                txtCESS_Advol_Amt.Enabled = false;
                txtCESS_non_Advol_Amt.Enabled = false;
                txtOther_Amt.Enabled = false;
                txtTotal_Bill_Amt.Enabled = false;
                txtTransporter_Name.Enabled = false;
                txtTransporter_ID.Enabled = false;
                txtApproximate_Distance.Enabled = false;
                drpTrance_Mode.Enabled = false;
                drpVehicle_Type.Enabled = false;
                txtVehicle_No.Enabled = false;
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btntxtDoc_No.Text = "Choose No";
                btntxtDoc_No.Enabled = true;
                txtEditDoc_No.Enabled = false;
                lblMsg.Text = string.Empty;
                drpDoc_Type.Enabled = true;
                txtDoc_Date.Enabled = true;
                CalendarExtenderDatetxtDoc_Date.Enabled = true;
                drpTran_Type.Enabled = true;
                drpSupply_Type.Enabled = true;
                drpSub_Type.Enabled = true;
                txtBill_From_Name.Enabled = true;
                txtBill_From_Address.Enabled = true;
                txtBill_From_Address2.Enabled = true;
                txtBill_FromGST_No.Enabled = true;
                txtBill_From_Place.Enabled = true;
                txtBill_From_State.Enabled = true;
                txtBill_From_State2.Enabled = true;
                txtBill_From_PinCode.Enabled = true;
                txtBill_To_Name.Enabled = true;
                txtBill_To_Add.Enabled = true;
                txtBill_To_Add2.Enabled = true;
                txtBill_TO_GSTNo.Enabled = true;
                txtBill_To_Place.Enabled = true;
                txtBill_To_State.Enabled = true;
                txtBill_To_PinCode.Enabled = true;
                txtItem_Name.Enabled = true;
                txtItem_Description.Enabled = true;
                txtHSN.Enabled = true;
                txtQty.Enabled = true;
                txtUnit.Enabled = true;
                txtTaxable_Value.Enabled = true;
                drpCGST_SGST_Rate.Enabled = true;
                drpIGST_Rate.Enabled = true;
                drpCESS_Advol_Rate.Enabled = true;
                drpCESS_NonAdvol_Rate.Enabled = true;
                txtTaxable_Amt.Enabled = true;
                txtCGST_Amt.Enabled = true;
                txtSGST_Amt.Enabled = true;
                txtIGST_Amt.Enabled = true;
                txtCESS_Advol_Amt.Enabled = true;
                txtCESS_non_Advol_Amt.Enabled = true;
                txtOther_Amt.Enabled = true;
                txtTotal_Bill_Amt.Enabled = true;
                txtTransporter_Name.Enabled = true;
                txtTransporter_ID.Enabled = true;
                txtApproximate_Distance.Enabled = true;
                drpTrance_Mode.Enabled = true;
                drpVehicle_Type.Enabled = true;
                txtVehicle_No.Enabled = true;
            }
            #region Always check this
            #endregion
        }
        catch
        {
        }
    }
    #endregion
    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select max(Doc_No) as Doc_No from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                        hdnf.Value = dt.Rows[0]["Doc_No"].ToString();
                        qry = getDisplayQuery();
                        bool recordExist = this.fetchRecord(qry);
                        if (recordExist == true)
                        {
                            btnEdit.Focus();
                        }
                        else                     //new code
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        //   int RecordCount = 0;
        //   string query = "";
        //   query = "select count(*) from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString());
        //string cnt = clsCommon.getString(query); 
        //  if (cnt != string.Empty) 
        //       {
        //RecordCount = Convert.ToInt32(cnt);
        //       }
        //   if (RecordCount != 0 && RecordCount == 1)
        //   {
        //       btnFirst.Enabled = true;
        //       btnPrevious.Enabled = false;
        //       btnNext.Enabled = false;
        //       btnLast.Enabled = false;
        //   }
        //   else if (RecordCount != 0 && RecordCount > 1)
        //   {
        //       btnFirst.Enabled = true;
        //       btnPrevious.Enabled = false;
        //       btnNext.Enabled = false;
        //       btnLast.Enabled = true;
        //   }
        //   if (txtDoc_No.Text != string.Empty)
        //   {
        //       #region check for next or previous record exist or not
        //       query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString())+ "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString())+" ORDER BY Doc_No asc  ";
        //       string strDoc_No = clsCommon.getString(query);
        //        if (strDoc_No != string.Empty)
        //        {
        //         btnNext.Enabled = true;
        //         btnLast.Enabled = true;
        //        }
        //       else
        //        {
        //         btnNext.Enabled = false;
        //         btnLast.Enabled = false;
        //        }
        //       query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString())+ "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString())+" ORDER BY Doc_No desc  ";
        //        if (strDoc_No != string.Empty)
        //        {
        //         btnPrevious.Enabled = true;
        //         btnFirst.Enabled = true;
        //        }
        //       else
        //        {
        //         btnPrevious.Enabled = false;
        //         btnFirst.Enabled = false;
        //        }
        //   }
        //       #endregion
        #endregion

        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
            + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(query);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
        }
        if (RecordCount != 0 && RecordCount == 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
        else if (RecordCount != 0 && RecordCount > 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = true;
        }
        if (txtDoc_No.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnNext.Enabled = true;
                            btnLast.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnNext.Enabled = false;
                            btnLast.Enabled = false;
                        }
                    }
                }
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value)
                    + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnPrevious.Enabled = true;
                            btnFirst.Enabled = true;
                        }
                        else
                        {
                            btnPrevious.Enabled = false;
                            btnFirst.Enabled = false;
                        }
                    }
                }

                #endregion
            }

        }
        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select Doc_No from " + tblHead + " where Doc_No=(select MIN(Doc_No) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion
    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDoc_No.Text != string.Empty)
            {
                string query = "";
                query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No< " + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Doc_No desc  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion
    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDoc_No.Text != string.Empty)
            {
                string query = "";
                query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No> " + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Doc_No asc  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion
    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select Doc_No from " + tblHead + " where Doc_No=(select MAX(Doc_No) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        setFocusControl(txtDoc_No);
        Int32 Doc_No = Convert.ToInt32(clsCommon.getString("select IDENT_CURRENT('" + tblHead + "') as Doc_No"));
        if (Doc_No != 0)
        {
            int doc_no = Doc_No + 1;
            Doc_No = doc_no;
        }
        else
        {
            Doc_No = 1;
        }
        txtDoc_No.Text = Convert.ToString(Doc_No);
        setFocusControl(drpSupply_Type);
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        this.makeEmptyForm("E");
        txtDoc_No.Enabled = false;
        setFocusControl(txtDoc_No);
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string str = string.Empty;
                DataSet xml_ds = new DataSet();
                if (str == string.Empty)
                {
                    string currentDoc_No = txtDoc_No.Text;
                    DataSet ds = new DataSet();
                    string strrev = "";
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        XElement root = new XElement("ROOT");
                        XElement child1 = new XElement("Head");
                        int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
                        int Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
                        string strRev = string.Empty;
                        child1.SetAttributeValue("Doc_No", txtDoc_No.Text);
                        child1.SetAttributeValue("Company_Code", Company_Code);
                        child1.SetAttributeValue("Year_Code", Year_Code);
                        child1.SetAttributeValue("Tran_Type", "");
                        root.Add(child1);
                        string XMLReport = root.ToString();
                        XDocument xDoc = XDocument.Parse(XMLReport, LoadOptions.None);
                        XMLReport = xDoc.ToString(SaveOptions.DisableFormatting);
                        string spname = "SP_EwayBill";
                        string xmlfile = XMLReport;
                        string op = "";
                        string returnmaxno = "";
                        int flag = 10;
                        xml_ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
                        strrev = op;





                    }
                    string query = "";
                    if (strrev == "-3")
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No>" + Convert.ToInt32(currentDoc_No) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ORDER BY Doc_No asc  ";
                        hdnf.Value = clsCommon.getString(query);
                        if (hdnf.Value == string.Empty)
                        {
                            query = "SELECT top 1 [Doc_No] from " + tblHead + " where Doc_No<" + Convert.ToInt32(currentDoc_No) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ORDER BY Doc_No desc  ";
                            hdnf.Value = clsCommon.getString(query);
                        }
                        if (hdnf.Value != string.Empty)
                        {
                            query = getDisplayQuery();
                            bool recordExist = this.fetchRecord(query);
                            this.makeEmptyForm("S");
                            clsButtonNavigation.enableDisable("S");
                        }
                        else
                        {
                            this.makeEmptyForm("N");
                            clsButtonNavigation.enableDisable("N");
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                    this.enableDisableNavigateButtons();
                }
                else
                {
                    lblMsg.Text = "Cannot delete this Group , it is in use";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (hdnf.Value != string.Empty)
        {
            string query = getDisplayQuery();
            bool recordExist = this.fetchRecord(query);
        }
        else
        {
            this.showLastRecord();
        }
        string qry = clsCommon.getString("select count(Doc_No) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
        if (qry != "0")
        {
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            this.enableDisableNavigateButtons();
        }
        else
        {
            clsButtonNavigation.enableDisable("N");
            this.makeEmptyForm("N");
            this.enableDisableNavigateButtons();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    #endregion
    #region [fetchrecord]
    private bool fetchRecord(string qry)
    {
        try
        {
            bool recordExist = false;
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
                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                        if (lblModified != null)
                        {
                            lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        }
                        Label lblCreatedDate = (Label)Master.FindControl("MasterlblCreatedDate");
                        Label lblModifiedDate = (Label)Master.FindControl("MasterlblModifiedDate");
                        if (lblCreatedDate != null)
                        {
                            if (dt.Rows[0]["Created_Date"].ToString() == string.Empty)
                            {
                                lblCreatedDate.Text = "";
                            }
                            else
                            {
                                lblCreatedDate.Text = "Created Date" + dt.Rows[0]["Created_Date"].ToString();
                            }
                        }
                        if (lblModifiedDate != null)
                        {
                            if (dt.Rows[0]["Modified_Date"].ToString() == string.Empty)
                            {
                                lblModifiedDate.Text = "";
                            }
                            else
                            {
                                lblModifiedDate.Text = "Modified Date" + dt.Rows[0]["Modified_Date"].ToString();
                            }
                        }

                        string doc_no = "shubhag" + dt.Rows[0]["Doc_No"].ToString();
                        txtDoc_No.Text = dt.Rows[0]["Doc_No"].ToString();

                        drpDoc_Type.SelectedValue = dt.Rows[0]["Doc_Type"].ToString();
                        txtDoc_Date.Text = dt.Rows[0]["Doc_Date"].ToString();
                        drpTran_Type.SelectedValue = dt.Rows[0]["Tran_Type"].ToString();
                        drpSupply_Type.SelectedValue = dt.Rows[0]["Supply_Type"].ToString();
                        drpSub_Type.SelectedValue = dt.Rows[0]["Sub_Type"].ToString();
                        txtBill_From_Name.Text = dt.Rows[0]["Bill_From_Name"].ToString();
                        txtBill_From_Address.Text = dt.Rows[0]["Bill_From_Address"].ToString();
                        txtBill_FromGST_No.Text = dt.Rows[0]["Bill_FromGST_No"].ToString();
                        txtBill_From_Place.Text = dt.Rows[0]["Bill_From_Place"].ToString();
                        txtBill_From_State.Text = dt.Rows[0]["Bill_From_State"].ToString();
                        txtBill_From_PinCode.Text = dt.Rows[0]["Bill_From_PinCode"].ToString();
                        txtBill_To_Name.Text = dt.Rows[0]["Bill_To_Name"].ToString();
                        txtBill_To_Add.Text = dt.Rows[0]["Bill_To_Add"].ToString();
                        txtBill_TO_GSTNo.Text = dt.Rows[0]["Bill_TO_GSTNo"].ToString();
                        txtBill_To_Place.Text = dt.Rows[0]["Bill_To_Place"].ToString();
                        txtBill_To_State.Text = dt.Rows[0]["Bill_To_State"].ToString();
                        txtBill_To_PinCode.Text = dt.Rows[0]["Bill_To_PinCode"].ToString();
                        txtItem_Name.Text = dt.Rows[0]["Item_Name"].ToString();
                        txtItem_Description.Text = dt.Rows[0]["Item_Description"].ToString();
                        txtHSN.Text = dt.Rows[0]["HSN"].ToString();
                        txtQty.Text = dt.Rows[0]["Qty"].ToString();
                        txtUnit.Text = dt.Rows[0]["Unit"].ToString();
                        txtTaxable_Value.Text = dt.Rows[0]["Taxable_Value"].ToString();
                        drpCGST_SGST_Rate.SelectedValue = dt.Rows[0]["CGST_SGST_Rate"].ToString();
                        drpIGST_Rate.SelectedValue = dt.Rows[0]["IGST_Rate"].ToString();
                        drpCESS_Advol_Rate.SelectedValue = dt.Rows[0]["CESS_Advol_Rate"].ToString();
                        drpCESS_NonAdvol_Rate.SelectedValue = dt.Rows[0]["CESS_NonAdvol_Rate"].ToString();
                        txtTaxable_Amt.Text = dt.Rows[0]["Taxable_Amt"].ToString();
                        txtCGST_Amt.Text = dt.Rows[0]["CGST_Amt"].ToString();
                        txtSGST_Amt.Text = dt.Rows[0]["SGST_Amt"].ToString();
                        txtIGST_Amt.Text = dt.Rows[0]["IGST_Amt"].ToString();
                        txtCESS_Advol_Amt.Text = dt.Rows[0]["CESS_Advol_Amt"].ToString();
                        txtCESS_non_Advol_Amt.Text = dt.Rows[0]["CESS_non_Advol_Amt"].ToString();
                        txtOther_Amt.Text = dt.Rows[0]["Other_Amt"].ToString();
                        txtTotal_Bill_Amt.Text = dt.Rows[0]["Total_Bill_Amt"].ToString();
                        txtTransporter_Name.Text = dt.Rows[0]["Transporter_Name"].ToString();
                        txtTransporter_ID.Text = dt.Rows[0]["Transporter_ID"].ToString();
                        txtApproximate_Distance.Text = dt.Rows[0]["Approximate_Distance"].ToString();
                        drpTrance_Mode.SelectedValue = dt.Rows[0]["Trance_Mode"].ToString();
                        drpVehicle_Type.SelectedValue = dt.Rows[0]["Vehicle_Type"].ToString();
                        txtVehicle_No.Text = dt.Rows[0]["Vehicle_No"].ToString();

                        recordExist = true;
                        lblMsg.Text = "";
                    }
                }
            }
            hdnf.Value = txtDoc_No.Text;
            this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion
    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtEditDoc_No")
            {
                setFocusControl(txtEditDoc_No);
            }
            if (strTextBox == "txtDoc_No")
            {
                setFocusControl(txtDoc_No);
            }
            if (strTextBox == "drpDoc_Type")
            {
                setFocusControl(drpDoc_Type);
            }
            if (strTextBox == "txtDoc_Date")
            {
                try
                {
                    string dt = DateTime.Parse(txtDoc_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    //if (clsCommon.isValidDateforOp(dt) == true || dt == "")
                    if (dt == "")
                    {
                        setFocusControl(txtDoc_Date);
                    }
                    else
                    {
                        txtDoc_Date.Text = "";
                        setFocusControl(txtDoc_Date);
                    }
                }
                catch
                {
                    txtDoc_Date.Text = "";
                    setFocusControl(txtDoc_Date);
                }
            }
            if (strTextBox == "drpTran_Type")
            {
                setFocusControl(drpTran_Type);
            }
            if (strTextBox == "drpSupply_Type")
            {
                setFocusControl(drpSupply_Type);
            }
            if (strTextBox == "drpSub_Type")
            {
                setFocusControl(drpSub_Type);
            }
            if (strTextBox == "txtBill_From_Name")
            {
                setFocusControl(txtBill_From_Name);
            }
            if (strTextBox == "txtBill_From_Address")
            {
                setFocusControl(txtBill_From_Address);
            }
            if (strTextBox == "txtBill_FromGST_No")
            {
                setFocusControl(txtBill_FromGST_No);
            }
            if (strTextBox == "txtBill_From_Place")
            {
                setFocusControl(txtBill_From_Place);
            }
            if (strTextBox == "txtBill_From_State")
            {
                setFocusControl(txtBill_From_State);
            }
            if (strTextBox == "txtBill_From_PinCode")
            {
                setFocusControl(txtBill_From_PinCode);
            }
            if (strTextBox == "txtBill_To_Name")
            {
                setFocusControl(txtBill_To_Name);
            }
            if (strTextBox == "txtBill_To_Add")
            {
                setFocusControl(txtBill_To_Add);
            }
            if (strTextBox == "txtBill_TO_GSTNo")
            {
                setFocusControl(txtBill_TO_GSTNo);
            }
            if (strTextBox == "txtBill_To_Place")
            {
                setFocusControl(txtBill_To_Place);
            }
            if (strTextBox == "txtBill_To_State")
            {
                setFocusControl(txtBill_To_State);
            }
            if (strTextBox == "txtBill_To_PinCode")
            {
                setFocusControl(txtBill_To_PinCode);
            }
            if (strTextBox == "txtItem_Name")
            {
                setFocusControl(txtItem_Name);
            }
            if (strTextBox == "txtItem_Description")
            {
                setFocusControl(txtItem_Description);
            }
            if (strTextBox == "txtHSN")
            {
                setFocusControl(txtHSN);
            }
            if (strTextBox == "txtQty")
            {
                setFocusControl(txtQty);
            }
            if (strTextBox == "txtUnit")
            {
                setFocusControl(txtUnit);
            }
            if (strTextBox == "txtTaxable_Value")
            {
                setFocusControl(txtTaxable_Value);
            }
            if (strTextBox == "drpCGST_SGST_Rate")
            {
                setFocusControl(drpCGST_SGST_Rate);
            }
            if (strTextBox == "drpIGST_Rate")
            {
                setFocusControl(drpIGST_Rate);
            }
            if (strTextBox == "drpCESS_Advol_Rate")
            {
                setFocusControl(drpCESS_Advol_Rate);
            }
            if (strTextBox == "drpCESS_NonAdvol_Rate")
            {
                setFocusControl(drpCESS_NonAdvol_Rate);
            }
            if (strTextBox == "txtTaxable_Amt")
            {
                setFocusControl(txtTaxable_Amt);
            }
            if (strTextBox == "txtCGST_Amt")
            {
                setFocusControl(txtCGST_Amt);
            }
            if (strTextBox == "txtSGST_Amt")
            {
                setFocusControl(txtSGST_Amt);
            }
            if (strTextBox == "txtIGST_Amt")
            {
                setFocusControl(txtIGST_Amt);
            }
            if (strTextBox == "txtCESS_Advol_Amt")
            {
                setFocusControl(txtCESS_Advol_Amt);
            }
            if (strTextBox == "txtCESS_non_Advol_Amt")
            {
                setFocusControl(txtCESS_non_Advol_Amt);
            }
            if (strTextBox == "txtOther_Amt")
            {
                setFocusControl(txtOther_Amt);
            }
            if (strTextBox == "txtTotal_Bill_Amt")
            {
                setFocusControl(txtTotal_Bill_Amt);
            }
            if (strTextBox == "txtTransporter_Name")
            {
                setFocusControl(txtTransporter_Name);
            }
            if (strTextBox == "txtTransporter_ID")
            {
                setFocusControl(txtTransporter_ID);
            }
            if (strTextBox == "txtApproximate_Distance")
            {
                setFocusControl(txtApproximate_Distance);
            }
            if (strTextBox == "drpTrance_Mode")
            {
                setFocusControl(drpTrance_Mode);
            }
            if (strTextBox == "drpVehicle_Type")
            {
                setFocusControl(drpVehicle_Type);
            }
            if (strTextBox == "txtVehicle_No")
            {
                setFocusControl(txtVehicle_No);
            }

        }
        catch
        {
        }
    }
    #endregion
    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = " select * from " + qryCommon + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Doc_No=" + hdnf.Value + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion
    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                txtDoc_No.Text = hdnf.Value;
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }
                this.enableDisableNavigateButtons();
                this.makeEmptyForm("S");
            }
            else
            {
                showLastRecord();
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            e.Row.Cells[0].Width = new Unit("120px");
            e.Row.Cells[0].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[0].Style["overflow" ] = "hidden";
            //    e.Row.Cells[0].Visible =true;
        }
    }
    #endregion
    #region [grdPopup_PageIndexChanging]
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
    #endregion
    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
           e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region [txtEditDoc_No_TextChanged]
    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtEditDoc_No.Text;
        strTextBox = "txtEditDoc_No";
        csCalculations();
    }
    #endregion
    #region [txtDoc_No_TextChanged]
    protected void txtDoc_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_No.Text;
        strTextBox = "txtDoc_No";
        csCalculations();
    }
    #endregion
    #region [drpDoc_Type_TextChanged]
    protected void drpDoc_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [txtDoc_Date_TextChanged]
    protected void txtDoc_Date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_Date.Text;
        strTextBox = "txtDoc_Date";
        csCalculations();
    }
    #endregion
    #region [drpTran_Type_TextChanged]
    protected void drpTran_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpTran_Type.SelectedValue == "1")
        {
            string qrynm1 = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtBill_From_Address.Text = qrynm1.ToUpper();

            string city = clsCommon.getString("select UPPER(City_E) as City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtBill_From_Place.Text = city.ToUpper();

            string pincode = clsCommon.getString("select UPPER(PIN) as PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtBill_From_PinCode.Text = pincode;

            string state = clsCommon.getString("select UPPER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtBill_From_State2.Text = state.ToUpper();
        }
        else
        {
            string qryelement = "select UPPER(millname) as millname,upper(milladdress) as milladdress,(case millpincode when 0 then 999999  else millpincode end) as millpincode," +
               " millcityname,millstatename as millstatename,millstatecode "
               + " from NT_1_qryNameEwayBill_Hinglaj where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
               + " and DO_No=" + Dono + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
              DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qryelement);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = null;
                        txtBill_From_Address.Text = dt.Rows[0]["millname"].ToString();
                        txtBill_From_Address2.Text = dt.Rows[0]["milladdress"].ToString();
                        txtBill_From_Place.Text = dt.Rows[0]["millcityname"].ToString();
                        txtBill_From_PinCode.Text = dt.Rows[0]["millpincode"].ToString();
                        txtBill_From_State2.Text = dt.Rows[0]["millstatename"].ToString();


                    }
                }
            }
        }
    }
    #endregion
    #region [drpSupply_Type_TextChanged]
    protected void drpSupply_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [drpSub_Type_TextChanged]
    protected void drpSub_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [txtBill_From_Name_TextChanged]
    protected void txtBill_From_Name_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_From_Name.Text;
        strTextBox = "txtBill_From_Name";
        csCalculations();
    }
    #endregion
    #region [txtBill_From_Address_TextChanged]
    protected void txtBill_From_Address_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_From_Address.Text;
        strTextBox = "txtBill_From_Address";
        csCalculations();
    }
    #endregion
    #region [txtBill_FromGST_No_TextChanged]
    protected void txtBill_FromGST_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_FromGST_No.Text;
        strTextBox = "txtBill_FromGST_No";
        csCalculations();
    }
    #endregion
    #region [txtBill_From_Place_TextChanged]
    protected void txtBill_From_Place_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_From_Place.Text;
        strTextBox = "txtBill_From_Place";
        csCalculations();
    }
    #endregion
    #region [txtBill_From_State_TextChanged]
    protected void txtBill_From_State_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_From_State.Text;
        strTextBox = "txtBill_From_State";
        csCalculations();
    }
    #endregion
    #region [txtBill_From_PinCode_TextChanged]
    protected void txtBill_From_PinCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_From_PinCode.Text;
        strTextBox = "txtBill_From_PinCode";
        csCalculations();
    }
    #endregion
    #region [txtBill_To_Name_TextChanged]
    protected void txtBill_To_Name_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To_Name.Text;
        strTextBox = "txtBill_To_Name";
        csCalculations();
    }
    #endregion
    #region [txtBill_To_Add_TextChanged]
    protected void txtBill_To_Add_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To_Add.Text;
        strTextBox = "txtBill_To_Add";
        csCalculations();
    }
    #endregion
    #region [txtBill_TO_GSTNo_TextChanged]
    protected void txtBill_TO_GSTNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_TO_GSTNo.Text;
        strTextBox = "txtBill_TO_GSTNo";
        csCalculations();
    }
    #endregion
    #region [txtBill_To_Place_TextChanged]
    protected void txtBill_To_Place_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To_Place.Text;
        strTextBox = "txtBill_To_Place";
        csCalculations();
    }
    #endregion
    #region [txtBill_To_State_TextChanged]
    protected void txtBill_To_State_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To_State.Text;
        strTextBox = "txtBill_To_State";
        csCalculations();
    }
    #endregion
    #region [txtBill_To_PinCode_TextChanged]
    protected void txtBill_To_PinCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To_PinCode.Text;
        strTextBox = "txtBill_To_PinCode";
        csCalculations();
    }
    #endregion
    #region [txtItem_Name_TextChanged]
    protected void txtItem_Name_TextChanged(object sender, EventArgs e)
    {
        searchString = txtItem_Name.Text;
        strTextBox = "txtItem_Name";
        csCalculations();
    }
    #endregion
    #region [txtItem_Description_TextChanged]
    protected void txtItem_Description_TextChanged(object sender, EventArgs e)
    {
        searchString = txtItem_Description.Text;
        strTextBox = "txtItem_Description";
        csCalculations();
    }
    #endregion
    #region [txtHSN_TextChanged]
    protected void txtHSN_TextChanged(object sender, EventArgs e)
    {
        searchString = txtHSN.Text;
        strTextBox = "txtHSN";
        csCalculations();
    }
    #endregion
    #region [txtQty_TextChanged]
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQty.Text;
        strTextBox = "txtQty";
        csCalculations();
    }
    #endregion
    #region [txtUnit_TextChanged]
    protected void txtUnit_TextChanged(object sender, EventArgs e)
    {
        searchString = txtUnit.Text;
        strTextBox = "txtUnit";
        csCalculations();
    }
    #endregion
    #region [txtTaxable_Value_TextChanged]
    protected void txtTaxable_Value_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTaxable_Value.Text;
        strTextBox = "txtTaxable_Value";
        csCalculations();
    }
    #endregion
    #region [drpCGST_SGST_Rate_TextChanged]
    protected void drpCGST_SGST_Rate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [drpIGST_Rate_TextChanged]
    protected void drpIGST_Rate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [drpCESS_Advol_Rate_TextChanged]
    protected void drpCESS_Advol_Rate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [drpCESS_NonAdvol_Rate_TextChanged]
    protected void drpCESS_NonAdvol_Rate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [txtTaxable_Amt_TextChanged]
    protected void txtTaxable_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTaxable_Amt.Text;
        strTextBox = "txtTaxable_Amt";
        csCalculations();
    }
    #endregion
    #region [txtCGST_Amt_TextChanged]
    protected void txtCGST_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGST_Amt.Text;
        strTextBox = "txtCGST_Amt";
        csCalculations();
    }
    #endregion
    #region [txtSGST_Amt_TextChanged]
    protected void txtSGST_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGST_Amt.Text;
        strTextBox = "txtSGST_Amt";
        csCalculations();
    }
    #endregion
    #region [txtIGST_Amt_TextChanged]
    protected void txtIGST_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGST_Amt.Text;
        strTextBox = "txtIGST_Amt";
        csCalculations();
    }
    #endregion
    #region [txtCESS_Advol_Amt_TextChanged]
    protected void txtCESS_Advol_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCESS_Advol_Amt.Text;
        strTextBox = "txtCESS_Advol_Amt";
        csCalculations();
    }
    #endregion
    #region [txtCESS_non_Advol_Amt_TextChanged]
    protected void txtCESS_non_Advol_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCESS_non_Advol_Amt.Text;
        strTextBox = "txtCESS_non_Advol_Amt";
        csCalculations();
    }
    #endregion
    #region [txtOther_Amt_TextChanged]
    protected void txtOther_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOther_Amt.Text;
        strTextBox = "txtOther_Amt";
        csCalculations();
    }
    #endregion
    #region [txtTotal_Bill_Amt_TextChanged]
    protected void txtTotal_Bill_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTotal_Bill_Amt.Text;
        strTextBox = "txtTotal_Bill_Amt";
        csCalculations();
    }
    #endregion
    #region [txtTransporter_Name_TextChanged]
    protected void txtTransporter_Name_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransporter_Name.Text;
        strTextBox = "txtTransporter_Name";
        csCalculations();
    }
    #endregion
    #region [txtTransporter_ID_TextChanged]
    protected void txtTransporter_ID_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransporter_ID.Text;
        strTextBox = "txtTransporter_ID";
        csCalculations();
    }
    #endregion
    #region [txtApproximate_Distance_TextChanged]
    protected void txtApproximate_Distance_TextChanged(object sender, EventArgs e)
    {
        searchString = txtApproximate_Distance.Text;
        strTextBox = "txtApproximate_Distance";
        csCalculations();
    }
    #endregion
    #region [drpTrance_Mode_TextChanged]
    protected void drpTrance_Mode_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [drpVehicle_Type_TextChanged]
    protected void drpVehicle_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion
    #region [txtVehicle_No_TextChanged]
    protected void txtVehicle_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVehicle_No.Text;
        strTextBox = "txtVehicle_No";
        csCalculations();
    }
    #endregion
    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string searchtxt = "";
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = "";
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }
            if (hdnfClosePopup.Value == "txtDoc_No" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtDoc_No.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtDoc_No.Text = string.Empty;
                    txtDoc_No.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtDoc_No);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtDoc_No.Text == "Choose No")
                {
                    foreach (var s in split)
                    {
                        string aa = s.ToString();
                        name += "Doc_No Like '%" + aa + "%'or";
                    }
                    name = name.Remove(name.Length - 2);
                    lblPopupHead.Text = "--Select Group--";
                    string qry = " select Doc_No from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + name + ") order by Doc_No";
                    this.showPopup(qry);
                }
            }

        }
        catch
        {
        }
    }
    #endregion

    #region [Popup Button Code]
    protected void showPopup(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
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
                        grdPopup.DataSource = dt;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion
    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "Close")
            {
                txtSearchText.Text = string.Empty;
                pnlPopup.Style["display"] = "none";
                grdPopup.DataSource = null;
                grdPopup.DataBind();
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                searchString = txtSearchText.Text;
                strTextBox = hdnfClosePopup.Value;
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion
    #region [btntxtDoc_No_Click]
    protected void btntxtDoc_No_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDoc_No";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    public static string GetPageHtml(string link, System.Net.WebProxy proxy = null)
    {
        System.Net.WebClient client = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        if (proxy != null)
        {
            client.Proxy = proxy;
        }

        using (client)
        {
            try
            {
                return client.DownloadString(link);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
    #region [btnSave_Click]


    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string urlAddress="https://gsp.adaequare.com/gsp/authenticate?grant_type=token";
        //string urlAddress1 = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=GENEWAYBILL";
        //string username = "C8FE9A87F18A48D8B209F13DEF9CAF9B";  
        //string password = "A61455D7GAF5CG48A4GA838G4E3EAB8FD4B3";

        string username = "C1C2ED38DD58491597584CE1199B9ECF";
        string password = "5C14DBC6GAD4CG4FC4G9FE3GC2E4B88A8FCA";


        string urlAddress = "https://gsp.adaequare.com/gsp/authenticate?grant_type=token";
        //string urlAddress1 = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=GENEWAYBILL";
        string urlAddress1 = " https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=GENEWAYBILL";

        //string username = "E3EEDDAC588C4F0592471EBA314847E5";
        //string password = "791C8251GB220G4F7AG9889G10176CC4E77F";

        string USERNAME = "05AAACG2115R1ZN";
        string PASSWORD = "abc123@@";
        string gstin = "05AAACG2115R1ZN";
        string requestid = "shubhangi150594_unique_l8jh45851425635";//Auto generate no 
        string token = GetAuthToken(urlAddress, username, password);
        //string Ewaybullno = GenrateEWaybill(urlAddress1, USERNAME, PASSWORD, gstin, requestid, token);



        #region Testing
        //var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create("http://localhost:1069/accowebnavkar/Sugar/pgrSugarsaleForGSTxml.aspx");
        //httpWebRequest.ContentType = "application/json";
        //httpWebRequest.Method = "POST";

        //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //{
        //    //string json1 = "{\"user\":\"test\"," +
        //    //              "\"password\":\"bla\"}";
        //    //string json = "{\"action\":\"ACCESSTOKEN\"," +
        //    //               "\"client-id\":\"C8FE9A87F18A48D8B209F13DEF9CAF9B\"," +
        //    //               "\"client-secret\":\"A61455D7GAF5CG48A4GA838G4E3EAB8FD4B3\"}";
        //    string json = "{\"client-id\":\"C8FE9A87F18A48D8B209F13DEF9CAF9B\"," +
        //              "\"client-secret\":\"A61455D7GAF5CG48A4GA838G4E3EAB8FD4B3\"}";


        //    streamWriter.Write(json);
        //}

        //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //{
        //    var result = streamReader.ReadToEnd();
        //}
        #endregion

        #region [Validation Part]
        bool isValidated = true;
        //        if textbox is date then if condition will be like this if(clsCommon.isValidDate(txtDoc_Date.Text==true))
        #endregion

        #region -Head part declearation

        XElement root = new XElement("ROOT");
        XElement child1 = new XElement("Head");

        int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
        int Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
        int Branch_Code = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
        string Created_By = Session["user"].ToString();
        string Modified_By = Session["user"].ToString();
        string Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
        string Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");
        string retValue = string.Empty;
        string strRev = string.Empty;
        #endregion-End of Head part declearation
        #region Save Head Part
        //child1.SetAttributeValue("Company_Code", Company_Code);
        //child1.SetAttributeValue("Year_Code", Year_Code);
        //child1.SetAttributeValue("Branch_Code", Branch_Code);
        //if (btnSave.Text != "Save")
        //{
        //    child1.SetAttributeValue("Modified_By", Modified_By);
        //    child1.SetAttributeValue("Modified_Date", Modified_Date);
        //    child1.SetAttributeValue("", txt.Text != string.Empty ? txt.Text : "0");

        //}
        //else
        //{
        //    child1.SetAttributeValue("Created_By", Created_By);
        //    child1.SetAttributeValue("Created_Date", Created_Date);
        //}
        //root.Add(child1);
        #endregion-End of Head part Save

        #region save Head Master
        //string XMLReport = root.ToString();
        //XDocument xDoc = XDocument.Parse(XMLReport, LoadOptions.None);
        //XMLReport = xDoc.ToString(SaveOptions.DisableFormatting);
        //DataSet xml_ds = new DataSet();
        //string spname = "SP_";
        //string xmlfile = XMLReport;
        //string op = "";
        //string returnmaxno = "";
        //int flag;
        //if (btnSave.Text == "Save")
        //{
        //    #region[Insert]
        //    flag = 1;
        //    xml_ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
        //    #endregion
        //}
        //else
        //{
        //    #region[Update]
        //    flag = 2;
        //    xml_ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
        //    #endregion
        //}
        //txt.Text = returnmaxno;
        //hdnf.Value = txt.Text;
        //retValue = op;
        //int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
        //int Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
        //int Branch_Code = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
        //string Created_By = Session["user"].ToString();
        //string Modified_By = Session["user"].ToString();
        //string Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
        //string Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");
        //string retValue = string.Empty;
        //string strRev = string.Empty;
        #endregion-End of Head part declearation
        #region Save Head Part
        //child1.SetAttributeValue("EditDoc_No", txtEditDoc_No.Text);
        //child1.SetAttributeValue("Doc_No", txtDoc_No.Text != string.Empty ? txtDoc_No.Text : "0");
        child1.SetAttributeValue("Doc_Type", drpDoc_Type.SelectedValue);
        child1.SetAttributeValue("Doc_Date", DateTime.Parse(txtDoc_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd"));
        child1.SetAttributeValue("Tran_Type", drpTran_Type.SelectedValue);
        child1.SetAttributeValue("Supply_Type", drpSupply_Type.SelectedValue);
        child1.SetAttributeValue("Sub_Type", drpSub_Type.SelectedValue);
        child1.SetAttributeValue("Bill_From_Name", txtBill_From_Name.Text);
        child1.SetAttributeValue("Bill_From_Address", txtBill_From_Address.Text);
        child1.SetAttributeValue("Bill_From_Address2", txtBill_From_Address2.Text);
        child1.SetAttributeValue("Bill_FromGST_No", txtBill_FromGST_No.Text);
        child1.SetAttributeValue("Bill_From_Place", txtBill_From_Place.Text);
        child1.SetAttributeValue("Bill_From_State", txtBill_From_State.Text);
        child1.SetAttributeValue("Bill_From_State", txtBill_From_State2.Text);
        child1.SetAttributeValue("Bill_From_PinCode", txtBill_From_PinCode.Text != string.Empty ? txtBill_From_PinCode.Text : "0");
        child1.SetAttributeValue("Bill_To_Name", txtBill_To_Name.Text);
        child1.SetAttributeValue("Bill_To_Add", txtBill_To_Add.Text);
        child1.SetAttributeValue("Bill_To_Add", txtBill_To_Add2.Text);
        child1.SetAttributeValue("Bill_TO_GSTNo", txtBill_TO_GSTNo.Text);
        child1.SetAttributeValue("Bill_To_Place", txtBill_To_Place.Text);
        child1.SetAttributeValue("Bill_To_State", txtBill_To_State.Text);
        child1.SetAttributeValue("Bill_To_PinCode", txtBill_To_PinCode.Text != string.Empty ? txtBill_To_PinCode.Text : "0");
        child1.SetAttributeValue("Item_Name", txtItem_Name.Text);
        child1.SetAttributeValue("Item_Description", txtItem_Description.Text);
        child1.SetAttributeValue("HSN", txtHSN.Text);
        child1.SetAttributeValue("Qty", txtQty.Text != string.Empty ? txtQty.Text : "0.00");
        child1.SetAttributeValue("Unit", txtUnit.Text != string.Empty ? txtUnit.Text : "0");
        child1.SetAttributeValue("Taxable_Value", txtTaxable_Value.Text != string.Empty ? txtTaxable_Value.Text : "0.00");
        child1.SetAttributeValue("CGST_SGST_Rate", drpCGST_SGST_Rate.SelectedValue);
        child1.SetAttributeValue("IGST_Rate", drpIGST_Rate.SelectedValue);
        child1.SetAttributeValue("CESS_Advol_Rate", drpCESS_Advol_Rate.SelectedValue);
        child1.SetAttributeValue("CESS_NonAdvol_Rate", drpCESS_NonAdvol_Rate.SelectedValue);
        child1.SetAttributeValue("Taxable_Amt", txtTaxable_Amt.Text != string.Empty ? txtTaxable_Amt.Text : "0.00");
        child1.SetAttributeValue("CGST_Amt", txtCGST_Amt.Text != string.Empty ? txtCGST_Amt.Text : "0.00");
        child1.SetAttributeValue("SGST_Amt", txtSGST_Amt.Text != string.Empty ? txtSGST_Amt.Text : "0.00");
        child1.SetAttributeValue("IGST_Amt", txtIGST_Amt.Text != string.Empty ? txtIGST_Amt.Text : "0.00");
        child1.SetAttributeValue("CESS_Advol_Amt", txtCESS_Advol_Amt.Text != string.Empty ? txtCESS_Advol_Amt.Text : "0.00");
        child1.SetAttributeValue("CESS_non_Advol_Amt", txtCESS_non_Advol_Amt.Text != string.Empty ? txtCESS_non_Advol_Amt.Text : "0.00");
        child1.SetAttributeValue("Other_Amt", txtOther_Amt.Text != string.Empty ? txtOther_Amt.Text : "0.00");
        child1.SetAttributeValue("Total_Bill_Amt", txtTotal_Bill_Amt.Text != string.Empty ? txtTotal_Bill_Amt.Text : "0.00");
        child1.SetAttributeValue("Transporter_Name", txtTransporter_Name.Text);
        child1.SetAttributeValue("Transporter_ID", txtTransporter_ID.Text);
        child1.SetAttributeValue("Approximate_Distance", txtApproximate_Distance.Text != string.Empty ? txtApproximate_Distance.Text : "0.00");
        child1.SetAttributeValue("Trance_Mode", drpTrance_Mode.SelectedValue);
        child1.SetAttributeValue("Vehicle_Type", drpVehicle_Type.SelectedValue);
        child1.SetAttributeValue("Vehicle_No", txtVehicle_No.Text);
        child1.SetAttributeValue("Company_Code", Company_Code);
        child1.SetAttributeValue("Year_Code", Year_Code);
        child1.SetAttributeValue("Branch_Code", Branch_Code);
        if (btnSave.Text != "Save")
        {
            child1.SetAttributeValue("Modified_By", Modified_By);
            child1.SetAttributeValue("Modified_Date", Modified_Date);
            child1.SetAttributeValue("Doc_No", txtDoc_No.Text != string.Empty ? txtDoc_No.Text : "0");

        }
        else
        {
            child1.SetAttributeValue("Created_By", Created_By);
            child1.SetAttributeValue("Created_Date", Created_Date);
        }
        root.Add(child1);
        #endregion-End of Head part Save

        #region save Head Master
        string XMLReport = root.ToString();
        XDocument xDoc = XDocument.Parse(XMLReport, LoadOptions.None);
        XMLReport = xDoc.ToString(SaveOptions.DisableFormatting);
        DataSet xml_ds = new DataSet();
        string spname = "SP_EwayBill";
        string xmlfile = XMLReport;
        string op = "";
        string returnmaxno = "";
        int flag;
        if (btnSave.Text == "Save")
        {
            #region[Insert]
            flag = 1;
            xml_ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
            #endregion
        }
        else
        {
            #region[Update]
            flag = 2;
            xml_ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
            #endregion
        }
        txtDoc_No.Text = returnmaxno;
        hdnf.Value = txtDoc_No.Text;
        retValue = op;

        if (retValue == "-1")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Successfully Added! No=" + returnmaxno + "');", true);
        }
        if (retValue == "-2" || retValue == "-3")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Successfully Updated! No=" + returnmaxno + "');", true);
        }
        clsButtonNavigation.enableDisable("S");
        this.enableDisableNavigateButtons();
        this.makeEmptyForm("S");
        qry = getDisplayQuery();
        this.fetchRecord(qry);
        #endregion
    }
    #endregion
    public static string EncryptAsymmetric(string data, string key)
    {
        byte[] keyBytes = Convert.FromBase64String(key);
        AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
        RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
        RSAParameters rsaParameters = new RSAParameters();
        rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
        rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(rsaParameters);
        byte[] plaintext = Encoding.UTF8.GetBytes(data);
        byte[] ciphertext = rsa.Encrypt(plaintext, false);
        string cipherresult = Convert.ToBase64String(ciphertext);
        return cipherresult;
    }

    public string GetAuthToken(string urlAddress, string username, string password)
    {
        string jsonData = string.Empty;
        string authtoken = string.Empty;
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlAddress);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("gspappid", username);//Client Id
        request.Headers.Add("gspappsecret", password);//Client Secret
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = "{\"gspappid\":\"" + username + "\"," +
              "\"gspappsecret\":\"" + password + "\"}";
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        try
        {
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                jsonData = streamReader.ReadToEnd();
            }
            var jsonObject = JObject.Parse(jsonData);
            jsonData = (string)jsonObject.SelectToken("access_token");
            authtoken = "Bearer " + jsonData;
        }
        catch (Exception ex)
        {
            return jsonData = "Issue occured, " + ex.Message;
        }

        return authtoken;
    }

    public string GenrateEWaybill(string urlAddress1, string username, string password, string gstin, string requestid, string token, string json)
    {
        string jsonData = string.Empty;
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlAddress1);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("username", username);
        request.Headers.Add("password", password);
        request.Headers.Add("gstin", gstin);
        request.Headers.Add("requestid", requestid);
        request.Headers.Add("Authorization", token);


        try
        {
            //Place the serialized content of the object to be posted into the request stream
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //  string json = "{\"gspappid\":\"" + username + "\"," +
                //"\"gspappsecret\":\"" + password + "\"}";
                //string acb = "{\"supplyType\":\"O\",\"subSupplyType\":\"1\",\"docType\":\"INV\",\"docNo\":\"123021544-8\",\"docDate\":\"15/12/2017\",\"fromGstin\":\"05AAACG2115R1ZN\",\"fromTrdName\":\"welton\",\"fromAddr1\":\"2ND CROSS NO 59  19  A\",\r\n\"fromAddr2\":\"GROUND FLOOR OSBORNE ROAD\",\r\n\"fromPlace\":\"FRAZER TOWN\",\r\n\"fromPincode\":560042,\r\n\"actFromStateCode\":29,\"fromStateCode\":29,\r\n\"toGstin\":\"05AAACG2140A1ZL\",\r\n\"toTrdName\":\"sthuthya\",\r\n\"toAddr1\":\"Shree Nilaya\",\r\n\"toAddr2\":\"Dasarahosahalli\",\r\n\"toPlace\":\"Beml Nagar\",\r\n\"toPincode\":689788,\r\n\"actToStateCode\":29,\r\n\"toStateCode\":28,\r\n\"totalValue\":5609889,\r\n\"cgstValue\":0,\r\n\"sgstValue\":0,\r\n\"igstValue\":168296.67,\r\n\"cessValue\":224395.56,\r\n\"totInvValue\":435678,\r\n\"transporterId\":\"\",\r\n\"transporterName\":\"\",\r\n\"transDocNo\":\"\",\r\n\"transMode\":\"1\",\r\n\"transDistance\":\"25\",\r\n\"transDocDate\":\"\",\r\n\"vehicleNo\":\"PVC1234\",\r\n\"vehicleType\":\"R\",\r\n\"itemList\":\r\n[{\r\n\"productName\":\"Wheat\",\r\n\"productDesc\":\"Wheat\",\r\n\"hsnCode\":1001,\r\n\"quantity\":4,\r\n\"qtyUnit\":\"BOX\",\r\n\"cgstRate\":0,\r\n\"sgstRate\":0,\r\n\"igstRate\":3,\r\n\"cessRate\":4,\r\n\"cessAdvol\":0,\r\n\"taxableAmount\":5609889\r\n}\r\n]}";
                // string acb = "{\r\n\"supplyType\":\"O\",\r\n\"subSupplyType\":\"1\",\r\n\r\n\"docType\":\"INV\",\r\n\"docNo\":\"8985123-8\",\r\n\"docDate\":\"15/12/2017\",\r\n\"fromGstin\":\"05AAACG2115R1ZN\",\r\n\"fromTrdName\":\"welton\",\r\n\"fromAddr1\":\"2ND CROSS NO 59  19  A\",\r\n\"fromAddr2\":\"GROUND FLOOR OSBORNE ROAD\",\r\n\"fromPlace\":\"FRAZER TOWN\",\r\n\"fromPincode\":560042,\r\n\"actFromStateCode\":29,\"fromStateCode\":29,\r\n\"toGstin\":\"05AAACG2140A1ZL\",\r\n\"toTrdName\":\"sthuthya\",\r\n\"toAddr1\":\"Shree Nilaya\",\r\n\"toAddr2\":\"Dasarahosahalli\",\r\n\"toPlace\":\"Beml Nagar\",\r\n\"toPincode\":689788,\r\n\"actToStateCode\":29,\r\n\"toStateCode\":28,\r\n\"totalValue\":5609889,\r\n\"cgstValue\":0,\r\n\"sgstValue\":0,\r\n\"igstValue\":168296.67,\r\n\"cessValue\":224395.56,\r\n\"totInvValue\":435678,\r\n\"transporterId\":\"\",\r\n\"transporterName\":\"\",\r\n\"transDocNo\":\"\",\r\n\"transMode\":\"1\",\r\n\"transDistance\":\"25\",\r\n\"transDocDate\":\"\",\r\n\"vehicleNo\":\"PVC1234\",\r\n\"vehicleType\":\"R\",\r\n\"itemList\":\r\n[{\r\n\"productName\":\"Wheat\",\r\n\"productDesc\":\"Wheat\",\r\n\"hsnCode\":1001,\r\n\"quantity\":4,\r\n\"qtyUnit\":\"BOX\",\r\n\"cgstRate\":0,\r\n\"sgstRate\":0,\r\n\"igstRate\":3,\r\n\"cessRate\":4,\r\n\"cessAdvol\":0,\r\n\"taxableAmount\":5609889\r\n}\r\n]}";
                // string jsonNew = "{\"supplyType\":\"O\",\"subSupplyType\":\"1\",\"docType\":\"INV\",\"docNo\":\"1120shu1bha1110897854540\",\"docDate\":\"15/12/2017\",\"fromGstin\":\"05AAACG2115R1ZN\",\"fromTrdName\":\"welton\",\"fromAddr1\":\"2ND CROSS NO 59  19  A\",\r\n\"fromAddr2\":\"GROUND FLOOR OSBORNE ROAD\",\r\n\"fromPlace\":\"FRAZER TOWN\",\r\n\"fromPincode\":560042,\r\n\"actFromStateCode\":29,\"fromStateCode\":29,\r\n\"toGstin\":\"05AAACG2140A1ZL\",\r\n\"toTrdName\":\"sthuthya\",\r\n\"toAddr1\":\"Shree Nilaya\",\r\n\"toAddr2\":\"Dasarahosahalli\",\r\n\"toPlace\":\"Beml Nagar\",\r\n\"toPincode\":500003,\r\n\"actToStateCode\":36,\r\n\"toStateCode\":36,\r\n\"totalValue\":5609889,\r\n\"cgstValue\":0,\r\n\"sgstValue\":0,\r\n\"igstValue\":168296.67,\r\n\"cessValue\":224395.56,\r\n\"totInvValue\":6002581.23,\r\n\"transporterId\":\"\",\r\n\"transporterName\":\"\",\r\n\"transDocNo\":\"\",\r\n\"transMode\":\"1\",\r\n\"transDistance\":\"25\",\r\n\"transDocDate\":\"\",\r\n\"vehicleNo\":\"PVC1234\",\r\n\"vehicleType\":\"R\",\r\n\"TransactionType\":\"1\",\r\n\"itemList\":\r\n[{\r\n\"productName\":\"Wheat\"\r\n,\"productDesc\":\"Wheat\",\r\n\"hsnCode\":1001,\r\n\"quantity\":4,\r\n\"qtyUnit\":\"BOX\",\r\n\"cgstRate\":0,\r\n\"sgstRate\":0,\r\n\"igstRate\":3,\r\n\"cessRate\":1,\r\n\"cessAdvol\":0,\r\n\"taxableAmount\":5609889\r\n},{\r\n\"productName\":\"Wheat\",\r\n\"productDesc\":\"Wheat\",\r\n\"hsnCode\":1001,\"quantity\":4,\r\n\"qtyUnit\":\"BOX\",\r\n\"cgstRate\":0,\r\n\"sgstRate\":0,\r\n\"igstRate\":3,\r\n\"cessRate\":1,\r\n\"cessAdvol\":0,\r\n\"taxableAmount\":5609889\r\n}]\r\n}\r\n}";

                //// string asb = "{\"supplyType\":\"O\",\"subSupplyType\":\"1\",\"docType\":\"INV\",\"docNo\":\"3g545jn10\",\"docDate\":\"15/12/2017\",\"fromGstin\":\"05AAACG2115R1ZN\",\"fromTrdName\":\"welton\",\"fromAddr1\":\"2ND CROSS NO 59  19  A\",\"fromAddr2\":\"GROUND FLOOR OSBORNE ROAD\",\"fromPlace\":\"FRAZER TOWN\",\"fromPincode\":560042,\"actFromStateCode\":29,\"fromStateCode\":29,\"toGstin\":\"05AAACG2140A1ZL\",\"toTrdName\":\"sthuthya\",\"toAddr1\":\"Shree Nilaya\",\"toAddr2\":\"Dasarahosahalli\",\"toPlace\":\"Beml Nagar\",\"toPincode\":500003,\"actToStateCode\":36,\"toStateCode\":36,\"totalValue\":5609889,\"cgstValue\":0,\"sgstValue\":0,\"igstValue\":168296.67,\"cessValue\":224395.56,\"totInvValue\":6002581.23,\"transporterId\":\"\",\"transporterName\":\"\",\"transDocNo\":\"\",\"transMode\":\"1\",\"transDistance\":\"25\",\"transDocDate\":\"\",\"vehicleNo\":\"PVC1234\",\"vehicleType\":\"R\",\"TransactionType\":\"1\",\"itemList\":[{\"productName\":\"Wheat\",\"productDesc\":\"Wheat\",\"hsnCode\":1001,\"quantity\":4,\"qtyUnit\":\"BOX\",\"cgstRate\":0,\"sgstRate\":0,\"igstRate\":3,\"cessRate\":1,\"cessAdvol\":0,\"taxableAmount\":5609889},{\"productName\":\"Wheat\",\"productDesc\":\"Wheat\",\"hsnCode\":1001,\"quantity\":4,\"qtyUnit\":\"BOX\",\"cgstRate\":0,\"sgstRate\":0,\"igstRate\":3,\"cessRate\":1,\"cessAdvol\":0,\"taxableAmount\":5609889}]}";

                // string asb = "{"supplyType":"O","subSupplyType":"1","docType":"INV","docNo":"3ghj10","docDate":"15/12/2017","fromGstin":"05AAACG2115R1ZN","fromTrdName":"welton","fromAddr1":"2ND CROSS NO 59  19  A","fromAddr2":"GROUND FLOOR OSBORNE ROAD","fromPlace":"FRAZER TOWN","fromPincode":560042,"actFromStateCode":29,"fromStateCode":29,"toGstin":"05AAACG2140A1ZL","toTrdName":"sthuthya","toAddr1":"Shree Nilaya","toAddr2":"Dasarahosahalli","toPlace":"Beml Nagar","toPincode":500003,"actToStateCode":36,"toStateCode":36,"totalValue":5609889,"cgstValue":0,"sgstValue":0,"igstValue":168296.67,"cessValue":224395.56,"totInvValue":6002581.23,"transporterId":"","transporterName":"","transDocNo":"","transMode":"1","transDistance":"25","transDocDate":"","vehicleNo":"PVC1234","vehicleType":"R","TransactionType":"1","itemList":[{"productName":"Wheat","productDesc":"Wheat","hsnCode":1001,"quantity":4,"qtyUnit":"BOX","cgstRate":0,"sgstRate":0,"igstRate":3,"cessRate":1,"cessAdvol":0,"taxableAmount":5609889},{"productName":"Wheat","productDesc":"Wheat","hsnCode":1001,"quantity":4,"qtyUnit":"BOX","cgstRate":0,"sgstRate":0,"igstRate":3,"cessRate":1,"cessAdvol":0,"taxableAmount":5609889}]}";

                //  var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(asb);


                string asb = json;
                streamWriter.Write(asb);
                streamWriter.Flush();
                streamWriter.Close();
            }



            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                readStream = new StreamReader(receiveStream);
                jsonData = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                var jsonObject = JObject.Parse(jsonData);
            }
        }
        catch (Exception ex)
        {
            return jsonData = "Issue occured, " + ex.Message;
        }
        return jsonData;
    }
    #region [btnGenEwayBill_Click]
    protected void btnGenEwayBill_Click(object sender, EventArgs e)
    {
        string acname = "";
        string vno;
        string from_address;
        string from_address1;
        string from_partnm;
        string Bill_From_Add;
        string from_city;
        string to_ac_name_e;
        string to_address1;
        string to_address2;
        string to_place;
        string fromTrdName;
        string Description;
        bool isValidated = true;
        if (txtBill_From_PinCode.Text != string.Empty && txtBill_From_PinCode.Text != "999999")
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtBill_From_PinCode);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invalied Mill Pincode!!!!!');", true);
            return;
        }
        if (txtBill_To_PinCode.Text != string.Empty && txtBill_To_PinCode.Text != "999999")
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtBill_To_PinCode);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invalied Bill To Pincode!!!!!');", true);
            return;
        }
        if (txtApproximate_Distance.Text != string.Empty && txtBill_To_PinCode.Text != "0.00")
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtApproximate_Distance);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Proper Distance!!!!!');", true);
            return;
        }

        #region Assing Value
        string Supply_Type = drpSupply_Type.SelectedValue;
        string SubType = drpSub_Type.SelectedValue;
        string Doc_Type = drpDoc_Type.SelectedValue;
        string Doc_No = txtDoc_No.Text;
        string Doc_Date = txtDoc_Date.Text;
        string Tran_Type = drpTran_Type.SelectedValue;
        #region replace from_partnm
        from_partnm = txtBill_From_Name.Text;
        from_partnm = from_partnm.Replace("-", "");
        from_partnm = from_partnm.Replace("/", "");
        from_partnm = from_partnm.Replace("&", "AND");
        from_partnm = from_partnm.Replace(".", "");
        from_partnm = from_partnm.Replace("#", "");
        from_partnm = from_partnm.Replace("(", "");
        from_partnm = from_partnm.Replace(")", "");
        from_partnm = from_partnm.Replace(":", "");
        from_partnm = from_partnm.Replace("_", "");
        from_partnm = from_partnm.Replace("@", "");
        from_partnm = from_partnm.Replace(";", "");
        from_partnm = from_partnm.Replace("=", "");
        from_partnm = from_partnm.Replace("  ", "");
        from_partnm = from_partnm.Replace("\t", "");
        from_partnm = from_partnm.Replace("\n", "");
        from_partnm = from_partnm.Replace("*", "");

        string Bill_From_Name = from_partnm;
        #endregion
        #region replace Bill_From_Add

        from_address1 = txtBill_From_Address.Text;
        from_address1 = from_address1.Replace(".", "");
        from_address1 = from_address1.Replace("(", "");
        from_address1 = from_address1.Replace(")", "");
        from_address1 = from_address1.Replace(":", "");
        from_address1 = from_address1.Replace("_", "");
        from_address1 = from_address1.Replace("@", "");
        from_address1 = from_address1.Replace(";", "");
        from_address1 = from_address1.Replace("=", "");
        from_address1 = from_address1.Replace("#", "");
        from_address1 = from_address1.Replace("-", "");
        from_address1 = from_address1.Replace("/", "");
        from_address1 = from_address1.Replace("&", "AND");
        from_address1 = from_address1.Replace("\n", "");
        from_address1 = from_address1.Replace("*", "");
        from_address1 = from_address1.Replace("'", "");
        from_address1 = from_address1.Replace(",", "");
        from_address1 = from_address1.Replace(" ", "");
        from_address1 = from_address1.Replace("\t", "");
        from_address1 = from_address1.Replace("\"", "");
        Bill_From_Add = from_address1;


        from_address = txtBill_From_Address2.Text;
        from_address = from_address.Replace(".", "");
        from_address = from_address.Replace("(", "");
        from_address = from_address.Replace(")", "");
        from_address = from_address.Replace(":", "");
        from_address = from_address.Replace("_", "");
        from_address = from_address.Replace("@", "");
        from_address = from_address.Replace(";", "");
        from_address = from_address.Replace("=", "");
        from_address = from_address.Replace("#", "");
        from_address = from_address.Replace("-", "");
        from_address = from_address.Replace("/", "");
        from_address = from_address.Replace("&", "AND");
        from_address = from_address.Replace("\n", "");
        from_address = from_address.Replace("*", "");
        #endregion

        string Bill_From_Address2 = from_address;


        string Bill_From_GSTIn = "27AAJCD6087E1ZF";
        ////string Bill_From_GSTIn = "05AAACG2115R1ZN";
        #region replace from_city
        from_city = txtBill_From_Place.Text;
        from_city = from_city.Replace(".", "");
        from_city = from_city.Replace("(", "");
        from_city = from_city.Replace(")", "");
        from_city = from_city.Replace(":", "");
        from_city = from_city.Replace("_", "");
        from_city = from_city.Replace("@", "");
        from_city = from_city.Replace(";", "");
        from_city = from_city.Replace("=", "");
        from_city = from_city.Replace("-", "");
        from_city = from_city.Replace("&", "AND");
        from_city = from_city.Replace("\n", "");
        from_city = from_city.Replace("/", "");
        from_city = from_city.Replace("*", "");
        string Bill_From_Place = from_city;
        #endregion


        string Bill_From_State = "27";
        //if (txtBill_From_State.Text != string.Empty)
        //{
        //    acname = clsCommon.getString("select ISNULL(State_Code,0) from GSTStateMasterNew where  State_Name=" + txtBill_From_State.Text + "");
        //    Bill_From_State = "29";
        //}
        string Bill_From_State2;

        if (drpTran_Type.SelectedValue == "1")
        {
             Bill_From_State2 = "27";
        }
        else
        {
             Bill_From_State2 = hdnfMillState_Code.Value;
        }
        //if (txtBill_From_State2.Text != string.Empty)
        //{
        //    acname = clsCommon.getString("select ISNULL(State_Code,0) from GSTStateMasterNew where  State_Name=" + txtBill_From_State2.Text + "");
        //    Bill_From_State2 = "29";
        //}
        string Bill_From_PinCode = txtBill_From_PinCode.Text;
        // string Bill_From_PinCode = "560042";
        #region[replae to party name]
        to_ac_name_e = txtBill_To_Name.Text;
        to_ac_name_e = to_ac_name_e.Replace("-", "");
        to_ac_name_e = to_ac_name_e.Replace("/", "");
        to_ac_name_e = to_ac_name_e.Replace("&", "AND");
        to_ac_name_e = to_ac_name_e.Replace(".", "");
        to_ac_name_e = to_ac_name_e.Replace("#", "");
        to_ac_name_e = to_ac_name_e.Replace("(", "");
        to_ac_name_e = to_ac_name_e.Replace(")", "");
        to_ac_name_e = to_ac_name_e.Replace(":", "");
        to_ac_name_e = to_ac_name_e.Replace("_", "");
        to_ac_name_e = to_ac_name_e.Replace("@", "");
        to_ac_name_e = to_ac_name_e.Replace(";", "");
        to_ac_name_e = to_ac_name_e.Replace("=", "");
        to_ac_name_e = to_ac_name_e.Replace("\n", "");
        to_ac_name_e = to_ac_name_e.Replace("*", "");
        string Bill_To_Name = to_ac_name_e;
        #endregion

        #region[replace to address1]
        to_address1 = txtBill_To_Add.Text;
        to_address1 = to_address1.Replace("-", "");
        to_address1 = to_address1.Replace("/", "");
        to_address1 = to_address1.Replace("&", "AND");
        to_address1 = to_address1.Replace(".", "");
        to_address1 = to_address1.Replace("#", "");
        to_address1 = to_address1.Replace(",", "");
        to_address1 = to_address1.Replace("(", "");
        to_address1 = to_address1.Replace(")", "");
        to_address1 = to_address1.Replace(":", "");
        to_address1 = to_address1.Replace("_", "");
        to_address1 = to_address1.Replace("@", "");
        to_address1 = to_address1.Replace(";", "");
        to_address1 = to_address1.Replace("=", "");
        to_address1 = to_address1.Replace("\n", "");
        to_address1 = to_address1.Replace("*", "");
        string Bill_To_Add = to_address1;
        #endregion
        #region[replace to address2]
        to_address2 = txtBill_To_Add2.Text;
        to_address2 = to_address2.Replace("-", "");
        to_address2 = to_address2.Replace("/", "");
        to_address2 = to_address2.Replace("&", "AND");
        to_address2 = to_address2.Replace(".", "");
        to_address2 = to_address2.Replace("#", "");
        to_address2 = to_address2.Replace(",", "");
        to_address2 = to_address2.Replace("(", "");
        to_address2 = to_address2.Replace(")", "");
        to_address2 = to_address2.Replace(":", "");
        to_address2 = to_address2.Replace("_", "");
        to_address2 = to_address2.Replace("@", "");
        to_address2 = to_address2.Replace(";", "");
        to_address2 = to_address2.Replace("=", "");
        to_address2 = to_address2.Replace("*", "");
        to_address2 = to_address2.Replace("\n", "");
        string Bill_To_Add2 = to_address2;
        #endregion


        string Bill_To_GSTIn = txtBill_TO_GSTNo.Text;
        ////string Bill_To_GSTIn = "05AAACG2140A1ZL";


        #region[replace to place]
        to_place = txtBill_To_Place.Text;

        to_place = to_place.Replace(".", "");

        to_place = to_place.Replace("(", "");
        to_place = to_place.Replace(")", "");
        to_place = to_place.Replace(":", "");
        to_place = to_place.Replace("_", "");
        to_place = to_place.Replace("@", "");
        to_place = to_place.Replace(";", "");
        to_place = to_place.Replace("=", "");
        to_place = to_place.Replace("&", "AND");
        to_place = to_place.Replace("/", "");
        to_place = to_place.Replace("\n", "");
        to_place = to_place.Replace(",", "");
        to_place = to_place.Replace("*", "");
        string Bill_To_Place = to_place;
        #endregion


        string Bill_To_State = hdnfState_Code_BillTo.Value;
        string Bill_To_State2 = hdnfState_Code.Value;
        //if (txtBill_To_State.Text != string.Empty)
        //{
        //    acname = clsCommon.getString("select ISNULL(State_Code,0) from GSTStateMasterNew where  State_Name=" + txtBill_To_State.Text + "");
        //    Bill_To_State = "36";
        //}

        string Bill_To_Pincode = txtBill_To_PinCode.Text;
        //string Bill_To_Pincode = "500003";
        string Product_Nmae = txtItem_Name.Text;

       

        Description = txtItem_Description.Text;
        Description = Description.Replace(".", "");
        Description = Description.Replace("(", "");
        Description = Description.Replace(")", "");
        Description = Description.Replace(":", "");
        Description = Description.Replace("_", "");
        Description = Description.Replace("@", "");
        Description = Description.Replace(";", "");
        Description = Description.Replace("=", "");
        Description = Description.Replace("&", "AND");
        Description = Description.Replace("/", "");
        Description = Description.Replace("\n", "");
        Description = Description.Replace(",", "");
        Description = Description.Replace("*", "");
        Description = Description.Replace("-", "");
       string Product_Desc = Description;
        string HSN = txtHSN.Text;
        double to_Qty;
        to_Qty = Convert.ToDouble(txtQty.Text);
        double Qty = to_Qty;
        string Unit = txtUnit.Text;

        double to_Taxable_Value;
        to_Taxable_Value = Convert.ToDouble(txtTaxable_Value.Text);
        double Taxable_Value = to_Taxable_Value;

        //string CGT_SGT_Rate = "0";
        //string IGST_Rate = "0";
        //string CESS_Advol_Rate = "0";
        //string CESS_NonAdvol_Rate = "0";

        double to_Taxable_Amt;
        to_Taxable_Amt = Convert.ToDouble(txtTaxable_Amt.Text);
        double Taxable_Amt = to_Taxable_Amt;
        // string Taxable_Amt = txtTaxable_Amt.Text;

        double to_CGST_Amt;
        to_CGST_Amt = Convert.ToDouble(txtCGST_Amt.Text);
        double CGST_Amt = to_CGST_Amt;
        //// double CGST_Amt = 0;
        // string CGST_Amt = txtCGST_Amt.Text;

        double to_SCGT_Amt;
        to_SCGT_Amt = Convert.ToDouble(txtSGST_Amt.Text);
        double SCGT_Amt = to_SCGT_Amt;
        ////double SCGT_Amt = 0;
        //string SCGT_Amt = txtSGST_Amt.Text;
        double to_IGST_Amt;
        to_IGST_Amt = Convert.ToDouble(txtIGST_Amt.Text);
        double IGST_Amt = to_IGST_Amt;
        ////double IGST_Amt = 1560;
        //string IGST_Amt = IGST_Amt.Text;
        double to_CESS_Advol_Amt;
        to_CESS_Advol_Amt = Convert.ToDouble(txtCESS_Advol_Amt.Text);
        double CESS_Advol_Amt = to_CESS_Advol_Amt;
        //string CESS_Advol_Amt = txtCESS_Advol_Amt.Text;
        double to_CESS_NonAdvol_Amt;
        to_CESS_NonAdvol_Amt = Convert.ToDouble(txtCESS_non_Advol_Amt.Text);
        double CESS_NonAdvol_Amt = to_CESS_NonAdvol_Amt;
        //string CESS_NonAdvol_Amt = txtCESS_non_Advol_Amt.Text;
        string Other_Amt = txtOther_Amt.Text;
        string Tot_Bill_Amt = txtTotal_Bill_Amt.Text;
        string Trans_DocNo = "";
        string Transport_Name = txtTransporter_Name.Text;
        string Transport_Id = txtTransporter_ID.Text;
        double to_Distance;
        to_Distance = Convert.ToDouble(txtApproximate_Distance.Text);
        double Distance = to_Distance;
        string Tans_Mode = drpTrance_Mode.SelectedValue;
        string Vehical_Type = drpVehicle_Type.SelectedValue;

        #region Relace Vehicle_No
        vno = txtVehicle_No.Text;
        vno = vno.Replace("-", "");
        vno = vno.Replace("/", "");
        vno = vno.Replace(" ", "");
        vno = vno.Replace("&", "");
        vno = vno.Replace(".", "");
        vno = vno.Replace("#", "");
        vno = vno.Replace("(", "");
        vno = vno.Replace(")", "");
        vno = vno.Replace(":", "");
        vno = vno.Replace("_", "");
        vno = vno.Replace("@", "");
        vno = vno.Replace(";", "");
        vno = vno.Replace("=", "");
        string Vehical_No = vno;
        #endregion
        string Trans_Datenew = Trans_Date;
        //string CGST_Ratenew = "0";
        //string SGST_Ratenew = "0";
        //string IGST_Ratenew = "5";

        string CGST_Ratenew = hdnfCGST_Rate.Value;
        string SGST_Ratenew = hdnfSGST_Rate.Value;
        string IGST_Ratenew = hdnfIGST_Rate.Value;
        string CessRate = "0";
        string CessAdvol = "0";
        #endregion

        #region Json file
        string json = "{\"supplyType\":\"" + Supply_Type + "\"," +
              "\"subSupplyType\":\"" + SubType + "\"," +
              "\"docType\":\"" + Doc_Type + "\"," +
              "\"docNo\":\"" + Doc_No + "\"," +
              "\"docDate\":\"" + Doc_Date + "\"," +
              "\"fromGstin\":\"" + Bill_From_GSTIn + "\"," +
              "\"fromTrdName\":\"" + Bill_From_Name + "\"," +
              "\"fromAddr1\":\"" + Bill_From_Add + "\"," +
              "\"fromAddr2\":\"" + Bill_From_Address2 + "\"," +
              "\"fromPlace\":\"" + Bill_From_Place + "\"," +
              "\"fromPincode\":" + Bill_From_PinCode + "," +
              "\"actFromStateCode\":" + Bill_From_State2 + "," +
              "\"fromStateCode\":" + Bill_From_State + "," +
              "\"toGstin\":\"" + Bill_To_GSTIn + "\"," +
              "\"toTrdName\":\"" + Bill_To_Name + "\"," +
              "\"toAddr1\":\"" + Bill_To_Add + "\"," +
              "\"toAddr2\":\"" + Bill_To_Add2 + "\"," +
              "\"toPlace\":\"" + Bill_To_Place + "\"," +
              "\"toPincode\":" + Bill_To_Pincode + "," +
               "\"actToStateCode\":" + Bill_To_State2 + "," +
              "\"toStateCode\":" + Bill_To_State + "," +
              "\"totalValue\":" + Taxable_Amt + "," +
              "\"cgstValue\":" + CGST_Amt + "," +
              "\"sgstValue\":" + SCGT_Amt + "," +
              "\"igstValue\":" + IGST_Amt + "," +
              "\"cessValue\":" + CESS_Advol_Amt + "," +
              "\"totInvValue\":" + Tot_Bill_Amt + "," +
              "\"transporterId\":\"" + Transport_Id + "\"," +
              "\"transporterName\":\"" + Transport_Name + "\"," +
              "\"transDocNo\":\"" + Trans_DocNo + "\"," +                     //as balnk value
              "\"transMode\":\"" + Tans_Mode + "\"," +
              "\"transDistance\":\"" + Distance + "\"," +
              "\"transDocDate\":\"" + Trans_Date + "\"," +
              "\"vehicleNo\":\"" + Vehical_No + "\"," +
              "\"vehicleType\":\"" + Vehical_Type + "\"," +
              "\"TransactionType\":\"" + Tran_Type + "\"," +
              "\"itemList\":[{" +
              "\"productName\":\"" + Product_Nmae + "\"," +
              "\"productDesc\":\"" + Product_Desc + "\"," +
              "\"hsnCode\":" + HSN + "," +
              "\"quantity\":" + Qty + "," +
              "\"qtyUnit\":\"" + Unit + "\"," +
              "\"cgstRate\":" + CGST_Ratenew + "," +
              "\"sgstRate\":" + SGST_Ratenew + "," +
              "\"igstRate\":" + IGST_Ratenew + "," +
              "\"cessRate\":" + CessRate + "," +
              "\"cessAdvol\":" + CessAdvol + "," +
              "\"taxableAmount\":" + Taxable_Value + "}]}";
        #endregion

        ////string username = "C1C2ED38DD58491597584CE1199B9ECF";
        ////string password = "5C14DBC6GAD4CG4FC4G9FE3GC2E4B88A8FCA";

        string username = "A8680BA035F14330BBCCD4723E38547C";
        string password = "9D2D7737G437EG442BG8D17G5E833C1CA4F1";



        string urlAddress = "https://gsp.adaequare.com/gsp/authenticate?grant_type=token";
       //// string urlAddress1 = " https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=GENEWAYBILL";
        string urlAddress1 = " https://gsp.adaequare.com/enriched/ewb/ewayapi?action=GENEWAYBILL";

        string USERNAME = "27AAJCD608_API_123";
        string PASSWORD = "Dash@1234";
        string gstin = "27AAJCD6087E1ZF";

        ////string USERNAME = "05AAACG2115R1ZN";
        ////string PASSWORD = "abc123@@";
        ////string gstin = "05AAACG2115R1ZN";

        // string DDate = DateTime.Parse(txtDoc_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyyMMdd");
        string DDate = DateTime.Now.ToString();
        string SBNo = txtDoc_No.Text;
        string requestid = DDate + SBNo + "SN";
        //string requestid = "shubhangi_unique_mkf587";//Auto generate no 
        string token = GetAuthToken(urlAddress, username, password);
        string Ewaybullno = GenrateEWaybill(urlAddress1, USERNAME, PASSWORD, gstin, requestid, token, json);


        string str = Ewaybullno;
        str = str.Replace("{", "");
        str = str.Replace("}", "");
        str = str.Replace(":", "");
        str = str.Replace(",", "");
        str = str.Replace("\"", "");
        string sub2 = "true";
        bool b = str.Contains(sub2);

        string sub4 = "false";
        bool s = str.Contains(sub4);
        string dist = "distance";

        string sub3 = "WARNING";
        bool n = str.Contains(sub3);
        Int64 ewaybillno = 0;
        string ewaybildate;
        if (b)
        {
            if (n)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('" + str + "');", true);
            }
            else
            {
                int index = str.IndexOf(sub2);
                if (index > 0)
                {
                    string EwaybillNo = getBetween(str, "ewayBillNo", "ewayBillDate");
                    ewaybillno = Convert.ToInt64(EwaybillNo);
                    string EwaybillDate = getBetween(str, "ewayBillDate", "validUpto");
                    EwaybillDate = EwaybillDate.Remove(EwaybillDate.Length - 9);
                    ewaybildate = DateTime.Parse(EwaybillDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    string sbno = txtDoc_No.Text;
                    string sbnoNew = txtDoc_No.Text;
                    //sbno = sbno.Remove(sbno.Length - 2);
                    DataSet ds = new DataSet();
                    qry = "";
                    qry1 = "";
                    qry = "update NT_1_deliveryorder set EWay_Bill_No=" + ewaybillno + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + Dono;
                    ds = clsDAL.SimpleQuery(qry);
                    qry1 = "update NT_1_SugarSale set Eway_Bill_No=" + ewaybillno + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and newsbno=" + NewSBNumber;
                    ds = clsDAL.SimpleQuery(qry1);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('EwayBill Generated Successfully !');", true);
                }

            }
        }
        else if (s)
        {
            int index = str.IndexOf(dist);
            if (index > 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Alert1", "alert('" + str + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('" + str + "');", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('" + str + "');", true);
        }




    }
    #endregion

    #region[update pincode]
    protected void btnUpdatePincode_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        qry = "update NT_1_AccountMaster set Pincode=" + txtBill_From_PinCode.Text + " where Ac_Code='" + hdnfmillCode.Value + "' and  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString());
        ds = clsDAL.SimpleQuery(qry);
        qry1 = "update NT_1_AccountMaster set Pincode=" + txtBill_To_PinCode.Text + " where Ac_Code='" + hdnfUnitCode.Value + "' and  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString());
        ds = clsDAL.SimpleQuery(qry1);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('successfully updated...!!');", true);
    }
    #endregion

    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }
        else
        {
            return "";
        }
    }
}

