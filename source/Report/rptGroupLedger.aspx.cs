using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Data;

public partial class Report_rptGroupLedger : System.Web.UI.Page
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
    int defaultAccountCode = 0;
    int tempcounter = 0;
    string email = string.Empty;
    static WebControl objAsp = null;
    string prefix = string.Empty;
    string accode = string.Empty;
    string fromdt = string.Empty;
    string todt = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        accode = Request.QueryString["accode"];
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
        email = txtEmail.Text;
        if (!Page.IsPostBack)
        {
            lblCompany.Text = Session["Company_Name"].ToString();
            this.bindData();
        }
    }
    private void bindData()
    {
        try
        {
            string mail = "";
            string partycode = accode.Split(',')[0];
            // pnlPopup.Style["display"] = "none";
            if (accode != string.Empty)
            {
                mail = clsCommon.getString("Select Email_Id from " + AccountMasterTable + "  where  Ac_Code=" + partycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            }
            if (mail != string.Empty)
            {
                txtEmail.Text = mail;
            }
            else
            {
                email = txtEmail.Text.ToString();
            }
            
            lblParty.Text = clsCommon.getString("select Ac_name_e from " + AccountMasterTable + "  where  Ac_Code=" + partycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            
            lblFromDt.Text = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            lblToDt.Text = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string qry = "select SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as  OpBal from " + tblGLEDGER + " where DOC_DATE < '" + fromdt + "' and Ac_code IN(" + accode + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " group by AC_CODE ";
            ds = clsDAL.SimpleQuery(qry);
            double opBal = 0.0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        opBal = Convert.ToDouble(dt.Compute("SUM(OpBal)", string.Empty));
                    }
                }
            }
            qry = "select TRAN_TYPE,DOC_NO,Convert(varchar(10),DOC_DATE,102) as DOC_DATE ,NARRATION,AMOUNT,ADJUSTED_AMOUNT,AC_CODE,UNIT_Code,DRCR,SORT_TYPE,SORT_NO from " + tblGLEDGER +
                    " where AC_CODE IN(" + accode + ") and DOC_DATE between '" + fromdt + "' and '" + todt + "' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE asc,SORT_TYPE,SORT_NO,ORDER_CODE ";
            ds = clsDAL.SimpleQuery(qry);

            DataTable dtT = new DataTable();
            //dtT = null;
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
                    string SORT_TYPE = dt.Rows[i]["SORT_TYPE"].ToString();
                    string SORT_NO = dt.Rows[i]["SORT_NO"].ToString();
                    dr[3] = Server.HtmlDecode(dt.Rows[i]["NARRATION"].ToString() + "(" + SORT_TYPE + " " + SORT_NO + ")");

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
                double balance = netdebit - netcredit;
                if (balance > 0)
                {
                    grdDetail.FooterRow.Cells[7].Text = "Dr";
                }
                if (balance < 0)
                {
                    grdDetail.FooterRow.Cells[7].Text = "Cr";
                }
                grdDetail.FooterRow.Cells[6].Text = Math.Abs(balance).ToString();
            }
            else
            {
                grdDetail.FooterRow.Cells[6].Text = "Nil";
            }
            grdDetail.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            grdDetail.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            grdDetail.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            grdDetail.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
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
            msg.Subject = "GLedger Report  " + DateTime.Now.ToString("dd/MM/yyyy");

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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
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
        PrintPanel.RenderControl(tw);
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
    protected void btpGrdprint_Click(object sender, EventArgs e)
    {
        grdDetail.AllowPaging = false;
        grdDetail.DataBind();
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        grdDetail.RenderControl(hw);
        string gridHTML = sw.ToString().Replace("\"", "'")
            .Replace(System.Environment.NewLine, "");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0");
        sb.Append(",top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        sb.Append(gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();};");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
        grdDetail.AllowPaging = true;
        grdDetail.DataBind();
    }
}