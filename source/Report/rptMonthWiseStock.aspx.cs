using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;

public partial class Report_rptMonthWiseStock : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string itemCode = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"];
        toDT = Request.QueryString["toDT"];
        lblCompanyName.Text = Session["Company_Name"].ToString();
        this.BindList();
    }
    private void BindList()
    {
        try
        {
            qry = "Select System_Code as item_code,System_Name_E as item_name from " + tblPrefix + "SystemMaster where System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by System_Code";
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
            fromDT = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            toDT = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            Label lblItemCode = (Label)e.Item.FindControl("lblItemCode");
            string itemCode = lblItemCode.Text;
            //Label lbltrandate = (Label)e.Item.FindControl("lbltrandate");
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            //string trandate = DateTime.Parse(lbltrandate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");

            qry = "Select Tran_Type,SUM(case Tran_Type when 'PS' then amount when 'SB' then -amount end) as OpVal from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date< '" + fromDT + "' and item_code=" + itemCode + " Group By Tran_Type";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            double OpVal = 0.00;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    double psamt = 0.00;
                    double sbamt = 0.00;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string tran_type = ds.Tables[0].Rows[i]["Tran_Type"].ToString();
                        if (tran_type == "PS")
                        {
                            psamt = Convert.ToDouble(ds.Tables[0].Rows[i]["OpVal"].ToString());
                        }
                        else
                        {
                            sbamt = Convert.ToDouble(ds.Tables[0].Rows[i]["OpVal"].ToString());
                        }
                    }
                    OpVal = psamt - sbamt;
                }
            }

            qry = "Select Tran_Type,SUM(case Tran_Type when 'PS' then qntl when 'SB' then -qntl end) as OpQntl from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date< '" + fromDT + "' and item_code=" + itemCode + " Group By Tran_Type";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            double OpQntl = 0.00;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    double psqntl = 0.00;
                    double sbqntl = 0.00;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string tran_type = ds.Tables[0].Rows[i]["Tran_Type"].ToString();
                        if (tran_type == "PS")
                        {
                            psqntl = Convert.ToDouble(ds.Tables[0].Rows[i]["OpQntl"].ToString());
                        }
                        else
                        {
                            sbqntl = Convert.ToDouble(ds.Tables[0].Rows[i]["OpQntl"].ToString());
                        }
                    }
                    OpQntl = psqntl - sbqntl;
                }
            }
            double ClosingQntl = 0.0;
            double ClosingValue = 0.0;

            double NetInwardQntl = 0.0;
            double NetInwardValue = 0.0;
            double NetOutwardQntl = 0.0;
            double NetOutwardValue = 0.0;

            qry = "";
            qry = "select YEAR(doc_date) As Year,MONTH(doc_date) AS Month, isnull(SUM(case Tran_Type when 'PS' then qntl end),0) as purcQntl,isnull(SUM(case Tran_Type when 'PS' then amount end),0) as purcVal,SUM(case Tran_Type when 'SB' then qntl end) as saleQntl,SUM(case Tran_Type when 'SB' then amount end)as saleVal from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "' and item_code=" + itemCode + " group by YEAR(doc_date),MONTH(doc_date) order by YEAR(doc_date),MONTH(doc_date)";
            DataSet dst = new DataSet();
            dst = clsDAL.SimpleQuery(qry);
            if (dst.Tables[0].Rows.Count > 0)
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add(new DataColumn("Year", typeof(string)));
                dtt.Columns.Add(new DataColumn("Month", typeof(string)));
                dtt.Columns.Add(new DataColumn("Tran_Type", typeof(string)));
                dtt.Columns.Add(new DataColumn("#", typeof(string)));
                dtt.Columns.Add(new DataColumn("Supplier", typeof(string)));
                dtt.Columns.Add(new DataColumn("OpeningQty", typeof(double)));
                dtt.Columns.Add(new DataColumn("OpeningVal", typeof(double)));
                dtt.Columns.Add(new DataColumn("PurcQty", typeof(double)));
                dtt.Columns.Add(new DataColumn("PurcVal", typeof(double)));
                dtt.Columns.Add(new DataColumn("SaleQty", typeof(double)));
                dtt.Columns.Add(new DataColumn("SaleVal", typeof(double)));
                dtt.Columns.Add(new DataColumn("BalQty", typeof(double)));
                dtt.Columns.Add(new DataColumn("BalValue", typeof(double)));

                for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["Year"] = dst.Tables[0].Rows[i]["Year"].ToString();
                    dr["Month"] = dst.Tables[0].Rows[i]["Month"].ToString();
                    dr["OpeningQty"] = OpQntl;
                    dr["OpeningVal"] = OpVal;
                    double purcQntl = Convert.ToDouble(dst.Tables[0].Rows[i]["purcQntl"].ToString());
                    double purcVal = Convert.ToDouble(dst.Tables[0].Rows[i]["purcVal"].ToString());
                    double saleQntl = Convert.ToDouble(dst.Tables[0].Rows[i]["saleQntl"].ToString());
                    double saleVal = Convert.ToDouble(dst.Tables[0].Rows[i]["saleVal"].ToString());



                    dr["PurcQty"] = purcQntl;
                    dr["PurcVal"] = purcVal;

                    NetInwardQntl += purcQntl;
                    NetInwardValue += purcVal;


                    dr["SaleQty"] = saleQntl;
                    dr["SaleVal"] = saleVal;
                    NetOutwardQntl += saleQntl;
                    NetOutwardValue += saleVal;

                    ClosingQntl = ((OpQntl + purcQntl) - saleQntl);
                    ClosingValue = ((OpVal + purcVal) - saleVal);
                    dr["BalQty"] = ClosingQntl;
                    dr["BalValue"] = ClosingValue;

                    OpQntl = ClosingQntl;
                    OpVal = ClosingValue;
                    dtt.Rows.Add(dr);
                }

                if (dtt.Rows.Count > 0)
                {
                    Label lblNetInwardQntl = (Label)e.Item.FindControl("lblNetInwardQntl");
                    Label lblNetInwardValue = (Label)e.Item.FindControl("lblNetInwardValue");
                    Label lblNetOutwardQntl = (Label)e.Item.FindControl("lblNetOutwardQntl");
                    Label lblNetOutwardValue = (Label)e.Item.FindControl("lblNetOutwardValue");
                    lblNetInwardQntl.Text = NetInwardQntl.ToString();
                    lblNetInwardValue.Text = NetInwardValue.ToString();
                    lblNetOutwardQntl.Text = NetOutwardQntl.ToString();
                    lblNetOutwardValue.Text = NetOutwardValue.ToString();
                    dtlDetails.DataSource = dtt;
                    dtlDetails.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
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
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(StrExport.ToString());
            Response.Flush();
            Response.End();
        }
        catch (Exception)
        {

            throw;
        }
    }
}