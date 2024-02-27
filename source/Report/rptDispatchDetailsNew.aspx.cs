using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Net.Mail;

public partial class Report_rptDispatchDetailsNew : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string Lot_No = string.Empty;
    string Sr_No = string.Empty;
    string qry = string.Empty;
    string Tender_No = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string f = "../GSReports/DispatchDetails_.htm";
    string f_Main = "../Report/rptDispatchDetails";
    string Branch_Code = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        Mill_Code = Request.QueryString["Mill_Code"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Lot_No = Request.QueryString["Lot_No"].ToString();
        Sr_No = Request.QueryString["Sr_No"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.BindData();
        }
    }
    private void BindData()
    {
        string fromdate = DateTime.Parse(fromDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToString("yyyy-MM-dd");
        string todate = DateTime.Parse(toDT, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToString("yyyy-MM-dd");
        string Dispatched = "";

        if (string.IsNullOrEmpty(Branch_Code))
        {
            if (Mill_Code != string.Empty)
            {
                if (Mill_Code != string.Empty && Lot_No != string.Empty)
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code" +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Mill_Code=" + Mill_Code + " AND qt.Tender_No=" + Lot_No + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }
                else
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Mill_Code=" + Mill_Code + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }
            }
            if (Mill_Code == string.Empty)
            {
                if (Mill_Code == string.Empty && Lot_No == string.Empty)
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                                       " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                                       " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                                       " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                                       " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Buyer=2 AND " +
                                       " Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }

                if (Mill_Code == string.Empty && Lot_No != string.Empty)
                {
                    if (Lot_No != string.Empty && Sr_No == string.Empty)
                    {
                        qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Tender_No=" + Lot_No + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                    }
                    else
                    {

                    }
                }
            }
        }
        else
        {
            if (Mill_Code != string.Empty)
            {
                if (Mill_Code != string.Empty && Lot_No != string.Empty)
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code" +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Branch_Code=" + Branch_Code + " and qt.Mill_Code=" + Mill_Code + " AND qt.Tender_No=" + Lot_No + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }
                else
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Branch_Code=" + Branch_Code + " and qt.Mill_Code=" + Mill_Code + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }
            }
            if (Mill_Code == string.Empty)
            {
                if (Mill_Code == string.Empty && Lot_No == string.Empty)
                {
                    qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                                       " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                                       " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                                       " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                                       " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Branch_Code=" + Branch_Code + " and qt.Buyer=2 AND " +
                                       " Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                }

                if (Mill_Code == string.Empty && Lot_No != string.Empty)
                {
                    if (Lot_No != string.Empty && Sr_No == string.Empty)
                    {
                        qry = "select qt.ID,qt.Tender_No as Tender_No,CONVERT(varchar(10),qt.Tender_Date,103) as Tender_Date ,A.Ac_Name_E as Mill,qt.Mill_Code " +
                            " ,qt.Grade as Grade,qt.Quantal as Quantal,qt.Mill_Rate as Mill_Rate, " +
                            " CONVERT(varchar(10),qt.Lifting_Date,103) as Lifting_Date,B.Ac_Name_E as Tender_DO from " + tblPrefix + "qryTenderListReport qt " +
                            " left outer  join " + tblPrefix + "AccountMaster A on qt.Mill_Code=A.Ac_Code AND qt.Company_Code=A.Company_Code " +
                            " left outer  join " + tblPrefix + "AccountMaster B on qt.Tender_DO=B.Ac_Code AND qt.Company_Code=B.Company_Code where qt.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and qt.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and qt.Branch_Code=" + Branch_Code + " and qt.Tender_No=" + Lot_No + " AND qt.Buyer=2 " +
                            " AND Tender_Date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                    }
                    else
                    {

                    }
                }
            }
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        dt = new DataTable();
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataColumn disp = new DataColumn("Dispatched", typeof(double));
                ds.Tables[0].Columns.Add(disp);

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    string tenderid = ds.Tables[0].Rows[j]["Tender_No"].ToString();
                    if (string.IsNullOrEmpty(Branch_Code))
                    {
                        Dispatched = clsCommon.getString("Select SUM(quantal) as TD_Dispatch from " + tblPrefix + "deliveryorder WHERE company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " and purc_no=" + tenderid + " AND tran_type='DO' AND desp_type='DI'");
                    }
                    else
                    {
                        Dispatched = clsCommon.getString("Select SUM(quantal) as TD_Dispatch from " + tblPrefix + "deliveryorder WHERE company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " and purc_no=" + tenderid + " AND tran_type='DO' AND desp_type='DI'");
                    }
                    //string tdid = ds.Tables[0].Rows[j]["ID"].ToString();
                    double dispatc = Dispatched != string.Empty ? double.Parse(Dispatched) : 0;
                    ds.Tables[0].Rows[j]["Dispatched"] = dispatc;
                }
                dt = ds.Tables[0];
                if (Lot_No != string.Empty)
                {
                    string millCode = dt.Rows[0]["Mill_Code"].ToString();
                    string millMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + millCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    txtEmail.Text = millMail;
                }

                dtl.DataSource = dt;
                dtl.DataBind();
                //DataColumn balance = new DataColumn("Bal", typeof(string));
                //ds.Tables[0].Columns.Add(balance);
                //DataList dtltddetails = ((DataList)dtl.FindControl("dtlTenderDetails"));
                //DataTable dtDetails = new DataTable();
                //DataColumn totalBal = new DataColumn("totalBal", typeof(double));
                //dtDetails.Columns.Add(totalBal);
                //foreach (DataListItem i in dtltddetails.Items)
                //{
                //    Label lblTdBal = (Label)dtltddetails.FindControl("lbltdbal");
                //    double tdbal = double.Parse(lblTdBal.Text);
                //    DataRow drDetails = dtDetails.NewRow();
                //    dtDetails.Rows.Add(drDetails);
                //}
            }
            else
            {
                dtl.DataSource = null;
                dtl.DataBind();
            }
        }
    }
    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataList dtlDispatch = (DataList)e.Item.FindControl("dtlDispatch");
        Label tenderno = (Label)e.Item.FindControl("lblTenderNo");
        string Purc_No = tenderno.Text;
        if (string.IsNullOrEmpty(Branch_Code))
        {
            qry = " select do.doc_no as detail_id,Convert(varchar(10),do.doc_date,103) as DI_Date,A.Ac_Name_E as Getpass,truck_no, " +
                " quantal as DI_Qty,B.Short_Name as DI_DO from " + tblPrefix + "deliveryorder do left outer  join " + tblPrefix + "AccountMaster A on do.GETPASSCODE=A.Ac_Code AND do.company_code=A.Company_Code " +
                " left outer  join " + tblPrefix + "AccountMaster B on B.Ac_Code=do.DO AND do.company_code=B.Company_Code where do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and purc_no=" + Purc_No + " and tran_type='DO' AND desp_type='DI' ";
        }
        else
        {
            qry = " select do.doc_no as detail_id,Convert(varchar(10),do.doc_date,103) as DI_Date,A.Ac_Name_E as Getpass,truck_no, " +
                           " quantal as DI_Qty,B.Short_Name as DI_DO from " + tblPrefix + "deliveryorder do left outer  join " + tblPrefix + "AccountMaster A on do.GETPASSCODE=A.Ac_Code AND do.company_code=A.Company_Code " +
                           " left outer  join " + tblPrefix + "AccountMaster B on B.Ac_Code=do.DO AND do.company_code=B.Company_Code where do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and do.Branch_Code=" + Branch_Code + " and purc_no=" + Purc_No + "  and tran_type='DO' AND desp_type='DI' ";
        }
        ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                dtlDispatch.DataSource = dt;
                dtlDispatch.DataBind();
            }
            else
            {
                dtlDispatch.DataSource = null;
                dtlDispatch.DataBind();
            }
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
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
                catch (Exception)
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
                msg.Body = "Dispatch Details";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                msg.Subject = "Dispatch Details Report" + " " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                msg.IsBodyHtml = true;
                if (smtpPort != string.Empty)
                {
                    SmtpServer.Port = Convert.ToInt32(smtpPort);
                }
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                SmtpServer.Send(msg);
            }
        }
        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");

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