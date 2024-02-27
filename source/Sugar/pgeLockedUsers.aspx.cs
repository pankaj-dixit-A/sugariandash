using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Sugar_pgeLockedUsers : System.Web.UI.Page
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
                Bindgrid();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }

    private void Bindgrid()
    {
        try
        {
            qry = "Select User_Name from tblUser where IsLocked=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        datagrid.DataSource = dt;
                        datagrid.DataBind();
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        datagrid.PageIndex = e.NewPageIndex;
        Bindgrid();
    }
    protected void datagrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("300px");
        }
    }
    protected void datagrid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
}