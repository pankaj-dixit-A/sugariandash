using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;


public partial class pgeGLedgerReport : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string AccountMasterTable = string.Empty;
    string searchStr = "";
    string strTextbox = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                txtFromDt.Text = clsGV.Start_Date;
                txtToDt.Text = clsGV.To_date;
                drpFilter.SelectedValue = "A";
                pnlAcNameWise.Visible = true;
                pnlBSGroupWise.Visible = false;
                setFocusControl(txtAcCode);
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnAcCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAcCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchStr != string.Empty && strTextbox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchStr;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtAcCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account--";
                string qry = "select a.Ac_Code as [Account Code], a.Ac_Name_E as [Account Name],a.Short_Name as [Short Name],CityName,ISNULL(SUM(case g.DRCR when 'D' then g.AMOUNT when 'C' then -g.AMOUNT end),0) as Balance from " + tblPrefix + "qryAccountsList a LEFT OUTER JOIN NT_1_GLEDGER g ON a.Ac_Code=g.AC_CODE and a.Company_Code=g.COMPANY_CODE where (a.Ac_Code like '%" + txtSearchText.Text + "%' or a.Ac_Name_E like '%" + txtSearchText.Text + "%' or a.Short_Name like '%" + txtSearchText.Text + "%' or a.CityName like '%" + txtSearchText.Text + "%') and a.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' group by a.Ac_Name_E,a.Ac_Code,a.Short_Name,a.CityName order by a.Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnitCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account Unit--";
                string qry = "select a.Ac_Code as [Account Code], a.Ac_Name_E as [Account Name],a.Short_Name as [Short Name],CityName,ISNULL(SUM(case g.DRCR when 'D' then g.AMOUNT when 'C' then -g.AMOUNT end),0) as Balance from " + tblPrefix + "qryAccountsList a LEFT OUTER JOIN NT_1_GLEDGER g ON a.Ac_Code=g.AC_CODE and a.Company_Code=g.COMPANY_CODE where (a.Ac_Code like '%" + txtSearchText.Text + "%' or a.Ac_Name_E like '%" + txtSearchText.Text + "%' or a.Short_Name like '%" + txtSearchText.Text + "%' or a.CityName like '%" + txtSearchText.Text + "%') and a.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' group by a.Ac_Name_E,a.Ac_Code,a.Short_Name,a.CityName order by a.Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBSGroupCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Group--";
                string qry = "select [group_Code], [group_Name_E] as [Group Name] from " + tblPrefix + "BSGroupMaster  where (group_Code like '%" + txtSearchText.Text + "%' or group_Name_E like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.showPopup(qry);
            }
        }
        catch
        {

        }
    }
    #endregion

    protected void btndepri_Click(object sender, EventArgs e)
    {
        string frmdt = clsGV.Start_Date;
        string todt = clsGV.End_Date;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asdas1", "javascript:depr('" + frmdt + "','" + todt + "')", true);
    }

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            //    hdnfClosePopup.Value = "Close";
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

                searchStr = txtSearchText.Text;
                strTextbox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
            }
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
                e.Row.Attributes["onselectstart"] = "javascript:return true;";

                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    private void showPopup(string qry)
    {
        try
        {

            this.setFocusControl(txtSearchText);

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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion


    #region [btnGetData_Click]
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (drpFilter.SelectedValue == "A")
            {
                string accode = txtAcCode.Text;

                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + accode + "','" + fromdt + "','" + todt + "','DrCr')", true);
            }
            if (drpFilter.SelectedValue == "G")
            {
                string groupcode = txtBSGroupCode.Text;
                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp_multipleLedger('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
            }
            else
            {
            }
        }
        catch (Exception eex)
        {

        }
    }
    #endregion

    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtAcCode.Text != string.Empty)
            {
                searchStr = txtAcCode.Text;
                strTextbox = "txtAcCode";

                bool a = clsCommon.isStringIsNumeric(txtAcCode.Text);
                if (a == false)
                {
                    btnAcCode_Click(this, new EventArgs());
                }
                else
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAcCode.Text);
                    if (str != string.Empty)
                    {
                        lblAcCodeName.Text = str;
                        string mob = clsCommon.getString("select Mobile_No from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAcCode.Text);
                        txtMobileNo.Text = mob;
                        string balance = clsCommon.getString("select SUM(case DRCR when 'D' then AMOUNT when 'C' then -AMOUNT end) as Balance from " + tblPrefix + "GLEDGER" +
                                        " where AC_CODE=" + txtAcCode.Text + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString()) + " group by AC_CODE");
                        txtBalance.Text = balance;
                        setFocusControl(txtUnitCode);
                        pnlPopup.Style["display"] = "none";
                    }
                    else
                    {
                        txtAcCode.Text = string.Empty;
                        lblAcCodeName.Text = string.Empty;
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

    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpFilter.SelectedValue == "A")
            {
                pnlAcNameWise.Visible = true;
                pnlBSGroupWise.Visible = false;
                setFocusControl(txtAcCode);
            }
            else
            {
                pnlAcNameWise.Visible = false;
                pnlBSGroupWise.Visible = true;
                setFocusControl(txtBSGroupCode);
            }
        }
        catch
        {

        }
    }

    protected void txtBSGroupCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtBSGroupCode.Text != string.Empty)
            {
                searchStr = txtBSGroupCode.Text;
                strTextbox = "txtBSGroupCode";
                string str = clsCommon.getString("select [group_Name_E] from " + tblPrefix + "BSGroupMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [group_Code]=" + txtBSGroupCode.Text);
                if (str != string.Empty)
                {
                    lblGroupName.Text = str;
                    setFocusControl(txtFromDt);
                    pnlPopup.Style["display"] = "none";
                }
                else
                {
                    txtBSGroupCode.Text = string.Empty;
                    lblGroupName.Text = string.Empty;
                    setFocusControl(txtBSGroupCode);
                }
            }
        }
        catch
        {

        }
    }

    private void csCalculations()
    {
        try
        {

        }
        catch
        {

        }
    }

    protected void btnGroupCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBSGroupCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("250px");
            e.Row.Cells[2].Width = new Unit("100px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    protected void btnGroupWise_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (txtUnitCode.Text != string.Empty)
            {
                if (txtAcCode.Text == string.Empty)
                {
                    setFocusControl(txtAcCode);
                    return;
                }
            }
            if (drpFilter.SelectedValue == "A")
            {
                string accode = txtAcCode.Text;
                string unit_Code = txtUnitCode.Text;
                pnlPopup.Style["display"] = "none";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:gw('" + accode + "','" + unit_Code + "','" + fromdt + "','" + todt + "','DrCr')", true);
            }
            if (drpFilter.SelectedValue == "G")
            {
                string groupcode = txtBSGroupCode.Text;
                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp_multipleLedger('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
            }
            else
            {
            }
        }
        catch (Exception eex)
        {

        }
    }

    

    protected void btnUnitWise_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (txtUnitCode.Text != string.Empty)
            {
                if (txtAcCode.Text == string.Empty)
                {
                    setFocusControl(txtAcCode);
                    return;
                }
            }
            if (drpFilter.SelectedValue == "A")
            {
                string accode = txtAcCode.Text;
                string unit_Code = txtUnitCode.Text;
                pnlPopup.Style["display"] = "none";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:uw('" + accode + "','" + unit_Code + "','" + fromdt + "','" + todt + "')", true);
            }
            if (drpFilter.SelectedValue == "G")
            {
                string groupcode = txtBSGroupCode.Text;
                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp_multipleLedger('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
            }
            else
            {
            }
        }
        catch (Exception eex)
        {

        }
    }
    protected void txtUnitCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtUnitCode.Text != string.Empty)
            {
                searchStr = txtUnitCode.Text;
                strTextbox = "txtUnitCode";
                bool a = clsCommon.isStringIsNumeric(txtUnitCode.Text);
                if (a == false)
                {
                    btnUnitCode_Click(this, new EventArgs());
                }
                else
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtUnitCode.Text);
                    if (str != string.Empty)
                    {
                        lblUnitName.Text = str;
                        setFocusControl(txtFromDt);
                        pnlPopup.Style["display"] = "none";
                    }
                    else
                    {
                        txtUnitCode.Text = string.Empty;
                        lblUnitName.Text = string.Empty;
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void btnUnitCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUnitCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btnOnlyCr_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (drpFilter.SelectedValue == "A")
            {
                string accode = txtAcCode.Text;

                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kyasd", "javascript:sp('" + accode + "','" + fromdt + "','" + todt + "','Cr')", true);
            }
            if (drpFilter.SelectedValue == "G")
            {
                string groupcode = txtBSGroupCode.Text;
                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kyasa", "javascript:sp_multipleLedger('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
            }
            else
            {
            }
        }
        catch (Exception eex)
        {

        }
    }
    protected void btnOnlyDr_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (drpFilter.SelectedValue == "A")
            {
                string accode = txtAcCode.Text;

                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kasy", "javascript:sp('" + accode + "','" + fromdt + "','" + todt + "','Dr')", true);
            }
            if (drpFilter.SelectedValue == "G")
            {
                string groupcode = txtBSGroupCode.Text;
                pnlPopup.Style["display"] = "none";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp_multipleLedger('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
            }
            else
            {
            }
        }
        catch (Exception eex)
        {

        }
    }
    private void datefunction()
    {
        if (txtFromDt.Text != string.Empty)
        {
            fromDT = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            fromDT = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (txtToDt.Text != string.Empty)
        {
            toDT = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            toDT = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
    }
    protected void btnDispMillWise_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            string Mill_Code = txtAcCode.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "posiasd", "javascript:dispmillwise('" + fromDT + "','" + toDT + "','" + Mill_Code + "')", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnShortPayment_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            string Broker_Code = txtAcCode.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ktasd", "javascript:bsp('" + Broker_Code + "','" + fromDT + "','" + toDT + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ksa", "javascript:bwspd('" + Broker_Code + "','" + fromDT + "','" + toDT + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ksasaa", "javascript:bwspdzero('" + Broker_Code + "','" + fromDT + "','" + toDT + "')", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnTransportAc_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            string Transport = txtAcCode.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgysdaf", "javascript:TBR('" + fromDT + "','" + toDT + "','" + Transport + "','')", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnDaywiseSummary_Click(object sender, EventArgs e)
    {
        datefunction();
        string ac_code = txtAcCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "poi", "javascript:DispSummary('" + fromDT + "','" + toDT + "','','" + ac_code + "')", true);
        pnlPopup.Style["display"] = "none";
    }
    protected void btnAccountWise_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            string ac_code = txtAcCode.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyaf", "javascript:PWDO('" + fromDT + "','" + toDT + "','','" + ac_code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hassdaf", "javascript:PWDOM('" + fromDT + "','" + toDT + "','','" + ac_code + "')", true);
            pnlPopup.Style["display"] = "none";
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnEnvelopPrint_Click(object sender, EventArgs e)
    {
        if (txtAcCode.Text.Trim().ToString() == string.Empty)
        {
            setFocusControl(txtAcCode);
            return;
        }

        string ac_code = txtAcCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asdasda", "javascript:envelop('" + ac_code + "')", true);
    }

    protected void btnFrtRegister_Click(object sender, EventArgs e)
    {
        if (txtAcCode.Text.Trim().ToString() == string.Empty)
        {
            setFocusControl(txtAcCode);
            return;
        }
        if (txtFromDt.Text != string.Empty)
        {
            fromDT = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        }
        else
        {
            fromDT = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        }
        if (txtToDt.Text != string.Empty)
        {
            toDT = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        }
        else
        {
            toDT = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        }

        string ac_code = txtAcCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asdas", "javascript:frtregi('" + ac_code + "','" + fromDT + "','" + toDT + "')", true);
    }

    protected void btnMultipleLedger_Click(object sender, EventArgs e)
    {
           try
            {
                string fromdt = txtFromDt.Text;
                string todt = txtToDt.Text;
                fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                
               string type = drpFilter.SelectedValue;
                
                    string groupcode = txtBSGroupCode.Text;
                    pnlPopup.Style["display"] = "none";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:party('" + type + "','" + fromdt + "','" + todt + "','DrCr')", true);
                  //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "z", "javascript:party('" + fromdt + "','" + todt + "','" + groupcode + "')", true);
                
            }
            catch (Exception eex)
            {

            }
        
    }

    protected void btnDayBook_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");




            string strdays = clsCommon.getString("select datediff(day,'" + fromdt + "','" + todt + "') as dayss");

            int days = Convert.ToInt32(strdays);



            if (days < 35)
            {
                pnlPopup.Style["display"] = "none";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "das", "javascript:daybook('" + fromdt + "','" + todt + "')", true);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "das1", "javascript:meaasge()", true);
            }
        }
        catch (Exception eex)
        {

        }
    }
}