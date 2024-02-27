using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptPartyWiseDO : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string fromDt = string.Empty;
    string toDt = string.Empty;
    string Branch_Code = string.Empty;
    string ac_code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDt = Request.QueryString["fromDT"];
        toDt = Request.QueryString["toDT"];
        Branch_Code = Request.QueryString["Branch_Code"];
        ac_code = Request.QueryString["ac_code"];
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            if (Branch_Code == string.Empty)
            {
                if (ac_code == string.Empty)
                {
                    qry = "select DISTINCT(d.voucher_by) as code,a.Ac_Name_E as getpass from " + tblPrefix + "deliveryorder d left outer join " +
                        " " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code where d.tran_type='DO' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' order by a.Ac_Name_E";
                }
                else
                {
                    qry = "select DISTINCT(d.voucher_by) as code,a.Ac_Name_E as getpass from " + tblPrefix + "deliveryorder d left outer join " +
                        " " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code where d.tran_type='DO' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.voucher_by=" + ac_code + " order by a.Ac_Name_E";
                }

            }
            else
            {
                if (ac_code == string.Empty)
                {
                    qry = "select DISTINCT(d.voucher_by) as code,a.Ac_Name_E as getpass from " + tblPrefix + "deliveryorder d left outer join " +
                                       " " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code where d.tran_type='DO' and  d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' order by a.Ac_Name_E";
                }
                else
                {
                    qry = "select DISTINCT(d.voucher_by) as code,a.Ac_Name_E as getpass from " + tblPrefix + "deliveryorder d left outer join " +
                                       " " + tblPrefix + "AccountMaster a on d.voucher_by=a.Ac_Code and d.company_code=a.Company_Code where d.tran_type='DO' and  d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.voucher_by=" + ac_code + " order by a.Ac_Name_E";
                }
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string from = DateTime.Parse(fromDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    string to = DateTime.Parse(toDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    lblTransportName.Text = "Dispatch Register For the Period <b>" + from + "</b> To <b>" + to + "</b>";
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
            Label lblQntlTotal = (Label)e.Item.FindControl("lblQntlTotal");
            string getpass = lblPartyCode.Text;

            if (Branch_Code == string.Empty)
            {
                qry = "select d.doc_no as #,Convert(varchar(10),d.doc_date,103) as dt,a.Short_Name as mill,d.grade,d.quantal As Qntl,d.mill_rate as MR,d.truck_no as lorry,d.sale_rate as SR,b.Short_Name AS DispTo,d.voucher_no as VN,d.voucher_type as VT,d.SB_No as SB " +
                    " from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code left outer join " + tblPrefix + "AccountMaster b on d.GETPASSCODE=b.Ac_Code and " +
                    " d.company_code=b.Company_Code where d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.voucher_by=" + getpass + " and d.tran_type='DO' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            else
            {
                qry = "select d.doc_no as #,Convert(varchar(10),d.doc_date,103) as dt,a.Short_Name as mill,d.grade as Grade,d.quantal As Qntl,d.mill_rate as MR,d.truck_no as lorry,d.sale_rate as SR,b.Short_Name AS DispTo,d.voucher_no as VN,d.voucher_type as VT,d.SB_No as SB " +
                    " from " + tblPrefix + "deliveryorder d left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code left outer join " + tblPrefix + "AccountMaster b on d.GETPASSCODE=b.Ac_Code and" +
                    " d.company_code=b.Company_Code where d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.voucher_by=" + getpass + " and d.tran_type='DO' and d.Branch_Code=" + Branch_Code + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("dt", typeof(string)));
                dt.Columns.Add(new DataColumn("mill", typeof(string)));
                dt.Columns.Add(new DataColumn("Grade", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("MR", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("SR", typeof(string)));
                dt.Columns.Add(new DataColumn("DispTo", typeof(string)));
                dt.Columns.Add(new DataColumn("OV", typeof(string)));
                dt.Columns.Add(new DataColumn("SB", typeof(string)));
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                string VT = ds.Tables[0].Rows[i]["VT"].ToString();
                if (VT == "OV")
                {
                    dr["OV"] = ds.Tables[0].Rows[i]["VN"].ToString();
                }
                else
                {
                    dr["OV"] = "";
                }
                dr["SB"] = ds.Tables[0].Rows[i]["SB"].ToString();
                dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                dr["dt"] = ds.Tables[0].Rows[i]["dt"].ToString();
                dr["mill"] = ds.Tables[0].Rows[i]["mill"].ToString();
                dr["Grade"] = ds.Tables[0].Rows[i]["Grade"].ToString();
                dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                dr["MR"] = ds.Tables[0].Rows[i]["MR"].ToString();
                dr["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                dr["SR"] = ds.Tables[0].Rows[i]["SR"].ToString();
                dr["DispTo"] = ds.Tables[0].Rows[i]["DispTo"].ToString();
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                dtlDetails.DataSource = dt;
                dtlDetails.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void lnkOV_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkOV = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkOV.NamingContainer;
            string no = lnkOV.Text;
            Session["VOUC_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:loadingvoucher();", true);
            lnkOV.Focus();
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void lnkSB_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSB = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkSB.NamingContainer;
            string no = lnkSB.Text;
            Session["SB_NO"] = no;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsdsd", "javascript:salebill();", true);
            lnkSB.Focus();
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