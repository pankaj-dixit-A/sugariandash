using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class Sugar_pgeCreateReturnFilesNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCreateb2b_Click(object sender, EventArgs e)
    {
        string fromdt = txtFromDt.Text;
        string todt = txtToDt.Text;
        fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        try
        {
            //string qry = "select Ac_Code,PartyName,LTRIM(RTRIM(PartyGST)) as [GSTIN/UIN of Recipient],ISNULL(NULLIF(PartyStateCode,''),0) as PartyStateCode,CONVERT(NVARCHAR,newsbno) as [Invoice Number],REPLACE(CONVERT(CHAR(11),doc_date, 106),' ','-') as [Invoice date]," +
            //             "Bill_Amount as [Invoice Value],(RIGHT('0'+CONVERT(NVARCHAR,PartyStateCode),2) +'-'+ LTRIM(RTRIM(PartyState))) as [Place Of Supply],'N' as [Reverse Charge],'Regular' as [Invoice Type]," +
            //             "'' as [E-Commerce GSTIN],5 as Rate,TaxableAmount as [Taxable Value],'' as [Cess Amount] from NT_1_qrySugarSaleForGSTReturn where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
            //             " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and UnregisterGST=0";

            string qry = "select Ac_Code,PartyName,LTRIM(RTRIM(PartyGST)) as [GSTIN/UIN of Recipient],ISNULL(NULLIF(PartyStateCode,''),0) as PartyStateCode,CONVERT(NVARCHAR,newsbno) as [Invoice Number],REPLACE(CONVERT(CHAR(11),doc_date, 106),' ','-') as [Invoice date]," +
                       "Bill_Amount as [Invoice Value],(RIGHT('0'+CONVERT(NVARCHAR,PartyStateCode),2) +'-'+ LTRIM(RTRIM(PartyState))) as [Place Of Supply],'N' as [Reverse Charge],'Regular' as [Invoice Type]," +
                       "'' as [E-Commerce GSTIN],5 as Rate,TaxableAmount as [Taxable Value],'' as [Cess Amount] from NT_1_qrySugarSaleForGSTReturn where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                       " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and UnregisterGST=0 group by  Ac_Code,PartyName,PartyGST,PartyStateCode,Bill_Amount,doc_date,TaxableAmount,newsbno,PartyState order by newsbno";


            string qry1 = "select v.Ac_Code,a.Ac_Name_E as PartyName,LTRIM(RTRIM(a.Gst_No)) as [GSTIN/UIN of Recipient],ISNULL(NULLIF(a.GSTStateCode,''),0) as PartyStateCode,'D'+CONVERT(NVARCHAR,v.Doc_No) as [Invoice Number],REPLACE(CONVERT(CHAR(11),v.Doc_Date, 106),' ','-') as [Invoice date]," +
                          " v.Voucher_Amount as [Invoice Value],(RIGHT('0'+CONVERT(NVARCHAR,a.GSTStateCode),2) +'-'+ LTRIM(RTRIM(a.GSTStateName))) as [Place Of Supply],'N' as [Reverse Charge],'Regular' as [Invoice Type],'' as [E-Commerce GSTIN],18 as Rate,v.TaxableAmount as [Taxable Value],'' as [Cess Amount]  from NT_1_Voucher v " +
                          " left outer join NT_1_qryAccountsList a on v.Company_Code=a.Company_Code and v.Ac_Code=a.Ac_Code where a.UnregisterGST=0 and v.Tran_Type='LV' and v.Voucher_Amount>0 and v.Doc_Date between '" + fromdt + "' and '" + todt + "'  and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                          " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            DataSet dsDNote = new DataSet();
            dsDNote = clsDAL.SimpleQuery(qry1);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    if (dsDNote.Tables[0].Rows.Count > 0)
                    {
                        dt.Merge(dsDNote.Tables[0]);
                    }

                    string fileName = "";
                    string strForCSV = "";

                    DataView dvWrongGst = new DataView(dt);
                    dvWrongGst.RowFilter = "LEN([GSTIN/UIN of Recipient]) <> 15 ";

                    DataView dvWrongState = new DataView(dt);
                    dvWrongState.RowFilter = "PartyStateCode = 0";

                    if (dvWrongGst.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongGSTNumbers.csv";
                        DataTable dtnew = dvWrongGst.ToTable();

                        int colindex = 3;
                        for (int i = 0; i < dvWrongGst.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 4)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "Ac_Code", "PartyName");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else if (dvWrongState.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongState.csv";
                        DataTable dtnew = dvWrongState.ToTable();

                        int colindex = 3;
                        for (int i = 0; i < dvWrongState.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 4)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "Ac_Code", "PartyName");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else
                    {
                        fileName = "b2b.csv";
                        dt.Columns.Remove("Ac_Code");
                        dt.Columns.Remove("PartyName");
                        dt.Columns.Remove("PartyStateCode");
                        strForCSV = clsCommon.DataTableToCSV(dt, ',');
                    }
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(strForCSV.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnCreatePurchaseBillSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,doc_no as OurNo,Bill_No as MillInvoiceNo,PartyName,FromGSTNo,FromStateCode,Convert(varchar(10),doc_date,103) as Date, " +
            //             " LORRYNO as Vehicle_No,Quantal as Quintal,rate as Rate,subTotal as TaxableAmount,CGSTAmount as CGST,SGSTAmount as SGST,IGSTAmount as IGST,Bill_Amount as Payable_Amount from NT_1_qrySugarPurchListForReport " +
            //             " where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by doc_date";

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,doc_no as OurNo,Bill_No as MillInvoiceNo,PartyName,FromGSTNo,FromStateCode,Convert(varchar(10),doc_date,103) as Date, " +
             " LORRYNO as Vehicle_No,sum(Quantal) as Quintal,sum(item_Amount)/sum(Quantal) as Rate,subTotal as TaxableAmount,CGSTAmount as CGST,SGSTAmount as SGST,IGSTAmount as IGST,Bill_Amount as Payable_Amount from NT_1_qrySugarPurchListForReport " +
             " where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " group by doc_no,Bill_No,PartyName,FromGSTNo,FromStateCode,doc_date,  LORRYNO, " +
             " subTotal ,CGSTAmount,SGSTAmount ,IGSTAmount ,Bill_Amount   order by doc_date";


            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Purchase Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnCreateSaleBillSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,s.doc_no as Invoice_No,newsbno as NewInvoice_No,s.PartyGSTNo,Ac_Code as PartyCode,PartyName,PartyStateCode,CONVERT(varchar(10),s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.TaxableAmount, " +
            //    " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount from NT_1_qrySugarSaleListForReport s  " +
            //    " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by doc_date";

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,s.doc_no as Invoice_No,newsbno as NewInvoice_No,s.PartyGSTNo,Ac_Code as PartyCode,PartyName,PartyStateCode,CONVERT(varchar(10),s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,sum(s.Quantal) as Quintal,sum(s.item_Amount)/sum(s.Quantal) as Rate,s.TaxableAmount, " +
               " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount from NT_1_qrySugarSaleListForReport s  " +
               " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " " +
             " group by doc_no,newsbno,PartyGSTNo,Ac_Code,PartyName,PartyStateCode,doc_date,LORRYNO,TaxableAmount, " +
             " SGSTAmount,CGSTAmount,IGSTAmount,Bill_Amount order by doc_date";
        
            
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Sale Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[8].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[14].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnFrieghtSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "select '' as Challan_No,d.memo_no as Memo_No,CONVERT(varchar(10),d.doc_date,103) as Date,mill_code as Mill_Code,m.Ac_Name_E as MillName,d.MillGSTStateCode as MillStateCode,"+
                        " d.SaleBillTo as Billed_To,p.Ac_Name_E as ShippedToName,d.SalebilltoGstStateCode as BillToStateCode, d.truck_no as Vehicle_No,d.quantal as Quintal,d.MM_Rate as Rate,d.Memo_Advance as Amount "+
                        " from NT_1_deliveryorder d left outer join NT_1_AccountMaster m on d.mill_code=m.Ac_Code and d.company_code=m.Company_Code "+
                        " left outer join NT_1_AccountMaster p on d.SaleBillTo=p.Ac_Code and d.company_code=p.Company_Code where d.doc_date>='2017-07-01' and d.doc_date between '" + fromdt + "' and '" + todt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and MM_Rate!=0 and memo_no!=0";

            DataSet ds1 = new DataSet();
            ds1 = clsDAL.SimpleQuery(qry);
            if (ds1 != null)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = ds1.Tables[0];

                    dt1.Columns.Add(new DataColumn("CGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("SGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("IGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("FinalAmount", typeof(double)));

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        double cgstrate = 2.5;
                        double sgstrate = 2.5;
                        double igstrate = 5;

                        double CGSTAmount = 0.0;
                        double SGSTAmount = 0.0;
                        double IGSTAmount = 0.0;

                        int millStateCode = Convert.ToInt32(dt1.Rows[i]["MillStateCode"].ToString());
                        int partyStateCode = Convert.ToInt32(dt1.Rows[i]["BillToStateCode"].ToString());
                        double Amount = Convert.ToDouble(dt1.Rows[i]["Amount"].ToString());
                        if (millStateCode == partyStateCode)
                        {
                            CGSTAmount = Math.Round((Amount * cgstrate / 100), 2);
                            SGSTAmount = Math.Round((Amount * sgstrate / 100), 2);
                        }
                        else
                        {
                            IGSTAmount = Math.Round((Amount * igstrate / 100), 2);
                        }

                        dt1.Rows[i]["CGST"] = CGSTAmount;
                        dt1.Rows[i]["SGST"] = SGSTAmount;
                        dt1.Rows[i]["IGST"] = IGSTAmount;
                        dt1.Rows[i]["FinalAmount"] = Math.Round((Amount + CGSTAmount + SGSTAmount + IGSTAmount), 2);
                    }
                    lblSummary.Text = "Frieght Summary";
                    grdAll.DataSource = dt1;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt1.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt1.Compute("SUM(Amount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt1.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt1.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt1.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt1.Compute("SUM(FinalAmount)", string.Empty));

                    grdAll.FooterRow.Cells[10].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[14].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[15].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[16].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnDebitNoteSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,v.Doc_No as DebitNote_No,a.Gst_No as PartyGSTNo,v.Ac_Code as PartyCode,a.Ac_Name_E as PartyName,a.GSTStateCode as PartyStateCode," +
                        " CONVERT(varchar(10),v.Doc_Date,103) as Date,v.Quantal as Quintal,v.Diff_Amount as Rate,v.TaxableAmount,v.CGSTAmount as CGST,v.SGSTAmount as SGST," +
                        " v.IGSTAmount as IGST,v.Voucher_Amount as Final_Amount from NT_1_Voucher v " +
                        " left outer join NT_1_AccountMaster a on v.Ac_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                        " where v.Tran_Type='LV' and v.TaxableAmount!=0 and v.doc_date>='2017-07-01' and v.doc_date between '" + fromdt + "' and '" + todt + "' and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Debit Note Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Final_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    //grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                }

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnDebitCreditNote_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "";
            if (drpdebitcreditnote.SelectedValue == "All")
            {
                qry = "select 0+ ROW_NUMBER() OVER(ORDER BY Company_Code) as SRNO,Inovice_No,Invoice_Date,[Bill To ACC NO],BilltoName,[BILL TO GSTIN]," +
                   " [BillToStateCode] ,(RIGHT('0'+CONVERT(NVARCHAR,BillToStateCode),2) +'-'+ LTRIM(RTRIM(shiptostatename))) as PlaceOfSupply,TaxableAmt,CGST,SGST,IGST,TCS,Payable_Amount,OldInvNo,OldInvDate,ackno as ACKNo from qryDebitcreditnote_GST where  " +
               " doc_date between '" + fromdt + "' and '" + todt +
                   "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                   " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  order by Invoice_Date";
            }
            else
            {

                qry = "select 0+ ROW_NUMBER() OVER(ORDER BY Company_Code) as SRNO,Inovice_No,Invoice_Date,[Bill To ACC NO],BilltoName,[BILL TO GSTIN]," +
                " [BillToStateCode] ,(RIGHT('0'+CONVERT(NVARCHAR,BillToStateCode),2) +'-'+ LTRIM(RTRIM(shiptostatename))) as PlaceOfSupply,TaxableAmt,CGST,SGST,IGST,TCS,Payable_Amount,OldInvNo,OldInvDate,ackno as ACKNo from qryDebitcreditnote_GST where  " +
            " doc_date between '" + fromdt + "' and '" + todt +
                "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + drpdebitcreditnote.SelectedValue + "'  order by Invoice_Date";
            }

            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    //dt.Columns.Remove("RowNumber");
                    lblSummary.Text = drpdebitcreditnote.SelectedItem.ToString();


                    if (dt.Rows.Count > 0)
                    {


                        grdAll.DataSource = dt;
                        grdAll.DataBind();



                    }


                    //  double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));


                    double totalTaxAmt = Convert.ToDouble(dt.Compute("SUM(TaxableAmt)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));
                    double tcs = Convert.ToDouble(dt.Compute("SUM(TCS)", string.Empty));

                    //grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();


                    grdAll.FooterRow.Cells[8].Text = totalTaxAmt.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = tcs.ToString();

                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();



                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnExportToexcel_Click(object sender, EventArgs e)
    {
        Export(grdAll, lblSummary.Text);
    }

    private void Export(GridView grd, string Name)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        grd.RenderControl(tw);
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


    protected void drpSaleTCS_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFocus(btnSaleTCS);
    }

    protected void btnSaleTCS_Click(object sender, EventArgs e)
    {
        try
        {

            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "";
            //if (drpSaleTCS.SelectedValue == "SB")
            //{
            //    qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,'SB'+convert(varchar(50),doc_no) as InvoiceNo,CONVERT(varchar(10),doc_date,103) as date,billtoname as [Name Of Party]," +
            //                " billtopanno as Pan,billtoaddress as Address,Bill_Amount as Net, CGSTAmount as CGST," +
            //                 " SGSTAmount as SGST,IGSTAmount as IGST,TCS_Amt as TCS from qrysalehead where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt
            //                 + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and TCS_Amt!=0 order by doc_date";
            //}
         
            if (drpSaleTCS.SelectedValue == "RB")
            {
                qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,'RB'+convert(varchar(50),doc_no) as InvoiceNo,CONVERT(varchar(10),Date,103) as date,REPLACE(REPLACE(Ac_Name_E,',',' '),'.','') as [Name Of Party]," +
                            " REPLACE(CompanyPan,',',' ') as Pan,REPLACE(Tan_no,',',' ') as Tan, REPLACE(REPLACE(Address_E,',',' '),'.','') as Address,Subtotal AS Taxable_Amt, CGSTAmount as CGST," +
                             " SGSTAmount as SGST,IGSTAmount as IGST,Final_Amount as Bill_Amt,TCS_Amt as TCS from qryrentbillhead where Date>='2017-07-01' and Date between '" + fromdt + "' and '" + todt
                             + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and IsDeleted!=0 and TCS_Amt!=0 order by Date";
            }   
             
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            DataTable dt = new DataTable();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    dt = ds.Tables[0];
                     
                    if (drpSaleTCS.SelectedValue == "RB")
                    {
                        lblSummary.Text = "Rent Bill TCS Summary";
                    }
                    grdAll.DataSource = dt;
                    grdAll.DataBind();


                    double totalNet = Convert.ToDouble(dt.Compute("SUM(Taxable_Amt)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalTCS = Convert.ToDouble(dt.Compute("SUM(TCS)", string.Empty));
                    double totalBillAmt = Convert.ToDouble(dt.Compute("SUM(Bill_Amt)", string.Empty));


                    grdAll.FooterRow.Cells[7].Text = totalNet.ToString();
                    grdAll.FooterRow.Cells[8].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalBillAmt.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalTCS.ToString();


                    grdAll.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                }

                if (hdconfirm.Value == "Yes")
                {
                    #region[csv]
                    string fileName = "";
                    string strForCSV = "";
                    DataView dvWrongGst = new DataView(dt);
                    dvWrongGst.RowFilter = "LEN(Pan) < 1";

                    if (dvWrongGst.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongPanNumbers.csv";
                        DataTable dtnew = dvWrongGst.ToTable();

                        int colindex = 5;
                        for (int i = 0; i < dvWrongGst.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 6)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "InvoiceNo", "Pan");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else
                    {
                        fileName = "SaleTCS.csv";

                        strForCSV = clsCommon.DataTableToCSV(dt, ',');
                    }
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(strForCSV.ToString());
                    Response.Flush();
                    Response.End();
                    #endregion
                }
            }
            else
            {
                grdAll.DataSource = null;
                grdAll.DataBind();
                lblSummary.Text = "Records Not Found";
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnSaleTDS_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = ""; 
          
            if (drpSaleTCS.SelectedValue == "RB")
            {
                qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,'RB'+convert(varchar(50),doc_no) as InvoiceNo,CONVERT(varchar(10),Date,103) as date,REPLACE(REPLACE(Ac_Name_E,',',' '),'.','') as [Name Of Party]," +
                            " REPLACE(CompanyPan,',',' ') as Pan,REPLACE(Tan_no,',',' ') as Tan, REPLACE(REPLACE(Address_E,',',' '),'.','') as Address,Final_Amount as Net, CGSTAmount as CGST," +
                             " SGSTAmount as SGST,IGSTAmount as IGST, TDS as TDS from qryrentbillhead where Date>='2017-07-01' and Date between '" + fromdt + "' and '" + todt
                             + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and IsDeleted!=0 and TDS!=0 order by Date";
            }
          
        
            
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            DataTable dt = new DataTable();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    dt = ds.Tables[0];

                   
                    if (drpSaleTCS.SelectedValue == "RB")
                    {
                        lblSummary.Text = "Rent Bill TDS Summary";
                    }
                    grdAll.DataSource = dt;
                    grdAll.DataBind();


                    double totalNet = Convert.ToDouble(dt.Compute("SUM(Net)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalTCS = Convert.ToDouble(dt.Compute("SUM(TDS)", string.Empty));


                    grdAll.FooterRow.Cells[7].Text = totalNet.ToString();
                    grdAll.FooterRow.Cells[8].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalTCS.ToString();

                    grdAll.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                }

                if (hdconfirm.Value == "Yes")
                {
                    #region[csv]
                    string fileName = "";
                    string strForCSV = "";
                    DataView dvWrongGst = new DataView(dt);
                    dvWrongGst.RowFilter = "LEN(Pan) < 1";

                    if (dvWrongGst.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongPanNumbers.csv";
                        DataTable dtnew = dvWrongGst.ToTable();

                        int colindex = 5;
                        for (int i = 0; i < dvWrongGst.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 6)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "InvoiceNo", "Pan");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else
                    {
                        fileName = "SaleTDS.csv";

                        strForCSV = clsCommon.DataTableToCSV(dt, ',');
                    }
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(strForCSV.ToString());
                    Response.Flush();
                    Response.End();
                    #endregion
                }
            }
            else
            {
                grdAll.DataSource = null;
                grdAll.DataBind();
                lblSummary.Text = "Records Not Found";
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnServiceBill_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,'Rent'+CONVERT(NVARCHAR,s.Doc_No) as Invoice_No,s.Gst_No as PartyGSTNo,"
            //+ "Customer_Code as PartyCode,s.CustomerName as PartyName,s.CustomerStateCode as PartyStateCode,CONVERT(varchar(10),"
            //+ "s.Date,103) as Invoice_Date,s.Amount as TaxableAmount,s.CGSTAmount as CGST,s.SGSTAmount as "
            //+ "SGST,s.IGSTAmount as IGST,s.Final_Amount from NT_1_qryRentBill s  where s.Doc_Date>='01-07-2017' and s.Doc_Date between '" + fromdt + "' and '" + todt +
            //"' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by s.Date";

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,'RB'+CONVERT(NVARCHAR,s.Doc_No) as Invoice_No,s.Gst_No as PartyGSTNo," +
                        " Customer_Code as PartyCode,s.Ac_Name_E as PartyName,s.GSTStateCode as PartyStateCode,CONVERT(varchar(10),s.Date,103) as Invoice_Date," +
                        "s.Subtotal as TaxableAmount,s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Final_Amount,s.ackno as ACKNo from qryrentbillhead s " +
                        "where s.Date>='01-07-2017' and s.Date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + " and s.IsDeleted!=0 and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by s.Date";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Service Bill Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    //double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Final_Amount)", string.Empty));

                    //  grdAll.FooterRow.Cells[8].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[7].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[8].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
}