using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Data;

public partial class Sugar_pgeUploadSignature : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        user = Session["user"].ToString();
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
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (fu1.HasFile)
            {
                string fileName = Path.GetFileName(fu1.PostedFile.FileName);
                fu1.SaveAs(Server.MapPath("~/Images/" + fileName));

                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    string retVal = "";
                    string id = clsCommon.getString("Select ID from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (id == string.Empty)
                    {
                        obj.flag = 1;
                        obj.tableName = "tblSign";
                        obj.columnNm = "ID,ImageName,ImagePath,Company_Code";
                        obj.values = "'" + 1 + "','" + fileName + "','" + "~/Images/" + fileName + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                        DataSet ds = new DataSet();
                        ds = obj.insertAccountMaster(ref retVal);
                    }
                    else
                    {
                        obj.flag = 2;
                        obj.tableName = "tblSign";
                        obj.columnNm = "ImageName='" + fileName + "',ImagePath='" + "~/Images/" + fileName + "' where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and ID=1";
                        obj.values = "none";
                        DataSet ds = new DataSet();
                        ds = obj.insertAccountMaster(ref retVal);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Uploaded Successfully!')", true);
                    //Response.Redirect("~/Sugar/pgeUploadSignature.aspx", false);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}