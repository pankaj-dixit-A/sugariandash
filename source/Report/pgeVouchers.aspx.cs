using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_pgeLocalvoucher : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    int defaultAccountCode = 0;
    string searchString = "";
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "";
        tblDetails = tblPrefix + "";
        AccountMasterTable = tblPrefix + "AccountMaster";
        cityMasterTable = tblPrefix + "CityMaster";

        //qryCommon = tblPrefix + "qrySugarSaleList";
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
                pnlDate.Visible = true;
                pnlParty.Visible = false;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
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
                    setFocusControl(txtfromDt1);
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
        catch { }
    }
    protected void btnGetVoucher_Click(object sender, EventArgs e)
    {
        try
        {
            string Fromdt = "";
            string Todt = "";
            if (txtfromDt1.Text != string.Empty)
            {
                Fromdt = DateTime.Parse(txtfromDt1.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            }
            else
            {
                Fromdt = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            }
            if (txttoDt1.Text != string.Empty)
            {
                Todt = DateTime.Parse(txttoDt1.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            }
            else
            {
                Todt = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            }

            string s_item = drpSelectOption.SelectedValue;
            string c_item = drpCategory.SelectedValue;
            //if (s_item == "P")
            //{
            string ac_code = txtParty.Text;
            pnlPopup.Style["display"] = "none";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + ac_code + "','" + Fromdt + "','" + Todt + "','" + c_item + "')", true);
            //}
        }
        catch
        {
        }

    }
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
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
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
    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
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


    protected void drpSelectOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string s_item = drpSelectOption.SelectedValue;
            if (s_item == "P")
            {
                pnlDate.Visible = true;
                pnlParty.Visible = true;
            }
            if (s_item == "D")
            {
                pnlDate.Visible = true;
                pnlParty.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}