using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_pgeBillSummary : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    static WebControl objAsp = null;
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
                
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
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

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sd", "javascript:prs('" + fromDT + "','" + toDT + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:srs('" + fromDT + "','" + toDT + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "anku", "javascript:mprs('" + fromDT + "','" + toDT + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ankey", "javascript:msrs('" + fromDT + "','" + toDT + "')", true);
    }

    protected void btnMonthWisePurchaseSale_Click(object sender, EventArgs e)
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

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "assadsad", "javascript:mwpr('" + fromDT + "','" + toDT + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asasdsad", "javascript:mwsr('" + fromDT + "','" + toDT + "')", true);
    }
}