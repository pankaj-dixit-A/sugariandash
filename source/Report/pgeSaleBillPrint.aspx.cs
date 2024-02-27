using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class Reports_pgeSaleBillPrint : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string user = string.Empty;
    int defaultAccountCode = 0;
    string searchString = "";
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "";
        tblDetails = tblPrefix + "";
        AccountMasterTable = tblPrefix + "AccountMaster";
        cityMasterTable = tblPrefix + "CityMaster";
        qryCommon = tblPrefix + "qrySugarSaleList";
        pnlPopup.Style["display"] = "none";

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
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                pnlBillNo.Visible = true;
                pnlDate.Visible = false;
                pnlParty.Visible = false;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    #region [btnParty_Click]
    protected void btnParty_Click(object sender, EventArgs e)
    {
        try
        {

            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtParty";
            btnSearch_Click(sender, e);

        }
        catch
        {
        }
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            if (hdnfClosePopup.Value == "txtParty")
            {

                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
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

            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            if (hdnfClosePopup.Value == "txtParty")
            {
                setFocusControl(txtParty);
            }
            hdnfClosePopup.Value = "Close";
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
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


    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].Width = new Unit("80px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }

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
                setFocusControl(btnSearch);
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
                        hdnpagecount.Value = grdPopup.PageCount.ToString();
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdnpagecount.Value = "0";
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

    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string s_item = drpFilter.SelectedValue;
            if (s_item == "B")
            {
                pnlBillNo.Visible = true;
                pnlDate.Visible = false;
                pnlParty.Visible = false;
            }
            if (s_item == "P")
            {
                pnlBillNo.Visible = false;
                pnlDate.Visible = false;
                pnlParty.Visible = true;
            }
            if (s_item == "D")
            {
                pnlBillNo.Visible = false;
                pnlDate.Visible = true;
                pnlParty.Visible = false;
            }
        }
        catch
        {

        }
    }
    protected void btnget_Click(object sender, EventArgs e)
    {

        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string qry = "";
            string whereCondition = "";
            if (drpFilter.SelectedValue == "B")
            {
                whereCondition = "  doc_no between " + txtFromNo.Text + " and " + txttoNo.Text;
            }
            if (drpFilter.SelectedValue == "P")
            {
                whereCondition = "  doc_date between '" + txtfromDt.Text + "' and '" + txttoDt.Text + "' and Ac_Code=" + txtParty.Text;
            }
            if (drpFilter.SelectedValue == "D")
            {
                whereCondition = "  doc_date between '" + txtfromDt1.Text + "' and '" + txttoDt1.Text + "' ";
            }
            qry = "select doc_no,doc_date,millname,PartyName,NETQNTL,Bill_Amount,PartyEmail from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " + whereCondition +
                " group by doc_no,doc_date,millname,PartyName,NETQNTL,Bill_Amount,PartyEmail";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grd.DataSource = dt;
                        grd.DataBind();
                    }
                    else
                    {
                        grd.DataSource = null;
                        grd.DataBind();
                    }
                }
                else
                {
                    grd.DataSource = null;
                    grd.DataBind();
                }
            }
            else
            {
                grd.DataSource = null;
                grd.DataBind();
            }
        }
        catch
        {

        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string billno = string.Empty;
            for (int i = 0; i < grd.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grd.Rows[i].Cells[7].FindControl("chk");
                if (chk.Checked == true)
                {
                    billno = billno + grd.Rows[i].Cells[0].Text + ",";
                }
            }
            billno = billno.Substring(0, billno.Length - 1);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "javascript:viewreport('" + billno + "')", true);
            //btnView.OnClientClick = "javascript:sp('" + vouchernos + "')";
        }
        catch
        {

        }
    }
    protected void txtParty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string partyName = string.Empty;
            if (txtParty.Text != string.Empty)
            {
                searchString = txtParty.Text;
                string qry = "";
                qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                partyName = clsCommon.getString(qry);

                if (partyName != string.Empty)
                {
                    lblPartyName.Text = partyName;
                    setFocusControl(txtfromDt);
                }
                else
                {
                    lblPartyName.Text = string.Empty;
                    txtParty.Text = string.Empty;
                    setFocusControl(txtParty);
                }
            }
            else
            {
                setFocusControl(txtParty);
            }
        }
        catch
        {

        }
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

}