using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_CheckingUnitility : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtErrorDO = new DataTable();
            dtErrorDO.Columns.Add(new DataColumn("doc_no", typeof(string)));
            dtErrorDO.Columns.Add(new DataColumn("voucher_no", typeof(string)));
            dtErrorDO.Columns.Add(new DataColumn("voucher_type", typeof(string)));
            dtErrorDO.Columns.Add(new DataColumn("SB_No", typeof(string)));

            string qry = "Select doc_no,quantal,ISNULL(voucher_no,0) as voucher_no,voucher_type,ISNULL(SB_No,0) as SB_No,quantal,GETPASSCODE from NT_1_deliveryorder where tran_type='DO' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    bool isError = false;
                    int doc_no = Convert.ToInt32(ds.Tables[0].Rows[i]["doc_no"].ToString());
                    int voucher_no = ds.Tables[0].Rows[i]["voucher_no"].ToString() != string.Empty ? Convert.ToInt32(ds.Tables[0].Rows[i]["voucher_no"].ToString()) : 0;
                    string voucher_type = ds.Tables[0].Rows[i]["voucher_type"].ToString();
                    string SB_No = ds.Tables[0].Rows[i]["SB_No"].ToString();
                    double doquintal = Convert.ToDouble(ds.Tables[0].Rows[i]["quantal"].ToString());
                    int doparty = Convert.ToInt32(ds.Tables[0].Rows[i]["GETPASSCODE"].ToString());

                    if (voucher_no != 0)
                    {
                        if (voucher_type == "PS")
                        {
                            string ps = clsCommon.getString("Select ISNULL(doc_no,0) from NT_1_SugarPurchase where PURCNO=" + doc_no + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                            if (ps.Trim() == string.Empty)
                            {
                                isError = true;
                            }
                        }
                        else if (voucher_type == "OV")
                        {
                            string ov = clsCommon.getString("Select ISNULL(Doc_No,0) from NT_1_Voucher where DO_No=" + doc_no + " and  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='OV'");
                            if (ov.Trim() == string.Empty)
                            {
                                isError = true;
                            }
                            else
                            {
                                int party = Convert.ToInt32(clsCommon.getString("Select ISNULL(Doc_No,0) from NT_1_Voucher where DO_No=" + doc_no + " and  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='OV'"));
                                double qntl = Convert.ToDouble(clsCommon.getString("Select Quantal from NT_1_Voucher where DO_No=" + doc_no + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='OV'"));
                                if (doparty == party && doquintal == qntl)
                                {
                                    isError = true;
                                }
                            }
                        }
                        else if (voucher_type == "LV")
                        {
                            string Lv = clsCommon.getString("Select ISNULL(Doc_No,0) from NT_1_Voucher where DO_No=" + doc_no + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='LV'");
                            if (Lv.Trim() == string.Empty)
                            {
                                isError = true;
                            }
                        }

                        if (SB_No != "0" && SB_No != string.Empty)
                        {
                            string sb = clsCommon.getString("select doc_no from NT_1_SugarSale where DO_No=" + doc_no + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                            if (sb.Trim() == string.Empty)
                            {
                                isError = true;
                            }
                        }
                    }

                    if (isError == true)
                    {
                        DataRow dr = dtErrorDO.NewRow();
                        dr["doc_no"] = doc_no;
                        dr["voucher_no"] = voucher_no;
                        dr["voucher_type"] = voucher_type;
                        dr["SB_No"] = SB_No;
                        dtErrorDO.Rows.Add(dr);
                    }
                }

                if (dtErrorDO.Rows.Count > 0)
                {
                    grdRecords.DataSource = dtErrorDO;
                    grdRecords.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}