using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_pgeTransportAdvance : System.Web.UI.Page
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
            uptodate = DateTime.Now.ToString("yyyy/MM/dd");
            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    fillBranches();
                    txtFromDate.Text = clsGV.Start_Date;
                    txtToDate.Text = clsGV.End_Date;
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

    private void fillBranches()
    {
        try
        {
            ListItem li = new ListItem("All", "0");
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string qry = "select * from BranchMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            ds = clsDAL.SimpleQuery(qry);
            drpBranch.Items.Clear();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        drpBranch.DataSource = dt;
                        drpBranch.DataTextField = "Branch";
                        drpBranch.DataValueField = "Branch_Id";
                        drpBranch.DataBind();
                    }
                }
            }
            drpBranch.Items.Insert(0, li);
        }
        catch
        {

        }
    }
    private string BranchCode()
    {
        try
        {
            string branchname = drpBranch.SelectedItem.ToString();
            qry = "select Branch_Id from BranchMaster where Branch='" + branchname + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
            Branch_Code = clsCommon.getString(qry);
        }
        catch (Exception)
        {
            throw;
        }
        return Branch_Code;
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
                    btntxtAC_CODE_Click(this, new EventArgs());
                }
                else
                {
                    searchString = txtAC_CODE.Text;
                    string qry = "select Ac_Name_E from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code='" + txtAC_CODE.Text + "' and Ac_Type='T'";
                    bankName = clsCommon.getString(qry);
                    if (bankName != string.Empty)
                    {
                        lblAc_Name.Text = bankName;
                    }
                    else
                    {
                        lblAc_Name.Text = string.Empty;
                        txtAC_CODE.Text = string.Empty;
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
    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            BranchCode();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ki", "javascript:dr('" + txtAC_CODE.Text + "','" + Branch_Code + "','" + txtFromDate.Text + "','" + txtToDate.Text + "');", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnOnlyBalance_Click(object sender, EventArgs e)
    {
        try
        {
            BranchCode();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ki", "javascript:br('" + txtAC_CODE.Text + "','" + Branch_Code + "','" + txtFromDate.Text + "','" + txtToDate.Text + "');", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
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
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_Type='T' and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
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
}