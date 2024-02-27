using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgePendingDelivery : System.Web.UI.Page
{
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string tblPrefix = string.Empty;
    public DataSet ds = null;
    public DataTable dt = null;
    string deliverytype = string.Empty;
    string uptodate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        uptodate = DateTime.Now.ToString("yyyy/MM/dd");
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
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        try
        {
            deliverytype = drpCategory.SelectedValue.ToString();
            qry = "select millname as Mill,Lifting_Date as L_Date,buyerbrokerfullname as Party,Buyer_Quantal as Qntl,Sale_Rate,balance,a.Mobile_No as mobile from qrysugarBalancestock s" +
                " left outer join " + tblPrefix + "AccountMaster a on s.buyerbrokerfullname=a.Ac_Name_E and s.Company_Code=a.Company_Code" +
                " where s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and s.balance>0 and s.Delivery_Type='" + deliverytype + "' and s.Tender_Date<='" + uptodate + "'" +
                "  order by Convert(DateTime,s.Lifting_Date,103) asc";

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
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
        catch (Exception)
        {

            throw;
        }
    }
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("300px");
            e.Row.Cells[2].Width = new Unit("300px");
            // e.Row.Cells[2].Style.Add("overflow", "hidden");
            e.Row.Cells[1].Width = new Unit("80px");
            e.Row.Cells[3].Width = new Unit("80px");
            e.Row.Cells[4].Width = new Unit("80px");
            e.Row.Cells[5].Width = new Unit("80px");
            e.Row.Cells[7].Width = new Unit("60px");
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            foreach (TableCell cell in e.Row.Cells)
            {
                i++;
                string s = cell.Text;
                if (cell.Text.Length > 36)
                {
                    cell.Text = cell.Text.Substring(0, 36) + "..";
                    cell.ToolTip = s;
                }
            }
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
    }
    protected void btnSendSms_Click(object sender, EventArgs e)
    {

    }
}