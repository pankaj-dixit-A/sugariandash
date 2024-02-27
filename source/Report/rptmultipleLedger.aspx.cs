using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptmultipleLedger : System.Web.UI.Page
{
    #region data section

    double netdebit = 0.00; double netcredit = 0.00;
    string prefix = string.Empty;
    string tblPrefix = string.Empty;
    string tblGLEDGER = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string TranTyp = string.Empty;
    int defaultAccountCode = 0;
    int tempcounter = 0;
    string fromdt = string.Empty;
    string todt = string.Empty;

    static WebControl objAsp = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["tblPrefix"] != null)
            {
                tblPrefix = Session["tblPrefix"].ToString();
            }
            else
            {
                prefix = clsCommon.getString("Select tblPrefix from tblPrefix");
                tblPrefix = prefix.ToString();
            }
            tblGLEDGER = tblPrefix + "GLEDGER";
            tblDetails = tblPrefix + "VoucherDetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            bindData();

        }
    }

    private void bindData()
    {
        try
        {
            string groupcode = Request.QueryString["groupcode"];
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (groupcode != string.Empty)
            {
                qry = "select [Ac_Code],[Ac_Name_E] from " + AccountMasterTable + " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Group_Code]=" + groupcode;
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            dtl_Group.DataSource = dt;
                            dtl_Group.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Response.Write(ee.Message);
        }
    }

    protected void dtl_Group_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            string fromdt = Request.QueryString["fromdt"];
            string todt = Request.QueryString["todt"];
            Label lblCompany = (Label)e.Item.FindControl("lblCompany");
            Label lblFromDt = (Label)e.Item.FindControl("lblFromDt");
            Label lblToDt = (Label)e.Item.FindControl("lblToDt");
            Label lblPartyCode = (Label)e.Item.FindControl("lblPartyCode");
            lblFromDt.Text = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            lblToDt.Text = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            lblCompany.Text = Session["Company_Name"].ToString();
            GridView grdDetail = (GridView)e.Item.FindControl("grdDetail");
            this.bindGrid(lblPartyCode.Text, grdDetail);
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

    private void bindGrid(string partycode, GridView grdDetail)
    {
        try
        {
            string fromdt = Request.QueryString["fromdt"];
            string todt = Request.QueryString["todt"];
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //   string fromDate = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            //  string toDate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string qry = "select AC_CODE,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as  OpBal from " + tblGLEDGER + " where DOC_DATE < '" + fromdt + "' and Ac_code=" + partycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " group by AC_CODE ";
            ds = clsDAL.SimpleQuery(qry);
            double opBal = 0.0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        opBal = Convert.ToDouble(dt.Rows[0][1].ToString());
                    }
                }
            }


            qry = "select TRAN_TYPE,DOC_NO,Convert(varchar(10),DOC_DATE,102) as DOC_DATE ,NARRATION,AMOUNT,ADJUSTED_AMOUNT,AC_CODE,DRCR from " + tblGLEDGER +
        " where AC_CODE=" + partycode + " and DOC_DATE between '" + fromdt + "' and '" + todt + "' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE asc";
            ds = clsDAL.SimpleQuery(qry);


            DataTable dtT = new DataTable();
            //   dtT = null;
            dtT.Columns.Add("TranType", typeof(string));
            dtT.Columns.Add("DocNo", typeof(Int32));
            dtT.Columns.Add("Date", typeof(string));
            dtT.Columns.Add("Narration", typeof(string));
            dtT.Columns.Add("Debit", typeof(double));
            dtT.Columns.Add("Credit", typeof(double));
            dtT.Columns.Add("Balance", typeof(double));
            dtT.Columns.Add("DrCr", typeof(string));

            //if (dt.Rows.Count > 0)
            //{
            dt = ds.Tables[0];

            DataRow dr = dtT.NewRow();
            //  old dr[0] = dt.Rows[0]["TRAN_TYPE"].ToString();
            dr[0] = "OP";
            dr[1] = 0.00;
            dr[2] = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            dr[3] = "Opening Balance";
            if (opBal > 0)
            {
                dr[4] = Math.Round(opBal, 2);
                dr[5] = 0.00;
                dr[6] = Math.Round(opBal, 2);
                dr[7] = "Dr";
                netdebit += opBal;
            }
            else
            {
                dr[4] = 0.00;
                dr[5] = Math.Round(-opBal, 2);
                dr[6] = dr[5];
                dr[7] = "Cr";
                netcredit += -opBal;
            }
            dtT.Rows.Add(dr);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dtT.NewRow();

                    dr[0] = dt.Rows[i]["TRAN_TYPE"].ToString();
                    dr[1] = dt.Rows[i]["DOC_NO"].ToString();
                    if (dt.Rows[i]["DOC_DATE"].ToString() != string.Empty)
                    {
                        string s = dt.Rows[i]["DOC_DATE"].ToString();

                        dr[2] = DateTime.Parse(s, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    }
                    dr[3] = dt.Rows[i]["NARRATION"].ToString();

                    if (dt.Rows[i]["DRCR"].ToString() == "D")
                    {
                        opBal = opBal + Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());

                        dr[4] = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString()));
                        dr[5] = 0.00;
                        netdebit += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }
                    else
                    {
                        opBal = opBal - Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                        netcredit += Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());

                        dr[4] = 0.00;
                        dr[5] = Convert.ToDouble(dt.Rows[i]["AMOUNT"].ToString());
                    }

                    if (opBal > 0)
                    {
                        dr[6] = Math.Round(Convert.ToDouble(opBal), 2);
                        dr[7] = "Dr";
                    }
                    else
                    {
                        dr[6] = 0 - Math.Round(opBal, 2);
                        dr[7] = "Cr";
                    }
                    dtT.Rows.Add(dr);

                }
            }




            grdDetail.DataSource = dtT;
            grdDetail.DataBind();
            grdDetail.FooterRow.Cells[3].Text = "Total";
            grdDetail.FooterRow.Cells[4].Text = netdebit.ToString();
            grdDetail.FooterRow.Cells[5].Text = netcredit.ToString();
            if (netdebit - netcredit != 0)
            {
                grdDetail.FooterRow.Cells[6].Text = Math.Round((netdebit - netcredit), 2).ToString();
            }
            else
            {
                grdDetail.FooterRow.Cells[6].Text = "Nil";

            }
            grdDetail.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            grdDetail.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            grdDetail.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
        }
        catch (Exception exx)
        {
            Response.Write("grid binding exception:" + exx.Message);
        }
    }


    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            tempcounter = tempcounter + 1;
            if (tempcounter == 10)
            {
                e.Row.Attributes.Add("style", "page-break-after: always;");
                tempcounter = 0;
            }

            if (e.Row.Cells[4].Text == "0")
            {
                e.Row.Cells[4].Text = "";
            }
            if (e.Row.Cells[5].Text == "0")
            {
                e.Row.Cells[5].Text = "";
            }

            if (e.Row.Cells[6].Text == "0")
            {
                e.Row.Cells[6].Text = "Nil";
            }
        }
    }
}