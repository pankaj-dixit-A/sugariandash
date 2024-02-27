using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptSaleRegisterEntries : System.Web.UI.Page
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
        fromDT = Request.QueryString["fromDT"];
        toDT = Request.QueryString["toDT"];
        lblCompanyName.Text = Session["Company_Name"].ToString();
        if (!IsPostBack)
        {
            this.BindList();
        }
    }


    private void BindList()
    {
        qry = "select doc_no as purc_no,Convert(varchar(10),doc_date,103) as purc_date,FROM_STATION as from_station,TO_STATION as to_station,(subTotal/NETQNTL) as rate,NETQNTL as quantal,Bill_Amount as amount,a.Ac_Name_E as supplier from " + tblPrefix + "SugarSale s" +
            " left outer join " + tblPrefix + "AccountMaster a on s.mill_code=a.Ac_Code and s.Company_Code=a.Company_Code where s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and s.doc_date between '" + fromDT + "' and '" + toDT + "' order by doc_date";
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("purc_no", typeof(string)));
            dt.Columns.Add(new DataColumn("purc_date", typeof(string)));
            dt.Columns.Add(new DataColumn("from_station", typeof(string)));
            dt.Columns.Add(new DataColumn("to_station", typeof(string)));
            dt.Columns.Add(new DataColumn("rate", typeof(double)));
            dt.Columns.Add(new DataColumn("quantal", typeof(double)));
            dt.Columns.Add(new DataColumn("amount", typeof(double)));
            dt.Columns.Add(new DataColumn("supplier", typeof(string)));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["purc_no"] = ds.Tables[0].Rows[i]["purc_no"].ToString();
                    dr["purc_date"] = ds.Tables[0].Rows[i]["purc_date"].ToString();
                    dr["from_station"] = ds.Tables[0].Rows[i]["from_station"].ToString();
                    dr["to_station"] = ds.Tables[0].Rows[i]["to_station"].ToString();
                    double rate = Convert.ToDouble(ds.Tables[0].Rows[i]["rate"].ToString());
                    dr["rate"] = Math.Round(rate, 2);
                    dr["quantal"] = ds.Tables[0].Rows[i]["quantal"].ToString();
                    dr["amount"] = ds.Tables[0].Rows[i]["amount"].ToString();
                    dr["supplier"] = ds.Tables[0].Rows[i]["supplier"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblQnttotal.Text = Convert.ToString(dt.Compute("SUM(quantal)", string.Empty));
                    lblamounttotal.Text = Convert.ToString(dt.Compute("SUM(amount)", string.Empty));
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
            }
        }
    }
    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {

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