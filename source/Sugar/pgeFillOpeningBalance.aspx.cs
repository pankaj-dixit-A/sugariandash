using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeFillOpeningBalance : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string Branch_Code = string.Empty;
    string uptodate = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (User_Type == "A")
                {
                    //fillBranches();
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btntxtAC_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAC_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtAC_CODE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string bankName = string.Empty;
            if (txtAC_CODE.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
                if (a == false)
                {
                    searchString = txtAC_CODE.Text;
                    btntxtAC_CODE_Click(this, new EventArgs());
                }
                else
                {
                    searchString = txtAC_CODE.Text;
                    string qry = "select Ac_Name_E from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code='" + txtAC_CODE.Text + "'";
                    bankName = clsCommon.getString(qry);
                    if (bankName != string.Empty)
                    {
                        lblAc_Name.Text = bankName;
                        pnlPopup.Style["display"] = "none";

                        string opBal = clsCommon.getString("Select Opening_Balance from " + tblPrefix + "AccountMaster where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtOpeningBalance.Text = opBal;
                        string Drcr = clsCommon.getString("Select Drcr from " + tblPrefix + "AccountMaster where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        drpDrCr.SelectedValue = Drcr;
                        setFocusControl(txtOpeningBalance);
                    }
                    else
                    {
                        lblAc_Name.Text = string.Empty;
                        txtAC_CODE.Text = string.Empty;
                        setFocusControl(txtAC_CODE);
                    }
                }
            }
            else
            {
                this.setFocusControl(txtAC_CODE);
            }
        }
        catch
        {
        }
    }

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
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (v == "txtAC_CODE")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }
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
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    protected void imgBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                setFocusControl(txtAC_CODE);
            }
            hdnfClosePopup.Value = "Close";
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
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
    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    protected void drpDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(btnUpdate);
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                bool isValidated = true;

                if (txtAC_CODE.Text != string.Empty)
                {
                    string qry = "select Ac_Name_E from " + AccountMasterTable +
                   " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code='" + txtAC_CODE.Text + "'";
                    string str = clsCommon.getString(qry);
                    if (str != string.Empty)
                    {
                        isValidated = true;
                        lblAc_Name.Text = str;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtAC_CODE);
                        return;
                    }
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtAC_CODE);
                    return;
                }


                //if (txtOpeningBalance.Text != string.Empty)
                //{
                //    double OP = txtOpeningBalance.Text != string.Empty ? Convert.ToDouble(txtOpeningBalance.Text) : 0.00;
                //    if (OP != 0)
                //    {
                //        isValidated = true;
                //    }
                //    else
                //    {
                //        isValidated = false;
                //        setFocusControl(txtOpeningBalance);
                //        return;
                //    }
                //}
                //else
                //{
                //    isValidated = false;
                //    setFocusControl(txtOpeningBalance);
                //    return;
                //}



                Int32 Ac_Code = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
                double OPENING_BALANCE = txtOpeningBalance.Text != string.Empty ? Convert.ToDouble(txtOpeningBalance.Text) : 0.00;
                string DRCR = drpDrCr.SelectedValue;
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    try
                    {
                        string rev = "";
                        obj.flag = 2;
                        obj.tableName = AccountMasterTable;
                        obj.columnNm = "Opening_Balance='" + OPENING_BALANCE + "',Drcr='" + DRCR + "' where Ac_Code='" + Ac_Code + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        obj.values = "none";
                        ds = new DataSet();
                        ds = obj.insertAccountMaster(ref rev);
                        string retValue = rev;

                        string getmax = "select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "GLEDGER where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                        string maxdocno = clsCommon.getString(getmax);

                        qry = "delete from " + tblPrefix + "GLEDGER where TRAN_TYPE='OP' and AC_CODE=" + Ac_Code + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                        ds = clsDAL.SimpleQuery(qry);
                        if (OPENING_BALANCE != 0)
                        {
                            obj.flag = 1;
                            obj.tableName = tblPrefix + "GLEDGER";
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT";
                            obj.values = " 'OP','" + maxdocno + "','2015/03/31','" + Ac_Code + "','OPENING_BALANCE', " + OPENING_BALANCE + ",null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "',0,'" + DRCR + "',0,0";
                            ds = obj.insertAccountMaster(ref rev);
                        }

                        //string GlAc_Code = clsCommon.getString("Select AC_CODE from " + tblPrefix + "GLEDGER where TRAN_TYPE='OP' and DOC_NO=" + Ac_Code + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //if (GlAc_Code == string.Empty)
                        //{
                        //    if (OPENING_BALANCE != 0)
                        //    {
                        //        obj.flag = 1;
                        //        obj.tableName = tblPrefix + "GLEDGER";
                        //        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT";
                        //        obj.values = " 'OP','" + Ac_Code + "','2015/03/31','" + Ac_Code + "','OPENING_BALANCE', " + OPENING_BALANCE + ",null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "',0,'" + DRCR + "',0,0";
                        //        ds = obj.insertAccountMaster(ref rev);
                        //    }
                        //}
                        //else
                        //{
                        //    obj.flag = 2;
                        //    obj.tableName = tblPrefix + "GLEDGER";
                        //    obj.columnNm = "AMOUNT='" + OPENING_BALANCE + "',DOC_DATE='2015/03/31',DRCR='" + DRCR + "' where  COMPANY_CODE='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and AC_CODE=" + Ac_Code + " AND TRAN_TYPE='OP'";
                        //    ds = obj.insertAccountMaster(ref rev);
                        //}

                        Response.Redirect("~/Sugar/pgeFillOpeningBalance.aspx", false);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert(' Successfully Updated !')", true);
                    }
                    catch (Exception)
                    {
                        throw;
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