using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptTransportOnlyBal : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    string uptodt = string.Empty;
    string TransPort = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        TransPort = Request.QueryString["Ac_Code"];
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            string txt = "All";
            if (TransPort == string.Empty)
            {
                if (Branch_Code == string.Empty)
                {
                    qry = "select v.Cash_Account as TransportCode,a.Ac_Name_E as TransportName from " + tblPrefix + "Voucher v Left Outer Join " + tblPrefix + "AccountMaster a" +
                        " on v.Cash_Account=a.Ac_Code and v.Company_Code=a.Company_Code where v.Tran_Type='OV' and v.Cash_Account!=0 and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by TransportName asc ";
                }
                else
                {
                    qry = "select v.Cash_Account as TransportCode,a.Ac_Name_E as TransportName from " + tblPrefix + "Voucher v Left Outer Join " + tblPrefix + "AccountMaster a" +
                        " on v.Cash_Account=a.Ac_Code and v.Company_Code=a.Company_Code where v.Tran_Type='OV' and v.Cash_Account!=0 and v.Branch_Code=" + Branch_Code + " and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by TransportName asc ";
                }
            }
            else
            {
                txt = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + TransPort + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (Branch_Code == string.Empty)
                {
                    qry = "select v.Cash_Account as TransportCode,a.Ac_Name_E as TransportName from " + tblPrefix + "Voucher v Left Outer Join " + tblPrefix + "AccountMaster a" +
                        " on v.Cash_Account=a.Ac_Code and v.Company_Code=a.Company_Code where v.Tran_Type='OV' and v.Cash_Account=" + TransPort + " and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }
                else
                {
                    qry = "select v.Cash_Account as TransportCode,a.Ac_Name_E as TransportName from " + tblPrefix + "Voucher v Left Outer Join " + tblPrefix + "AccountMaster a" +
                        " on v.Cash_Account=a.Ac_Code and v.Company_Code=a.Company_Code where v.Tran_Type='OV' and v.Cash_Account=" + TransPort + " and v.Branch_Code=" + Branch_Code + " and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    lblTransportName.Text = "Transport Advance Report of " + "<b>" + txt + "</b>";
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
                else
                {
                    dtlist.DataSource = null;
                    dtlist.DataBind();
                }
            }
            else
            {
                dtlist.DataSource = null;
                dtlist.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblTransportCode = (Label)e.Item.FindControl("lblTransportCode");
            string transport = lblTransportCode.Text;
            if (Branch_Code == string.Empty)
            {
                qry = "select v.Doc_No as #,v.Tran_Type as ttype,CONVERT(VARCHAR(10),v.Doc_Date,103) as dt,a.Ac_Name_E as Mill,(v.Quantal+ISNULL(v.Quantal1,0)) as Qntl,v.Cash_Ac_Amount as Advance," +
                      " (select SUM(t.amount) from " + tblPrefix + "Transact t where t.credit_ac=v.Cash_Account and t.Company_Code=v.Company_Code and t.Year_Code=v.Year_Code and t.Tran_Type in('BP','CP')) as Paid" +
                      " from " + tblPrefix + "Voucher v left outer join " + tblPrefix + "AccountMaster a on v.Mill_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                      " where v.Tran_Type='OV' and v.Cash_Account!=0 and v.Cash_Account=" + transport + " and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            else
            {
                qry = "select v.Doc_No as #,v.Tran_Type as ttype,CONVERT(VARCHAR(10),v.Doc_Date,103) as dt,a.Ac_Name_E as Mill,(v.Quantal+ISNULL(v.Quantal1,0)) as Qntl,v.Cash_Ac_Amount as Advance," +
                      " (select SUM(t.amount) from " + tblPrefix + "Transact t where t.credit_ac=v.Cash_Account and t.Company_Code=v.Company_Code and t.Year_Code=v.Year_Code and t.Tran_Type in('BP','CP')) as Paid" +
                      " from " + tblPrefix + "Voucher v left outer join " + tblPrefix + "AccountMaster a on v.Mill_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                      " where v.Tran_Type='OV' and v.Cash_Account!=0 and v.Cash_Account=" + transport + " and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and v.Branch_Code=" + Branch_Code + "";
            }

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("ttype", typeof(string)));
                dt.Columns.Add(new DataColumn("dt", typeof(string)));
                dt.Columns.Add(new DataColumn("Mill", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(string)));
                dt.Columns.Add(new DataColumn("Advance", typeof(double)));
                dt.Columns.Add(new DataColumn("Paid", typeof(double)));
                dt.Columns.Add(new DataColumn("Balance", typeof(double)));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["ttype"] = ds.Tables[0].Rows[i]["ttype"].ToString();
                    dr["dt"] = ds.Tables[0].Rows[i]["dt"].ToString();
                    dr["Mill"] = ds.Tables[0].Rows[i]["Mill"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    double advance = Convert.ToDouble(ds.Tables[0].Rows[i]["Advance"].ToString());
                    dr["Advance"] = advance;
                    double paid = Convert.ToDouble(ds.Tables[0].Rows[i]["Paid"].ToString());
                    dr["Paid"] = paid;
                    dr["Balance"] = advance - paid;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}