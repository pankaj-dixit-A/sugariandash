using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptAcList : System.Web.UI.Page
{
    string accountMasterList = string.Empty;
    string tblPrefix = string.Empty;
    string cityMasterTable = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            tblPrefix = Session["tblPrefix"].ToString();
            accountMasterList = tblPrefix + "qryAccountsList";
            cityMasterTable = tblPrefix + "CityMaster";
            ViewState["cityCode"] = Request.QueryString["cityCode"];
            this.bindData();
        }
    }

    private void bindData()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            if (ViewState["cityCode"] != null)
            {
                string cityCode = ViewState["cityCode"].ToString();

                lblcityName.Text = clsCommon.getString("select [city_name_e] from " + cityMasterTable + " where [city_code]=" + cityCode + " and [company_code]=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string qry = "";
                qry = "select [Ac_Code],[Ac_Name_E] from " + accountMasterList + " where [City_Code]=" + cityCode + " and [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            dtlAcList.DataSource = dt;
                            dtlAcList.DataBind();
                        }
                    }
                }

            }
        }
        catch (Exception eec)
        {
            Response.Write(eec.Message);
        }
    }
}