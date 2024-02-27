using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_rptTransportAdvanceBalDetailsNew : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string Branch_Code = string.Empty;
    string uptodt = string.Empty;
    string TransPort = string.Empty;
    double AllQntlTotal = 0.00;
    double AllAdvanceTotal = 0.00;
    double AllPaidTotal = 0.00;
    double AllBalTotal = 0.00;
    double AllFrtPaidTotal = 0.00;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        TransPort = Request.QueryString["Ac_Code"];

        if (!Page.IsPostBack)
        {
            BindList();
        }
    }

    private void BindList()
    {
        try
        {
            string txt = "All";
            if (TransPort == string.Empty)
            {
                if (Branch_Code == string.Empty)
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by TransportName asc ";
                }
                else
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " order by TransportName asc";
                }
            }
            else
            {
                txt = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + TransPort + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (Branch_Code == string.Empty)
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.transport=" + TransPort + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by TransportName asc ";
                }
                else
                {
                    qry = "select distinct(d.transport) as TransportCode,d.TransportName as TransportName  from " + tblPrefix + "qryDeliveryOrderListReport d where d.tran_type='DO' and d.transport!=0 and d.transport=" + TransPort + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + " order by TransportName asc";
                }
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //dt = new DataTable();
                //ds.Tables[0].Columns.Add("hasBal", typeof(string));
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    string TransportCode = ds.Tables[0].Rows[i]["TransportCode"].ToString();
                //    string qry3 = "select doc_no as DO,tran_type,millShortName,PartyShortName,GetpassShortName,quantal as Qntl,Memo_Advance as Advance,vasuli_amount as FrtPaid,vasuli_amount as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA" +
                //      " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance " +
                //      " from " + tblPrefix + "qryDeliveryOrderListReport as do where tran_type='DO' and transport=" + TransportCode + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";

                //    DataSet ds3 = new DataSet();
                //    ds3 = clsDAL.SimpleQuery(qry3);
                //    if (ds3.Tables[0].Rows.Count > 0)
                //    {
                //        DataTable dt3 = new DataTable();
                //        dt3 = ds3.Tables[0];

                //        double bal = Convert.ToDouble(dt3.Compute("SUM(Balance)", string.Empty));
                //        if (bal == 0)
                //        {
                //            ds.Tables[0].Rows[i]["hasBal"] = "1";
                //        }
                //    }
                //}
                dt = ds.Tables[0];
                //DataTable dtset = dt.Clone();
                //foreach (DataRow row in dt.Rows)
                //{
                //    string hasBal = row["hasBal"].ToString();
                //    if (hasBal != "1")
                //    {
                //        dtset.Rows.Add(row.ItemArray);
                //    }
                //}
                if (dt.Rows.Count > 0)
                {
                    lblCmpName.Text = Session["Company_Name"].ToString();
                    lblTransportName.Text = "Transport Advance Report of " + "<b>" + txt + "</b>";
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
            DataList dtlDoDetails = (DataList)e.Item.FindControl("dtlDoDetails");
            Label lblTransportCode = (Label)e.Item.FindControl("lblTransportCode");
            Label lblAllQntlTotal = (Label)e.Item.FindControl("lblAllQntlTotal");
            Label lblAllAdvanceTotal = (Label)e.Item.FindControl("lblAllAdvanceTotal");
            Label lblAllPaidTotal = (Label)e.Item.FindControl("lblAllPaidTotal");
            Label lblAllBalTotal = (Label)e.Item.FindControl("lblAllBalTotal");
            Label lblAllFrieghtPaid = (Label)e.Item.FindControl("lblAllFrieghtPaid");
            string transport = lblTransportCode.Text;

            if (Branch_Code == string.Empty)
            {
                qry = "select doc_no as DO,tran_type,millShortName,PartyShortName,GetpassShortName,quantal as Qntl,Memo_Advance as Advance,vasuli_amount as FrtPaid,vasuli_amount as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA" +
                      " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance " +
                      " from " + tblPrefix + "qryDeliveryOrderListReport as do where tran_type='DO' and MM_CC!='Cash' and transport=" + transport + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
            }
            else
            {
                qry = "select doc_no as DO,tran_type,millShortName,PartyShortName,GetpassShortName,quantal as Qntl,Memo_Advance as Advance,vasuli_amount as FrtPaid,vasuli_amount as Paid,((do.Memo_Advance)-(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA " +
                      " from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance " +
                      " from " + tblPrefix + "qryDeliveryOrderListReport as do where tran_type='DO' and MM_CC!='Cash' and transport=" + transport + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + "";
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("DO", typeof(string)));
                dt.Columns.Add(new DataColumn("tran_type", typeof(string)));
                dt.Columns.Add(new DataColumn("millShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("PartyShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("GetpassShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("Advance", typeof(double)));
                dt.Columns.Add(new DataColumn("FrtPaid", typeof(double)));
                dt.Columns.Add(new DataColumn("Paid", typeof(double)));
                dt.Columns.Add(new DataColumn("Balance", typeof(double)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["DO"] = ds.Tables[0].Rows[i]["DO"].ToString();
                    dr["tran_type"] = ds.Tables[0].Rows[i]["tran_type"].ToString();
                    dr["millShortName"] = ds.Tables[0].Rows[i]["millShortName"].ToString();
                    dr["PartyShortName"] = ds.Tables[0].Rows[i]["PartyShortName"].ToString();
                    dr["GetpassShortName"] = ds.Tables[0].Rows[i]["GetpassShortName"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["Advance"] = ds.Tables[0].Rows[i]["Advance"].ToString();
                    dr["FrtPaid"] = ds.Tables[0].Rows[i]["FrtPaid"].ToString();
                    dr["Paid"] = ds.Tables[0].Rows[i]["Paid"].ToString();
                    double balance = Convert.ToDouble(ds.Tables[0].Rows[i]["Balance"].ToString());
                    dr["Balance"] = balance;
                    //if (balance != 0)
                    //{
                        dt.Rows.Add(dr);
                    //}
                }
                if (dt.Rows.Count > 0)
                {
                    dtlDoDetails.DataSource = dt;
                    dtlDoDetails.DataBind();

                    foreach (DataListItem item in dtlDoDetails.Items)
                    {
                        Label lblQntl = (Label)item.FindControl("lblQntl");
                        double totalQntl = Convert.ToDouble(lblQntl.Text);
                        AllQntlTotal += totalQntl;
                        lblAllQntlTotal.Text = Convert.ToString(AllQntlTotal);

                        Label lblAdvance = (Label)item.FindControl("lblAdvance");
                        double totalAdvance = Convert.ToDouble(lblAdvance.Text);
                        AllAdvanceTotal += totalAdvance;
                        lblAllAdvanceTotal.Text = Convert.ToString(AllAdvanceTotal);

                        Label lblPaidTotal = (Label)item.FindControl("lblPaidTotal");
                        double totalpaid = Convert.ToDouble(lblPaidTotal.Text);
                        AllPaidTotal += totalpaid;
                        lblAllPaidTotal.Text = Convert.ToString(AllPaidTotal);

                        Label lblBalTotal = (Label)item.FindControl("lblBalTotal");
                        double totalBal = Convert.ToDouble(lblBalTotal.Text);
                        AllBalTotal += totalBal;
                        lblAllBalTotal.Text = Convert.ToString(AllBalTotal);

                        Label lblFrieghtPaid = (Label)item.FindControl("lblFrieghtPaid");
                        double totalFrtPaid = Convert.ToDouble(lblFrieghtPaid.Text);
                        AllFrtPaidTotal += totalFrtPaid;
                        lblAllFrieghtPaid.Text = Convert.ToString(AllFrtPaidTotal);
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtlDoDetails_OnItemDataBound(object sender, DataListItemEventArgs e)
    {

        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblDONo = (Label)e.Item.FindControl("lblDONo");
            Label lblTran_Type = (Label)e.Item.FindControl("lblTran_Type");
            Label lblPaidTotal = (Label)e.Item.FindControl("lblPaidTotal");
            double PaidTotal = Convert.ToDouble(lblPaidTotal.Text);
            string DO = lblDONo.Text;
            string tran_type = lblTran_Type.Text;

            qry = "select doc_no as #,Tran_Type as ttype,CONVERT(VARCHAR(10),doc_date,103) as dt,narration,SUM(amount) as Paid from " + tblPrefix + "Transact where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Voucher_No=" + DO + " and Voucher_Type='" + tran_type + "' group by Voucher_No,doc_no,Tran_Type,doc_date,narration";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("ttype", typeof(string)));
                dt.Columns.Add(new DataColumn("dt", typeof(string)));
                dt.Columns.Add(new DataColumn("narration", typeof(string)));
                dt.Columns.Add(new DataColumn("Paid", typeof(double)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["ttype"] = ds.Tables[0].Rows[i]["ttype"].ToString();
                    dr["dt"] = ds.Tables[0].Rows[i]["dt"].ToString();
                    dr["narration"] = ds.Tables[0].Rows[i]["narration"].ToString();
                    dr["Paid"] = ds.Tables[0].Rows[i]["Paid"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    double paid = Convert.ToDouble(dt.Compute("SUM(Paid)", string.Empty));
                    PaidTotal += paid;
                    lblPaidTotal.Text = Convert.ToString(PaidTotal);
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
}