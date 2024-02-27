using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;

public partial class Reports_rptpanrlToPdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Bindata();
        }
    }

    private void Bindata()
    {
        try
        {
            DataSet ds = new DataSet();
            string qry = "select Tender_No,Tender_Date,millname,Grade,Quantal,Mill_Rate,Purc_Rate,Lifting_Date,doname,sum(balance) as bal,sum(despatchqty) as desp from qrysugarBalancestock where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                "  group by Tender_No,Tender_Date,millname,Grade,Quantal,Mill_Rate,Purc_Rate,Lifting_Date,doname order by millname  ";
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataList1.DataSource = ds;
                DataList1.DataBind();

               DataList2.DataSource = ds;
                DataList2.DataBind();
            }

        }
        catch
        {

        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataList dtl = (DataList)e.Item.FindControl("dtl");
        Label lblTenderNo = (Label)e.Item.FindControl("Label1");
        DataSet ds = new DataSet();
        string qry = "select * from qrysugarBalancestock where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + lblTenderNo.Text + " and balance!=0 order by ID";
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dtl.DataSource = ds;
            dtl.DataBind();

            
        }
    }

    protected void btnMail_Click(object sender, EventArgs e)
    {
        
       // File.WriteAllText(path, Document.Body.Parent.OuterHtml, Encoding.GetEncoding(browser.Document.Encoding));
        
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=sugarBS.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        pnl.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }

    public string path { get; set; }
}