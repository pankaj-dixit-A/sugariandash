using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptDateWisePurchaseRegister : System.Web.UI.Page
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
    string PartyorMill = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Mill_Code = Request.QueryString["AcCode"].ToString();
        fromDT = Request.QueryString["FromDt"].ToString();
        toDT = Request.QueryString["ToDt"].ToString();
       // PartyorMill = Request.QueryString["PorM"].ToString(); 
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
                if (PartyorMill != string.Empty)
                {
                    if (PartyorMill == "P")
                    {
                        qry = "Select DISTINCT(Convert(varchar(10),doc_date,103)) as Date,doc_date as DDate from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                             " doc_date between '" + from + "' and '" + to + "' and Ac_Code=" + Mill_Code + " order by DDate";
                    }
                    else
                    {
                        qry = "Select DISTINCT(Convert(varchar(10),doc_date,103)) as Date,doc_date as DDate from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                             " doc_date between '" + from + "' and '" + to + "' and mill_code=" + Mill_Code + " order by DDate";
                    }
                }
                else
                {
                    qry = "Select DISTINCT(Convert(varchar(10),doc_date,103)) as Date,doc_date as DDate from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                            " doc_date between '" + from + "' and '" + to + "' and mill_code=" + Mill_Code + " order by DDate";
                }
            }
            else
            {
                qry = "Select DISTINCT(Convert(varchar(10),doc_date,103)) as Date,doc_date as DDate from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                   " doc_date between '" + from + "' and '" + to + "'  order by DDate";
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
                    lblGrandNetQntl.Text = grandNetQntl.ToString();
                    lblGrandSubTotal.Text = grandSubtotal.ToString();
                    lblGrandExtraExp.Text = grandExtraexpenses.ToString();
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
            Label lbldtlDetailsNetQntl = (Label)e.Item.FindControl("lbldtlDetailsNetQntl");
            Label lbldtlDetailsSubTotal = (Label)e.Item.FindControl("lbldtlDetailsSubTotal");
            Label lbldtlDetailsExtraExp = (Label)e.Item.FindControl("lbldtlDetailsExtraExp");
            Label lbldtlDetailsBillAmount = (Label)e.Item.FindControl("lbldtlDetailsBillAmount");
            string PartyKivaMill = "Mill Name";
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblDate = (Label)e.Item.FindControl("lblDate");
            string Date = DateTime.Parse(lblDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            if (Mill_Code != string.Empty)
            {

                if (PartyorMill != string.Empty)
                {
                    if (PartyorMill == "P")
                    {
                        PartyKivaMill = "Party Name";
                        qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,s.PURCNO as Bill_No,a.Ac_Name_E as Mill,b.Ac_Name_E as Voucher_By,c.Ac_Name_E as Unit,p.LORRYNO as Lorry,p.NETQNTL as qntl," +
                    " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                    " from " + tblPrefix + "SugarPurchase p left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code and a.Company_Code=p.Company_Code and a.Company_Code=p.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=p.Ac_Code and b.Company_Code=p.Company_Code and b.Company_Code=p.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster c on c.Ac_Code=p.Unit_Code and c.Company_Code=c.Company_Code and c.Company_Code=p.Company_Code  left outer join " + tblPrefix + "SugarSale s on p.Company_Code=s.Company_Code and p.Year_Code=s.Year_Code and p.doc_no=s.PURCNO  where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and p.Ac_Code=" + Mill_Code + " and " +
                    " p.doc_date='" + Date + "' group by p.doc_no, p.doc_date,s.PURCNO, a.Ac_Name_E ,b.Ac_Name_E,c.Ac_Name_E,p.LORRYNO , p.NETQNTL , p.subTotal / p.NETQNTL , p.subTotal ,p.bank_commission + p.freight + p.OTHER_AMT , p.Bill_Amount";
                    }
                    else
                    {
                        qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,s.PURCNO as Bill_No,a.Ac_Name_E as Mill,b.Ac_Name_E as Voucher_By,c.Ac_Name_E as Unit,p.LORRYNO as Lorry,p.NETQNTL as qntl," +
                    " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                    " from " + tblPrefix + "SugarPurchase p left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code and a.Company_Code=p.Company_Code and a.Company_Code=p.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=p.Ac_Code and b.Company_Code=p.Company_Code and b.Company_Code=p.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster c on c.Ac_Code=p.Unit_Code and c.Company_Code=c.Company_Code and c.Company_Code=p.Company_Code  left outer join " + tblPrefix + "SugarSale s on p.Company_Code=s.Company_Code and p.Year_Code=s.Year_Code and p.doc_no=s.PURCNO  where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and p.mill_code=" + Mill_Code + " and " +
                    " p.doc_date='" + Date + "' group by p.doc_no, p.doc_date,s.PURCNO, a.Ac_Name_E ,b.Ac_Name_E,c.Ac_Name_E,p.LORRYNO , p.NETQNTL , p.subTotal / p.NETQNTL , p.subTotal ,p.bank_commission + p.freight + p.OTHER_AMT , p.Bill_Amount";
                    }
                }
                else
                {
                    qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,s.PURCNO as Bill_No,a.Ac_Name_E as Mill,b.Ac_Name_E as Voucher_By,c.Ac_Name_E as Unit,p.LORRYNO as Lorry,p.NETQNTL as qntl," +
                    " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                    " from " + tblPrefix + "SugarPurchase p left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code and a.Company_Code=p.Company_Code and a.Company_Code=p.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=p.Ac_Code and b.Company_Code=p.Company_Code and b.Company_Code=p.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster c on c.Ac_Code=p.Unit_Code and c.Company_Code=c.Company_Code and c.Company_Code=p.Company_Code  left outer join " + tblPrefix + "SugarSale s on p.Company_Code=s.Company_Code and p.Year_Code=s.Year_Code and p.doc_no=s.PURCNO  where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and p.mill_code=" + Mill_Code + " and " +
                    " p.doc_date='" + Date + "' group by p.doc_no, p.doc_date,s.PURCNO, a.Ac_Name_E ,b.Ac_Name_E,c.Ac_Name_E,p.LORRYNO , p.NETQNTL , p.subTotal / p.NETQNTL , p.subTotal ,p.bank_commission + p.freight + p.OTHER_AMT , p.Bill_Amount";
                }

            }
            else
            {
                qry = "select p.doc_no as P_No,Convert(varchar(10),p.doc_date,103) as P_Date,s.PURCNO as Bill_No,a.Ac_Name_E as Mill,b.Ac_Name_E as Voucher_By,c.Ac_Name_E as Unit,p.LORRYNO as Lorry,p.NETQNTL as qntl," +
                    " (p.subTotal/p.NETQNTL) as Rate,p.subTotal as Subtotal,(p.bank_commission+p.freight+p.OTHER_AMT) as Extra_Expense,p.Bill_Amount as Bill_Amount" +
                    " from " + tblPrefix + "SugarPurchase p left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=p.mill_code and a.Company_Code=p.Company_Code" +
                    " left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=p.Ac_Code and b.Company_Code=p.Company_Code and b.Company_Code=p.Company_Code " +
                    " left outer join " + tblPrefix + "AccountMaster c on c.Ac_Code=p.Unit_Code and c.Company_Code=c.Company_Code and c.Company_Code=p.Company_Code  left outer join " + tblPrefix + "SugarSale s on p.Company_Code=s.Company_Code and p.Year_Code=s.Year_Code and p.doc_no=s.PURCNO  where p.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and p.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and " +
                    " p.doc_date='" + Date + "' group by p.doc_no, p.doc_date,s.PURCNO, a.Ac_Name_E,b.Ac_Name_E,c.Ac_Name_E,p.LORRYNO , p.NETQNTL , p.subTotal / p.NETQNTL , p.subTotal ,p.bank_commission + p.freight + p.OTHER_AMT , p.Bill_Amount";
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
                dt.Columns.Add(new DataColumn("Voucher_By", typeof(string)));
                dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                dt.Columns.Add(new DataColumn("Lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("Rate", typeof(double)));
                dt.Columns.Add(new DataColumn("Subtotal", typeof(double)));
                dt.Columns.Add(new DataColumn("Extra_Expense", typeof(double)));
                dt.Columns.Add(new DataColumn("Bill_Amount", typeof(double)));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["P_No"] = ds.Tables[0].Rows[i]["P_No"].ToString();
                        //dr["P_Date"] = ds.Tables[0].Rows[i]["P_Date"].ToString();
                        dr["Bill_No"] = ds.Tables[0].Rows[i]["Bill_No"].ToString();
                        dr["Mill"] = ds.Tables[0].Rows[i]["Mill"].ToString();
                        dr["Voucher_By"] = ds.Tables[0].Rows[i]["Voucher_By"].ToString();
                        dr["Unit"] = ds.Tables[0].Rows[i]["Unit"].ToString();
                        dr["Lorry"] = ds.Tables[0].Rows[i]["Lorry"].ToString();

                        dr["qntl"] = ds.Tables[0].Rows[i]["qntl"].ToString();
                        string rate = ds.Tables[0].Rows[i]["Rate"].ToString();
                        dr["Rate"] = Math.Round(double.Parse(rate), 3);
                        dr["Subtotal"] = ds.Tables[0].Rows[i]["Subtotal"].ToString();
                        string expens = ds.Tables[0].Rows[i]["Extra_Expense"].ToString();
                        dr["Extra_Expense"] = double.Parse("0" + expens);
                        dr["Bill_Amount"] = ds.Tables[0].Rows[i]["Bill_Amount"].ToString();
                        dt.Rows.Add(dr);
                    }
                    double qntl = Convert.ToDouble(dt.Compute("SUM(qntl)", string.Empty));
                    double Subtotal = Convert.ToDouble(dt.Compute("SUM(Subtotal)", string.Empty));
                    double expenses = Convert.ToDouble(dt.Compute("SUM(Extra_Expense)", string.Empty));
                    double BillAmount = Convert.ToDouble(dt.Compute("SUM(Bill_Amount)", string.Empty));
                    lbldtlDetailsNetQntl.Text = Convert.ToString(qntl);
                    lbldtlDetailsSubTotal.Text = Convert.ToString(Subtotal);
                    lbldtlDetailsExtraExp.Text = Convert.ToString(expenses);
                    lbldtlDetailsBillAmount.Text = Convert.ToString(BillAmount);

                    grandNetQntl += qntl;
                    grandSubtotal += Subtotal;
                    grandExtraexpenses += expenses;
                    grandBillAmount += BillAmount;


                    if (dt.Rows.Count > 0)
                    {
                        if (Mill_Code != string.Empty)
                        {
                            lblBrokerName.Text = PartyKivaMill + ":" + " " + clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + Mill_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        }
                        else
                        {
                            lblBrokerName.Text = "All Mills";
                        }
                        lblDate1.Text = "Purchase List From :" + fromDT + " To " + toDT;
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