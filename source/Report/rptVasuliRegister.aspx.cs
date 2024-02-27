using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptVasuliRegister : System.Web.UI.Page
{
    string fromDate = string.Empty;
    string toDate = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDate = Request.QueryString["FromDT"].ToString();
        toDate = Request.QueryString["ToDt"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindList();
        }
    }

    private void BindList()
    {
        if (string.IsNullOrEmpty(Branch_Code))
        {
            qry = "Select distinct(d.transport),a.Ac_Name_E from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=d.transport where tran_type='MM' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDate + "' and '" + toDate + "' order by a.Ac_Name_E asc";
        }
        else
        {
            qry = "Select distinct(d.transport),a.Ac_Name_E from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=d.transport where tran_type='MM' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.doc_date between '" + fromDate + "' and '" + toDate + "' order by a.Ac_Name_E asc";
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Transport_Code", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Transport_Name", typeof(string)));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string transcode = ds.Tables[0].Rows[i]["transport"].ToString();
                    dr["transport_Code"] = transcode;
                    string transport_Name = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_code=" + transcode + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='T'");
                    dr["Transport_Name"] = transport_Name;
                    dt.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(Branch_Code))
                    {
                        lblTotalVasuli.Text = clsCommon.getString("select SUM(vasuli_amount) from " + tblPrefix + "deliveryorder where tran_type='MM' and transport!=0 and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDate + "' and '" + toDate + "'");
                    }
                    else
                    {
                        lblTotalVasuli.Text = clsCommon.getString("select SUM(vasuli_amount) from " + tblPrefix + "deliveryorder where tran_type='MM' and transport!=0 and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " and doc_date between '" + fromDate + "' and '" + toDate + "'");
                    }

                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
                else
                {

                    dtl.DataSource = null;
                    dtl.DataBind();
                }
            }
        }
    }

    protected void DataList_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
        Label lblTransportcode = (Label)e.Item.FindControl("lblTrasportCode");
        string trans_code = lblTransportcode.Text.ToString();

        if (trans_code != string.Empty)
        {
            if (string.IsNullOrEmpty(Branch_Code))
            {
                qry = "Select doc_no,Convert(varchar(10),doc_date,103) as doc_date,millShortName,quantal,PartyName,truck_no,vasuli_amount from " + tblPrefix + "qryDeliveryOrderListReport" +
                      " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='MM' and transport=" + trans_code + " and doc_date between '" + fromDate + "' and '" + toDate + "'";
            }
            else
            {
                qry = "Select doc_no,Convert(varchar(10),doc_date,103) as doc_date,millShortName,quantal,PartyName,truck_no,vasuli_amount from " + tblPrefix + "qryDeliveryOrderListReport" +
                                    " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " and tran_type='MM' and transport=" + trans_code + " and doc_date between '" + fromDate + "' and '" + toDate + "'";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("doc_no", typeof(string)));
                dt.Columns.Add(new DataColumn("doc_date", typeof(string)));
                dt.Columns.Add(new DataColumn("millShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("quantal", typeof(double)));
                dt.Columns.Add(new DataColumn("PartyName", typeof(string)));
                dt.Columns.Add(new DataColumn("truck_no", typeof(string)));
                dt.Columns.Add(new DataColumn("vasuli_amount", typeof(double)));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["doc_no"] = ds.Tables[0].Rows[i]["doc_no"].ToString();
                        dr["doc_date"] = ds.Tables[0].Rows[i]["doc_date"].ToString();
                        dr["millShortName"] = ds.Tables[0].Rows[i]["millShortName"].ToString();
                        dr["quantal"] = ds.Tables[0].Rows[i]["quantal"].ToString();
                        dr["PartyName"] = ds.Tables[0].Rows[i]["PartyName"].ToString();
                        dr["truck_no"] = ds.Tables[0].Rows[i]["truck_no"].ToString();
                        dr["vasuli_amount"] = ds.Tables[0].Rows[i]["vasuli_amount"].ToString();
                        dt.Rows.Add(dr);
                    }
                    Label lblVasuliTotal = (Label)e.Item.FindControl("lblVasuliTotal");

                    if (dt.Rows.Count > 0)
                    {
                        lblVasuliTotal.Text = Convert.ToString(dt.Compute("SUM(vasuli_amount)", string.Empty));
                        dtlDetails.DataSource = dt;
                        dtlDetails.DataBind();
                    }
                    else
                    {
                        dtlDetails.DataSource = null;
                        dtlDetails.DataBind();
                    }
                }
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