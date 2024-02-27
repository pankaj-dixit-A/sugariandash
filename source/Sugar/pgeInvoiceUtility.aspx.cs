using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeInvoiceUtility : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "InvoiceUtility";
        user = Session["user"].ToString();
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                this.showLastRecord();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
        if (objAsp != null)
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
    }
    private void showLastRecord()
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            txtBranchCode.Text = Convert.ToInt32(Session["Branch_Code"].ToString()).ToString();
            txtBranchname.Text = clsCommon.getString("select Branch from BranchMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");


            qry = "select * from " + tblHead + " where Branch_Code=" + Convert.ToInt32(Session["Branch_Code"].ToString()) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtInvoiceHeader.Text = dt.Rows[0]["Invoice_Header"].ToString();
                        txtInvLeft.Text = dt.Rows[0]["Invoice_Leftside"].ToString();
                        txtInvRight.Text = dt.Rows[0]["Invoice_Rightside"].ToString();
                        txtInvfooter.Text = dt.Rows[0]["Invoice_Footer"].ToString();
                        txtbankName.Text = dt.Rows[0]["Bank_Name"].ToString();
                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string retValue = string.Empty;
            string strRev = string.Empty;
            DataSet ds = new DataSet();
            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
            {
                string branchid = clsCommon.getString("select Branch_Code from " + tblHead + " where Branch_Code=" + Convert.ToInt32(Session["Branch_Code"].ToString()) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (branchid != string.Empty)
                {
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "Invoice_Header='" + txtInvoiceHeader.Text + "',Invoice_Leftside='" + txtInvLeft.Text + "', " +
                        " Invoice_Rightside='" + txtInvRight.Text + "' ,Bank_Name='" + txtbankName.Text + "',Invoice_Footer='" + txtInvfooter.Text + "',Modified_By='" + userinfo + "' " +
                        " where Branch_Code=" + Convert.ToInt32(Session["Branch_Code"].ToString()) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strRev);

                    retValue = strRev;
                }
                else
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "Branch_Code,Invoice_Header,Invoice_Leftside, Invoice_Rightside ,Bank_Name,Invoice_Footer,Company_Code,Created_By";

                    obj.values = "'" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + txtInvoiceHeader.Text + "','" + txtInvLeft.Text + "','" + txtInvRight.Text + "','" + txtbankName.Text + "','" + txtInvfooter.Text + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + user + "'";
                    ds = obj.insertAccountMaster(ref strRev);

                    retValue = strRev;
                }

                if (retValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);

                }
                if (retValue == "-2")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                }
            }
        }
        catch
        {

        }
    }
}