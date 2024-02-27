using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptPurchaseRegister : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string tblPrefix = string.Empty;
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
    protected void BindList()
    {
        string from = Convert.ToDateTime(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string to = Convert.ToDateTime(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        if (Mill_Code != string.Empty)
        {
            qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,p.Bill_No,a.Ac_Name_E as Mill,p.NETQNTL as qntl," +
                " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                " from " + tblPrefix + "SugarPurchase p left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code and a.Company_Code=p.Company_Code  where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and p.mill_code=" + Mill_Code + " and " +
                " p.doc_date between '" + from + "' and '" + to + "'";
        }
        else
        {

            qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,p.Bill_No,a.Ac_Name_E as Mill,p.NETQNTL as qntl," +
                " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                " from " + tblPrefix + "SugarPurchase p inner join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                " p.doc_date between '" + from + "' and '" + to + "'";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {

            dt.Columns.Add(new DataColumn("P_No", typeof(string)));
            dt.Columns.Add(new DataColumn("P_Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Bill_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Mill", typeof(string)));
            dt.Columns.Add(new DataColumn("qntl", typeof(double)));
            dt.Columns.Add(new DataColumn("Rate", typeof(double)));
            dt.Columns.Add(new DataColumn("Subtotal", typeof(double)));
            dt.Columns.Add(new DataColumn("Extra_Expense", typeof(double)));
            dt.Columns.Add(new DataColumn("Bill_Amount", typeof(double)));
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["P_No"] = ds.Tables[0].Rows[i]["P_No"].ToString();
                    dr["P_Date"] = ds.Tables[0].Rows[i]["P_Date"].ToString();
                    dr["Bill_No"] = ds.Tables[0].Rows[i]["Bill_No"].ToString();
                    dr["Mill"] = ds.Tables[0].Rows[i]["Mill"].ToString();
                    dr["qntl"] = ds.Tables[0].Rows[i]["qntl"].ToString();
                    string rate = ds.Tables[0].Rows[i]["Rate"].ToString();
                    dr["Rate"] = Math.Round(double.Parse(rate), 3);
                    dr["Subtotal"] = ds.Tables[0].Rows[i]["Subtotal"].ToString();
                    string expens = ds.Tables[0].Rows[i]["Extra_Expense"].ToString();
                    dr["Extra_Expense"] = double.Parse("0" + expens);
                    dr["Bill_Amount"] = ds.Tables[0].Rows[i]["Bill_Amount"].ToString();
                    dt.Rows.Add(dr);
                }
                lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(qntl)", string.Empty));
                lblRateTotal.Text = Convert.ToString(dt.Compute("SUM(Rate)", string.Empty));
                lblSubtotalTotal.Text = Convert.ToString(dt.Compute("SUM(Subtotal)", string.Empty));
                lblExpsTotal.Text = Convert.ToString(dt.Compute("SUM(Extra_Expense)", string.Empty));
                lblBillTotal.Text = Convert.ToString(dt.Compute("SUM(Bill_Amount)", string.Empty));
                if (dt.Rows.Count > 0)
                {
                    if (Mill_Code != string.Empty)
                    {
                        lblBrokerName.Text = "Mill Name :" + " " + clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + Mill_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    }
                    else
                    {
                        lblBrokerName.Text = "All Mills";
                    }
                    lblDate.Text = "Purchase List From :" + fromDT + " To " + toDT;
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