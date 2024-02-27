using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;


public partial class Sugar_pgeTenderPurchase : System.Web.UI.Page
{
    public DataSet ds = null;
    public DataTable dt = null;
    Hashtable hash = null;
    string tblHeadVoucher = string.Empty;
    string GLedgerTable = string.Empty;
    public string tableName { get; set; }
    public string code { get; set; }

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string strTextbox = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;
    string Tran_Type = "LV";
    string millShortName = string.Empty;
    string DOShortname = string.Empty;
    string voucherbyshortname = string.Empty;
    string AUTO_VOUCHER = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region [tn]
            ViewState["tn"] = Request.QueryString["tn"];
            ViewState["source"] = Request.QueryString["source"];
            #endregion

            #region set company name
            string Company_Name_E = clsCommon.getString("select Company_Name_E from Company where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            Label lbl = (Label)Master.FindControl("lblCompanyName");
            lbl.Text = Company_Name_E;
            #endregion

            tblHeadVoucher = tblPrefix + "voucher";
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "Tender";
            tblDetails = tblPrefix + "TenderDetails";
            AccountMasterTable = tblPrefix + "qryAccountsList";
            qryCommon = tblPrefix + "qryTenderList";
            GLedgerTable = tblPrefix + "GLEDGER";

            defaultAccountCode = Convert.ToInt32(clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Name_E='Self'"));
            pnlPopup.Style["display"] = "none";

            #region purc rate enable/disables
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Enabled = false;
                // txtDate.Focus();
                rfvtxtPurcRate.Enabled = false;
                rfvtxtDO.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
            }
            else
            {
                txtPurcRate.Enabled = true;
                txtDate.Focus();
                rfvtxtPurcRate.Enabled = true;
                rfvtxtDO.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
            }
            #endregion
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    if (ViewState["source"] != null)
                    {
                        if (ViewState["source"].ToString() == "R")
                        {
                            //      btnSave.OnClientClick = "javascript:refreshparent('R');";
                        }
                    }
                    pnlPopup.Style["display"] = "none";
                    ViewState["currentTable"] = null;
                    clsButtonNavigation.enableDisable("N");
                    this.makeEmptyForm("N");

                    ViewState["mode"] = "I";
                    if (ViewState["tn"] == null)
                    {
                        this.showLastRecord();
                    }
                    else
                    {
                        if (ViewState["tn"].ToString() == "0")
                        {
                            this.showLastRecord();
                        }
                        else
                        {
                            this.fetchRecord(ViewState["tn"].ToString());
                        }
                    }
                    calculateBalanceSelf();
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
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
                //if (objAsp != null)
            }
        }
        catch
        {
            //  Response.Redirect("http://localhost:3994/HomePage/pgeloginForm.aspx");
        }
    }
    #endregion

    #region [makeEmptyForm]
    private void makeEmptyForm(string dAction)
    {
        try
        {
            if (dAction == "N")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }

                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }

                }
                pnlPopup.Style["display"] = "none";
                Button1.Enabled = false;
                btnSave.Text = "Save";
                btnSave.Enabled = false;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                lblMsg.Text = string.Empty;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                txtEditDoc_No.Enabled = true;
                ViewState["currentTable"] = null;
                //  Button1.Enabled = false;
            }
            if (dAction == "A")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btnSave.Text = "Save";
                btnChangeNo.Text = "Change No";
                btnChangeNo.Enabled = true;
                txtEditDoc_No.Enabled = false;
                #region set Business logic for save
                string start = DateTime.Now.ToShortDateString(); //clsCommon.getString("select Convert(varchar(10),GETDATE(),103) as d");
                DateTime startdate = DateTime.Parse(start);
                txtDate.Text = clsCommon.getString("select Convert(varchar(10),GETDATE(),103) as d"); //startdate.ToShortDateString();
                DateTime liftingdate = startdate.AddDays(15);//  clsCommon.getString("select Convert(varchar(10),DATEADD(day,15,getdate()),103) as d");
                txtLiftingDate.Text = clsCommon.getString("select Convert(varchar(10),DATEADD(day,15,getdate()),103) as d"); //liftingdate.ToString("dd/MM/yyyy");
                txtTenderNo.Enabled = false;
                pnlgrdDetail.Enabled = true;

                grdDetail.DataSource = null;
                grdDetail.DataBind();
                lblVoucherNo.Text = "";
                ViewState["currentTable"] = null;
                #endregion


            }

            if (dAction == "S")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }


                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                txtEditDoc_No.Enabled = true;
            }

            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }


                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
                //txtTenderNo.Enabled = true;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = true;
                lblMsg.Text = string.Empty;
                setFocusControl(drpResale);
                txtEditDoc_No.Enabled = false;
            }

            #region Always check this
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Text = string.Empty;
                txtPurcRate.Enabled = false;

                rfvtxtPurcRate.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
                rfvtxtDO.Enabled = false;
            }
            else
            {
                //  txtPurcRate.Text = string.Empty;
                //   txtPurcRate.Enabled = true;

                rfvtxtPurcRate.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
                rfvtxtDO.Enabled = true;
            }
            #endregion

            #region common
            if (dAction == "S" || dAction == "N")
            {
                pnlgrdDetail.Enabled = false;

                btnMillCode.Enabled = false;
                btnGrade.Enabled = false;
                btnPaymentTo.Enabled = false;
                btnTenderFrom.Enabled = false;
                btnTenderDO.Enabled = false;
                btnVoucherBy.Enabled = false;
                btnBroker.Enabled = false;
                Button1.Enabled = false;
                calenderExtenderDate.Enabled = false;
                calenderExtenderLiftingdate.Enabled = false;
                drpResale.Enabled = false;

            }
            if (dAction == "A" || dAction == "N")
            {
                lblMillName.Text = string.Empty;
                lblPaymentTo.Text = string.Empty;
                lblTenderFrom.Text = string.Empty;
                lblDO.Text = string.Empty;
                lblVoucherBy.Text = string.Empty;
                lblBroker.Text = string.Empty;
                lblMsg.Text = string.Empty;

                lbldiff.Text = "0";
                lblAmount.Text = "0";
                drpResale.SelectedValue = "M";

                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }

            if (dAction == "A" || dAction == "E")
            {
                btnMillCode.Enabled = true;
                btnGrade.Enabled = true;
                btnPaymentTo.Enabled = true;
                btnTenderFrom.Enabled = true;
                btnTenderDO.Enabled = true;
                btnVoucherBy.Enabled = true;
                btnBroker.Enabled = true;

                Button1.Enabled = true;
                calenderExtenderDate.Enabled = true;
                calenderExtenderLiftingdate.Enabled = true;
                drpResale.Enabled = true;
            }
            #endregion
            // AutoPostBackControl.Dispose();

        }
        catch
        {

        }
    }
    #endregion

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select max(Tender_No) as Tender_No from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                        if (dt.Rows[0][0].ToString() != string.Empty)
                        {
                            //Response.Write("Last" + dt.Rows[0]["Tender_No"].ToString());
                            bool recordExist = this.fetchRecord(dt.Rows[0]["Tender_No"].ToString());
                            if (recordExist == true)
                            {
                                btnAdd.Focus();
                            }
                            else                     //new code
                            {
                                btnEdit.Enabled = false;
                                btnDelete.Enabled = false;
                            }
                        }
                        else                     //new code
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                }
            }

            this.enableDisableNavigateButtons();
        }
        catch
        {

        }
    }
    #endregion

    #region [Button1_Click]
    protected void Button1_Click(object sender, EventArgs e)
    {
        btnADDBuyerDetails.Text = "ADD";
        pnlPopupTenderDetails.Style["display"] = "block";
        txtBuyerCommission.Enabled = false;
        txtBuyer.Focus();
        txtBuyerSaleRate.Text = txtMillRate.Text;
        if (txtMillCode.Text != string.Empty)
        {
            txtBuyerCommission.Text = clsCommon.getString("select Commission from " + AccountMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code='" + txtMillCode.Text + "'");
        }
    }
    #endregion

    #region [btnClose_Click]
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtBuyer.Text = string.Empty;
        lblBuyerName.Text = string.Empty;
        txtBuyerParty.Text = string.Empty;
        lblBuyerPartyName.Text = string.Empty;
        txtBuyerQuantal.Text = string.Empty;
        txtBuyerSaleRate.Text = string.Empty;
        txtBuyerCommission.Text = string.Empty;
        txtBuyerNarration.Text = string.Empty;

        pnlPopupTenderDetails.Style["display"] = "none";
        hdnfNextFocus.Value = "";
        btnSave.Focus();
    }
    #endregion

    #region [btnADDBuyerDetails_Click]
    protected void btnADDBuyerDetails_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfNextFocus.Value = "";
            int rowIndex = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt = new DataTable();
            if (ViewState["currentTable"] != null)
            {
                dt = (DataTable)ViewState["currentTable"];

                if (btnADDBuyerDetails.Text == "ADD")
                {
                    dr = dt.NewRow();

                    #region calculate rowindex

                    int maxIndex = 0;

                    int[] index = new int[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        index[i] = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                    }
                    if (index.Length > 0)
                    {
                        for (int i = 0; i < index.Length; i++)
                        {
                            if (index[i] > maxIndex)
                            {
                                maxIndex = index[i];
                            }
                        }
                        rowIndex = maxIndex + 1;
                    }
                    else
                    {
                        rowIndex = maxIndex;          //0
                    }
                    #endregion

                    //rowIndex = dt.Rows.Count + 1;
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
                else
                {
                    //update row
                    int n = Convert.ToInt32(lblno.Text);
                    rowIndex = Convert.ToInt32(lblID.Text);
                    dr = (DataRow)dt.Rows[n - 1];
                    dr["ID"] = rowIndex;
                    dr["SrNo"] = 0;

                    #region decide whether actual row is updating or virtual [rowAction]
                    string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='" + lblID.Text + "' and IsActive='true' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    if (id != string.Empty)
                    {
                        dr["rowAction"] = "U";   //actual row
                    }
                    else
                    {
                        id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='" + lblID.Text + "' and IsActive='False' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                        if (id != string.Empty)  //added but deleted by another user
                        {
                            dr["rowAction"] = "N";
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }
                    }

                    #endregion
                }

            }
            else
            {
                rowIndex = 1;
                dr = null;

                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Party", typeof(int)));
                dt.Columns.Add(new DataColumn("Name of Party", typeof(string)));
                dt.Columns.Add(new DataColumn("Broker", typeof(int)));
                dt.Columns.Add(new DataColumn("Name Of Broker", typeof(string)));
                dt.Columns.Add(new DataColumn("Quantal", typeof(float)));
                dt.Columns.Add(new DataColumn("Sale Rate", typeof(float)));
                dt.Columns.Add(new DataColumn("Commission", typeof(float)));
                dt.Columns.Add(new DataColumn("Sauda Narration", typeof(string)));
                dt.Columns.Add(new DataColumn("Delivery_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));

                dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }

            if (rowIndex != 1)
            {
                dr["Party"] = Convert.ToInt32(txtBuyer.Text);
                dr["Name of Party"] = lblBuyerName.Text;
                if (txtBuyerParty.Text != string.Empty)
                {
                    dr["Broker"] = Convert.ToInt32(txtBuyerParty.Text);
                }
                else
                {
                    dr["Broker"] = 2;
                }
                if (lblBuyerPartyName.Text == string.Empty)
                {
                    dr["Name Of Broker"] = "Self";
                }
                else
                {
                    dr["Name Of Broker"] = Server.HtmlDecode(lblBuyerPartyName.Text);
                }
                dr["Quantal"] = txtBuyerQuantal.Text;

                if (txtBuyerSaleRate.Text != string.Empty)
                {
                    dr["Sale Rate"] = float.Parse(txtBuyerSaleRate.Text);
                }
                else
                {
                    dr["Sale Rate"] = 0.00;
                }
                if (txtBuyerCommission.Text != string.Empty)
                {
                    dr["Commission"] = float.Parse(txtBuyerCommission.Text);
                }
                else
                {
                    dr["Commission"] = 0.00;
                }
                dr["Sauda Narration"] = txtBuyerNarration.Text;
                if (drpDeliveryType.SelectedValue == "C")
                {
                    dr["Delivery_Type"] = "Commission";
                }
                else
                {
                    dr["Delivery_Type"] = "Naka Delivery";
                }
                if (btnADDBuyerDetails.Text == "ADD")
                {
                    dt.Rows.Add(dr);
                }

                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='1' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                if (id != string.Empty)
                {
                    dt.Rows[0]["rowAction"] = "U";
                }
                else
                {
                    dt.Rows[0]["rowAction"] = "A";
                }
            }
            else
            {
                dr["rowAction"] = "A";
                dr["SrNo"] = 1;
                dr["Party"] = "2";
                dr["Name of Party"] = "Self";
                dr["Broker"] = "2";
                dr["Name Of Broker"] = "Self";
                if (txtQuantal.Text != string.Empty)
                {
                    dr["Quantal"] = float.Parse(txtQuantal.Text);
                }
                else
                {
                    dr["Quantal"] = 0;
                }
                if (txtMillRate.Text != string.Empty)
                {
                    dr["Sale Rate"] = float.Parse(txtMillRate.Text);
                }
                else
                {
                    dr["Sale Rate"] = 0.00;
                }
                dr["Commission"] = 0.00;
                dr["Sauda Narration"] = string.Empty;
                dr["Delivery_Type"] = "";

                dt.Rows.Add(dr);
            }

            #region set sr no
            DataRow drr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drr = (DataRow)dt.Rows[i];
                    drr["SrNo"] = i + 1;
                }
            }
            #endregion

            grdDetail.DataSource = dt;
            grdDetail.DataBind();

            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
            #region set grid view ro colors
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[12].Text == "D" || grdDetail.Rows[i].Cells[12].Text == "R")
                {
                    grdDetail.Rows[i].Style["background-color"] = "#64BB7F";
                    grdDetail.Rows[i].ForeColor = System.Drawing.Color.White;
                    //  grdDetail.Rows[i].BackColor = System.Drawing.Color.Red;
                }
            }

            #endregion

            ViewState["currentTable"] = dt;
            //PopupTenderDetails.Show();

            txtBuyer.Text = string.Empty;
            drpDeliveryType.SelectedIndex = 0;
            lblBuyerName.Text = string.Empty;
            txtBuyerParty.Text = string.Empty;
            lblBuyerPartyName.Text = string.Empty;
            txtBuyerQuantal.Text = string.Empty;
            txtBuyerSaleRate.Text = txtMillRate.Text;
            txtBuyerCommission.Text = string.Empty;
            txtBuyerNarration.Text = string.Empty;
            lblID.Text = string.Empty;

            if (btnADDBuyerDetails.Text == "ADD")
            {
                pnlPopupTenderDetails.Style["display"] = "block";

                txtBuyer.Focus();
            }
            else
            {
                pnlPopupTenderDetails.Style["display"] = "none";

                Button1.Focus();
            }

            btnADDBuyerDetails.Text = "ADD";

            //calculate balance self
            this.calculateBalanceSelf();

        }
        catch
        {

        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            clsButtonNavigation.enableDisable("A");

            ViewState["mode"] = null;
            ViewState["mode"] = "I";
            this.makeEmptyForm("A");

            this.getMaxCode();

            #region Set Excise Rate
            txtExciseRate.Text = clsCommon.getString("select EXCISE_RATE from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");

            #endregion

            #region add self row into grid
            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            #endregion
            pnlPopupTenderDetails.Style["display"] = "none";
            setFocusControl(drpResale);
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "Tender_No";
                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    txtTenderNo.Text = ds.Tables[0].Rows[0][0].ToString();
                                }
                            }

                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //if tender details contains at least one record then only save the tender
        bool rowExist = false;
        string msg = "";
        txtEditDoc_No.Text = "";
        if (grdDetail.Rows.Count > 0)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[12].Text != "D")
                {
                    rowExist = true;

                    string retValue = saveData();

                    if (retValue != "-3" && retValue != "" && retValue != "-2")
                    {
                        hdnf.Value = txtTenderNo.Text;
                        saveTenderDetails(txtTenderNo.Text, Convert.ToInt32(Session["Company_Code"].ToString()).ToString());

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Saved Successfully!')", true);

                        if (ViewState["source"] == null)
                        {

                            clsButtonNavigation.enableDisable("S");
                            this.makeEmptyForm("S");
                            this.enableDisableNavigateButtons();
                        }
                        else
                        {
                            if (ViewState["source"].ToString() == "R")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, GetType(), "ClosePopup", "javascript:window.close();window.opener.location.reload();", true);
                            }
                            else
                            {
                                clsButtonNavigation.enableDisable("S");
                                this.makeEmptyForm("S");
                                this.enableDisableNavigateButtons();
                            }
                        }
                    }
                    if (retValue == "-2")
                    {
                        saveTenderDetails(txtTenderNo.Text, Convert.ToInt32(Session["Company_Code"].ToString()).ToString());
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Updated Successfully!')", true);

                        //msg = "Successfully Updated!";
                        if (ViewState["source"] == null)
                        {
                            this.fetchRecord(txtTenderNo.Text);
                            clsButtonNavigation.enableDisable("S");
                            this.makeEmptyForm("S");
                            this.enableDisableNavigateButtons();
                        }
                        else
                        {
                            if (ViewState["source"].ToString() == "R")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, GetType(), "ClosePopup", "javascript:window.close();window.opener.location.reload();", true);
                            }
                            else
                            {
                                clsButtonNavigation.enableDisable("S");
                                this.makeEmptyForm("S");
                                this.enableDisableNavigateButtons();
                            }
                        }
                    }
                    break;
                }
            }//end for
            //-----------fetch currently saved/updated record---------------
            this.fetchRecord(txtTenderNo.Text);
            lblMsg.Text = msg;
            lblMsg.ForeColor = System.Drawing.Color.LimeGreen;
        }


        if (rowExist == false)
        {
            lblMsg.Text = "At least one Tender details Record necessary";
            lblMsg.ForeColor = System.Drawing.Color.Red;
        }
    }
    #endregion

    #region [saveTenderDetails]
    private void saveTenderDetails(string tenderNo, string companyCode)
    {
        try
        {
            string str = "";
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Int32 Buyer = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    double Buyer_Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    double Sale_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[8].Text);
                    double Commission_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    string Narration = "";// DateTime.Now.ToString("dd/MM/yyyy");
                    string dtype = grdDetail.Rows[i].Cells[11].Text.ToString();
                    string Delivery_Type = "";
                    if (dtype == "Commission")
                    {
                        Delivery_Type = "C";
                    }
                    else
                    {
                        Delivery_Type = "N";
                    }
                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[10].Text);
                    int ID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    double Buyer_Party = 0;
                    //if (grdDetail.Rows.Count == 1)
                    //{
                    //    Buyer_Party = Convert.ToInt32(txtBroker.Text);
                    //}
                    //else
                    //{

                    Buyer_Party = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));

                    //}
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        if (grdDetail.Rows[i].Cells[12].Text != "N")   //For N do nothing for that row
                        {
                            if (grdDetail.Rows[i].Cells[12].Text == "A")
                            {
                                #region check whether same id is inserted in table already or not (if then insert next no)
                                string id = clsCommon.getString("select AutoID from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + ID + "'");
                                if (id != string.Empty)
                                {
                                    //this id is already inserted Get max id
                                    string newId = clsCommon.getString("select max(ID) from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                    ID = Convert.ToInt32(newId) + 1;
                                }
                                #endregion

                                obj.flag = 1;
                                obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,Narration,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                obj.values = "'" + tenderNo + "','" + companyCode + "','" + Buyer + "','" + Buyer_Quantal + "','" + Sale_Rate + "','" + Commission_Rate + "','" + Narration + "','" + ID + "','" + Buyer_Party + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                            }
                            if (grdDetail.Rows[i].Cells[12].Text == "U")
                            {
                                obj.flag = 2;
                                obj.columnNm = " Buyer='" + Buyer + "',Buyer_Quantal='" + Buyer_Quantal + "',Sale_Rate='" + Sale_Rate + "',Commission_Rate='" + Commission_Rate + "',Narration='" + Narration + "',Buyer_Party='" + Buyer_Party + "',Delivery_Type='" + Delivery_Type + "' where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'  and ID='" + ID + "'";
                                obj.values = "none";
                            }
                            if (grdDetail.Rows[i].Cells[12].Text == "D")
                            {
                                obj.flag = 3;
                                obj.columnNm = "Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and ID='" + ID + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                obj.values = "none";
                            }
                            obj.tableName = tblDetails;

                            DataSet ds = new DataSet();
                            ds = obj.insertAccountMaster(ref str);
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [saveData]
    private string saveData()
    {
        string qry = "";
        try
        {
            string str = "";
            bool isValidated = true;
            double Diff_Amount = lbldiff.Text != string.Empty ? Convert.ToDouble(lbldiff.Text) : 0.00;
            int docno = 0;
            //Int32 Voucher_No = 0;
            Int32 Tender_No = Convert.ToInt32(txtTenderNo.Text);
            Int32 Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
            string Tender_Date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string Lifting_Date = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            Int32 Mill_Code = Convert.ToInt32(txtMillCode.Text);
            string Sell_Note_No = txtSellNoteNo.Text;
            double Mill_Rate = double.Parse(txtMillRate.Text);
            string Grade = txtGrade.Text;
            float Quantal = float.Parse(txtQuantal.Text);
            int Packing = Convert.ToInt32(txtPacking.Text);
            int Bags = Convert.ToInt32(Math.Round(float.Parse(txtBags.Text)));
            string amt1 = lblAmount.Text.ToString();
            double VOUCHERAMOUNT = Convert.ToDouble(amt1);
            int DIFF = Convert.ToInt32(lbldiff.Text);
            bool isNumeric;
            int n;
            double PURCHASE_RATE = txtPurcRate.Text != string.Empty ? Convert.ToDouble(txtPurcRate.Text) : 0.00;
            int Payment_To, Tender_From, Tender_DO, Voucher_By, Broker;
            Payment_To = string.IsNullOrEmpty(txtPaymentTo.Text) ? Mill_Code : int.Parse(txtPaymentTo.Text);
            Tender_From = string.IsNullOrEmpty(txtTenderFrom.Text) ? 0 : int.Parse(txtTenderFrom.Text);
            Tender_DO = string.IsNullOrEmpty(txtDO.Text) ? 2 : int.Parse(txtDO.Text);
            Voucher_By = string.IsNullOrEmpty(txtVoucherBy.Text) ? 0 : int.Parse(txtVoucherBy.Text);
            Broker = string.IsNullOrEmpty(txtBroker.Text) ? 2 : int.Parse(txtBroker.Text);
            if (int.TryParse(txtPaymentTo.Text, out n))
            {
                Payment_To = n;
            }
            if (int.TryParse(txtTenderFrom.Text, out n))
            {
                Tender_From = n;
            }
            if (int.TryParse(txtDO.Text, out n))
            {
                Tender_DO = n;
            }
            if (int.TryParse(txtVoucherBy.Text, out n))
            {
                Voucher_By = n;
            }
            if (int.TryParse(txtBroker.Text, out n))
            {
                Broker = n;
            }
            #region previous code
            //int Payment_To = defaultAccountCode;
            //int n;
            //bool isNumeric = int.TryParse(txtPaymentTo.Text, out n);
            //if (isNumeric == true)
            //    Payment_To = n;
            //int Tender_From = defaultAccountCode;

            //isNumeric = int.TryParse(txtTenderFrom.Text, out n);
            //if (isNumeric == true)
            //Tender_From = n;

            //int Tender_DO = defaultAccountCode;
            //isNumeric = int.TryParse(txtDO.Text, out n);
            //if (isNumeric == true)
            //    Tender_DO = n;
            //int Voucher_By = defaultAccountCode;
            //isNumeric = int.TryParse(txtVoucherBy.Text, out n);
            //if (isNumeric == true)
            //    Voucher_By = n;
            //int Broker = defaultAccountCode;
            //isNumeric = int.TryParse(txtBroker.Text, out n);
            //if (isNumeric == true)
            //    Broker = n;
            #endregion
            float m = 0;
            float Excise_Rate = 0;
            isNumeric = float.TryParse(txtExciseRate.Text, out m);
            if (isNumeric == true)
                Excise_Rate = m;
            string Narration = txtNarration.Text;
            string userName = Session["user"].ToString();
            string Year_Code = Convert.ToInt32(Session["year"].ToString()).ToString();

            float Purc_Rate = 0;
            isNumeric = float.TryParse(txtPurcRate.Text, out m);
            if (isNumeric == true)
                Purc_Rate = m;
            string type = drpResale.SelectedValue;
            int Branch_Id = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
            string Created_By = clsGV.user;
            Created_By = Session["user"].ToString();
            //lblCreatedBy.Text = Created_By.ToString();
            string Modified_By = clsGV.user;
            Modified_By = Session["user"].ToString();
            //lblModifiedBy.Text = Modified_By.ToString();
            AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");

            string myNarration = string.Empty;
            millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Mill_Code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            DOShortname = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Tender_DO + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            voucherbyshortname = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Voucher_By + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (drpResale.SelectedValue != "M")
            {
                if (PURCHASE_RATE > 0)
                {
                    myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + Mill_Rate + " P.R." + PURCHASE_RATE + ")";
                }
            }

            //else
            //{
            //    myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " P.R." + SALE_RATE + ")";
            //}
            #region ------------- Valiation ---------------
            if (txtTenderNo.Text == string.Empty)
            {
                isValidated = false;
                txtTenderNo.Focus();
            }
            if (txtMillCode.Text == string.Empty)
            {
                isValidated = false;
                txtMillCode.Focus();
            }
            else
            {
                string s = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtMillCode.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");

                if (s == string.Empty)
                {
                    isValidated = false;
                    txtMillCode.Text = string.Empty;
                    lblMillName.Text = string.Empty;
                    txtMillCode.Focus();
                }
            }
            if (txtMillRate.Text == string.Empty)
            {
                isValidated = false;
                txtMillRate.Focus();
            }
            if (txtGrade.Text == string.Empty)
            {
                isValidated = false;
                txtGrade.Focus();
            }
            if (txtQuantal.Text == string.Empty)
            {
                isValidated = false;
                txtQuantal.Focus();
            }
            if (txtPacking.Text == string.Empty)
            {
                isValidated = false;
                txtPacking.Focus();
            }

            if (drpResale.SelectedValue == "R")
            {
                if (txtPaymentTo.Text == string.Empty)
                {
                    isValidated = false;
                    setFocusControl(txtPaymentTo);
                }
                else
                {
                    string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtPaymentTo.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (v != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        txtPaymentTo.Text = string.Empty;
                        isValidated = false;
                        setFocusControl(txtPaymentTo);
                    }
                }
                if (txtPurcRate.Text == string.Empty)
                {
                    isValidated = false;
                    txtPurcRate.Focus();
                }
                if (txtDO.Text == string.Empty)
                {
                    isValidated = false;
                    setFocusControl(txtDO);
                }
                if (txtVoucherBy.Text == string.Empty)
                {
                    if (drpResale.SelectedValue == "R")
                    {
                        isValidated = false;
                        setFocusControl(txtVoucherBy);
                    }
                    else
                    {
                        Voucher_By = int.Parse(txtVoucherBy.Text);
                    }
                }
                else
                {
                    string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtDO.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (v != string.Empty)
                    {
                        isValidated = true;
                        if (txtTenderFrom.Text != string.Empty)
                        {
                            Tender_From = int.Parse(txtTenderFrom.Text);
                            isValidated = true;
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtTenderFrom.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty)
                            {
                                Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtTenderFrom.Text = "0";
                                Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                                txtTenderFrom.Text = Tender_From.ToString();
                                isValidated = true;
                            }
                        }
                        if (txtVoucherBy.Text != string.Empty)
                        {

                            Voucher_By = int.Parse(txtVoucherBy.Text);
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtVoucherBy.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty)
                            {
                                Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtVoucherBy.Text = "0";
                                Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                                txtVoucherBy.Text = Voucher_By.ToString();
                                isValidated = true;
                            }
                        }
                        if (txtBroker.Text != string.Empty)
                        {
                            Broker = int.Parse(txtBroker.Text);
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtBroker.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty)
                            {
                                Broker = Convert.ToInt32(txtBroker.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtBroker.Text = "2";
                                Broker = Convert.ToInt32(txtBroker.Text);
                                txtBroker.Text = Broker.ToString();
                                isValidated = true;
                            }
                        }
                    }
                    else
                    {
                        isValidated = false;
                        txtDO.Text = string.Empty;
                        setFocusControl(txtDO);
                    }
                }
            }
            #endregion

            if (isValidated == true)
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    if (ViewState["mode"] != null)
                    {
                        //------- Check whether tender no already exist or not ---------
                        string no = clsCommon.getString("select Tender_No from " + tblHead + " where Tender_No=" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        if (ViewState["mode"].ToString() == "I")
                        {
                            if (no == string.Empty)
                            {
                                //same tender no does not exist
                                Tender_No = Convert.ToInt32(txtTenderNo.Text);
                            }
                            else
                            {
                                //get next no and save
                                this.getMaxCode();
                                Tender_No = Convert.ToInt32(txtTenderNo.Text);
                            }
                            if (AUTO_VOUCHER == "YES")
                            {
                                if (drpResale.SelectedValue == "R")
                                {
                                    if (Mill_Rate > PURCHASE_RATE)
                                    {
                                        docno = MaxVoucher();
                                    }
                                }
                            }
                            obj.tableName = tblHead;
                            obj.flag = 1;
                            obj.columnNm = "Tender_No,Company_Code,Tender_Date,Lifting_Date,Mill_Code,Grade,Quantal,Packing,Bags,Payment_To,Tender_From,Tender_DO,Voucher_By,Broker,Excise_Rate,Narration,Mill_Rate,Created_By,Year_Code,Purc_Rate,type,Branch_Id,Voucher_No,Sell_Note_No";
                            obj.values = "'" + Tender_No + "','" + Company_Code + "','" + Tender_Date + "','" + Lifting_Date + "','" + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" + Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Voucher_By + "','" + Broker + "','" + Excise_Rate + "','" + Narration + "','" + Mill_Rate + "','" + user + "','" + Year_Code + "','" + Purc_Rate + "','" + type + "','" + Branch_Id + "','" + docno + "','" + Sell_Note_No + "'";
                            obj.insertAccountMaster(ref str);

                            #region Saving To Voucher Table
                            if (AUTO_VOUCHER == "YES")
                            {
                                if (drpResale.SelectedValue == "R")
                                {
                                    if (Mill_Rate > PURCHASE_RATE)
                                    {
                                        docno = MaxVoucher();
                                        ///int docno =Convert.ToInt32(clsCommon.getString("Select MAX(Doc_No)+1 from NT_1_Voucher"));
                                        vouchernumber.Value = docno.ToString();
                                        lblVoucherNo.Text = vouchernumber.Value;
                                        obj.flag = 1;
                                        obj.tableName = "" + tblPrefix + "Voucher";
                                        obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code,Tender_No,Mill_Code,Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE,Mill_Rate,Purchase_Rate,Diff_Amount,Voucher_Amount,Narration1,Diff_Type,Created_By";
                                        obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + string.Empty.Trim() + "','" + Company_Code + "','" + Year_Code + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tender_No + "','" + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" + Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate + "','" + Purc_Rate + "','" + Diff_Amount + "','" + VOUCHERAMOUNT + "','" + Narration + myNarration + "','TD','" + user + "'";
                                        obj.insertAccountMaster(ref str);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (no != string.Empty)
                            {
                                //tender no does exist then update  
                                obj.flag = 2;
                                obj.columnNm = "Tender_Date='" + Tender_Date + "',Lifting_Date='" + Lifting_Date + "',Mill_Code='" + Mill_Code + "',Grade='" + Grade + "',Quantal='" + Quantal + "',Packing='" + Packing + "',Bags='" + Bags + "',Payment_To='" + Payment_To + "',Tender_From='" + Tender_From + "',Tender_DO='" + Tender_DO + "',Voucher_By='" + Voucher_By + "',Broker='" + Broker + "',Excise_Rate='" + Excise_Rate + "',Narration='" + Narration + "',Mill_Rate='" + Mill_Rate + "',Modified_By='" + user + "',Purc_Rate='" + Purc_Rate + "',type='" + type + "',Sell_Note_No='" + Sell_Note_No + "' where Tender_No='" + Tender_No + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                obj.values = "none";
                                obj.tableName = tblHead;
                                obj.insertAccountMaster(ref str);

                                //Updating voucher table
                                if (drpResale.SelectedValue == "R")
                                {
                                    Tender_No = int.Parse(vouchernumber.Value.TrimStart());
                                    obj.flag = 2;
                                    obj.tableName = "" + tblPrefix + "Voucher";
                                    obj.columnNm = "Tran_Type='" + "LV" + "',Doc_Date='" + Tender_Date + "',Ac_Code='" + Voucher_By + "',Suffix='" + string.Empty.Trim() + "',Company_Code='" + Company_Code + "',Year_Code='" + Year_Code + "',Tender_No='" + Tender_No + "',Mill_Code='" + Mill_Code + "',Grade='" + Grade + "',Quantal='" + Quantal + "',PACKING='" + Packing + "',BAGS='" + Bags + "',Payment_To='" + Payment_To + "',Tender_From='" + Tender_From + "',Tender_DO='" + Tender_DO + "',Broker_CODE='" + Broker + "',Mill_Rate='" + Mill_Rate + "',Purchase_Rate='" + Purc_Rate + "',Diff_Amount='" + Diff_Amount + "',Voucher_Amount='" + VOUCHERAMOUNT + "',Narration1='" + Narration + "',Modified_By='" + user + "' where " +
                                                    " Company_Code='" + Company_Code + "' and  Year_Code='" + Year_Code + "'  and Tran_Type='" + "LV" + "' and Doc_No='" + Tender_No + "'";
                                    obj.values = "none";
                                    ds = obj.insertAccountMaster(ref str);
                                }
                            }
                            else
                            {
                                //show msg
                                pnlPopup.Style["display"] = "none";
                                ViewState["currentTable"] = null;
                                clsButtonNavigation.enableDisable("N");
                                this.makeEmptyForm("N");
                                ViewState["mode"] = "I";
                                this.showLastRecord();
                                lblMsg.Text = "This Tender No (" + txtTenderNo.Text + ") is deleted";
                            }
                        }

                        //Gledger effect for local voucher 
                        if (AUTO_VOUCHER == "YES")
                        {
                            if (drpResale.SelectedValue == "R")
                            {
                                if (Mill_Rate > PURCHASE_RATE)
                                {
                                    if (docno != 0)
                                    {
                                        //Gledger effect
                                        qry = "";
                                        qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + docno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                        ds = clsDAL.SimpleQuery(qry);
                                        Int32 GID = 0;
                                        if (VOUCHERAMOUNT > 0)
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" +Math.Abs(VOUCHERAMOUNT) + "','" + Tender_No + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                            obj.insertAccountMaster(ref str);
                                        }
                                        else
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                        // difference amount effect
                                        if (DIFF > 0)
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";

                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                        else
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";
                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }//end if isvalidated
            return str;
        }
        catch
        {
            return "";
        }
    }

    private int MaxVoucher()
    {
        int docno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='LV'"));
        return docno;
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int k = clsIsEdit.Tender_No;
        //if (!clsIsEdit.Tender.Any(a => a == Convert.ToInt32(txtTenderNo.Text)))
        //{
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        //Session["enableSave"] = 0;
        this.fetchRecord(txtTenderNo.Text);
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtTenderNo.Enabled = false;
        txtEditDoc_No.Text = "";
        //}
    }

    //private void SaveChanges()
    //{
    //    Session["enableSave"] = 1;
    //}
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string query = string.Empty;

                query = "select doc_no from " + tblPrefix + "deliveryorder where purc_no=" + txtTenderNo.Text + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                string dono = clsCommon.getString(query);

                string currentDoc_No = vouchernumber.Value;
                string currentSuffix = string.Empty.Trim();

                string delvoucher = "";
                delvoucher = "";

                if (dono != string.Empty)
                {
                    lblMsg.Text = "Cannot delete this entry It is in used!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    setFocusControl(txtDate);
                }
                else
                {

                    lblMsg.Text = "";
                    query = "delete from " + tblHead + " where Tender_No =" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                    DataSet ds = clsDAL.SimpleQuery(query);
                    query = "delete from " + tblDetails + " where Tender_No =" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    ds = clsDAL.SimpleQuery(query);
                    ds = new DataSet();
                    DataTable dt = new DataTable();

                    //Deleting Voucher Entry
                    delvoucher = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(query);

                    //string strrev = "";
                    //using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    //{
                    //    obj.flag = 3;
                    //    obj.tableName = "" + tblPrefix + "Voucher";
                    //    obj.columnNm = "  Tran_Type='" + Tran_Type + "' and Doc_No=" + currentDoc_No + " and Suffix='" + currentSuffix.Trim() + "'" +
                    //        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    //    obj.values = "none";
                    //    ds = obj.insertAccountMaster(ref strrev);

                    //}

                    lblMsg.Text = "Successfully Deleted";

                    query = "SELECT top 1 [Tender_No] from " + tblHead + "  where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY Tender_No asc  ";
                    string Tender_No = clsCommon.getString(query);
                    if (Tender_No != string.Empty)
                    {
                        fetchRecord(Tender_No);
                        this.makeEmptyForm("S");
                        clsButtonNavigation.enableDisable("S");
                    }
                    else
                    {
                        query = "SELECT top 1 [Tender_No] from " + tblHead + "  where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No DESC  ";
                        Tender_No = clsCommon.getString(query);
                        if (Tender_No != string.Empty)
                        {
                            fetchRecord(Tender_No);
                            this.makeEmptyForm("S");
                            clsButtonNavigation.enableDisable("S");
                        }
                        else
                        {
                            this.makeEmptyForm("N");
                            //new code

                            clsButtonNavigation.enableDisable("N");         //No record exist  Last record deleted.
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                    //clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                }

            }
        }
        catch
        {

        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        if (hdnf.Value != string.Empty)
        {
            //string query = getDisplayQuery(); ;
            bool recordExist = this.fetchRecord(hdnf.Value);
        }
        else
        {
            this.showLastRecord();
        }

        string str = clsCommon.getString("select count(Tender_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

        if (str != "0")
        {
            clsButtonNavigation.enableDisable("S");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("S");
        }
        else
        {
            clsButtonNavigation.enableDisable("N");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("N");

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
        txtEditDoc_No.Text = "";

    }
    #endregion

    #region [fetchRecord]
    private bool fetchRecord(string txtValue)
    {
        try
        {
            bool recordExist = false;
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            qry = "select * from " + qryCommon + " where Tender_No='" + txtValue + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hdnf.Value = txtValue;
                        txtTenderNo.Text = txtValue;
                        txtDate.Text = DateTime.Parse(dt.Rows[0]["Tender_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        txtLiftingDate.Text = DateTime.Parse(dt.Rows[0]["Lifting_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");

                        //txtDate.Text = dt.Rows[0]["Tender_Date"].ToString();
                        //txtLiftingDate.Text = dt.Rows[0]["Lifting_Date"].ToString();
                        //Response.Write("MillCode" + dt.Rows[0]["Mill_Code"].ToString());
                        txtMillCode.Text = dt.Rows[0]["Mill_Code"].ToString();
                        txtGrade.Text = dt.Rows[0]["Grade"].ToString();
                        txtQuantal.Text = dt.Rows[0]["Quantal"].ToString();//Convert.ToString(Math.Abs(Convert.ToDouble(dt.Rows[0]["Quantal"].ToString())));
                        txtPacking.Text = dt.Rows[0]["Packing"].ToString();
                        txtBags.Text = dt.Rows[0]["Bags"].ToString();
                        txtMillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                        // txtPurcRate.Text = dt.Rows[0][""].ToString();
                        txtTenderFrom.Text = dt.Rows[0]["Tender_From"].ToString();
                        txtPaymentTo.Text = dt.Rows[0]["Payment_To"].ToString();
                        txtDO.Text = dt.Rows[0]["Tender_DO"].ToString();
                        txtVoucherBy.Text = dt.Rows[0]["Voucher_By"].ToString();
                        txtBroker.Text = dt.Rows[0]["Broker"].ToString();
                        txtExciseRate.Text = dt.Rows[0]["Excise_Rate"].ToString();
                        txtSellNoteNo.Text = dt.Rows[0]["Sell_Note_No"].ToString();
                        txtNarration.Text = dt.Rows[0]["Narration"].ToString();
                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                        if (lblModified != null)
                        {
                            lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        }
                        //lblCreatedBy.Text = dt.Rows[0]["CreatedBy"].ToString();
                        // lblModifiedBy.Text = dt.Rows[0]["ModifiedBy"].ToString();
                        //set Label Texts
                        //lblVoucherNo.Text = dt.Rows[0]["Doc_No"].ToString();
                        vouchernumber.Value = dt.Rows[0]["vouchernumber"].ToString();
                        lblMillName.Text = dt.Rows[0]["millfullname"].ToString();
                        lblPaymentTo.Text = dt.Rows[0]["paymenttofullname"].ToString();
                        lblTenderFrom.Text = dt.Rows[0]["tenderfromfullname"].ToString();
                        lblVoucherBy.Text = dt.Rows[0]["voucherbyfullname"].ToString();
                        lblDO.Text = dt.Rows[0]["dofullname"].ToString();
                        lblBroker.Text = dt.Rows[0]["brokerfullname"].ToString();

                        txtPurcRate.Text = dt.Rows[0]["Purc_Rate"].ToString();
                        drpResale.SelectedValue = dt.Rows[0]["type"].ToString();
                        lblVoucherNo.Text = vouchernumber.Value;

                        recordExist = true;
                        lblMsg.Text = "";

                        #region Tender Details

                        qry = "select  ID,Buyer as Party,buyerbrokerfullname  as [Name of Party],Buyer_Party as Broker,salepartyfullname as [Name of Broker],Buyer_Quantal as [Quantal],Sale_Rate as [Sale Rate],Commission_Rate as [Commission],saudanarration as [Sauda Narration],Delivery_Type from " + qryCommon + " where Tender_No='" + txtValue + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and IsActive='True' and TDYearCode='" + Convert.ToInt32(Session["year"].ToString()) + "' order by ID";

                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["Delivery_Type"].ToString() == "C")
                                        {
                                            dt.Rows[i]["Delivery_Type"] = "Commission";
                                        }
                                        else
                                        {
                                            dt.Rows[i]["Delivery_Type"] = "Naka Delivery";
                                        }
                                    }
                                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                    dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                                    dt.Rows[0]["SrNo"] = 1;                           //self row
                                    dt.Rows[0]["rowAction"] = "U";                     //self row

                                    for (int i = 1; i < dt.Rows.Count; i++)
                                    {
                                        dt.Rows[i]["rowAction"] = "N";
                                        dt.Rows[i]["SrNo"] = i + 1;
                                    }

                                    grdDetail.DataSource = dt;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = dt;
                                }
                                else
                                {
                                    grdDetail.DataSource = null;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = null;
                                }
                            }
                            else
                            {
                                grdDetail.DataSource = null;
                                grdDetail.DataBind();
                                ViewState["currentTable"] = null;
                            }
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion

                        this.calculateDiff();
                        this.calculateAmount();
                        GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
                        gr.Enabled = false;
                        lblBalanceSelf.Text = Server.HtmlDecode(gr.Cells[7].Text);
                        pnlgrdDetail.Enabled = false;
                    }
                }
            }
            hdnf.Value = txtTenderNo.Text;
            return recordExist;
        }
        catch
        {
            // throw;
            return false;
        }
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        #region [code]
        try
        {
            string query = "";
            query = "select Tender_No from " + tblHead + " where Tender_No=(select MIN(Tender_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(query);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtTenderNo.Text = dt.Rows[0][0].ToString();
                        ViewState["mode"] = "U";
                        clsButtonNavigation.enableDisable("N");
                        bool recordExist = this.fetchRecord(dt.Rows[0][0].ToString());
                        if (recordExist == true)
                        {
                            btnEdit.Enabled = true;
                            btnEdit.Focus();
                        }

                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");

                    }
                }
            }


        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No DESC  ";

                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["mode"] = "U";
                            clsButtonNavigation.enableDisable("N");
                            bool recordExist = this.fetchRecord(dt.Rows[0]["Tender_No"].ToString());
                            if (recordExist == true)
                            {
                                btnEdit.Enabled = true;
                                btnEdit.Focus();
                            }
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }

    #endregion

    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";

                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["mode"] = "U";
                            clsButtonNavigation.enableDisable("N");
                            bool recordExist = this.fetchRecord(dt.Rows[0]["Tender_No"].ToString());
                            if (recordExist == true)
                            {
                                btnEdit.Enabled = true;
                                btnEdit.Focus();
                            }
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }
    #endregion

    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select Tender_No from " + tblHead + " where Tender_No=(select MAX(Tender_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(query);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtTenderNo.Text = dt.Rows[0][0].ToString();
                        ViewState["mode"] = "U";
                        clsButtonNavigation.enableDisable("N");
                        bool recordExist = this.fetchRecord(dt.Rows[0][0].ToString());
                        if (recordExist == true)
                        {
                            btnEdit.Enabled = true;
                            btnEdit.Focus();
                        }

                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");

                    }
                }
            }


        }
        catch
        {

        }
    }
    #endregion

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons

        //if (ViewState["mode"].ToString() == "U")
        //{

        int RecordCount = 0;

        string query = "";
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(query);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
        }

        if (RecordCount != 0 && RecordCount == 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
        else if (RecordCount != 0 && RecordCount > 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = true;
            //  btnLast.Focus();
        }

        if (txtTenderNo.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();

                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnLast.Enabled = true;
                            btnNext.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnLast.Enabled = false;
                            btnNext.Enabled = false;
                        }
                    }
                }

                ds = new DataSet();
                dt = new DataTable();

                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnFirst.Enabled = true;
                            btnPrevious.Enabled = true;
                        }
                        else
                        {
                            btnFirst.Enabled = false;
                            btnPrevious.Enabled = false;
                        }
                    }
                }
                #endregion
            }
        }

        // }

        #endregion
    }
    #endregion

    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            if (e.CommandArgument == "lnk")
            {
                switch (e.CommandName)
                {
                    case "EditRecord":
                        if (grdDetail.Rows[rowindex].Cells[12].Text != "D" && grdDetail.Rows[rowindex].Cells[12].Text != "R")
                        {
                            pnlPopupTenderDetails.Style["display"] = "block";
                            this.showRecord(grdDetail.Rows[rowindex]);
                            btnADDBuyerDetails.Text = "Update";
                        }
                        break;

                    case "DeleteRecord":
                        string action = "";
                        LinkButton lnkDelete = (LinkButton)e.CommandSource;
                        if (lnkDelete.Text == "Delete")
                        {
                            string tenderno = txtTenderNo.Text;
                            string tddetails = grdDetail.Rows[rowindex].Cells[2].Text.ToString();
                            qry = "Select * from " + tblPrefix + "deliveryorder Where purc_no=" + tenderno + " AND purc_order=" + tddetails + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Record is in Use You Cannot Delete this Entry')", true);
                                    return;
                                }
                                else
                                {
                                    action = "Delete";
                                    lnkDelete.Text = "Open";
                                }
                            }
                        }
                        else
                        {
                            action = "Open";
                            lnkDelete.Text = "Delete";
                        }
                        this.deleteTenderDetailsRecord(grdDetail.Rows[rowindex], action);
                        break;
                }
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [deleteTenderDetailsRecord]
    private void deleteTenderDetailsRecord(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;

            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["ID"].ToString());

                string IDExisting = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='" + ID + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "D";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "N";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                    //  dt.Rows[rowIndex]["rowAction"] = "D";  //isactive false
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;

                        grdDetail.Rows[rowIndex].Cells[12].Text = "R";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "A";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }

                    // dt.Rows[rowIndex]["rowAction"] = "N";   //Do nothing
                }
                ViewState["currentTable"] = dt;
                this.calculateBalanceSelf();
                //ViewState["currentTable"] = dt;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["RowNumber"] = i + 1;
            //}
        }
        catch
        {

        }
    }
    #endregion

    #region [showRecord]
    private void showRecord(GridViewRow gridViewRow)
    {
        lblno.Text = Server.HtmlDecode(gridViewRow.Cells[13].Text);
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
        txtBuyer.Text = Server.HtmlDecode(gridViewRow.Cells[3].Text);
        lblBuyerName.Text = Server.HtmlDecode(gridViewRow.Cells[4].Text);
        string deliveryType = Server.HtmlDecode(gridViewRow.Cells[11].Text);
        string type = "";
        if (deliveryType == "Commission")
        {
            type = "C";
        }
        else
        {
            type = "N";
        }
        drpDeliveryType.SelectedValue = type;
        txtBuyerParty.Text = Server.HtmlDecode(gridViewRow.Cells[5].Text);
        lblBuyerPartyName.Text = Server.HtmlDecode(gridViewRow.Cells[6].Text);
        double buyerqntl = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[7].Text));
        txtBuyerQuantal.Text = Convert.ToString(Math.Abs(buyerqntl));
        double salerate = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[8].Text));
        txtBuyerSaleRate.Text = Convert.ToString(Math.Abs(salerate));
        double buyercmmrate = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[9].Text));
        txtBuyerCommission.Text = Convert.ToString(Math.Abs(buyercmmrate));
        txtBuyerNarration.Text = Server.HtmlDecode(gridViewRow.Cells[10].Text);
        if (ViewState["currentTable"] != null)
        {

        }
        setFocusControl(txtBuyer);
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

    #region[btnMillCode_Click]
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
    #endregion

    #region [txtMillCode_TextChanged]
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
                        setFocusControl(txtGrade);
                        txtPaymentTo.Text = txtMillCode.Text;
                        lblPaymentTo.Text = millName;
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
    #endregion

    #region [btnGrade_Click]
    protected void btnGrade_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "GR";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region [txtGrade_TextChanged]
    protected void txtGrade_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGrade.Text;
        //if (txtGrade.Text != string.Empty)
        //{
        //    bool a = true;
        //    if (txtGrade.Text.Length < 8)
        //    {
        //        a = clsCommon.isStringIsNumeric(txtGrade.Text);
        //    }
        //    if (a == false)
        //    {
        //        btnGrade_Click(this, new EventArgs());
        //    }
        //    else
        //    {
        //        pnlPopup.Style["display"] = "none";
        //    }
        //}
        setFocusControl(txtQuantal);
    }
    #endregion

    #region[btnPaymentTo_Click]
    protected void btnPaymentTo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "PT";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPaymentTo_TextChanged]
    protected void txtPaymentTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtPaymentTo.Text;
            string paymentToname = string.Empty;
            if (txtPaymentTo.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtPaymentTo.Text);
                if (a == false)
                {
                    btnPaymentTo_Click(this, new EventArgs());
                }
                else
                {
                    paymentToname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtPaymentTo.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (paymentToname != string.Empty)
                    {
                        if (paymentToname.Length > 15)
                        {
                            paymentToname.Substring(0, 15);
                        }
                        else if (paymentToname.Length > 10)
                        {
                            paymentToname.Substring(0, 10);
                        }
                        lblPaymentTo.Text = paymentToname;
                        setFocusControl(txtDO);
                    }
                    else
                    {
                        txtPaymentTo.Text = string.Empty;
                        lblPaymentTo.Text = string.Empty;
                        setFocusControl(txtPaymentTo);
                    }
                }
            }
            else
            {
                txtPaymentTo.Text = string.Empty;
                lblPaymentTo.Text = paymentToname;
                setFocusControl(txtPaymentTo);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnTenderFrom_Click]
    protected void btnTenderFrom_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "TF";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtTenderFrom_TextChanged]
    protected void txtTenderFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtTenderFrom.Text;
            string tenderFromName = string.Empty;
            if (txtTenderFrom.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtTenderFrom.Text);
                if (a == false)
                {
                    btnTenderFrom_Click(this, new EventArgs());
                }
                else
                {
                    tenderFromName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtTenderFrom.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (tenderFromName != string.Empty)
                    {
                        if (tenderFromName.Length > 15)
                        {
                            tenderFromName.Substring(0, 15);
                        }
                        else if (tenderFromName.Length > 10)
                        {
                            tenderFromName.Substring(0, 10);
                        }
                        lblTenderFrom.Text = tenderFromName;
                        setFocusControl(txtVoucherBy);
                    }
                    else
                    {
                        txtTenderFrom.Text = string.Empty;
                        lblTenderFrom.Text = string.Empty;
                        setFocusControl(txtTenderFrom);
                    }
                }
            }
            else
            {
                txtTenderFrom.Text = string.Empty;
                lblTenderFrom.Text = tenderFromName;
                setFocusControl(txtTenderFrom);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnTenderDO_Click]
    protected void btnTenderDO_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "DO";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtDO_TextChanged]
    protected void txtDO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtDO.Text;
            string doName = string.Empty;
            if (txtDO.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtDO.Text);
                if (a == false)
                {
                    btnTenderDO_Click(this, new EventArgs());
                }
                else
                {
                    doName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtDO.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (doName != string.Empty)
                    {
                        if (doName.Length > 15)
                        {
                            doName.Substring(0, 15);
                        }
                        else if (doName.Length > 10)
                        {
                            doName.Substring(0, 10);
                        }
                        lblDO.Text = doName;
                        setFocusControl(txtBroker);
                    }
                    else
                    {
                        txtDO.Text = string.Empty;
                        lblDO.Text = string.Empty;
                        setFocusControl(txtDO);
                    }
                }
            }
            else
            {
                txtDO.Text = string.Empty;
                lblDO.Text = doName;
                setFocusControl(txtDO);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region[btnVoucherBy_Click]
    protected void btnVoucherBy_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "VB";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);

        }
        catch
        {

        }
    }
    #endregion

    #region[txtVoucherBy_TextChanged]
    protected void txtVoucherBy_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtVoucherBy.Text;
            string voucherByName = string.Empty;
            if (txtVoucherBy.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtVoucherBy.Text);
                if (a == false)
                {
                    btnVoucherBy_Click(this, new EventArgs());
                }
                else
                {
                    voucherByName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtVoucherBy.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (voucherByName != string.Empty)
                    {
                        if (voucherByName.Length > 15)
                        {
                            voucherByName.Substring(0, 15);
                        }
                        else if (voucherByName.Length > 10)
                        {
                            voucherByName.Substring(0, 10);
                        }
                        lblVoucherBy.Text = voucherByName;
                        setFocusControl(txtExciseRate);
                    }
                    else
                    {
                        txtVoucherBy.Text = string.Empty;
                        lblVoucherBy.Text = string.Empty;
                        setFocusControl(txtVoucherBy);
                    }
                }
            }
            else
            {
                txtVoucherBy.Text = string.Empty;
                lblVoucherBy.Text = voucherByName;
                setFocusControl(txtVoucherBy);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnBroker_Click]
    protected void btnBroker_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "BR";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtBroker_TextChanged]
    protected void txtBroker_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBroker.Text;
            string brokerName = string.Empty;
            if (txtBroker.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtBroker.Text);
                if (a == false)
                {
                    btnBroker_Click(this, new EventArgs());
                }
                else
                {
                    brokerName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtBroker.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (brokerName != string.Empty)
                    {
                        if (brokerName.Length > 15)
                        {
                            brokerName.Substring(0, 15);
                        }
                        else if (brokerName.Length > 10)
                        {
                            brokerName.Substring(0, 10);
                        }
                        lblBroker.Text = brokerName;
                        //if (grdDetail.Rows.Count > 0)
                        //{
                        //    grdDetail.Rows[0].Cells[5].Text = Convert.ToString(txtBroker.Text);
                        //    grdDetail.Rows[0].Cells[6].Text = Convert.ToString(lblBroker.Text);
                        //}
                        setFocusControl(txtTenderFrom);
                    }
                    else
                    {
                        txtBroker.Text = string.Empty;
                        lblBroker.Text = string.Empty;
                        setFocusControl(txtBroker);
                    }
                }
            }
            else
            {
                txtBroker.Text = string.Empty;
                lblBroker.Text = brokerName;
                setFocusControl(txtBroker);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region[btnBuyer_Click]
    protected void btnBuyer_Click(object sender, EventArgs e)
    {
        pnlPopupTenderDetails.Style["display"] = "block";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "BU";
        btnSearch_Click(sender, e);
    }
    #endregion

    #region[txtBuyer_TextChanged]
    protected void txtBuyer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBuyer.Text;
            string buyerName = string.Empty;
            if (txtBuyer.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtBuyer.Text);
                if (a == false)
                {
                    btnBuyer_Click(this, new EventArgs());
                }
                else
                {
                    buyerName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBuyer.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (buyerName != string.Empty)
                    {
                        lblBuyerName.Text = buyerName;
                        setFocusControl(drpDeliveryType);
                        // if (ViewState["mode"].ToString() == "I")
                        //{
                        txtBuyerCommission.Text = clsCommon.getString("select Commission from " + AccountMasterTable + " where Ac_Code=" + txtBuyer.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //}
                    }
                    else
                    {
                        txtBuyer.Text = string.Empty;
                        lblBuyerName.Text = string.Empty;
                        txtBuyer.Focus();
                        setFocusControl(txtBuyer);
                    }
                }
            }
            else
            {
                txtBuyer.Text = string.Empty;
                lblBuyerName.Text = buyerName;
                setFocusControl(txtBuyer);
            }

        }
        catch
        {
        }

    }
    #endregion

    #region[txtBuyerParty_TextChanged]
    protected void txtBuyerParty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBuyerParty.Text;
            string buyerPartyName = string.Empty;
            if (txtBuyerParty.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtBuyerParty.Text);
                if (a == false)
                {
                    btnBuyerParty_Click(this, new EventArgs());
                }
                else
                {
                    buyerPartyName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBuyerParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (buyerPartyName != string.Empty)
                    {
                        lblBuyerPartyName.Text = buyerPartyName;
                        setFocusControl(txtBuyerQuantal);
                    }
                    else
                    {
                        txtBuyerParty.Text = string.Empty;
                        lblBuyerPartyName.Text = string.Empty;
                        setFocusControl(txtBuyerParty);

                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region[btnBuyerParty_Click]
    protected void btnBuyerParty_Click(object sender, EventArgs e)
    {
        pnlPopupTenderDetails.Style["display"] = "block";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "BP";
        btnSearch_Click(sender, e);
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

    #region [txtQuantal_TextChanged]
    protected void txtQuantal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtPacking);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }



            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            else
            {
                #region decide whether actual row is updating or virtual [rowAction]
                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='1' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                DataRow dr = null;
                DataTable dt = (DataTable)ViewState["currentTable"];

                dr = dt.Rows[0];
                if (id != string.Empty)
                {
                    dr["rowAction"] = "U";   //actual row
                }
                else
                {
                    dr["rowAction"] = "A";    //virtual row
                }


                #endregion
                ViewState["currentTable"] = dt;
                grdDetail.DataSource = dt;
                grdDetail.DataBind();
                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
            }

            this.calculateAmount();
            this.calculateBalanceSelf();
        }
        catch
        {

        }
    }
    #endregion

    #region[txtPacking_TextChanged]
    protected void txtPacking_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [txtDate_TextChanged]
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDate.Text != string.Empty)
            {
                DateTime d = new DateTime();
                // d = DateTime.Now;
                string date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                //  d = DateTime.Parse(date);
                // d = d.AddDays(15);

                txtLiftingDate.Text = clsCommon.getString("select Convert(varchar(10),DATEADD(day,15,'" + date + "'),103) as d");

                //  txtLiftingDate.Text = DateTime.Parse(d.ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                setFocusControl(txtLiftingDate);
            }
            else
            {
                setFocusControl(txtDate);
            }
        }
        catch
        {
            txtDate.Text = string.Empty;
            setFocusControl(txtDate);
            calenderExtenderDate.Animated = true;
        }
    }
    #endregion

    #region [txtLiftingDate_TextChanged]
    protected void txtLiftingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtLiftingDate.Text != string.Empty)
            {
                string d = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                setFocusControl(txtMillCode);
            }
        }
        catch
        {
            txtLiftingDate.Text = string.Empty;
            setFocusControl(txtLiftingDate);
        }
    }
    #endregion

    #region[txtMillRate_TextChanged]
    protected void txtMillRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMillRate.Text != string.Empty)
            {
                this.calculateAmount();
                if (drpResale.SelectedValue == "R")
                {
                    this.calculateDiff();
                }
                if (drpResale.SelectedValue == "M")
                {
                    this.setFocusControl(txtExciseRate);
                }
                else
                {
                    this.setFocusControl(txtPurcRate);
                }
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [calculateDiff]
    protected void calculateDiff()
    {
        try
        {
            float millrate = 0;
            float purcRate = 0;
            float diff = 0;
            if (txtMillRate.Text != string.Empty)
            {
                millrate = float.Parse(txtMillRate.Text);
            }
            if (txtPurcRate.Text != string.Empty)
            {
                purcRate = float.Parse(txtPurcRate.Text);
            }

            diff = millrate - purcRate;
            lbldiff.Text = diff.ToString();
        }
        catch
        {

        }
    }
    #endregion

    #region [calculateAmount]
    protected void calculateAmount()
    {
        try
        {
            float quantal = 0;
            float millrate = 0;
            double amount = 0;
            float purcrate = 0;
            float diff = 0;

            if (txtQuantal.Text != string.Empty)
            {
                quantal = float.Parse(txtQuantal.Text);
            }
            if (txtMillRate.Text != string.Empty)
            {
                millrate = float.Parse(txtMillRate.Text);
            }
            if (txtPurcRate.Text != string.Empty)
            {
                purcrate = float.Parse(txtPurcRate.Text);
            }
            if (lbldiff.Text != string.Empty)
            {
                diff = float.Parse(lbldiff.Text);
            }

            if (drpResale.SelectedValue == "M")
            {
                amount = quantal * millrate;

            }
            else
            {
                amount = quantal * diff;
            }
            lblAmount.Text = Math.Round(Math.Abs(amount), 2).ToString();

            double gstrate = Math.Round((millrate * 5 / 100), 2);
            txtExciseRate.Text = gstrate.ToString();
            lblMillRateGst.Text = (millrate + gstrate).ToString();
        }
        catch
        {

        }
    }
    #endregion

    #region[calculateBalanceSelf]

    /// <summary>
    /// /////////////
    /// </summary>
    protected void calculateBalanceSelf()
    {
        try
        {
            float quantal = 0;
            float balanceSelf = 0;
            float buyerQuantal = 0;
            float quantalTotal = 0;

            //calculate total of quantals in grid

            for (int i = 1; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[12].Text != "D" && grdDetail.Rows[i].Cells[12].Text != "R")
                {
                    //                   if (grdDetail.Rows[i].RowIndex != 0)
                    //                 {
                    quantalTotal = quantalTotal + float.Parse(grdDetail.Rows[i].Cells[7].Text);
                    //               }
                }
            }
            //  quantalTotal = quantalTotal + buyerQuantal;

            if (txtQuantal.Text != string.Empty)
            {
                quantal = float.Parse(txtQuantal.Text);
            }

            if (lblBalanceSelf.Text != string.Empty)
            {
                balanceSelf = float.Parse(lblBalanceSelf.Text);
            }
            if (txtBuyerQuantal.Text != string.Empty)
            {
                buyerQuantal = float.Parse(txtBuyerQuantal.Text);
            }
            balanceSelf = quantal - quantalTotal;
            lblBalanceSelf.Text = balanceSelf.ToString();

            //set to first row balance self
            grdDetail.Rows[0].Cells[7].Text = balanceSelf.ToString();
            //  grdDetail.Rows[0].Cells[12].Text = "U";
            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPurcRate_TextChanged]
    protected void txtPurcRate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            this.calculateDiff();
            this.calculateAmount();
            setFocusControl(txtPaymentTo);
        }
        catch
        {

        }
    }
    #endregion

    #region [drpResale_SelectedIndexChanged]
    protected void drpResale_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Enabled = false;
                rfvtxtPurcRate.Enabled = false;
                setFocusControl(txtDate);
            }
            else
            {
                txtPurcRate.Enabled = true;
                setFocusControl(txtDate);
                rfvtxtPurcRate.Enabled = true;
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [txtTenderNo_TextChanged]
    protected void txtTenderNo_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtTenderNo.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtTenderNo.Text != string.Empty)
                {
                    txtValue = txtTenderNo.Text;

                    string qry = "select * from " + tblHead + " where Tender_No='" + txtValue + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Tender No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        this.getMaxCode();
                                        txtTenderNo.Enabled = false;
                                        //Session["enableSave"] = 1;
                                        btnSave.Enabled = true;   //IMP
                                        setFocusControl(drpResale);
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        bool recordExist = this.fetchRecord(txtValue);
                                        if (recordExist == true)
                                        {
                                            //txtTenderNo.Enabled = true;
                                            pnlgrdDetail.Enabled = true;
                                            setFocusControl(drpResale);
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(drpResale);
                                    txtTenderNo.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                    calculateBalanceSelf();
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("A");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtTenderNo.Text = string.Empty;
                                    setFocusControl(txtTenderNo);
                                    calculateBalanceSelf();
                                    //txtTenderNo.Enabled = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    txtTenderNo.Focus();
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Tender No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtTenderNo.Text = string.Empty;
                txtTenderNo.Focus();
            }
        }
        catch
        {

        }
        #endregion
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
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }

            if (hdnfClosePopup.Value == "MM")
            {
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable + " where Ac_type='M' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'" + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "PT")
            {
                lblPopupHead.Text = "--Select Payment To--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "TF")
            {
                lblPopupHead.Text = "--Select Tender From--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "DO")
            {
                lblPopupHead.Text = "--Select DO--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "VB")
            {
                lblPopupHead.Text = "--Select Voucher By--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable + " where  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "BR")
            {
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],CityName as [City] from " + AccountMasterTable + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "BU")
            {
                lblPopupHead.Text = "--Select Buyer--";
                string qry = "SELECT     Ac_Code AS [Account  Code], Ac_Name_E AS [Account Name], Short_Name, CityMaster.city_name_e AS City "
                    + " FROM         AccountMaster LEFT OUTER JOIN "
                    + " CityMaster ON AccountMaster.Company_Code = CityMaster.company_code AND AccountMaster.City_Code = CityMaster.city_code "
                     + " where AccountMaster.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and AccountMaster.Ac_Name_E like'%" + txtSearchText.Text + "%' ";
                // + " where Ac_Code like'%" + txtSearchText.Text + "%' or Ac_Name_E like'%" + txtSearchText.Text + "%' or Short_Name like'%" + txtSearchText.Text + "%'";
                qry = qry.Replace("AccountMaster", AccountMasterTable);
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "BP")
            {
                lblPopupHead.Text = "--Select Buyer Party--";
                string qry = "SELECT     Ac_Code AS [Account  Code], Ac_Name_E AS [Account Name], Short_Name, CityMaster.city_name_e AS City "
                            + " FROM         AccountMaster LEFT OUTER JOIN "
                            + " CityMaster ON AccountMaster.Company_Code = CityMaster.company_code AND AccountMaster.City_Code = CityMaster.city_code "
                            + " where AccountMaster.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and AccountMaster.Ac_Code like'%" + txtSearchText.Text + "%' or AccountMaster.Ac_Name_E like'%" + txtSearchText.Text + "%' or AccountMaster.Short_Name like'%" + txtSearchText.Text + "%'";
                qry = qry.Replace("AccountMaster", AccountMasterTable);
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "TN")
            {
                lblPopupHead.Text = "--Select Tender --";
                // string qry = "select distinct Tender_No,CONVERT(Date, Tender_Date,103) as Tender_Date,millname,Quantal,Packing,Mill_Rate,doname from " + qryCommon + " where ([millname] like '%" + txtSearchText.Text + "%' or Tender_No like '%" + txtSearchText.Text + "%' or doname like '%" + txtSearchText.Text + "%') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Tender_Date desc  ,Tender_No";

                qry = "SELECT     Tender_No, Tender_Date,  millname, Quantal,Grade,buyerbrokershortname, Buyer_Quantal,Mill_Rate, Sale_Rate,doname" +
                " FROM  " + qryCommon + " where Buyer=2 and (Tender_No like '%" + txtSearchText.Text + "%' or Tender_Date like '%" + txtSearchText.Text + "%' or millname like '%" + txtSearchText.Text + "%') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Tender_No";
                this.showPopup(qry);
            }
            //  hdnfClosePopup.Value = "";
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
            if (hdnfClosePopup.Value == "MM")
            {
                setFocusControl(txtMillCode);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                setFocusControl(txtGrade);
            }
            if (hdnfClosePopup.Value == "PT")
            {
                setFocusControl(txtPaymentTo);
            }
            if (hdnfClosePopup.Value == "TF")
            {
                setFocusControl(txtTenderFrom);
            }
            if (hdnfClosePopup.Value == "DO")
            {
                setFocusControl(txtDO);
            }
            if (hdnfClosePopup.Value == "VB")
            {
                setFocusControl(txtVoucherBy);
            }
            if (hdnfClosePopup.Value == "BR")
            {
                setFocusControl(txtBroker);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtBuyer);
            }
            if (hdnfClosePopup.Value == "BP")
            {
                setFocusControl(txtBuyerParty);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtBuyer);
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
    #endregion

    #region [btnChangeNo_Click]
    protected void changeNo_click(object sender, EventArgs e)
    {
        try
        {

            //if (hdnfClosePopup.Value =="TN")
            //{

            if (btnChangeNo.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtTenderNo.Text = string.Empty;
                //txtTenderNo.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtTenderNo);

            }
            if (btnChangeNo.Text == "Choose No")
            {
                try
                {
                    pnlgrdDetail.Enabled = true;
                    setFocusControl(txtSearchText);


                    hdnfClosePopup.Value = "TN";
                    pnlPopup.Style["display"] = "block";
                    btnSearch_Click(sender, e);

                }
                catch
                {

                }
                //}
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[11].Visible = true;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(3);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(4);
            e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(3);
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(25);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(25);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(8);
            e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(8);
            e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(8);
            e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[11].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[10].Style["overflow"] = "hidden";
            e.Row.Cells[10].ToolTip = e.Row.Cells[10].Text;

            //e.Row.Cells[0].Style["overflow"] = "hidden";
            //e.Row.Cells[1].Style["overflow"] = "hidden";
            //e.Row.Cells[2].Style["overflow"] = "hidden";
            //e.Row.Cells[3].Style["overflow"] = "hidden";
            //e.Row.Cells[4].Style["overflow"] = "hidden";
            //e.Row.Cells[5].Style["overflow"] = "hidden";
            //e.Row.Cells[6].Style["overflow"] = "hidden";
            //e.Row.Cells[7].Style["overflow"] = "hidden";
            //e.Row.Cells[8].Style["overflow"] = "hidden";
            //e.Row.Cells[9].Style["overflow"] = "hidden";
            //e.Row.Cells[10].Style["overflow"] = "hidden";
            //e.Row.Cells[11].Style["overflow"] = "hidden";
            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;

                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 33)
                    {
                        cell.Text = cell.Text.Substring(0, 33) + "...";
                        cell.ToolTip = s;
                    }
                }
            }
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

        //for (int i = 0; i < e.Row.Cells.Count; i++)
        //{
        //    e.Row.Cells[i].Style["overflow"] = "hidden";
        //}
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (hdnfClosePopup.Value == "TN")
            {
                if (e.Row.RowType != DataControlRowType.Pager)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[3].Width = new Unit("80px");
                    e.Row.Cells[4].Width = new Unit("100px");

                }
            }

            if (v == "MM" || v == "PT" || v == "TF" || v == "DO" || v == "VB" || v == "BR")
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
                    //e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    //e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(60);
                    //e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(30);
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
            }

            if (v == "BU" || v == "BP")
            {
                if (e.Row.RowType != DataControlRowType.Pager)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                }
            }
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
                searchString = txtSearchText.Text;
                strTextbox = hdnfClosePopup.Value;
                setFocusControl(btnSearch);
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [txtBuyerQuantal_TextChanged]
    protected void txtBuyerQuantal_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerSaleRate);
    }
    #endregion

    #region [txtBuyerSaleRate_TextChanged]
    protected void txtBuyerSaleRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerCommission);
    }
    #endregion

    #region [txtBuyerCommission_TextChanged]
    protected void txtBuyerCommission_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerNarration);
    }
    #endregion

    #region [txtBuyerNarration_TextChanged]
    protected void txtBuyerNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnADDBuyerDetails);
    }
    #endregion

    #region [txtExciseRate_TextChanged]
    protected void txtExciseRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtSellNoteNo);
    }
    #endregion

    #region [txtNarration_TextChanged]
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        this.setFocusControl(Button1);
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    protected void btn_Click(object sender, EventArgs e)
    {
        //string url = "http://www.google.com";
        //StringBuilder sb = new StringBuilder();
        //sb.Append("<script type = 'text/javascript'>");
        //sb.Append("window.open('");
        //sb.Append(url);
        //sb.Append(",'_blank'");
        //sb.Append("');");
        //sb.Append("</script>");
        //ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void drpDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerParty);
        if (drpDeliveryType.SelectedValue == "N")
        {
            txtBuyerCommission.Enabled = false;
        }
        else
        {
            txtBuyerCommission.Enabled = true;
        }
    }
    protected void txtSellNoteNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            setFocusControl(txtNarration);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.fetchRecord(txtEditDoc_No.Text);
            pnlgrdDetail.Enabled = true;
            setFocusControl(txtEditDoc_No);
        }
        catch (Exception)
        {
            throw;
        }
    }
}


