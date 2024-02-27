using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptTransportAdvanceOnlyBalNew : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    string uptodt = string.Empty;
    string TransPort = string.Empty;
    string from = string.Empty;
    string to = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        from = Request.QueryString["Fromdt"];
        to = Request.QueryString["Todt"];
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
            string fromDt = DateTime.Parse(from, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            string toDt = DateTime.Parse(to, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string txt = "All";
            if (TransPort == string.Empty)
            {
                if (Branch_Code == string.Empty)
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "'  order by TransportName asc ";
                }
                else
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' order by TransportName asc";
                }
            }
            else
            {
                txt = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + TransPort + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (Branch_Code == string.Empty)
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.transport=" + TransPort + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' order by TransportName asc ";
                }
                else
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.transport=" + TransPort + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' order by TransportName asc";
                }
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                ds.Tables[0].Columns.Add("hasBal", typeof(string));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string TransportCode = ds.Tables[0].Rows[i]["TransportCode"].ToString();
                    string qry3 = "select do.doc_no as #,do.tran_type as ttype,' ' as Suffix,Convert(varchar(10),do.doc_date,103) as dt,do.millShortName as Mill,do.GetPassName as getpassname,c.city_name_e as GetpassCity,do.truck_no as lorry,do.TransportName,do.quantal as Qntl," +
                        " (do.Memo_Advance) as Advance,(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no" +
                        " and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code) as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA " +
                        " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance from " + tblPrefix + "qryDeliveryOrderListReport do left outer join " + tblPrefix + "CityMaster as c on c.city_code=do.getpasscityCode and c.company_code=do.company_code where do.tran_type='DO' and do.transport=" + TransportCode + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    DataSet ds3 = new DataSet();
                    ds3 = clsDAL.SimpleQuery(qry3);
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt3 = new DataTable();
                        dt3 = ds3.Tables[0];

                        double bal = Convert.ToDouble(dt3.Compute("SUM(Balance)", string.Empty));
                        if (bal == 0)
                        {
                            ds.Tables[0].Rows[i]["hasBal"] = "1";
                        }
                    }
                }
                dt = ds.Tables[0];
                DataTable dtset = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    string hasBal = row["hasBal"].ToString();
                    if (hasBal != "1")
                    {
                        dtset.Rows.Add(row.ItemArray);
                    }
                }

                if (dtset.Rows.Count > 0)
                {

                    lblCmpName.Text = Session["Company_Name"].ToString();
                    lblTransportName.Text = "Transport Advance Report of " + "<b>" + txt + "</b>";
                    dtlist.DataSource = dtset;
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
            Label lblAllQntlTotal = (Label)e.Item.FindControl("lblAllQntlTotal");
            Label lblAllAdvanceTotal = (Label)e.Item.FindControl("lblAllAdvanceTotal");
            Label lblAllPaidTotal = (Label)e.Item.FindControl("lblAllPaidTotal");
            Label lblAllBalTotal = (Label)e.Item.FindControl("lblAllBalTotal");
            string transport = lblTransportCode.Text;
            if (Branch_Code == string.Empty)
            {
                qry = "select do.doc_no as #,do.tran_type as ttype,' ' as Suffix,Convert(varchar(10),do.doc_date,103) as dt,do.millShortName as Mill,do.GetPassName as getpassname,c.city_name_e as GetpassCity,do.truck_no as lorry,do.TransportName,do.quantal as Qntl," +
                      " (do.Memo_Advance) as Advance,(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no" +
                      " and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code) as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA " +
                      " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance from " + tblPrefix + "qryDeliveryOrderListReport do left outer join " + tblPrefix + "CityMaster as c on c.city_code=do.getpasscityCode and c.company_code=do.company_code where do.tran_type='DO' and do.transport=" + transport + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            else
            {
                qry = "select do.doc_no as #,do.tran_type as ttype,' ' as Suffix,Convert(varchar(10),do.doc_date,103) as dt,do.millShortName as Mill,do.GetPassName as getpassname,c.city_name_e as GetpassCity,do.truck_no as lorry,do.TransportName,do.quantal as Qntl," +
                      " (do.Memo_Advance) as Advance,(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no" +
                      " and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code) as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA " +
                      " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance from " + tblPrefix + "qryDeliveryOrderListReport do left outer join " + tblPrefix + "CityMaster as c on c.city_code=do.getpasscityCode and c.company_code=do.company_code where do.tran_type='DO' and do.transport=" + transport + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Branch_Code=" + Branch_Code + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

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
                dt.Columns.Add(new DataColumn("getpassname", typeof(string)));
                dt.Columns.Add(new DataColumn("DispatchTo", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
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
                    string getpassname = ds.Tables[0].Rows[i]["getpassname"].ToString();
                    string getpasscity = ds.Tables[0].Rows[i]["GetpassCity"].ToString();
                    dr["DispatchTo"] = getpassname + " " + getpasscity;
                    dr["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    double advance = Convert.ToDouble(ds.Tables[0].Rows[i]["Advance"].ToString());
                    dr["Advance"] = advance;
                    double paid = Convert.ToDouble(ds.Tables[0].Rows[i]["Paid"].ToString());
                    dr["Paid"] = paid;
                    double balance = advance - paid;
                    dr["Balance"] = balance;
                    if (balance != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    lblAllQntlTotal.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                    lblAllAdvanceTotal.Text = Convert.ToString(dt.Compute("SUM(Advance)", string.Empty));
                    lblAllPaidTotal.Text = Convert.ToString(dt.Compute("SUM(Paid)", string.Empty));
                    lblAllBalTotal.Text = Convert.ToString(dt.Compute("SUM(Balance)", string.Empty));
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