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

public partial class Report_rptLedgerUnitwise : System.Web.UI.Page
{
    #region data section
    string f = "../GSReports/Ledger_.htm";
    string f_Main = "../Report/rptLedger";
    double netdebit = 0.00; double netcredit = 0.00;

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
    double netdebitall = 0.00;
    double netcreditall = 0.00;
    int defaultAccountCode = 0;
    int tempcounter = 0;
    string email = string.Empty;
    static WebControl objAsp = null;
    string prefix = string.Empty;
    string accode = string.Empty;
    string unit_code = string.Empty;
    string fromdt = string.Empty;
    string todt = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        accode = Request.QueryString["accode"];
        unit_code = Request.QueryString["unit_code"];
        fromdt = Request.QueryString["fromdt"];
        todt = Request.QueryString["todt"];
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
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.bindData();
        }
    }
    private void bindData()
    {
        try
        {
            string mail = "";
            if (accode != string.Empty)
            {
                mail = clsCommon.getString("Select Email_Id from " + AccountMasterTable + "  where  Ac_Code=" + accode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            }
            if (mail != string.Empty)
            {
                txtEmail.Text = mail;
            }
            else
            {
                email = txtEmail.Text.ToString();
            }
            lblParty.Text = clsCommon.getString("select Ac_name_e from " + AccountMasterTable + "  where  Ac_Code=" + accode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            lblFromDt.Text = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            lblToDt.Text = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            ds = new DataSet();
            dt = new DataTable();
            if (string.IsNullOrEmpty(unit_code))
            {
                qry = "Select DISTINCT(g.UNIT_Code),a.Ac_Name_E as Unit_Name from  " + tblGLEDGER + " as g left outer join " + tblPrefix + "AccountMaster as a on g.UNIT_Code=a.Ac_Code and g.COMPANY_CODE=a.Company_Code where g.AC_CODE=" + accode + " and UNIT_Code!=0 and g.DOC_DATE between '" + fromdt + "' and '" + todt + "' and  g.COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ";
            }
            else
            {
                qry = "Select DISTINCT(g.UNIT_Code),a.Ac_Name_E as Unit_Name from  " + tblGLEDGER + " as g left outer join " + tblPrefix + "AccountMaster as a on g.UNIT_Code=a.Ac_Code and g.COMPANY_CODE=a.Company_Code where g.AC_CODE=" + accode + " and UNIT_Code!=0 and UNIT_Code=" + unit_code + " and g.DOC_DATE between '" + fromdt + "' and '" + todt + "' and  g.COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ";
            }

            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {

                dt = new DataTable();
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
        catch (Exception)
        {


        }
    }
    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtDetails = (DataList)e.Item.FindControl("dtDetails");
            Label lblUnitDebit = (Label)e.Item.FindControl("lblUnitDebit");
            Label lblUnitCredit = (Label)e.Item.FindControl("lblUnitCredit");
            Label lblUnitBalance = (Label)e.Item.FindControl("lblUnitBalance");
            Label lblUnitDrCr = (Label)e.Item.FindControl("lblUnitDrCr");
            Label lblUnitCode = (Label)e.Item.FindControl("lblUnitCode");
            string Unit_Code = lblUnitCode.Text;

            ds = new DataSet();
            dt = new DataTable();
            string qry = "select AC_CODE,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as  OpBal from " + tblGLEDGER + " where DOC_DATE < '" + fromdt + "' and Ac_code=" + accode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " group by AC_CODE ";
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
            qry = "select TRAN_TYPE,DOC_NO,Convert(varchar(10),DOC_DATE,102) as DOC_DATE ,NARRATION,AMOUNT,ADJUSTED_AMOUNT,AC_CODE,UNIT_Code,DRCR from " + tblGLEDGER +
                    " where AC_CODE=" + accode + " and UNIT_Code=" + Unit_Code + " and DOC_DATE between '" + fromdt + "' and '" + todt + "' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE asc";
            ds = clsDAL.SimpleQuery(qry);
            double debit = 0.00;
            double credit = 0.00;
            DataTable dtcompute = new DataTable();
            dtcompute = ds.Tables[0];

            if (dtcompute.Rows.Count > 0)
            {
                for (int i = 0; i < dtcompute.Rows.Count; i++)
                {
                    if (dtcompute.Rows[i]["DRCR"].ToString() == "D")
                    {
                        debit += Convert.ToDouble(dtcompute.Rows[i]["AMOUNT"].ToString());
                    }
                    else
                    {
                        credit += Convert.ToDouble(dtcompute.Rows[i]["AMOUNT"].ToString());
                    }
                }
                netdebitall += debit;
                netcreditall += credit;

                double Balance = debit - credit;
                if (Balance < 0)
                {
                    lblUnitDrCr.Text = "Cr";
                }
                else
                {
                    lblUnitDrCr.Text = "Dr";
                }
                lblUnitDebit.Text = Convert.ToString(Math.Abs(debit));
                lblUnitCredit.Text = Convert.ToString(Math.Abs(credit));
                lblUnitBalance.Text = Convert.ToString(Math.Abs(Balance));
            }

            DataTable dtT = new DataTable();
            dtT.Columns.Add("TranType", typeof(string));
            dtT.Columns.Add("DocNo", typeof(Int32));
            dtT.Columns.Add("Date", typeof(string));
            dtT.Columns.Add("Narration", typeof(string));
            dtT.Columns.Add("Debit", typeof(double));
            dtT.Columns.Add("Credit", typeof(double));
            dtT.Columns.Add("Balance", typeof(double));
            dtT.Columns.Add("DrCr", typeof(string));

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
                    dr[3] = Server.HtmlDecode(dt.Rows[i]["NARRATION"].ToString());

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

            dtDetails.DataSource = dtT;
            dtDetails.DataBind();
            lblUnitDebitAll.Text = Convert.ToString(netdebitall);
            lblUnitCreditAll.Text = Convert.ToString(netcreditall);
            double netbalanceall = netdebitall - netcreditall;
            if (netbalanceall < 0)
            {
                lblUnitDrCrAll.Text = "Cr";
            }
            else
            {
                lblUnitDrCrAll.Text = "Dr";
            }
            lblUnitBalanceAll.Text = Convert.ToString(netbalanceall);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void dtDetails_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            //Label 
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
            CreateHtml();

            string mailFrom = Session["EmailId"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["EmailPassword"].ToString();

            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Host = clsGV.Email_Address;

            msg.From = new MailAddress(mailFrom);
            msg.To.Add(email);
            //mail.To.Add("toaddress2@gmail.com");
            msg.Body = "GLedger Report";

            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            msg.Subject = "Delivery Order Report  " + DateTime.Now.ToString("dd/MM/yyyy");

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

    private void CreateHtml()
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        PrintPanel.RenderControl(tw);
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
    }
}