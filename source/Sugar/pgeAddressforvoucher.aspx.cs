using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeAddressforvoucher : System.Web.UI.Page
{
    string user = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "CompanyParameters";

            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    string qry = "Select * from tblVoucherHeadAddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                    DataSet ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        txtAddressLine1.Text = ds.Tables[0].Rows[0]["AL1"].ToString();
                        txtAddressLine2.Text = ds.Tables[0].Rows[0]["AL2"].ToString();
                        txtAddressLine3.Text = ds.Tables[0].Rows[0]["AL3"].ToString();
                        txtAddressLine4.Text = ds.Tables[0].Rows[0]["AL4"].ToString();
                        txtOtherDetail.Text = ds.Tables[0].Rows[0]["Other"].ToString();
                    }

                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
        }
        catch
        {
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string AL1 = txtAddressLine1.Text;
            string AL2 = txtAddressLine2.Text;
            string AL3 = txtAddressLine3.Text;
            string AL4 = txtAddressLine4.Text;
            string Other = txtOtherDetail.Text;
            DataSet ds = new DataSet();
            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
            {
                string retValue = "";
                string strrev = "";
                string s = clsCommon.getString("select ID from tblVoucherHeadAddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (s == string.Empty)
                {
                    obj.flag = 1;
                    obj.tableName = "tblVoucherHeadAddress";
                    obj.columnNm = "ID,AL1,AL2,AL3,AL4,Other,Company_Code";
                    obj.values = "'" + 1 + "','" + AL1 + "','" + AL2 + "','" + AL3 + "','" + AL4 + "','" + Other + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                    ds = obj.insertAccountMaster(ref retValue);
                    strrev = retValue;
                }
                else
                {
                    obj.flag = 2;
                    obj.tableName = "tblVoucherHeadAddress";
                    obj.columnNm = "AL1='" + AL1 + "',AL2='" + AL2 + "',AL3='" + AL3 + "',AL4='" + AL4 + "',Other='" + Other + "' WHERE ID='" + s + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref retValue);
                    strrev = retValue;
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Saved!')", true);
            }


        }
        catch (Exception)
        {
            throw;
        }
    }
}