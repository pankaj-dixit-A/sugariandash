using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;

public partial class Report_pgeRegisters : System.Web.UI.Page
{
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    string tblPrefix = string.Empty;
    string searchString = string.Empty;
    string AccountMasterTable = string.Empty;
    string LotNo = string.Empty;
    static WebControl objAsp = null;
    string strTextbox = string.Empty;
    string Sr_No = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            //txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyy");
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                fillBranches();
                drpBranch.SelectedValue = Session["Branch_Code"].ToString();
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
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

    protected void btnDispRegister_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            BranchCode();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
        catch
        {

        }
    }

    private void datefunction()
    {
        if (txtFromDate.Text != string.Empty)
        {
            fromDT = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            fromDT = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        if (txtToDate.Text != string.Empty)
        {
            toDT = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            toDT = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
    }
    protected void btnResaleDiff_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        if (hdconfirm.Value == "Yes")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aqw", "javascript:difftopay('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asad", "javascript:difftorecieve('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "liii", "javascript:rsdp('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
    }
    protected void btnMillCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "MM";
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
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            if (hdnfClosePopup.Value == "MM")
            {
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Ac_type='M' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.showPopup(qry);
            }
        }
        catch
        { }
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
                strTextbox = hdnfClosePopup.Value;

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
            if (hdnfClosePopup.Value == "MM")
            {
                setFocusControl(txtMillCode);
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
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "MM")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = new Unit("100px");
                    e.Row.Cells[1].Width = new Unit("400px");
                    e.Row.Cells[2].Width = new Unit("100px");
                }
                if (e.Row.RowType != DataControlRowType.Pager)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
            }
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
    protected void txtMillCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            string millName = string.Empty;
            searchString = txtMillCode.Text;
            if (txtMillCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtMillCode.Text);
                if (a == false)
                {
                    btnMillCode_Click(this, new EventArgs());
                }
                else
                {
                    millName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtMillCode.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");
                    if (millName != string.Empty)
                    {
                        lblMillName.Text = millName;
                        setFocusControl(txtLotNo);
                    }
                    else
                    {
                        txtMillCode.Text = string.Empty;
                        lblMillName.Text = string.Empty;
                        setFocusControl(txtMillCode);
                    }
                }
            }
            else
            {
                txtMillCode.Text = string.Empty;
                lblMillName.Text = millName;
                setFocusControl(txtMillCode);
            }

        }
        catch
        {
        }
    }
    protected void txtLotNo_TextChanged(object sender, EventArgs e)
    {
        if (txtLotNo.Text != string.Empty)
        {
            lbllotno.Text = "";
            setFocusControl(txtSrNo);
        }
    }
    protected void txtSrNo_TextChanged(object sender, EventArgs e)
    {
        LotNo = txtLotNo.Text;
        if (LotNo != string.Empty)
        {
            // qry = "Select ID from " + tblPrefix + "Tenderdetails WHERE Tender_No=" + LotNo + "";
            Sr_No = txtSrNo.Text;
            qry = "Select A.Ac_Name_E from " + tblPrefix + "Tenderdetails T left outer join " + AccountMasterTable + " A on T.Buyer=A.Ac_Code WHERE T.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and T.year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and T.ID=" + Sr_No + " AND T.Tender_No=" + LotNo + "";
            string buyer = clsCommon.getString(qry);
            if (buyer != string.Empty)
            {
                lblBuyer.Text = buyer;
                lblSrNotExist.Text = "";
            }
            else
            {
                lblSrNotExist.Text = "Serial Number Not Exist!";
                lblBuyer.Text = "";
                txtSrNo.Text = string.Empty;
                setFocusControl(txtSrNo);
            }
        }
        else
        {
            lbllotno.Text = "Please Enter Lot No!";
            txtSrNo.Text = string.Empty;
            setFocusControl(txtLotNo);
        }
    }
    protected void btnDispDetails_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            BranchCode();
            string Mill_Code = txtMillCode.Text;
            LotNo = txtLotNo.Text;
            Sr_No = txtSrNo.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:DD('" + fromDT + "','" + toDT + "','" + Mill_Code + "','" + LotNo + "','" + Sr_No + "','" + Branch_Code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kysds", "javascript:DDN('" + fromDT + "','" + toDT + "','" + Mill_Code + "','" + LotNo + "','" + Sr_No + "','" + Branch_Code + "')", true);
        }
        catch
        {
        }
    }
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        e.Row.Cells[0].Width = new Unit("40px");
        e.Row.Cells[1].Width = new Unit("200px");
        e.Row.Cells[1].Style.Add("overflow", "hidden");
        e.Row.Cells[2].Width = new Unit("250px");
        e.Row.Cells[3].Width = new Unit("80px");
        e.Row.Cells[4].Width = new Unit("80px");
        e.Row.Cells[5].Width = new Unit("80px");
        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                i++;
                string s = cell.Text;
                if (cell.Text.Length > 38)
                {
                    cell.Text = cell.Text.Substring(0, 38) + "....";
                    cell.ToolTip = s;
                }
            }
        }
    }
    protected void imgEdit_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDetails = sender as ImageButton;
        GridViewRow gr = (GridViewRow)btnDetails.NamingContainer;
        txtDriverMobile.Text = gr.Cells[4].Text;
        txtPartyMobile.Text = gr.Cells[5].Text;
        this.modelPopup1.Show();
    }
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        this.modelPopup1.Hide();
    }
    protected void btnFrieghtReg_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        if (string.IsNullOrEmpty(Branch_Code))
        {
            qry = "select d.doc_no as Memo_No,a.Ac_Name_E as Party,Convert(varchar(10),d.doc_date,103) as dt,b.Short_Name as mill,d.quantal,d.truck_no,ISNULL((d.Memo_Advance-d.vasuli_amount),0) as Advance,d.MobileNo as [Driver Mobile],a.Mobile_No as [Party Mobile]," +
            " d.FreightPerQtl as frieght" +
            " from " + tblPrefix + "deliveryorder d  left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code" +
            " left outer join " + tblPrefix + "AccountMaster b on d.mill_code=b.Ac_Code AND d.company_code=b.Company_Code where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.tran_type='DO' and d.doc_date between '" + fromDT + "' and '" + toDT + "'";
        }
        else
        {
            qry = "select d.doc_no as Memo_No,a.Ac_Name_E as Party,Convert(varchar(10),d.doc_date,103) as dt,b.Short_Name as mill,d.quantal,d.truck_no,ISNULL((d.Memo_Advance-d.vasuli_amount),0) as Advance,d.MobileNo as [Driver Mobile],a.Mobile_No as [Party Mobile]," +
                       " d.FreightPerQtl as frieght" +
                       " from " + tblPrefix + "deliveryorder d  left outer join " + tblPrefix + "AccountMaster a on d.GETPASSCODE=a.Ac_Code AND d.company_code=a.Company_Code" +
                       " left outer join " + tblPrefix + "AccountMaster b on d.mill_code=b.Ac_Code AND d.company_code=b.Company_Code where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.tran_type='DO' and d.doc_date between '" + fromDT + "' and '" + toDT + "'";

        }
        string DriverMobile = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dt.Columns.Add(new DataColumn("Memo_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Party", typeof(string)));
            dt.Columns.Add(new DataColumn("Name Of Account", typeof(string)));
            dt.Columns.Add(new DataColumn("DriverMobile", typeof(string)));
            dt.Columns.Add(new DataColumn("PartMobile", typeof(string)));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Memo_No"] = ds.Tables[0].Rows[i]["Memo_No"].ToString();
                    dr["Party"] = ds.Tables[0].Rows[i]["Party"].ToString();
                    string date = ds.Tables[0].Rows[i]["dt"].ToString();
                    string mill = ds.Tables[0].Rows[i]["mill"].ToString();
                    string qntl = ds.Tables[0].Rows[i]["quantal"].ToString();
                    string truckno = ds.Tables[0].Rows[i]["truck_no"].ToString();
                    string frieght = ds.Tables[0].Rows[i]["frieght"].ToString();
                    string advance = ds.Tables[0].Rows[i]["Advance"].ToString();
                    dr["Name Of Account"] = "dt-" + date + "-" + mill + "-" + Math.Abs(double.Parse(qntl)) + "-" + truckno + "-" + "frieght" + " " + Math.Abs(double.Parse(frieght)) + "-" + "Advance" + " " + advance;
                    DriverMobile = ds.Tables[0].Rows[i]["Driver Mobile"].ToString();
                    dr["DriverMobile"] = DriverMobile;
                    dr["PartMobile"] = ds.Tables[0].Rows[i]["Party Mobile"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    //TemplateField drivermob = new TemplateField();
                    //drivermob.HeaderText = "drivermob Mobile";
                    //drivermob.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "DriverMobile", dt.Columns["DriverMobile"].ToString(), new TextBox());
                    //grdDetail.Columns.Add(drivermob);   
                    grdDetail.DataSource = dt;
                    grdDetail.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "li", "javascript:fr('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
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
    }
    protected void btnVasuliRegister_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "po", "javascript:vr('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
    }
    protected void btnenterkey_Click(object sender, EventArgs e)
    { }
    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grdDetail.Rows)
        {
            CheckBox grdCB = gr.Cells[5].FindControl("grdCB") as CheckBox;
            TextBox txtDriverMobile = gr.Cells[3].FindControl("TextBox1") as TextBox;
            TextBox txtPartyMobile = gr.Cells[3].FindControl("TextBox2") as TextBox;
            if (grdCB.Checked == true)
            {
                string msg = gr.Cells[2].ToolTip.ToString();
                string driverMobile = txtDriverMobile.Text;
                string mobile = txtPartyMobile.Text;
                string API = clsGV.msgAPI;
                string Url = API + "mobile=" + mobile + "&message=" + msg + " Driver Mob:" + driverMobile;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
                StreamReader reder = new StreamReader(resp.GetResponseStream());
                string respString = reder.ReadToEnd();
                reder.Close();
                resp.Close();
            }
        }
    }
    protected void btnDispatchDiff_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        if (hdconfirm.Value == "Yes")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "qaqw", "javascript:dispdifftopay('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "assaad", "javascript:dispdifftorecieve('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popya", "javascript:DispDiff('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
    }
    protected void btnDispSummary_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "poisd", "javascript:DispSummarySmall('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "poi", "javascript:DispSummary('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
    }
    protected void btnDispMillWise_Click(object sender, EventArgs e)
    {
        datefunction();
        BranchCode();
        string Mill_Code = txtMillCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "posi", "javascript:dispmillwise('" + fromDT + "','" + toDT + "','" + Mill_Code + "','" + Branch_Code + "')", true);
    }
    protected void drpBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

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
    protected void btnCatWiseDisp_Click(object sender, EventArgs e)
    {
        datefunction();
        Branch_Code = BranchCode();
        try
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyf", "javascript:MWDR('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyasaf", "javascript:TWDR('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyasf", "javascript:DOWDR('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnBSS_Click(object sender, EventArgs e)
    {
        datefunction();
        Branch_Code = BranchCode();
        try
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyasdasf", "javascript:BSS('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnPartyWiseDO_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            Branch_Code = BranchCode();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgyaf", "javascript:PWDO('" + fromDT + "','" + toDT + "','" + Branch_Code + "','')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hassdaf", "javascript:PWDOM('" + fromDT + "','" + toDT + "','" + Branch_Code + "','')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnTransportBal_Click(object sender, EventArgs e)
    {
        datefunction();
        Branch_Code = BranchCode();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hgysdaf", "javascript:TBR('" + fromDT + "','" + toDT + "','','" + Branch_Code + "')", true);

    }
    protected void btnDOVasli_Click(object sender, EventArgs e)
    {
        datefunction();
        Branch_Code = BranchCode();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "anbgs", "javascript:DOV('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
    }

    protected void btnDispDetailMill_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            BranchCode();
            string Mill_Code = txtMillCode.Text;
            LotNo = txtLotNo.Text;
            Sr_No = txtSrNo.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kysadsd", "javascript:DDM('" + fromDT + "','" + toDT + "','" + Mill_Code + "','" + LotNo + "','" + Sr_No + "','" + Branch_Code + "')", true);
        }
        catch
        {
        }
    }
    protected void btnWithPayment_Click(object sender, EventArgs e)
    {
        try
        {
            datefunction();
            BranchCode();
            if (hdconfirm.Value == "Yes")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aqdsw", "javascript:difftopayWithPay('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asasdsd", "javascript:difftorecieveWithPay('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "liii", "javascript:rsdpWithPay('" + fromDT + "','" + toDT + "','" + Branch_Code + "')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}