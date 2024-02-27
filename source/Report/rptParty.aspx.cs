using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptParty : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Party_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Party_Code = Request.QueryString["Party_Code"];
        if (!IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            if (!string.IsNullOrEmpty(Party_Code))
            {
                //qry = "Select Distinc(Ac_Code) from " + tblPrefix + "PartyUnit where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                qry = "Select Ac_Code as PartyCode,Ac_Name_E as PartyName,Address_E As PartyAddress,Mobile_No as PartyMobile from " + tblPrefix + "qryAccountsList where Ac_Code=" + Party_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        qry = "Select u.Unit_name as UnitCode,a.Ac_Name_E as UnitName,a.Address_E As UnitAddress,a.Mobile_No as UnitMobile from " + tblPrefix + "PartyUnit u LEFT OUTER JOIN  " + tblPrefix + "qryAccountsList a ON" +
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
}