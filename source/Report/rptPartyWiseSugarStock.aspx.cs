using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptPartyWiseSugarStock : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        if (!Page.IsPostBack)
        {
            BindList();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "khs", "javascript:pst();", true);
    }

    private void BindList()
    {
        try
        {
            qry = "select d.Buyer as PartyCode,a.Ac_Name_E as PartyName from qrysugarBalancestock d left outer join " + tblPrefix + "AccountMaster a on d.Buyer=a.Ac_Code" +
                " and d.Company_Code=a.Company_Code where d.Buyer_Quantal!=0 and d.balance!=0 and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " group by d.Buyer,a.Ac_Name_E order by a.Ac_Name_E asc";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    lblTransportName.Text = "Balance Stock As On <b>" + System.DateTime.Now.ToString("dd/MM/yyyy hh:MM:ss tt") + "</b>";
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
                else
                {
                    dtlist.DataSource = null;
                    dtlist.DataBind();
                }
            }
            else
            {
                dtlist.DataSource = null;
                dtlist.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblPartyCode = (Label)e.Item.FindControl("lblPartyCode");
            Label lblDispTotal = (Label)e.Item.FindControl("lblDispTotal");
            Label lblBalTotal = (Label)e.Item.FindControl("lblBalTotal");
            string partycode = lblPartyCode.Text;

            qry = "select t.Tender_No,t.ID,t.millname as MillName,t.Grade as Grade,t.Mill_Rate as MR,t.Sale_Rate as SR,t.Lifting_Date as LD,a.Short_Name as TDO,t.Buyer_Quantal as Qntl,isnull(SUM(d.quantal),0) as Disp," +
                  " t.Buyer_Quantal-isnull(SUM(d.quantal),0) as Balance from " + tblPrefix + "qryTenderList t LEFT OUTER JOIN " + tblPrefix + "qryDeliveryOrderDO d on t.Tender_No=d.purc_no" +
                  " and t.ID=d.purc_order and t.Company_Code=d.company_code and t.Year_Code=d.Year_Code LEFT OUTER JOIN " + tblPrefix + "AccountMaster a on t.Tender_DO=a.Ac_Code and t.Company_Code=a.Company_Code where t.Buyer=" + partycode + " and t.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and t.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and t.Buyer_Quantal!=0" +
                  " GROUP BY t.Tender_No,t.ID,t.millname,t.Grade,t.Mill_Rate,t.Sale_Rate,t.Lifting_Date,a.Short_Name,t.Buyer_Quantal order by t.millname asc";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("MillName", typeof(string)));
                dt.Columns.Add(new DataColumn("Grade", typeof(string)));
                dt.Columns.Add(new DataColumn("MR", typeof(string)));
                dt.Columns.Add(new DataColumn("SR", typeof(string)));
                dt.Columns.Add(new DataColumn("LD", typeof(string)));
                dt.Columns.Add(new DataColumn("TDO", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("Disp", typeof(double)));
                dt.Columns.Add(new DataColumn("Balance", typeof(double)));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["MillName"] = ds.Tables[0].Rows[i]["MillName"].ToString();
                    dr["Grade"] = ds.Tables[0].Rows[i]["Grade"].ToString();
                    dr["MR"] = ds.Tables[0].Rows[i]["MR"].ToString();
                    dr["SR"] = ds.Tables[0].Rows[i]["SR"].ToString();
                    dr["LD"] = ds.Tables[0].Rows[i]["LD"].ToString();
                    dr["TDO"] = ds.Tables[0].Rows[i]["TDO"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["Disp"] = ds.Tables[0].Rows[i]["Disp"].ToString();
                    double Balance = Convert.ToDouble(ds.Tables[0].Rows[i]["Balance"].ToString());
                    dr["Balance"] = Balance;
                    if (Balance != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    lblQntlGrandTotal.Text = clsCommon.getString("select SUM(Buyer_Quantal) from " + tblPrefix + "qryTenderList where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    lblBalTotal.Text = Convert.ToString(dt.Compute("SUM(Balance)", string.Empty));
                    lblDispTotal.Text = Convert.ToString(dt.Compute("SUM(Disp)", string.Empty));
                    dtlDetails.DataSource = dt;
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