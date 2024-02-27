using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgePendingVouchers : System.Web.UI.Page
{
    string qry = string.Empty;
    string tblPrefix = string.Empty;
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    string GLedgerTable = string.Empty;
    string AccountMasterTable = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        GLedgerTable = tblPrefix + "GLEDGER";
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {

            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
            this.FillGrid();
        }
    }

    private void FillGrid()
    {
        qry = "select T.Tender_No,Convert(varchar(10),T.Tender_Date,103) AS DO_Date,A.Ac_Name_E as Mill,T.Quantal as Qntl,B.Ac_Name_E as Tender_From,C.Ac_Name_E as Voucher_By,T.Mill_Rate,T.Purc_Rate,(T.Quantal*(T.Mill_Rate-T.Purc_Rate)) as Amount from " + tblPrefix + "Tender T Left outer join " + tblPrefix + "AccountMaster A" +
            " on T.Mill_Code=A.Ac_Code and A.Company_Code=T.Company_Code left outer join " + tblPrefix + "AccountMaster B on B.Ac_Code=T.Tender_From and B.Company_Code=T.Company_Code" +
               " Left Outer join " + tblPrefix + "AccountMaster C on T.Voucher_By=C.Ac_Code and C.Company_Code=T.Company_Code where (T.Mill_Rate<T.Purc_Rate) and T.Voucher_No=0 AND T.type='R' and T.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and T.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {

                    grdPendingVouchers.DataSource = dt;
                    grdPendingVouchers.DataBind();
                }
                else
                {
                    grdPendingVouchers.DataSource = null;
                    grdPendingVouchers.DataBind();
                }
            }
        }
    }
    protected void grdPendingVouchers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("40px");
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;

            CheckBox chk = (CheckBox)e.Row.FindControl("grdCB");
            TextBox txtBillNo = (TextBox)e.Row.FindControl("txtBillNo");
            TextBox txtBillDate = (TextBox)e.Row.FindControl("txtBillDate");

            chk.Attributes["onclick"] = string.Format("ChangeQuantityEnable('{0}', this.checked);", txtBillDate.ClientID);
            //chk.Attributes["onclick"] = string.Format("EnableBillNo('{0}', this.checked);", txtBillNo.ClientID);
        }
    }
    private void CreatingDebitNote()
    {
        try
        {
            string tenderno = string.Empty;
            string str = "";
            string Tender_Date = string.Empty;
            string Tender_From = string.Empty;
            string Tender_No = string.Empty;
            string Mill_Code = string.Empty;
            string Grade = string.Empty;
            string Quantal = string.Empty;
            string Packing = string.Empty;
            string Bags = string.Empty;
            string Payment_To = string.Empty;
            string Voucher_By = string.Empty;
            string Tender_DO = string.Empty;
            string Broker = string.Empty;
            string Mill_Rate = string.Empty;
            double Purc_Rate = 0.00;
            double Diff_Amount = 0.00;
            double VOUCHERAMOUNT = 0.00;
            string Narration = string.Empty;
            string Bill_No = string.Empty;
            string Bill_Date = string.Empty;
            string PartyName = string.Empty;
            string myNarration = string.Empty;
            if (grdPendingVouchers.Rows.Count > 0)
            {
                for (int i = 0; i < grdPendingVouchers.Rows.Count; i++)
                {
                    CheckBox chk = grdPendingVouchers.Rows[i].Cells[9].FindControl("grdCB") as CheckBox;
                    if (chk != null && chk.Checked == true)
                    {
                        tenderno = grdPendingVouchers.Rows[i].Cells[0].Text.ToString();
                        TextBox txtBillNo = grdPendingVouchers.Rows[i].Cells[10].FindControl("txtBillNo") as TextBox;
                        TextBox txtBillDate = grdPendingVouchers.Rows[i].Cells[11].FindControl("txtBillDate") as TextBox;
                        Bill_No = txtBillNo.Text.ToString();
                        Bill_Date = txtBillDate.Text.ToString();
                        PartyName = Server.HtmlDecode(grdPendingVouchers.Rows[i].Cells[4].Text.ToString());
                        if (Bill_No != string.Empty || Bill_Date != string.Empty)
                        {
                            qry = "Select *,Convert(varchar(10),Tender_Date,102) as Tender_Date1 from " + tblPrefix + "Tender Where Tender_No=" + tenderno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        Tender_Date = dt.Rows[0]["Tender_Date1"].ToString();//System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                                        Tender_From = dt.Rows[0]["Tender_From"].ToString();
                                        Tender_No = dt.Rows[0]["Tender_No"].ToString();
                                        Mill_Code = dt.Rows[0]["Mill_Code"].ToString();
                                        Grade = dt.Rows[0]["Grade"].ToString();
                                        Quantal = dt.Rows[0]["Quantal"].ToString();
                                        Packing = dt.Rows[0]["Packing"].ToString();
                                        Bags = dt.Rows[0]["Bags"].ToString();
                                        Payment_To = dt.Rows[0]["Payment_To"].ToString();
                                        Voucher_By = dt.Rows[0]["Voucher_By"].ToString();
                                        Tender_DO = dt.Rows[0]["Tender_DO"].ToString();
                                        Mill_Rate = dt.Rows[0]["Mill_Rate"].ToString();
                                        Broker = dt.Rows[0]["Broker"].ToString();
                                        Purc_Rate = Convert.ToDouble(dt.Rows[0]["Purc_Rate"].ToString());
                                        double diff_rate = double.Parse(Mill_Rate) - Purc_Rate;
                                        Diff_Amount = diff_rate * Convert.ToDouble(Quantal);

                                        VOUCHERAMOUNT = double.Parse(Quantal) * diff_rate;
                                        Narration = dt.Rows[0]["Narration"].ToString();
                                        string millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Mill_Code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                        if (Purc_Rate > 0)
                                        {
                                            myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + Mill_Rate + " P.R." + Purc_Rate + ")";
                                        }
                                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                                        {
                                            int docno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher where Tran_Type='LV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                            obj.flag = 1;
                                            obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code,Tender_No,Mill_Code,Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE,Mill_Rate,Purchase_Rate,Diff_Amount,Voucher_Amount,Narration1,Diff_Type,Bill_No,Bill_Date";
                                            obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + string.Empty.Trim() + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tender_No + "','" + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" + Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate + "','" + Purc_Rate + "','" + Diff_Amount + "','" + VOUCHERAMOUNT + "','" + Narration + myNarration + "','TD','" + Bill_No + "','" + Bill_Date + "'";
                                            obj.tableName = "" + tblPrefix + "Voucher";
                                            obj.insertAccountMaster(ref str);

                                            qry = "update " + tblPrefix + "Tender Set Voucher_No=" + docno + " WHERE Tender_No=" + tenderno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                                            ds = clsDAL.SimpleQuery(qry);


                                            //else
                                            //{
                                            //    myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + Mill_Rate + " P.R." +  + ")";
                                            //}

                                            #region Gleger effect
                                            qry = "";
                                            qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + "LV" + "' and DOC_NO=" + docno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                            ds = clsDAL.SimpleQuery(qry);
                                            Int32 GID = 0;

                                            if (VOUCHERAMOUNT > 0)
                                            {
                                                GID = GID + 1;
                                                obj.flag = 1;
                                                obj.tableName = GLedgerTable;
                                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                                                obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + Narration + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                                ds = obj.insertAccountMaster(ref str);
                                            }
                                            else
                                            {
                                                GID = GID + 1;
                                                obj.flag = 1;
                                                obj.tableName = GLedgerTable;
                                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                                                obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + Narration + myNarration + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','C','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                                ds = obj.insertAccountMaster(ref str);
                                            }
                                            // diffrance amount effect
                                            if (Diff_Amount > 0)
                                            {
                                                //------------Credit effect
                                                GID = GID + 1;
                                                obj.flag = 1;
                                                obj.tableName = GLedgerTable;
                                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                                                obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + Narration + myNarration + " " + PartyName + " " + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";
                                                ds = obj.insertAccountMaster(ref str);
                                            }
                                            else
                                            {
                                                //------------Credit effect
                                                GID = GID + 1;
                                                obj.flag = 1;
                                                obj.tableName = GLedgerTable;
                                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                                                obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + Narration + " " + PartyName + " " + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";
                                                ds = obj.insertAccountMaster(ref str);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblErrormsg.Text = "Please Enter Bill No or Date!";
                        }
                    }

                }
                this.FillGrid();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ki", "javascript:alert('" + "Vouchers Successfully Created" + "');", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnCreateVoucher_Click(object sender, EventArgs e)
    {
        CreatingDebitNote();
    }
}