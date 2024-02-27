using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeClubAccount : System.Web.UI.Page
{
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    string accountmasterlist = string.Empty;
    string AccountMasterTable = string.Empty;
    static WebControl objAsp = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        pnlPopup.Style["display"] = "none";
        accountmasterlist = tblPrefix + "qryAccountsList";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                //pnlPopup.Style["display"] = "none";
                //ViewState["currentTable"] = null;
                //clsButtonNavigation.enableDisable("N");
                //this.makeEmptyForm("N");
                //ViewState["mode"] = "I";
                //this.showLastRecord();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void txtac_code_TextChanged(object sender, EventArgs e)
    {

        string acname = "";
        if (txtac_code.Text != string.Empty)
        {
            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtac_code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (acname != string.Empty)
            {

                lblParty_name.Text = acname;
                setFocusControl(txtWrongAccoun);
            }
            else
            {
                txtac_code.Text = string.Empty;
                lblParty_name.Text = acname;
                setFocusControl(txtac_code);
            }
        }
        else
        {
            setFocusControl(txtac_code);
        }
    }
    protected void btntxtac_code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtac_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtWrongAccoun_TextChanged(object sender, EventArgs e)
    {
        string acname = "";
        if (txtWrongAccoun.Text != string.Empty)
        {
            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtWrongAccoun.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (acname != string.Empty)
            {

                lblWrongAccount.Text = acname;
                setFocusControl(btnClubAccount);
            }
            else
            {
                txtac_code.Text = string.Empty;
                lblWrongAccount.Text = acname;
                setFocusControl(txtWrongAccoun);
            }
        }
        else
        {
            setFocusControl(txtWrongAccoun);
        }
    }
    protected void btnWrongAccount_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtWrongAccoun";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btnClubAccount_Click(object sender, EventArgs e)
    {

    }
    protected void imgBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtac_code")
            {
                setFocusControl(txtac_code);
            }
            if (hdnfClosePopup.Value == "txtWrongAccoun")
            {
                setFocusControl(txtWrongAccoun);
            }
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
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            if (hdnfClosePopup.Value == "txtac_code" || hdnfClosePopup.Value == "txtWrongAccoun")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + accountmasterlist + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    "  and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }

        }
        catch
        {

        }
    }

    private void showPopup(string qry)
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

    }
}