using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_pgeStockBook : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                txtFromDt.Text = clsGV.Start_Date; //*DateTime.Now.ToString("dd/MM/yyyy");*/ DateTime.Parse(DateTime.Now.ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                txtToDt.Text = clsGV.End_Date; //*DateTime.Now.ToString("dd/MM/yyyy");*/ DateTime.Parse(DateTime.Now.ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnStockBook_Click(object sender, EventArgs e)
    {
        string fromDT = "";
        string toDT = "";

        fromDT = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
        toDT = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ki", "javascript:sb('" + fromDT + "','" + toDT + "')", true);
    }
    protected void btnStockSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromDT = "";
            string toDT = "";
            fromDT = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            toDT = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kids", "javascript:ss('" + fromDT + "','" + toDT + "')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnMonthWiseStock_Click(object sender, EventArgs e)
    {
        try
        {
            string fromDT = "";
            string toDT = "";
            fromDT = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            toDT = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kidss", "javascript:MWS('" + fromDT + "','" + toDT + "')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}