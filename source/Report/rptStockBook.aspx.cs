using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptStockBook : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        if (!IsPostBack)
        {
            fromDT = Request.QueryString["fromDT"];
            toDT = Request.QueryString["toDT"];
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindList();
        }

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
            string fromDate = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            string toDate = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            Label lblItemCode = (Label)e.Item.FindControl("lblItemCode");
            string itemCode = lblItemCode.Text;

            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");


            qry = "Select Tran_Type,SUM(case Tran_Type when 'PS' then amount when 'SB' then -amount end) as OpVal from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date< '" + fromDate + "' and item_code=" + itemCode + " Group By Tran_Type";
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

            qry = "Select Tran_Type,SUM(case Tran_Type when 'PS' then qntl when 'SB' then -qntl end) as OpQntl from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date< '" + fromDate + "' and item_code=" + itemCode + " Group By Tran_Type";
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
            double PurcQntl = 0.0;
            double PurcValue = 0.0;
            double SaleQntl = 0.0;
            double SaleValue = 0.0;
            double ClosingValue = 0.0;

            double NetInwardQntl = 0.0;
            double NetInwardValue = 0.0;
            double NetOutwardQntl = 0.0;
            double NetOutwardValue = 0.0;

            qry = "";
            qry = "select #,Tran_Type,Convert(varchar(10),doc_date,103) as date,millShortName as mill,PartyName as party,qntl,amount,DO_No from " + tblPrefix + "qryStockBook where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDate + "' and '" + toDate + "' and item_code=" + itemCode + " order by doc_date";
            DataSet dst = new DataSet();
            dst = clsDAL.SimpleQuery(qry);
            if (dst.Tables[0].Rows.Count > 0)
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add(new DataColumn("date", typeof(string)));
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
                dtt.Columns.Add(new DataColumn("DONo", typeof(string)));



                for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    string Tran_Type = dst.Tables[0].Rows[i]["Tran_Type"].ToString();
                    dr["date"] = dst.Tables[0].Rows[i]["date"].ToString();
                    dr["Tran_Type"] = dst.Tables[0].Rows[i]["Tran_Type"].ToString();
                    dr["#"] = dst.Tables[0].Rows[i]["#"].ToString();
                    dr["Supplier"] = "";
                    dr["OpeningQty"] = OpQntl;
                    dr["OpeningVal"] = OpVal;
                    dr["DONo"] = dst.Tables[0].Rows[i]["DO_No"].ToString();
                    if (Tran_Type == "PS" || Tran_Type == "PR")
                    {
                        dr["Supplier"] = dst.Tables[0].Rows[i]["mill"].ToString();
                        PurcQntl = Convert.ToDouble(dst.Tables[0].Rows[i]["qntl"].ToString());
                        PurcValue = Convert.ToDouble(dst.Tables[0].Rows[i]["amount"].ToString());
                        dr["PurcQty"] = PurcQntl;
                        dr["PurcVal"] = PurcValue;
                        NetInwardQntl += PurcQntl;
                        NetInwardValue += PurcValue;
                        SaleQntl = 0.0;
                        SaleValue = 0.0;

                    }
                    else
                    {
                        dr["Supplier"] = dst.Tables[0].Rows[i]["party"].ToString();
                        SaleQntl = Convert.ToDouble(dst.Tables[0].Rows[i]["qntl"].ToString());
                        SaleValue = Convert.ToDouble(dst.Tables[0].Rows[i]["amount"].ToString());
                        dr["SaleQty"] = SaleQntl;
                        dr["SaleVal"] = SaleValue;
                        NetOutwardQntl += SaleQntl;
                        NetOutwardValue += SaleValue;
                        PurcQntl = 0.0;
                        PurcValue = 0.0;
                    }
                    ClosingQntl = ((OpQntl + PurcQntl) - SaleQntl);
                    ClosingValue = ((OpVal + PurcValue) - SaleValue);
                    dr["BalQty"] = ClosingQntl;
                    dr["BalValue"] = ClosingValue;

                    OpQntl = ClosingQntl;
                    OpVal = ClosingValue;
                    PurcQntl = 0.0;
                    PurcValue = 0.0;
                    SaleQntl = 0.0;
                    SaleValue = 0.0;
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