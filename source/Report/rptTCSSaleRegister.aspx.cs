using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptTCSSaleRegister : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string tblPrefix = string.Empty;
    DataSet ds = null;
    DataTable dt = null;
    double grandNetQntl = 0.00;
    double grandSubtotal = 0.00;
    double grandExtraexpenses = 0.00;
    double grandBillAmount = 0.00;
    double grandTCSAmount = 0.00;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Mill_Code = Request.QueryString["AcCode"].ToString();
        fromDT = Request.QueryString["FromDt"].ToString();
        toDT = Request.QueryString["ToDt"].ToString();
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindList();
        }
    }
    private void BindList()
    {
        try
        {
            string from = Convert.ToDateTime(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string to = Convert.ToDateTime(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (Mill_Code != string.Empty)
            {
                qry = "Select DISTINCT Ac_Name_E,Ac_Code,CompanyPan from " + tblPrefix + "qryTCSSaleReport where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                             " doc_date between '" + fromDT + "' and '" + toDT + "' and Ac_Code=" + Mill_Code + " and TCS_Amt!=0";
            }
            else
            {
                qry = "Select DISTINCT Ac_Name_E,Ac_Code,CompanyPan from " + tblPrefix + "qryTCSSaleReport where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                   " doc_date between '" + fromDT + "' and '" + toDT + "' and TCS_Amt!=0";
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

                    lblGrandTCSAmount.Text = grandTCSAmount.ToString();
                    lblGrandBillAmount.Text = grandBillAmount.ToString();

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

            Label lbldtlDetailsTCSAmount = (Label)e.Item.FindControl("lbldtlDetailsTCSAmount");
            Label lbldtlDetailsBillAmount = (Label)e.Item.FindControl("lbldtlDetailsBillAmount");

            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblDate = (Label)e.Item.FindControl("lblDate");
            Label lblAc_Code = (Label)e.Item.FindControl("lblAc_Code");
            string Ac_Code = lblAc_Code.Text;

            qry = "select doc_no,Convert(varchar(10),doc_date,103) as doc_date,Bill_Amount,TCS_Amt from NT_1_qryTCSSaleReport where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                + Convert.ToInt32(Session["year"].ToString()) + "  and doc_date between '" + fromDT + "' and '" + toDT + "' and Ac_Code=" + Ac_Code + "  and TCS_Amt!=0 ";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {

                dt.Columns.Add(new DataColumn("doc_no", typeof(string)));
                dt.Columns.Add(new DataColumn("doc_date", typeof(string)));

                dt.Columns.Add(new DataColumn("Bill_Amount", typeof(double)));
                dt.Columns.Add(new DataColumn("TCS_Amt", typeof(double)));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["doc_no"] = ds.Tables[0].Rows[i]["doc_no"].ToString();
                        dr["doc_date"] = ds.Tables[0].Rows[i]["doc_date"].ToString();

                        dr["Bill_Amount"] = ds.Tables[0].Rows[i]["Bill_Amount"].ToString();
                        dr["TCS_Amt"] = ds.Tables[0].Rows[i]["TCS_Amt"].ToString();
                        dt.Rows.Add(dr);
                    }


                    double BillAmount = Convert.ToDouble(dt.Compute("SUM(Bill_Amount)", string.Empty));
                    double tcsamt = Convert.ToDouble(dt.Compute("SUM(TCS_Amt)", string.Empty));
                    //lbldtlDetailsNetQntl.Text = Convert.ToString(qntl);
                    //lbldtlDetailsSubTotal.Text = Convert.ToString(Subtotal);
                    //lbldtlDetailsExtraExp.Text = Convert.ToString(expenses);
                    lbldtlDetailsBillAmount.Text = Convert.ToString(BillAmount);
                    lbldtlDetailsTCSAmount.Text = Convert.ToString(tcsamt);

                    //grandNetQntl += qntl;
                    //grandSubtotal += Subtotal;
                    //grandExtraexpenses += expenses;
                    grandBillAmount += BillAmount;
                    grandTCSAmount += tcsamt;

                    if (dt.Rows.Count > 0)
                    {
                        lblDate1.Text = "TCS Sale Register From :" + fromDT + " To " + toDT;
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
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        //StringBuilder StrHtmlGenerate = new StringBuilder();
        //StringBuilder StrExport = new StringBuilder();
        //StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        //StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        //StrExport.Append("<DIV  style='font-size:12px;'>");
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter tw = new HtmlTextWriter(sw);
        //pnlMain.RenderControl(tw);
        //string sim = sw.ToString();
        //StrExport.Append(sim);
        //StrExport.Append("</div></body></html>");
        //string strFile = "report.xls";
        //string strcontentType = "application/excel";
        //Response.ClearContent();
        //Response.ClearHeaders();
        //Response.BufferOutput = true;
        //Response.ContentType = strcontentType;
        //Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        //Response.Write(StrExport.ToString());
        //Response.Flush();
        //Response.Close();
        //Response.End();
        string Name = "purchase_Bill";
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
        Response.AddHeader("content-disposition", "attachment;filename=" + Name + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        string style = @"<style> .textmode { } </style>";
        Response.Write(style);
        Response.Output.Write(StrExport.ToString());
        Response.Flush();
        Response.End();
    }
}