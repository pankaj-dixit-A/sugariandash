using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using CSASPNETMessageBox;

public partial class Report_pgeBalanceReminder : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    static WebControl objAsp = null;
    string AccountMasterTable = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    DataSet ds;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";

        if (!Page.IsPostBack)
        {
            txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string uptodate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            qry = "select s.salepartyfullname as saleparty,b.Mobile_No as mobile,ISNULL(a.Short_Name,s.millname) as mill,s.balance,s.Sale_Rate,CONVERT(VARCHAR(10),s.Lifting_Date,103) as liftdate from qrysugarBalancestock s" +
                " left outer join " + AccountMasterTable + " a on s.millname=a.Ac_Name_E and s.Company_Code=a.Company_Code left outer join " + AccountMasterTable + " b on s.salepartyfullname=b.Ac_Name_E and s.Company_Code=b.Company_Code" +
                "   where  s.Lifting_Date<='" + uptodate + "' and s.balance !=0 and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                " order by s.Lifting_Date";
            //s.salepartyfullname not in('self') and
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("saleparty", typeof(string)));
                dt.Columns.Add(new DataColumn("message", typeof(string)));
                dt.Columns.Add(new DataColumn("mobile", typeof(string)));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["saleparty"] = ds.Tables[0].Rows[i]["saleparty"].ToString();
                        string mill = ds.Tables[0].Rows[i]["mill"].ToString();
                        double bal = Math.Abs(double.Parse(ds.Tables[0].Rows[i]["balance"].ToString()));
                        double salerate = Math.Abs(double.Parse(ds.Tables[0].Rows[i]["Sale_Rate"].ToString()));
                        string liftdate = ds.Tables[0].Rows[i]["liftdate"].ToString();

                        dr["message"] = mill + " " + bal + "-" + salerate + " " + liftdate + " Pls Lift on time and co-operate";
                        dr["mobile"] = ds.Tables[0].Rows[i]["mobile"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        grdReport.DataSource = dt;
                        grdReport.DataBind();
                    }
                    else
                    {
                        grdReport.DataSource = null;
                        grdReport.DataBind();
                    }
                }
            }
            else
            {
                grdReport.DataSource = null;
                grdReport.DataBind();
            }
        }
        catch
        {

        }
    }
    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("340px");
            e.Row.Cells[1].Width = new Unit("360px");
            e.Row.Cells[2].Width = new Unit("130px");
            e.Row.Cells[3].Width = new Unit("50px");
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            foreach (TableCell cell in e.Row.Cells)
            {
                i++;
                string s = cell.Text;
                if (cell.Text.Length > 59)
                {
                    cell.Text = cell.Text.Substring(0, 59) + "...";
                    cell.ToolTip = s;
                }
            }
        }
    }
    protected void btnEnter_Click(object sender, EventArgs e)
    {

    }
    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdReport.Rows.Count > 0)
            {
                for (int i = 0; i < grdReport.Rows.Count; i++)
                {
                    CheckBox chk = grdReport.Rows[i].Cells[3].FindControl("grdCB") as CheckBox;
                    if (chk.Checked == true)
                    {
                        string msg = grdReport.Rows[i].Cells[1].ToolTip.ToString();
                        TextBox txtMobile = grdReport.Rows[i].Cells[2].FindControl("txtMobile") as TextBox;
                        string mobile = txtMobile.Text;

                        if (!string.IsNullOrEmpty(mobile))
                        {
                            string API = clsGV.msgAPI;
                            string Url = API + "mobile=" + mobile + "&message=" + msg;
                            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                            HttpWebResponse myRespo = (HttpWebResponse)myReq.GetResponse();
                            StreamReader strReader = new StreamReader(myRespo.GetResponseStream());
                            string respString = strReader.ReadToEnd();
                            strReader.Close();
                            myRespo.Close();
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}