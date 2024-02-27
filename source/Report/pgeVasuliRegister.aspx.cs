using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;

public partial class Report_pgeVasuliRegister : System.Web.UI.Page
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
                txtFromDt.Text = clsGV.Start_Date;
                txtToDt.Text = clsGV.End_Date;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnShowVasuli_Click(object sender, EventArgs e)
    {
        try
        {
            fromDT = txtFromDt.Text;
            toDT = txtToDt.Text;
            if (fromDT != string.Empty)
            {
                fromDT = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            }
            else
            {
                fromDT = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            }
            if (toDT != string.Empty)
            {
                toDT = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            }
            else
            {
                toDT = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            }
            qry = "select CONVERT(varchar(10),Doc_Date,103) as doc_date,Ac_Code,Doc_No,Tran_Type as Type,PartyName,BrokerName as Broker,Quantal as Qntl,from_station as From_Station,Voucher_Amount as Voc_Amt," +
                " ISNULL((Select SUM(t.amount) from " + tblPrefix + "Transact t where t.Tran_Type='BR' and t.Voucher_Type=u.Tran_Type and t.Voucher_No=u.Doc_No and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code) ,0) as recieved," +
                //" CONVERT(varchar(10),(Select t.doc_date from " + tblPrefix + "Transact t where t.Tran_Type='BR' and t.Voucher_Type=u.Tran_Type and t.Voucher_No=u.Doc_No),103) as transdate," +
                " (Voucher_Amount - ISNULL((Select SUM(t.amount) from " + tblPrefix + "Transact t where t.Tran_Type='BR' and t.Voucher_Type=u.Tran_Type and t.Voucher_No=u.Doc_No and t.Company_Code=u.Company_Code and t.Year_Code=u.Year_Code),0)) as short ," +
                " ISNULL(DueDays,0) as DueDays from " + tblPrefix + "qryUnionVoucherSale u where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and u.Doc_Date between '" + fromDT + "' and '" + toDT + "' order by u.Doc_Date";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("doc_date", typeof(string)));
                dt.Columns.Add(new DataColumn("Ac_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Doc_No", typeof(string)));
                dt.Columns.Add(new DataColumn("Type", typeof(string)));
                dt.Columns.Add(new DataColumn("PartyName", typeof(string)));
                dt.Columns.Add(new DataColumn("Broker", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("From_Station", typeof(string)));
                dt.Columns.Add(new DataColumn("Voc_Amt", typeof(double)));
                dt.Columns.Add(new DataColumn("DueDays", typeof(Int32)));
                dt.Columns.Add(new DataColumn("recieved", typeof(double)));
                dt.Columns.Add(new DataColumn("transdate", typeof(string)));
                dt.Columns.Add(new DataColumn("short", typeof(double)));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["doc_date"] = ds.Tables[0].Rows[i]["doc_date"].ToString();
                        dr["Ac_Code"] = ds.Tables[0].Rows[i]["Ac_Code"].ToString();
                        dr["Doc_No"] = ds.Tables[0].Rows[i]["Doc_No"].ToString();
                        dr["Type"] = ds.Tables[0].Rows[i]["Type"].ToString();
                        dr["PartyName"] = ds.Tables[0].Rows[i]["PartyName"].ToString();
                        dr["Broker"] = ds.Tables[0].Rows[i]["Broker"].ToString();
                        dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                        dr["From_Station"] = ds.Tables[0].Rows[i]["From_Station"].ToString();
                        dr["Voc_Amt"] = ds.Tables[0].Rows[i]["Voc_Amt"].ToString();
                        dr["DueDays"] = ds.Tables[0].Rows[i]["DueDays"].ToString();
                        dr["recieved"] = ds.Tables[0].Rows[i]["recieved"].ToString();
                        //dr["transdate"] = ds.Tables[0].Rows[i]["transdate"].ToString();
                        dr["short"] = ds.Tables[0].Rows[i]["short"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        lblVoucherAmount.Text = Convert.ToString(dt.Compute("SUM(Voc_Amt)", string.Empty));
                        lblRecieved.Text = Convert.ToString(dt.Compute("SUM(recieved)", string.Empty));
                        lblShort.Text = Convert.ToString(dt.Compute("SUM(short)", string.Empty));
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                    }
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("70px");
            e.Row.Cells[1].Width = new Unit("70px");
            e.Row.Cells[3].Width = new Unit("400px");
            e.Row.Cells[4].Width = new Unit("400px");
            e.Row.Cells[5].Width = new Unit("140px");
            e.Row.Cells[6].Width = new Unit("180px");
            e.Row.Cells[7].Width = new Unit("180px");
            e.Row.Cells[8].Width = new Unit("70px");
            e.Row.Cells[9].Width = new Unit("200px");
            e.Row.Cells[10].Width = new Unit("80px");
            e.Row.Cells[11].Width = new Unit("200px");
            //e.Row.Cells[12].Width = new Unit("200px");

            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;

            int i = 0;
            foreach (TableCell cell in e.Row.Cells)
            {
                i++;
                string s = cell.Text;
                if (cell.Text.Length > 24)
                {
                    cell.Text = cell.Text.Substring(0, 24) + "...";
                    cell.ToolTip = s;
                }
            }
        }
    }
    protected void btnEnter_Click(object sender, EventArgs e)
    {
    }
    protected void btnUpdateAll_Click(object sender, EventArgs e)
    {
        if (grdDetail.Rows.Count > 0)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                string type = grdDetail.Rows[i].Cells[2].Text.ToString();
                TextBox txtDueDays = grdDetail.Rows[i].Cells[9].FindControl("txtDueDays") as TextBox;
                int due_days = txtDueDays.Text != string.Empty ? Convert.ToInt32(txtDueDays.Text) : 0;
                if (txtDueDays.Text != string.Empty)
                {
                    if (type == "SB")
                    {
                        string doc_no = grdDetail.Rows[i].Cells[1].Text.ToString();
                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                        {
                            string b = "";
                            obj.flag = 2;
                            obj.tableName = tblPrefix + "SugarSale";
                            obj.columnNm = "Due_Days='" + due_days + "' where doc_no=" + doc_no + " and Tran_Type='" + type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                            obj.values = "none";
                            DataSet ds = obj.insertAccountMaster(ref b);
                        }
                    }
                    else
                    {
                        string doc_no = grdDetail.Rows[i].Cells[1].Text.ToString();
                        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                        {
                            string b = "";
                            obj.flag = 2;
                            obj.tableName = tblPrefix + "Voucher";
                            obj.columnNm = "Due_Days='" + due_days + "' where doc_no=" + doc_no + " and Tran_Type='" + type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                            obj.values = "none";
                            DataSet ds = obj.insertAccountMaster(ref b);
                        }
                    }
                }
            }
            grdDetail.DataSource = null;
            grdDetail.EmptyDataText = "Recods Updated Successfully!";
            grdDetail.DataBind();
            setFocusControl(txtToDt);
            lblRecieved.Text = "";
            lblShort.Text = "";
            lblVoucherAmount.Text = "";
        }
    }
    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion
}