using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_pgeTrialBalance : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = clsGV.Start_Date;
            txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                radioFilter.SelectedValue = "B";
                drpGroup.Enabled = true;
                drpAcType.Enabled = false;
                this.fillDropdown();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    private void fillDropdown()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string qry = "";
            ListItem li = new ListItem("--- All ---", "0");
            drpGroup.Items.Clear();
            qry = "select group_Code,group_Name_E from " + tblPrefix + "BSGroupMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by group_Name_E asc";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        drpGroup.DataSource = dt;
                        drpGroup.DataTextField = "group_Name_E";
                        drpGroup.DataValueField = "group_Code";
                        drpGroup.DataBind();
                    }
                }
            }
            drpGroup.Items.Insert(0, li);
        }
        catch
        {

        }
    }
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        try
        {
            string Cwhere = "";
            string Doc_Date = "";
            string FromDate = "";
            string ToDate = "";

            if (txtToDt.Text != string.Empty)
            {
                try
                {
                    string fromDt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    string dt = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                    FromDate = fromDt;
                    ToDate = dt;
                    Doc_Date = " DOC_DATE<='" + dt + "'";
                }
                catch
                {
                    txtToDt.Text = string.Empty;
                    setFocusControl(txtToDt);
                    return;
                }
            }

            if (radioFilter.SelectedValue == "B")
            {
                //if (drpGroup.SelectedIndex != 0)
                //{
                if (drpGroup.SelectedValue == "0")
                {

                }
                else
                {
                    Cwhere = " and Group_Code=" + drpGroup.SelectedValue;
                }
                //}
                //else
                //{
                //    setFocusControl(drpGroup);
                //    return;
                //}
            }
            else
            {
                if (drpAcType.SelectedValue == "PM")
                {
                    Cwhere = " and Ac_type in ('P','M')";
                }
                else
                {
                    Cwhere = " and Ac_type='" + drpAcType.SelectedValue + "'";
                }

            }
            Cwhere = Cwhere.Replace(' ', '+');
            Cwhere = Cwhere.Replace("'", "-");
            Doc_Date = Doc_Date.Replace("'", "-");
            //  string dt=
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:sp('" + Doc_Date + "','" + Cwhere + "','" + FromDate + "','" + ToDate + "')", true);
        }
        catch
        {

        }
    }
    protected void radioFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (radioFilter.SelectedValue == "B")
            {
                drpGroup.Enabled = true;
                drpAcType.Enabled = false;
            }
            else
            {
                drpGroup.Enabled = false;
                drpAcType.Enabled = true;
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


    protected void btnDetailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = "";
            string toDate = "";
            string whr1 = "";
            if (txtFromDate.Text != string.Empty)
            {
                try
                {
                    fromdt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                }
                catch
                {
                    txtFromDate.Text = string.Empty;
                    setFocusControl(txtFromDate);
                    return;
                }
            }

            if (txtToDt.Text != string.Empty)
            {
                try
                {
                    toDate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                }
                catch
                {
                    txtToDt.Text = string.Empty;
                    setFocusControl(txtToDt);
                    return;
                }
            }
            if (drpGroup.SelectedValue == "0")
            {

            }
            else
            {
                whr1 = " and Group_Code=" + drpGroup.SelectedValue;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:DetailReport('" + fromdt + "','" + toDate + "','" + whr1 + "')", true);
        }
        catch
        {

        }
    }

    protected void btnTFormat_Click(object sender, EventArgs e)
    {
        try
        {
            string toDate = "";
            string whr1 = "";
            if (txtToDt.Text != string.Empty)
            {
                try
                {
                    toDate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                }
                catch
                {
                    txtToDt.Text = string.Empty;
                    setFocusControl(txtToDt);
                    return;
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:TFormat('" + toDate + "','" + whr1 + "')", true);
        }
        catch
        {


        }
    }
    protected void btnCheckdiff_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string break1 = string.Empty;
        qry = "select  TRAN_TYPE,CASHCREDIT,DOC_NO,sum(case AMOUNT,DRCR FROM " + tblPrefix + "GLEDGER WHERE COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " AND YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString()) + " order by TRAN_TYPE,CASHCREDIT,DOC_NO";
        ds = clsDAL.SimpleQuery(qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                break1 = ds.Tables[0].Rows[i]["TRAN_TYPE"].ToString() + ds.Tables[0].Rows[i]["CASHCREDIT"].ToString() + ds.Tables[0].Rows[i]["DOC_NO"].ToString();

                for (int J = 0; J < ds.Tables[0].Rows.Count; J++)
                {

                }
            }
        }

    }
    protected void btnOpeningBalance_Click(object sender, EventArgs e)
    {
        try
        {
            string Cwhere = "";
            string Doc_Date = "";
            if (txtToDt.Text != string.Empty)
            {
                try
                {
                    string dt = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    Doc_Date = " DOC_DATE<='" + dt + "'";
                }
                catch
                {
                    txtToDt.Text = string.Empty;
                    setFocusControl(txtToDt);
                    return;
                }
            }

            if (radioFilter.SelectedValue == "B")
            {
                //if (drpGroup.SelectedIndex != 0)
                //{
                if (drpGroup.SelectedValue == "0")
                {

                }
                else
                {
                    Cwhere = " and Group_Code=" + drpGroup.SelectedValue;
                }
                //}
                //else
                //{
                //    setFocusControl(drpGroup);
                //    return;
                //}
            }
            else
            {
                if (drpAcType.SelectedValue == "PM")
                {
                    Cwhere = " and Ac_type in ('P','M')";
                }
                else
                {
                    Cwhere = " and Ac_type='" + drpAcType.SelectedValue + "'";
                }

            }
            Cwhere = Cwhere.Replace(' ', '+');
            Cwhere = Cwhere.Replace("'", "-");
            Doc_Date = Doc_Date.Replace("'", "-");
            //  string dt=
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kysds", "javascript:op('" + Doc_Date + "','" + Cwhere + "')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}