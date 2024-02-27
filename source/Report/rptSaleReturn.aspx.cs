using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Net.Mail;

public partial class Report_rptSaleReturn : System.Web.UI.Page
{
    string f = "../GSReports/SaleReturn_.htm";
    string f_Main = "../Report/rptsaleReturn";
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string email = string.Empty;
    string billno = string.Empty;
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        billno = Request.QueryString["billno"];
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        cityMasterTable = tblPrefix + "CityMaster";
        qryCommon = tblPrefix + "qrySugarPurcListReturn";
        if (!Page.IsPostBack)
        {
            BindList();
        }
    }
    private void BindList()
    {
        try
        {
            if (billno != string.Empty)
            {
                qry = "select s.doc_no as #,CONVERT(VARCHAR(10),s.doc_date,103) as dt,s.FROM_STATION as From_Place,a.Pincode as Party_Pin,s.TO_STATION as To_Place,s.LORRYNO as lorry,s.wearhouse,s.subTotal as Sub_Total,ISNULL(s.freight,0.00) as Less_Frieght," +
                    " ISNULL(s.cash_advance,0.00) as Cash_Advance,ISNULL(s.bank_commission,0.00) as Bank_Commission,ISNULL(s.OTHER_AMT,0.00) as Other_Expenses,s.Bill_Amount as Bill_Amount,a.Ac_Name_E as Party_Name,a.Address_E as Party_Address," +
                    " a.Local_Lic_No as Party_SLN,a.Tin_No as Party_TIN,a.ECC_No as Party_Ecc,a.Cst_no as Party_Cst,a.Gst_No as Party_Gst,c.city_name_e as Party_City,c.state as Party_State,b.Ac_Name_E as Mill_Name,a.Email_Id from " + tblPrefix + "SugarPurchaseReturn s" +
                    " left outer join " + tblPrefix + "AccountMaster a on s.Ac_Code=a.Ac_Code and s.Company_Code=a.Company_Code left outer join " + tblPrefix + "AccountMaster b on s.mill_code=b.Ac_Code and s.Company_Code=b.Company_Code" +
                    " left outer join " + tblPrefix + "CityMaster c on a.City_Code=c.city_code and a.Company_Code=c.company_code where s.doc_no IN(" + billno + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_TIN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_TIN"] = "&nbsp;&nbsp;TIN: " + ds.Tables[0].Rows[i]["Party_TIN"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Ecc"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Ecc"] = "&nbsp;&nbsp;ECC: " + ds.Tables[0].Rows[i]["Party_Ecc"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Cst"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Cst"] = "&nbsp;&nbsp;CST: " + ds.Tables[0].Rows[i]["Party_Cst"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_Gst"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_Gst"] = "&nbsp;&nbsp;GST: " + ds.Tables[0].Rows[i]["Party_Gst"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_SLN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_SLN"] = "&nbsp;&nbsp;Sugar Lic No: " + ds.Tables[0].Rows[i]["Party_SLN"].ToString();
                        }
                    }

                    dt = new DataTable();
                    dt = ds.Tables[0];
                    DataTable dt1 = new DataTable();
                    dt1 = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtEmail.Text = dt.Rows[0]["Email_Id"].ToString();
                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                        dtlist1.DataSource = dt1;
                        dtlist1.DataBind();
                    }
                }
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
            Label lblSB_No = (Label)e.Item.FindControl("lblSB_No");
            Label lblBillAmount = (Label)e.Item.FindControl("lblBillAmount");
            Label lblCityStatePin = (Label)e.Item.FindControl("lblCityStatePin");
            Label lblCmpMobile = (Label)e.Item.FindControl("lblCmpMobile");
            Label lblCompanyName = (Label)e.Item.FindControl("lblCompanyName");
            Label lblCompanyAddress = (Label)e.Item.FindControl("lblCompanyAddress");
            Label lblInwords = (Label)e.Item.FindControl("lblInwords");
            Label lblNameCmp = (Label)e.Item.FindControl("lblNameCmp");
            DataList dtItemDetails = (DataList)e.Item.FindControl("dtItemDetails");
            string sbNo = lblSB_No.Text;
            lblInwords.Text = clsNoToWord.ctgword(lblBillAmount.Text);


            string city = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string Pin = clsCommon.getString("Select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("Select State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            lblCityStatePin.Text = city + " (" + Pin + ") " + state;
            lblCmpMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            lblCompanyAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            qry = " select s.System_Name_E+' '+ISNULL(d.narration,'') as Item  ,d.bags as Bags,d.packing as Packing,d.Quantal as Qntl,d.rate as Rate,d.item_Amount as Value from " + tblPrefix + "sugarsaleDetails d" +
                " left outer join " + tblPrefix + "SystemMaster s on d.item_code=s.System_Code and d.Company_Code=s.Company_Code and s.System_Type='I' where d.doc_no=" + sbNo + " and d.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by d.detail_id";

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblNameCmp.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    dtItemDetails.DataSource = dt;
                    dtItemDetails.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            email = txtEmail.Text.ToString();
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            pnlMain.RenderControl(tw);
            string s = sw.ToString();
            try
            {
                using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(s);
                    }
                }
            }
            catch (Exception ee)
            {
                f = f_Main + ".htm";
                using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(s);
                    }
                }
            }
            string mailFrom = Session["EmailId"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["EmailPassword"].ToString();
            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Host = clsGV.Email_Address;
            msg.From = new MailAddress(mailFrom);
            msg.To.Add(email);
            msg.Body = "Sales Return ";
            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            msg.Subject = "Sales Return" + DateTime.Now.ToString("dd/MM/yyyy");
            msg.IsBodyHtml = true;
            msg.IsBodyHtml = true;
            if (smtpPort != string.Empty)
            {
                SmtpServer.Port = Convert.ToInt32(smtpPort);
            }
                                 SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            SmtpServer.Send(msg);
        }
        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }
}