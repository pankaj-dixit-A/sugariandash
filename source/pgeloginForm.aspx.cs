using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class pgeloginForm : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        txtPassword.Attributes.Add("onmousedown", "return noCopyMouse(event);");
        txtPassword.Attributes.Add("onkeydown", "return noCopyKey(event);");
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string MyLogin = txtPassword.Text;
            qry = "select Password from GroupUser where Login_Name COLLATE Latin1_general_CS_AS='" + txtLoginName.Text + "' and userType='A'";
            string Password = clsCommon.getString(qry);
            if (MyLogin == Password)
            {
                string groupCode = "0";
                qry = "select Group_Code from GroupUser where Login_Name='" + txtLoginName.Text + "' and Password='" + txtPassword.Text + "' and userType='A'";
                ds = clsDAL.SimpleQuery(qry);
                //Response.Write("<script>alert('Before');</script>");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        Response.Write("<script>alert('Wrong User Name or Password!');</script>");
                        if (dt.Rows.Count > 0)
                        {
                            Response.Write("<script>alert('In');</script>");
                            groupCode = dt.Rows[0]["Group_Code"].ToString();
                            EncryptPass En = new EncryptPass();
                            string pass = En.Encrypt(txtPassword.Text);
                            string nm = En.Encrypt(txtLoginName.Text);
                            //connect to another database of specific group
                            /// API Call to pass group code
                            Response.Redirect("Sugar/pgeStartup.aspx?GC=" + groupCode + "&nm=" + nm + "&pd=" + pass + "", true);
                            // Response.Redirect("http://192.168.1.75/AccoWeb/Sugar/pgeStartup.aspx?GroupCode=" + groupCode + "&uname=" + txtLoginName.Text + "&pass=" + txtPassword.Text + "", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Wrong User Name Or Password!');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Wrong User Name Or Password!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Wrong User Name Or Password!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Wrong User Name Or Password!');", true);
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }
    protected void btnCoustomerLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string Ac_Code = string.Empty;
            //qry = "select * from tblUser where User_Name=" + txtCustomerLoginName.Text + " AND Password='" + txtCustomerPassword.Text + "' AND User_Type='C'";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Ac_Code = dt.Rows[0]["User_Name"].ToString();
                        Response.Redirect("Customer Page/pgeCustStartup.aspx?Ac_Code=" + Ac_Code + "", false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Wrong User Name or Password!')", true);
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/login.aspx", false);
    }
}