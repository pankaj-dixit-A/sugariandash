using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptTransportWiseDispatch : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string Tender_No = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string f = "";
    string f_Main = "../Report/";
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindData();
        }
    }
    private void BindData()
    {
        try
        {
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select distinct(transport) as TransportCode,TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderList where desp_type='DI' and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and  doc_date between '" + fromDT + "' and '" + toDT + "' order by TransportName asc";
            }
            else
            {
                qry = "select distinct(transport) as TransportCode,TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderList where desp_type='DI' and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "' and Branch_Code='" + Branch_Code + "' order by TransportName asc";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
                else
                {
                    dtl.DataSource = null;
                    dtl.DataBind();
                }
            }
            else
            {
                dtl.DataSource = null;
                dtl.DataBind();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtl_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblMillCode = (Label)e.Item.FindControl("lblMillCode");
            Label lblQntlTotal = (Label)e.Item.FindControl("lblQntlTotal");
            string transport = lblMillCode.Text;
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "select d.doc_no as #,CONVERT(varchar(5),d.doc_date,103) as dodate,d.millShortName as millShortName,a.Short_Name as VocBy,d.GetPassName as GetPass" +
                    ",d.mill_rate as MR,d.quantal as Qntl,d.sale_rate as SR,d.truck_no as lorry,d.Freight_Amount as frt,d.vasuli_amount AS vasuli,d.TransportName as transport," +
                    " d.DOName as do,d.purc_no as tender from " + tblPrefix + "qryDeliveryOrderList d left outer join " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code" +
                    " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.transport='" + transport + "' and d.desp_type='DI' and d.tran_type='DO'";
            }
            else
            {
                qry = "select d.doc_no as #,CONVERT(varchar(5),d.doc_date,103) as dodate,d.millShortName as millShortName,a.Short_Name as VocBy,d.GetPassName as GetPass" +
                    ",d.mill_rate as MR,d.quantal as Qntl,d.sale_rate as SR,d.truck_no as lorry,d.Freight_Amount as frt,d.vasuli_amount AS vasuli,d.TransportName as transport," +
                    " d.DOName as do,d.purc_no as tender from " + tblPrefix + "qryDeliveryOrderList d left outer join " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code" +
                    " where d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDT + "' and '" + toDT + "' and d.transport='" + transport + "' and d.Branch_Code='" + Branch_Code + "' and d.desp_type='DI' and d.tran_type='DO'";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("dodate", typeof(string)));
                dt.Columns.Add(new DataColumn("millShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("VocBy", typeof(string)));
                dt.Columns.Add(new DataColumn("GetPass", typeof(string)));
                dt.Columns.Add(new DataColumn("MR", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("SR", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("frt", typeof(string)));
                dt.Columns.Add(new DataColumn("vasuli", typeof(string)));
                dt.Columns.Add(new DataColumn("transport", typeof(string)));
                dt.Columns.Add(new DataColumn("do", typeof(string)));
                dt.Columns.Add(new DataColumn("tender", typeof(string)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["dodate"] = ds.Tables[0].Rows[i]["dodate"].ToString();
                    dr["millShortName"] = ds.Tables[0].Rows[i]["millShortName"].ToString();
                    dr["VocBy"] = ds.Tables[0].Rows[i]["VocBy"].ToString();
                    dr["GetPass"] = ds.Tables[0].Rows[i]["GetPass"].ToString();
                    dr["MR"] = ds.Tables[0].Rows[i]["MR"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["SR"] = ds.Tables[0].Rows[i]["SR"].ToString();
                    dr["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                    dr["frt"] = ds.Tables[0].Rows[i]["frt"].ToString();
                    dr["vasuli"] = ds.Tables[0].Rows[i]["vasuli"].ToString();
                    dr["transport"] = ds.Tables[0].Rows[i]["transport"].ToString();
                    dr["do"] = ds.Tables[0].Rows[i]["do"].ToString();
                    dr["tender"] = ds.Tables[0].Rows[i]["tender"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblfromtodate.Text = "Transport Wise Dispatch Register From  <b>" + fromDT + "</b>  To  <b>" + toDT + "</b>";
                    lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
                else
                {
                    dtlDetails.DataSource = null;
                    dtlDetails.DataBind();
                }
            }
            else
            {
                dtlDetails.DataSource = null;
                dtlDetails.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        pnlMain.RenderControl(tw);
        string sim = sw.ToString();
        StrExport.Append(sim);
        StrExport.Append("</div></body></html>");
        string strFile = "report.xls";
        string strcontentType = "application/excel";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.BufferOutput = true;
        Response.ContentType = strcontentType;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        Response.Write(StrExport.ToString());
        Response.Flush();
        Response.Close();
        Response.End();
    }
}