using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptPartyUnitDetails : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        if (!IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            qry = "Select Distinct(u.Ac_Code) as PartyCode,a.Ac_Name_E as PartyName,a.Address_E As PartyAddress,a.Person_Mobile as PartyMobile from " + tblPrefix + "PartyUnit u LEFT OUTER JOIN  " + tblPrefix + "qryAccountsList a ON" +
                    " u.Ac_Code=a.Ac_Code and a.Company_Code=u.Company_Code where u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            //qry = "Select Ac_Code as PartyCode,Ac_Name_E as PartyName,Address_E As PartyAddress,Mobile_No as PartyMobile from " + tblPrefix + "qryAccountsList where Ac_Code=" + Party_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
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
    protected void dtlist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
        Label lblPartyCode = (Label)e.Item.FindControl("lblPartyCode");
        string party = lblPartyCode.Text.ToString();

        //string Unitcode = clsCommon.getString("Select Unit_name from " + tblPrefix + "PartyUnit where Ac_Code=" + party + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        qry = "Select u.Unit_name as UnitCode,a.Ac_Name_E as UnitName,a.Address_E As UnitAddress,a.Person_Mobile as UnitMobile from " + tblPrefix + "PartyUnit u LEFT OUTER JOIN  " + tblPrefix + "qryAccountsList a ON" +
                    " u.Unit_name=a.Ac_Code and a.Company_Code=u.Company_Code where u.Ac_Code=" + party + " and u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
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