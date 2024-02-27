using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeCarporateRegister : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string Branch_Code = string.Empty;
    string uptodate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uptodate = DateTime.Now.ToString("yyyy/MM/dd");
            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    txtFromDate.Text = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    fillBranches();
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
    protected void btnBalanceReport_Click(object sender, EventArgs e)
    {
        BranchCode();
        string fromDt;
        string toDt;
        DateFormat(out fromDt, out toDt);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kt", "javascript:br('" + fromDt + "','" + toDt + "','" + Branch_Code + "')", true);
    }

    private void DateFormat(out string fromDt, out string toDt)
    {
        fromDt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        toDt = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BranchCode();
        string fromDt;
        string toDt;
        DateFormat(out fromDt, out toDt);
        string pds = drpSellingType.SelectedValue.ToString();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kd", "javascript:csd('" + fromDt + "','" + toDt + "','" + pds + "','" + Branch_Code + "')", true);
    }
}