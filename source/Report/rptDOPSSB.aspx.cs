using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptDOPSSB : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string Lot_No = string.Empty;
    string Sr_No = string.Empty;
    string qry = string.Empty;
    string Tender_No = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string f = "../GSReports/DispatchDetails_.htm";
    string f_Main = "../Report/rptDispatchDetails";
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        if (!Page.IsPostBack)
        {
            this.BindData();
        }
    }

    private void BindData()
    {
        try
        {
            string self_ac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            qry = "Select doc_no as DO_No,Convert(varchar(10),doc_date,103) as Date,quantal as DO_Qntl,sale_rate as Sale_Rate from " + tblPrefix + "deliveryorder where GETPASSCODE=" + self_ac + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("DO_No", typeof(string)));
                dt.Columns.Add(new DataColumn("Date", typeof(string)));
                dt.Columns.Add(new DataColumn("DO_Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("Sale_Rate", typeof(double)));
                dt.Columns.Add(new DataColumn("PS_No", typeof(string)));
                dt.Columns.Add(new DataColumn("PS_Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("PS_Rate", typeof(double)));
                dt.Columns.Add(new DataColumn("Bill_No", typeof(string)));
                dt.Columns.Add(new DataColumn("SB_Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("SB_Rate", typeof(double)));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string DO_no =  ds.Tables[0].Rows[i]["DO_No"].ToString();
                    dr["DO_No"] = DO_no;
                    dr["Date"] = ds.Tables[0].Rows[i]["Date"].ToString();
                    dr["DO_Qntl"] = ds.Tables[0].Rows[i]["DO_Qntl"].ToString();
                    dr["Sale_Rate"] = ds.Tables[0].Rows[i]["Sale_Rate"].ToString();

                    string qryPurc = "select doc_no as PS_No,Quantal as PS_Qntl,rate as PS_Rate from " + tblPrefix + "qrySugarPurcList where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and PURCNO=" + DO_no + "";
                    DataSet dsPS = new DataSet();
                    dsPS = clsDAL.SimpleQuery(qryPurc);
                    string psno = "0";
                    if (dsPS.Tables[0].Rows.Count > 0)
                    {
                        psno = dsPS.Tables[0].Rows[0]["PS_No"].ToString();
                        dr["PS_No"] = psno;
                        dr["PS_Qntl"] = dsPS.Tables[0].Rows[0]["PS_Qntl"].ToString();
                        dr["PS_Rate"] = dsPS.Tables[0].Rows[0]["PS_Rate"].ToString();
                    }
                    else
                    {
                        dr["PS_No"] = 0.0;
                        dr["PS_Qntl"] = 0.0;
                        dr["PS_Rate"] = 0.0;
                    }

                    string qryBill = "select doc_no as SB_No,Quantal as SB_Qntl,rate as SB_Rate from " + tblPrefix + "qrySugarSaleList where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and PURCNO=" + psno + "";
                    DataSet dsSB = new DataSet();
                    dsSB = clsDAL.SimpleQuery(qryBill);
                    if (dsSB.Tables[0].Rows.Count > 0)
                    {
                        dr["Bill_No"] = dsSB.Tables[0].Rows[0]["SB_No"].ToString();
                        dr["SB_Qntl"] = dsSB.Tables[0].Rows[0]["SB_Qntl"].ToString();
                        dr["SB_Rate"] = dsSB.Tables[0].Rows[0]["SB_Rate"].ToString();
                    }
                    else
                    {
                        dr["Bill_No"] = 0.0;
                        dr["SB_Qntl"] = 0.0;
                        dr["SB_Rate"] = 0.0;
                    }
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblDOQntlTotal.Text = Convert.ToString(dt.Compute("SUM(DO_Qntl)", string.Empty));
                    lblPSQntlTotal.Text = Convert.ToString(dt.Compute("SUM(PS_Qntl)", string.Empty));
                    lblSBQntlTotal.Text = Convert.ToString(dt.Compute("SUM(SB_Qntl)", string.Empty));
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
}