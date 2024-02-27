using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_pgePendingSaudaAmount : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string searchStr = string.Empty;
    string strTextbox = string.Empty;
    static WebControl objAsp = null;
    string AccountMasterTable = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
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

            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtAcCode.Text != string.Empty)
            {
                searchStr = txtAcCode.Text;
                strTextbox = "txtAcCode";
                string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAcCode.Text);
                if (str != string.Empty)
                {
                    lblAcCodeName.Text = str;
                    setFocusControl(txtFromDt);
                }
                else
                {
                    txtAcCode.Text = string.Empty;
                    lblAcCodeName.Text = string.Empty;
                }
            }
        }
        catch
        {

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
    protected void txtPartyCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtPartyCode.Text != string.Empty)
            {
                searchStr = txtAcCode.Text;
                strTextbox = "txtAcCode";
                string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Type NOT IN('B','M') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtPartyCode.Text);
                if (str != string.Empty)
                {
                    lblPartyCode.Text = str;
                    setFocusControl(txtFromDt);
                }
                else
                {
                    txtPartyCode.Text = string.Empty;
                    lblPartyCode.Text = string.Empty;
                }
            }
        }
        catch
        {

        }
    }
    protected void btnPartyCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPartyCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchStr != string.Empty && strTextbox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchStr;
            }
            if (hdnfClosePopup.Value == "txtAcCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where Ac_Type='M' and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPartyCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where Ac_Type NOT IN('B','M') and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                this.showPopup(qry);
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
                e.Row.Attributes["onselectstart"] = "javascript:return true;";
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
            e.Row.Cells[1].Width = new Unit("250px");
            e.Row.Cells[2].Width = new Unit("100px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    protected void imgBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {

        }
    }
    protected void btnPendingReport_Click(object sender, EventArgs e)
    {
        string fromDate = "";
        string toDate = "";
        string Mill_Code = txtAcCode.Text.Trim();
        string Party_Code = txtPartyCode.Text.Trim();
        if (fromDate != string.Empty)
        {
            fromDate = Convert.ToDateTime(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            fromDate = Convert.ToDateTime(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (toDate != string.Empty)
        {
            toDate = Convert.ToDateTime(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            toDate = Convert.ToDateTime(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (Mill_Code != string.Empty)
        {
            if (Mill_Code != string.Empty && Party_Code != string.Empty)
            {
                qry = "select Tender_No,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,millname,Buyer_Quantal,salerate,Commission_Rate,salevalue " +
                    " ,received,balance from " + tblPrefix + "qrySaudaBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and buyerbrokershortname not in('Self')" +
                    " and Mill_Code=" + Mill_Code + " and Buyer_Party=" + Party_Code + "  and Tender_Date between '" + fromDate + "' and '" + toDate + "' ORDER BY millname";
            }
            else
            {
                qry = "select Tender_No,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,millname,Buyer_Quantal,salerate,Commission_Rate,salevalue " +
                                  " ,received,balance from " + tblPrefix + "qrySaudaBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and buyerbrokershortname not in('Self')" +
                                  " and Mill_Code=" + Mill_Code + " and Tender_Date between '" + fromDate + "' and '" + toDate + "' ORDER BY millname";
            }
        }
        if (Party_Code != string.Empty && Mill_Code == string.Empty)
        {
            qry = "select Tender_No,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,millname,Buyer_Quantal,salerate,Commission_Rate,salevalue " +
                  " ,received,balance from " + tblPrefix + "qrySaudaBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and buyerbrokershortname not in('Self')" +
                  " and Buyer_Party=" + Party_Code + "  and Tender_Date between '" + fromDate + "' and '" + toDate + "' ORDER BY millname";
        }
        if (Mill_Code == string.Empty)
        {
            if (Party_Code == string.Empty)
            {
                qry = "select Tender_No,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,millname,Buyer_Quantal,salerate,Commission_Rate,salevalue " +
                                   " ,received,balance,salepartyfullname,Buyer_Party from " + tblPrefix + "qrySaudaBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and buyerbrokershortname not in('Self')" +
                                   " and Tender_Date between '" + fromDate + "' and '" + toDate + "' ORDER BY salepartyfullname,millname";
            }
        }
        DataSet ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (Party_Code != string.Empty)
        {
            lblPartyName.Text = "Pending Sauda Of " + " " + clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Party_Code + "");
        }
        if (ds != null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("party", typeof(string)));
            dt.Columns.Add(new DataColumn("Tender_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Lifting_Date", typeof(string)));
            dt.Columns.Add(new DataColumn("millname", typeof(string)));
            dt.Columns.Add(new DataColumn("Buyer_Quantal", typeof(double)));
            dt.Columns.Add(new DataColumn("salerate", typeof(double)));
            dt.Columns.Add(new DataColumn("Commission_Rate", typeof(string)));
            dt.Columns.Add(new DataColumn("salevalue", typeof(double)));
            dt.Columns.Add(new DataColumn("received", typeof(double)));
            dt.Columns.Add(new DataColumn("balance", typeof(double)));
            dt.Columns.Add(new DataColumn("Party_Mobile", typeof(string)));
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    if (Mill_Code == string.Empty)
                    {
                        if (Party_Code == string.Empty)
                        {
                            dr["party"] = ds.Tables[0].Rows[i]["salepartyfullname"].ToString();
                        }
                    }
                    dr["Tender_No"] = ds.Tables[0].Rows[i]["Tender_No"].ToString();
                    dr["Lifting_Date"] = ds.Tables[0].Rows[i]["Lifting_Date"].ToString();
                    dr["millname"] = ds.Tables[0].Rows[i]["millname"].ToString();
                    dr["Buyer_Quantal"] = ds.Tables[0].Rows[i]["Buyer_Quantal"].ToString();
                    dr["salerate"] = ds.Tables[0].Rows[i]["salerate"].ToString();
                    dr["Commission_Rate"] = ds.Tables[0].Rows[i]["Commission_Rate"].ToString();
                    dr["salevalue"] = ds.Tables[0].Rows[i]["salevalue"].ToString();
                    dr["received"] = ds.Tables[0].Rows[i]["received"].ToString();
                    dr["balance"] = ds.Tables[0].Rows[i]["balance"].ToString();
                    string mobile = clsCommon.getString("Select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code=" + Party_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    dr["Party_Mobile"] = mobile;
                    dt.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    if (Mill_Code == string.Empty)
                    {
                        if (Party_Code == string.Empty)
                        {
                            List<DataControlField> dtColumns = new List<DataControlField>();
                            foreach (DataControlField columns in grdReport.Columns)
                            {
                                dtColumns.Add(columns);
                            }
                            grdReport.Columns.Clear();
                            BoundField bfield = new BoundField();
                            bfield.HeaderText = "Party";
                            bfield.DataField = "party";
                            grdReport.Columns.Add(bfield);
                            foreach (DataControlField col2 in dtColumns)
                            {
                                grdReport.Columns.Add(col2);
                            }
                            var sq = dt.AsEnumerable().GroupBy(g => g.Field<double>("party")).Select(p => p.Sum(s => s.Field<double>("Buyer_Quantal")));
                            DataTable distinct = new DataTable();
                            distinct = dt.DefaultView.ToTable(true, "party");
                            if (distinct.Rows.Count > 0)
                            {

                            }
                        }
                    }
                    grdReport.Columns[2].FooterText = "Total";
                    grdReport.Columns[3].FooterText = dt.AsEnumerable().Select(x => x.Field<double>("Buyer_Quantal")).Sum().ToString();
                    grdReport.Columns[6].FooterText = dt.AsEnumerable().Select(x => x.Field<double>("salevalue")).Sum().ToString();
                    grdReport.Columns[7].FooterText = dt.AsEnumerable().Select(x => x.Field<double>("received")).Sum().ToString();
                    grdReport.Columns[8].FooterText = dt.AsEnumerable().Select(x => x.Field<double>("balance")).Sum().ToString();

                    grdReport.DataSource = dt;
                    grdReport.DataBind();
                }
                else
                {
                    grdReport.DataSource = null;
                    grdReport.DataBind();
                }
            }
        }
        pnlPopup.Style["display"] = "none";
    }
    protected void btnDetailReport_Click(object sender, EventArgs e)
    {

    }
    protected void selectAllCheckBoxes(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)grdReport.HeaderRow.FindControl("chkAll");
        foreach (GridViewRow gr in grdReport.Rows)
        {
            CheckBox chk = (CheckBox)gr.FindControl("grdCB");
            if (chkAll.Checked == true)
            {
                chk.Checked = true;
            }
            else
            {
                chk.Checked = false;
            }
        }
    }
}