using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Net;
using System.IO;
public partial class pgeStartup : System.Web.UI.Page
{
    //data
    string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ///API 
        //http://localhost:3455/AccoWeb/pgeStartup.aspx?GroupCode=#GroupCode#
        try
        {
            ViewState["Group_Code"] = Request.QueryString["Group_Code"];
            Session["Group_Code"] = Request.QueryString["GC"];
            ViewState["uname"] = Request.QueryString["nm"];
            ViewState["pass"] = Request.QueryString["pd"];
            Session["tblPrefix"] = "NT_1_";
            //Session["tblPrefix"] = Session["tblPrefix"].ToString();
            Session["uname"] = ViewState["uname"].ToString();
            Session["pass"] = ViewState["pass"].ToString();
            string uname = ViewState["uname"].ToString();
            string pass = ViewState["pass"].ToString();
            Session["user"] = uname;
            //  ViewState["GroupCode"] = "1";
            if (!Page.IsPostBack)
            {
                string qry = "select count(*) from Company where Group_Code=" + Session["Group_Code"].ToString() + " ";
                int cnt = Convert.ToInt32(clsCommon.getString(qry));
                if (cnt > 0)
                {
                    this.fillGrid();
                    this.fillDropdownYear();
                }
                else
                {
                    #region insert into user table admin user entry
                    qry = "insert into tblUser ( User_Name,Password,User_Type) "
                       + " values('" + uname + "','" + pass + "','A')";
                    clsDAL.SimpleQuery(qry);
                    #endregion
                    Response.Redirect("pgeCreateNewCompany.aspx", true);
                }
            }
        }
        catch
        {
            //  Response.Redirect("http://localhost:2316/Sugar/HomePage/pgeloginForm.aspx");
        }
    }



    private void fillDropdownYear()
    {
        ListItem liDefault = new ListItem("Default", "0");
        try
        {
            drpYear.Items.Clear();
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ListItem li = new ListItem("---Select---", "0");
            qry = "select * from AccountingYear where Company_Code=" + Session["Company_Code"].ToString() + " order by yearCode desc";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        drpYear.DataSource = dt;
                        drpYear.DataTextField = dt.Columns[1].ToString();
                        drpYear.DataValueField = dt.Columns[0].ToString();
                        drpYear.DataBind();
                    }
                }
            }

            //drpYear.Items.Insert(0, li);

            drpBranch.Items.Clear();


            qry = "select Branch_Id,Branch from BranchMaster where Company_Code='" + Session["Company_Code"].ToString() + "'";
            ds = clsDAL.SimpleQuery(qry);
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
            //drpBranch.Items.Insert(0, liDefault);
        }
        catch
        {
            //drpBranch.Items.Insert(0, liDefault);
        }
    }

    private void fillGrid()
    {
        try
        {
            string qry = "select Company_Name_E,Company_Code from Company where Group_Code='" + Session["Group_Code"].ToString() + "' order by Company_Code asc";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdCompany.DataSource = dt;
                        grdCompany.DataBind();
                    }
                }
            }
        }
        catch
        {

        }
    }


    protected void grdCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            Int32 companyCode = 0;
            companyCode = Convert.ToInt32(grdCompany.Rows[rowindex].Cells[0].Text);
            Session["Company_Code"] = companyCode;

            clsGV.Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            clsGV.CompanyName = clsCommon.getString("select Company_Name_E from Company where Company_Code=" + Session["Company_Code"].ToString());
            Session["Company_Name"] = clsCommon.getString("select Company_Name_E from Company where Company_Code=" + Session["Company_Code"].ToString());
            clsGV.CompanyAddress = clsCommon.getString("select Address_E from Company where Company_Code=" + Session["Company_Code"].ToString());
            clsGV.CompanyPhone = clsCommon.getString("select Mobile_No from Company where Company_Code=" + Session["Company_Code"].ToString());

            this.fillDropdownYear();

            modalPoppLogin.Show();
            txtLoginName.Text = string.Empty;
            txtLoginName.Focus();
            lblLoginFailedMsg.Visible = false;
            txtPassword.Attributes.Add("onmousedown", "return noCopyMouse(event);");
            txtPassword.Attributes.Add("onkeydown", "return noCopyKey(event);");
        }
        catch
        {

        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string qry = string.Empty;
            string companyCode = Convert.ToString(Session["Company_Code"].ToString());
            //msg = AuthenticateUser(txtLoginName.Text, txtPassword.Text, msg);
            //if (msg == "1")
            //{
            string MyLogin = txtPassword.Text;
            qry = "select Password from tblUser where User_Name COLLATE Latin1_general_CS_AS='" + txtLoginName.Text + "'  and User_Type!='C'";
            string Password = clsCommon.getString(qry);
            if (MyLogin == Password)
            {
                qry = "select * from tblUser where User_Name COLLATE Latin1_general_CS_AS='" + txtLoginName.Text + "' and Password COLLATE Latin1_general_CS_AS= '" + txtPassword.Text + "' and User_Type!='C'";
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            Session["user"] = txtLoginName.Text;
                            Session["User_Id"] = dt.Rows[0]["User_Id"].ToString();
                            Session["year"] = drpYear.SelectedValue;
                            Session["Branch_Code"] = drpBranch.SelectedValue;
                            Session["User_Type"] = dt.Rows[0]["User_Type"].ToString();

                            Session["printinsalebill"] = clsCommon.getString("select printinsalebill from accountingyear where Company_Code=" + Session["Company_Code"].ToString() + " and yearCode=" + Session["year"].ToString());
                            Session["selectedyear"] = clsCommon.getString("select year from accountingyear where Company_Code=" + Session["Company_Code"].ToString() + " and yearCode=" + Session["year"].ToString());


                            #region for email sending
                            Session["EmailId"] = dt.Rows[0]["EmailId"].ToString();
                            Session["EmailPassword"] = dt.Rows[0]["EmailPassword"].ToString();
                            Session["smtpServerPort"] = dt.Rows[0]["smtpServerPort"].ToString();

                            clsGV.EmailId = dt.Rows[0]["EmailId"].ToString();
                            clsGV.EmailPassword = dt.Rows[0]["EmailPassword"].ToString();
                            clsGV.smtpServerPort = dt.Rows[0]["smtpServerPort"].ToString();
                            #endregion

                            qry = "select Convert(varchar(10),Start_Date,103 ) as Start_Date,Convert(varchar(10),End_Date,103) as End_Date from AccountingYear where yearCode=" + Session["year"].ToString();
                            ds = clsDAL.SimpleQuery(qry);
                            dt = ds.Tables[0];
                            string sd = dt.Rows[0]["Start_Date"].ToString();
                            string ed = dt.Rows[0]["End_Date"].ToString();
                            Session["Start_Date"] = sd;
                            Session["End_Date"] = ed;

                            Session["accountingYear"] = sd + "-" + ed;

                            #region global variable declaration
                            clsGV.Start_Date = Session["Start_Date"].ToString();
                            clsGV.End_Date = Session["End_Date"].ToString();

                            DateTime dStart = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                            clsGV.Start_Date = dStart.ToString("dd/MM/yyyy");
                            DateTime dEnd = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                            string todayDate = clsCommon.getString("select Convert(varchar(10),getdate(),103)as todayDt");
                            DateTime dToday = DateTime.Parse(todayDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                            if (dToday < dEnd)
                            {
                                clsGV.To_date = dToday.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                clsGV.To_date = dEnd.ToString("dd/MM/yyyy");
                            }

                            clsGV.Year_Code = Convert.ToInt32(Session["year"].ToString());
                            clsGV.Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
                            Session["Company_Code"] = companyCode;
                            clsGV.user = Session["user"].ToString();
                            clsGV.User_Id = Convert.ToInt32(Session["User_Id"].ToString());
                            string computerName = System.Net.Dns.GetHostName();
                            string VisitorsIPAddr = string.Empty;
                            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                            {
                                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                            }
                            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                            {
                                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
                            }
                            clsGV.userInfo = clsGV.User_Id.ToString() + "-" + clsGV.user + "-" + computerName + "-" + VisitorsIPAddr + "-";
                            //userInfo-> clsGV.userInfo
                            ViewState["tblprefix"] = Session["tblPrefix"].ToString();
                            string tblPrefix = ViewState["tblprefix"].ToString(); // Session["tblPrefix"].ToString(); //"NT_1_"; 
                            qry = "select * from " + tblPrefix + "CompanyParameters where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString();
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        Session["BROKRAGE_AC"] = dt.Rows[0]["BROKRAGE_AC"].ToString();
                                        Session["SERVICE_CHARGE_AC"] = dt.Rows[0]["SERVICE_CHARGE_AC"].ToString();
                                        Session["COMMISSION_AC"] = dt.Rows[0]["COMMISSION_AC"].ToString();
                                        Session["QUALITY_DIFF_AC"] = dt.Rows[0]["QUALITY_DIFF_AC"].ToString();
                                        Session["BANK_COMMISSION_AC"] = dt.Rows[0]["BANK_COMMISSION_AC"].ToString();
                                        Session["INTEREST_AC"] = dt.Rows[0]["INTEREST_AC"].ToString();
                                        Session["TRANSPORT_AC"] = dt.Rows[0]["TRANSPORT_AC"].ToString();
                                        Session["SALE_DALALI_AC"] = dt.Rows[0]["SALE_DALALI_AC"].ToString();
                                        Session["LOADING_CHARGE_AC"] = dt.Rows[0]["LOADING_CHARGE_AC"].ToString();
                                        Session["MOTOR_FREIGHT_AC"] = dt.Rows[0]["MOTOR_FREIGHT_AC"].ToString();
                                        Session["POSTAGE_AC"] = dt.Rows[0]["POSTAGE_AC"].ToString();
                                        Session["OTHER_AMOUNT_AC"] = dt.Rows[0]["OTHER_AMOUNT_AC"].ToString();
                                        Session["SELF_AC"] = dt.Rows[0]["SELF_AC"].ToString();
                                        Session["AUTO_VOUCHER"] = dt.Rows[0]["AutoVoucher"].ToString();
                                        Session["EXCISE_RATE"] = dt.Rows[0]["EXCISE_RATE"].ToString();
                                        string companygstStateCode = dt.Rows[0]["GSTStateCode"].ToString();
                                        Session["CompanyGSTStateCode"] = companygstStateCode;
                                        Session["Company_GST"] = companygstStateCode;
                                        string statename = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + companygstStateCode + "");
                                        Session["CompanyState"] = statename;
                                        Session["SaleCGSTAc"] = dt.Rows[0]["CGSTAc"].ToString();
                                        Session["SaleSGSTAc"] = dt.Rows[0]["SGSTAc"].ToString();
                                        Session["SaleIGSTAc"] = dt.Rows[0]["IGSTAc"].ToString();
                                        Session["PurchaseCGSTAc"] = dt.Rows[0]["PurchaseCGSTAc"].ToString();
                                        Session["PurchaseSGSTAc"] = dt.Rows[0]["PurchaseSGSTAc"].ToString();
                                        Session["PurchaseIGSTAc"] = dt.Rows[0]["PurchaseIGSTAc"].ToString();

                                        Session["TCS_Rate"] = dt.Rows[0]["TCS"].ToString();
                                        Session["PurchaseTCSAc"] = dt.Rows[0]["PurchaseTCSAc"].ToString();
                                        Session["SaleTCSAc"] = dt.Rows[0]["SaleTCSAc"].ToString();

                                        Session["SaleTDS_Rate"] = dt.Rows[0]["SaleTDSRate"].ToString();
                                        Session["PurchaseTDS_Rate"] = dt.Rows[0]["PurchaseTDSRate"].ToString();
                                        Session["SaleTDSAc"] = dt.Rows[0]["SaleTDSAc"].ToString();
                                        Session["PurchaseTDSAc"] = dt.Rows[0]["PurchaseTDSAc"].ToString();
                                        Session["TCSLimit"] = dt.Rows[0]["TCSLimit"].ToString(); 
                                    }
                                }
                            }
                            #endregion
                            modalPoppLogin.Hide();
                            txtLoginName.Text = "";
                            txtPassword.Text = "";
                            Response.Redirect("~/Sugar/pgeHome.aspx", false);
                        }
                        else
                        {
                            //Session["Company_Code"] = null;
                            //Session["user"] = null;
                            //Session["year"] = null;
                            modalPoppLogin.Show();
                            txtLoginName.Focus();
                            lblLoginFailedMsg.Visible = true;
                        }
                    }
                    else
                    {
                        //Session["Company_Code"] = null;
                        //Session["user"] = null;
                        //Session["year"] = null;
                        modalPoppLogin.Show();
                        txtLoginName.Focus();
                        lblLoginFailedMsg.Visible = true;
                    }
                }
                else
                {
                    //Session["Company_Code"] = null;
                    //Session["user"] = null;
                    //Session["year"] = null;
                    modalPoppLogin.Show();
                    txtLoginName.Focus();
                    lblLoginFailedMsg.Visible = true;
                }
                //}
                //else
                //{
                //    lblLoginFailedMsg.Text = msg;
                //    modalPoppLogin.Show();
                //    txtLoginName.Focus();
                //    lblLoginFailedMsg.Visible = true;
                //}
            }
            else
            {
                //Session["Company_Code"] = null;
                //Session["user"] = null;
                //Session["year"] = null;
                modalPoppLogin.Show();
                txtLoginName.Focus();
                lblLoginFailedMsg.Visible = true;
            }
        }
        catch
        {
            Response.Redirect("~/Sugar/pgeHome.aspx", false);
        }
    }

    public static string AuthenticateUser(string Username, string Password, string msg)
    {
        msg = clsDAL.AuthenticateUser("spAuthenticateUser", Username, Password, msg);
        return msg;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        modalPoppLogin.Hide();
    }

    protected void lnkCreateCompany_Click(object sender, EventArgs e)
    {
        Session["Company_Code"] = null;
        if (Session["User_Type"] != null)
        {
            string uType = Session["User_Type"].ToString();
            if (uType == "A")
            {
                Response.Redirect("pgeCreateNewCompany.aspx");
            }
            else
            {
                modalPoppLogin.Show();
            }
        }
        else
        {
            modalPoppLogin.Show();
        }
    }
    protected void lnkNewUser_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Company_Code"] = null;
        }
        catch
        {

        }
    }

    protected void lnkCreateBranch_Click(object sender, EventArgs e)
    {
        Session["Company_Code"] = null;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = false;

            string qry = "";
            qry = "";
            string User_Id = "0";

            #region validation
            if (txtOldPassword.Text != string.Empty)
            {
                string str = clsCommon.getString("select User_Id from tblUser where User_Name='" + txtCUserName.Text + "' and Password='" + txtOldPassword.Text + "'");
                if (str != string.Empty)
                {
                    User_Id = str;
                    isValidated = true;
                    lblerrChangePassword.Visible = false;
                }
                else
                {
                    isValidated = false;
                    txtOldPassword.Text = string.Empty;
                    txtOldPassword.Focus();
                    lblerrChangePassword.Visible = true;
                    return;
                }
            }
            else
            {
                lblerrChangePassword.Visible = false;
                isValidated = false;
                txtOldPassword.Focus();
                return;
            }
            if (txtNewPassword.Text != string.Empty)
            {
                if (txtnewCPassword.Text == txtnewCPassword.Text)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    txtnewCPassword.Text = string.Empty;
                    txtnewCPassword.Focus();
                    return;
                }
            }
            else
            {
                isValidated = false;
                txtNewPassword.Focus();
                return;
            }

            #endregion

            #region update password
            if (isValidated == true)
            {
                try
                {
                    qry = "";
                    DataSet ds = new DataSet();
                    qry = "update tblUser set Password='" + txtNewPassword.Text + "' where User_Id='" + User_Id + "'";
                    ds = clsDAL.SimpleQuery(qry);

                    txtCUserName.Text = string.Empty;
                    txtOldPassword.Text = string.Empty;
                    txtnewCPassword.Text = string.Empty;
                    txtNewPassword.Text = string.Empty;
                    popupChangePassword.Hide();
                    modalPoppLogin.Show();
                    //document.getElementById("iframe_id").contentWindow.location.href
                }
                catch
                {

                }
            }
            #endregion
        }
        catch
        {

        }
    }

    protected void btnCancelNewPass_Click(object sender, EventArgs e)
    {
        try
        {
            txtCUserName.Text = string.Empty;
            txtOldPassword.Text = string.Empty;
            txtnewCPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            popupChangePassword.Hide();
            modalPoppLogin.Show();
        }
        catch
        {

        }
    }
    protected void lnkChangePass_Click(object sender, EventArgs e)
    {
        try
        {
            modalPoppLogin.Hide();
            popupChangePassword.Show();
            txtCUserName.Focus();
        }
        catch
        {

        }
    }
    protected void lnkForgotPass_Click(object sender, EventArgs e)
    {
        mpForgetPass.Show();
    }
    protected void btnSendForgetedPass_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserNameForgot.Text != string.Empty)
            {
                string username = txtUserNameForgot.Text.Trim();
                string UserPassword = clsCommon.getString("Select Password from tblUser where User_Name='" + username + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (UserPassword == string.Empty)
                {
                    lblWrongUserName.Text = "User Name is InCorrect!";
                    mpForgetPass.Show();
                }
                else
                {
                    string mobile = clsCommon.getString("Select Mobile from tblUser where User_Name='" + username + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (mobile == string.Empty)
                    {
                        lblWrongUserName.Text = "Mobile Number is not Present Please Contact Admin!";
                        mpForgetPass.Show();
                    }
                    else
                    {
                        string API = clsGV.msgAPI;
                        string Url = API + "mobile=" + mobile + "&message=" + "Hello " + username + ",Your Password is : " + UserPassword + " Please Dont Forget again..";
                        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                        HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                        StreamReader reader = new StreamReader(myResp.GetResponseStream());
                        string respString = reader.ReadToEnd();
                        reader.Close();
                        myResp.Close();
                        modalPoppLogin.Show();
                    }
                }

            }
            else
            {
                lblWrongUserName.Text = "Please Enter User Name!";
                mpForgetPass.Show();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btncan_Click(object sender, EventArgs e)
    {
        modalPoppLogin.Show();
    }
}