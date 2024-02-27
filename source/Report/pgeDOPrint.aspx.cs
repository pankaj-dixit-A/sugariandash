using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class Report_pgeDOPrint : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string AccountMasterTable = string.Empty;
    string searchStr = "";
    string strTextBox = string.Empty;
    string tblHead = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "deliveryorder";
        AccountMasterTable = tblPrefix + "AccountMaster";
        isAuthenticate = Security.Authenticate(tblPrefix, user);
        string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
        if (!Page.IsPostBack)
        {
            if (isAuthenticate == "1" || User_Type == "A")
            {
                txtFromDt.Text = clsGV.Start_Date;
                txtToDt.Text = clsGV.To_date;
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = new Unit("120px");
                e.Row.Cells[2].Width = new Unit("320px");
                e.Row.Cells[3].Width = new Unit("320px");
                e.Row.Cells[4].Width = new Unit("100px");
                e.Row.Cells[5].Width = new Unit("100px");
                e.Row.Cells[6].Width = new Unit("220px");
                e.Row.Cells[7].Width = new Unit("200px");
                //e.Row.Cells[8].Width = new Unit("30px");
                //e.Row.Cells[8].Visible = false;
                //e.Row.Cells[9].Visible = false;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[2].Style["overflow"] = "hidden";
                //e.Row.Cells[2].ControlStyle.Width = Unit.Pixel(120);
                //e.Row.Cells[3].Style["overflow"] = "hidden";

                //e.Row.Cells[3].ControlStyle.Width = Unit.Pixel(120);
                //e.Row.Cells[4].ControlStyle.Width = Unit.Pixel(50);
                //e.Row.Cells[5].ControlStyle.Width = Unit.Pixel(50);
                //e.Row.Cells[6].ControlStyle.Width = Unit.Pixel(50);
                //e.Row.Cells[0].ControlStyle.Width = Unit.Pixel(30);
                //e.Row.Cells[9].ControlStyle.Width = Unit.Pixel(20);
                //e.Row.Cells[8].ControlStyle.Width = Unit.Pixel(90);
                //e.Row.Cells[7].ControlStyle.Width = Unit.Pixel(90);
                //e.Row.Cells[1].ControlStyle.Width = Unit.Pixel(50);
                //e.Row.Cells[10].ControlStyle.Width = Unit.Pixel(30);
                //e.Row.Cells[11].ControlStyle.Width = Unit.Pixel(20);
                //if (e.Row.RowType != DataControlRowType.Header)
                //    e.Row.Cells[10].Text = "N";
            }
        }
        catch
        {

        }
    }

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

    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {

            pnlPopup.Style["display"] = "block";
            setFocusControl(btnSearch);

        }
        catch
        {

        }
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        //objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSearchText.Text == string.Empty)
            {
                txtSearchText.Text = searchStr;
            }
            pnlPopup.Style["display"] = "block";
            lblPopupHead.Text = "--Select Account--";
            string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

            this.showPopup(qry);
        }
        catch
        {

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


    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].Width = new Unit("80px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    #endregion

    protected void chkselectAll_checkchanged(object sender, EventArgs e)
    {
        //try
        //{
        //    CheckBox chkselectAll = (CheckBox)grdDetail.HeaderRow.Cells[9].FindControl("chkSelectAll");

        //    for (int i = 0; i < grdDetail.Rows.Count; i++)
        //    {
        //        CheckBox chkIsPrint = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
        //        if (chkselectAll.Checked)
        //        {
        //            chkIsPrint.Checked = true;
        //        }
        //        else
        //        {
        //            chkIsPrint.Checked = false;
        //        }
        //    }
        //}
        //catch
        //{

        //}
    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {

        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string do_no = string.Empty;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdDetail.Rows[i].Cells[7].FindControl("chkIsPrint");
                if (chk.Checked == true)
                {
                    do_no = do_no + grdDetail.Rows[i].Cells[0].Text + ",";
                }
            }
            do_no = do_no.Substring(0, do_no.Length - 1);
            string email = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + txtmillCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scripts", "javascript:doprint('" + do_no + "','" + email + "')", true);
        }
        catch
        {

        }
    }
    protected void btnmillcode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmillCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        try
        {
            string whrCondition = "";
            string fromDt = "";
            string toDt = "";
            string MILL_CODE = txtmillCode.Text;
            if (txtmillCode.Text != null)
            {
                if (txtFromDt.Text != string.Empty)
                {
                    fromDt = DateTime.Parse(txtFromDt.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    setFocusControl(txtFromDt);
                }
                if (txtToDt.Text != string.Empty)
                {
                    toDt = DateTime.Parse(txtToDt.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    setFocusControl(txtToDt);
                }
                whrCondition = "where Doc_Date between '" + fromDt + "' and '" + toDt + "' and " + tblHead + ".mill_code='" + MILL_CODE + "'";
            }
            this.fillGrid(whrCondition);
            setFocusControl(btnMail);
            pnlPopup.Style["display"] = "none";
        }
        catch { }
    }
    private void fillGrid(string whr)
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            qry = "Select " + tblHead + ".doc_no as No," + tblHead + ".truck_no as Lorry, " + tblHead + ".tran_type AS Type,Convert(varchar(10)," + tblHead + ".doc_date,103) AS DO_Date, Party.Ac_Name_E AS [Party Name], Mill.Ac_Name_E AS [Mill Name], " + tblHead + ".quantal," +
                "" + tblHead + ".mill_rate,Party.Email_ID As Email," + tblHead + ".mill_code" +
                " from  " + tblHead + " LEFT OUTER JOIN " +
                " " + AccountMasterTable + " AS Party ON " + tblHead + ".company_code=Party.Company_Code AND " + tblHead + ".GETPASSCODE=Party.Ac_Code LEFT OUTER JOIN " + AccountMasterTable + " AS Mill " +
                " ON " + tblHead + ".company_code =Mill.Company_Code AND " + tblHead + ".mill_code=Mill.Ac_Code";
            qry = qry + " " + whr + " and tran_type='DO' and " + tblHead + ".company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + tblHead + ".Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " Order By " + tblHead + ".mill_code";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                    }
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                    }
                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }
            }
            else
            {
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }
        }
        catch
        {

        }
    }
    protected void txtmillCode_TextChanged(object sender, EventArgs e)
    {
        searchStr = txtmillCode.Text;
        strTextBox = "txtmillCode";
        csCalculation();
    }

    private void csCalculation()
    {
        if (strTextBox == "txtmillCode")
        {
            string millName = "";
            if (txtmillCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtmillCode.Text);
                if (a == false)
                {
                    btnmillcode_Click(this, new EventArgs());
                }
                else
                {
                    millName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmillCode.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (millName != string.Empty)
                    {

                        lblmillname.Text = millName;
                        setFocusControl(btnGet);
                    }
                    else
                    {
                        txtmillCode.Text = string.Empty;
                        lblmillname.Text = millName;
                        setFocusControl(txtmillCode);
                    }
                }
            }
            else
            {
                setFocusControl(txtmillCode);
            }
        }
    }
}