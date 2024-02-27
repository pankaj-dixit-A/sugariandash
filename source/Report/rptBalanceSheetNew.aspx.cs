using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;

public partial class Report_rptBalanceSheetNew : System.Web.UI.Page
{
    string f = "../GSReports/BalanceSheet_" + clsGV.user + ".htm";
    string f_Main = "../Report/rptBalanceSheet";
    string email = string.Empty;
    string qryCommon = string.Empty;
    string tblPrefix = string.Empty;
    string cityMasterTable = string.Empty;
    string tblHead = string.Empty;
    double netDebit = 0.00;
    double netCredit = 0.00;
    double netProfit = 0.00;
    double netLoss = 0.00;
    double totalDebit = 0.00;
    double totalCredit = 0.00;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        qryCommon = "qryGledgernew";
        tblHead = tblPrefix + "BSGroupMaster";
        cityMasterTable = tblPrefix + "CityMaster";
        ViewState["VNO"] = Request.QueryString["VNO"];
        ViewState["mailID"] = Request.QueryString["mailID"];
        ViewState["pageBreak"] = Request.QueryString["pageBreak"];

        ViewState["BalDate"] = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                this.bindData();
                lblCompany.Text = Session["Company_Name"].ToString();
                lblCompanyAddr.Text = clsGV.CompanyAddress;
                lblCompanyMobile.Text = clsGV.CompanyPhone;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }

    private void bindData()
    {
        try
        {
            DataTable dtLeft = new DataTable();
            DataTable dtRight = new DataTable();

            dtLeft.Columns.Add("Group_Code", typeof(string));
            dtLeft.Columns.Add("groupname", typeof(string));
            dtLeft.Columns.Add("groupamount", typeof(double));
            dtLeft.Columns.Add("summary", typeof(string));

            dtRight.Columns.Add("Group_Code", typeof(string));
            dtRight.Columns.Add("groupname", typeof(string));
            dtRight.Columns.Add("groupamount", typeof(double));
            dtRight.Columns.Add("summary", typeof(string));

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtTemp = new DataTable();
            string qry = "";
            #region[left side]
            //fill dtl_Left 
            qry = "select Group_Code,BSGroupName,group_Summary,group_Order,AC_CODE,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance   from "
                + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE<=" + Convert.ToInt32(Session["year"].ToString())
                + " and group_Type='B' and DOC_DATE<='" + ViewState["BalDate"].ToString()
                + "' group by Group_Code,BSGroupName,group_Summary,group_Order,AC_CODE  having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) < 0 order by group_Order,Group_Code ";


            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Int32 grpcode = 0;
                        string groupname = "";
                        string groupsummary = "";
                        double grpbal = 0.00;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (grpcode == Convert.ToInt32(dt.Rows[i]["Group_Code"].ToString()))
                            {
                                grpbal += Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                            }
                            else
                            {
                                if (grpbal != 0)
                                {
                                    DataRow dr = dtLeft.NewRow();
                                    dr["Group_Code"] = grpcode;
                                    dr["groupname"] = groupname;
                                    dr["groupamount"] = Math.Abs(grpbal);
                                    dr["summary"] = groupsummary;
                                    dtLeft.Rows.Add(dr);

                                    netCredit += Math.Abs(grpbal);
                                   // grpbal = 0;
                                    grpbal = Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                }
                                else
                                {
                                    grpbal += Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                }
                                grpcode = Convert.ToInt32(dt.Rows[i]["Group_Code"].ToString());
                                groupname = dt.Rows[i]["BSGroupName"].ToString();
                                groupsummary = dt.Rows[i]["group_Summary"].ToString();
                              

                              

                            }
                        }
                        DataRow dr1 = dtLeft.NewRow();
                        dr1["Group_Code"] = grpcode;
                        dr1["groupname"] = groupname;
                        dr1["groupamount"] = Math.Abs(grpbal);
                        dr1["summary"] = groupsummary;
                        dtLeft.Rows.Add(dr1);
                        netCredit += Math.Abs(grpbal);

                        //'-----------------------------

                        dtl_Left.DataSource = dtLeft;
                        dtl_Left.DataBind();

                        //------right 
            #endregion

                        #region[left side]
                        //fill dtl_Left 
                        qry = "select Group_Code,BSGroupName,group_Summary,group_Order,AC_CODE,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance   from "
                            + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE<=" + Convert.ToInt32(Session["year"].ToString())
                            + " and group_Type='B' and DOC_DATE<='" + ViewState["BalDate"].ToString()
                            + "' group by Group_Code,BSGroupName,group_Summary,group_Order,AC_CODE  having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) > 0 order by group_Order,Group_Code ";


                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    grpcode = 0;
                                    groupname = "";
                                    groupsummary = "";
                                    grpbal = 0.00;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (grpcode == Convert.ToInt32(dt.Rows[i]["Group_Code"].ToString()))
                                        {
                                            grpbal += Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                        }
                                        else
                                        {
                                            if (grpbal != 0)
                                            {
                                                DataRow dr = dtRight.NewRow();
                                                dr["Group_Code"] = grpcode;
                                                dr["groupname"] = groupname;
                                                dr["groupamount"] = Math.Abs(grpbal);
                                                dr["summary"] = groupsummary;
                                                dtRight.Rows.Add(dr);

                                                netDebit += Math.Abs(grpbal);
                                               // grpbal = 0;
                                                grpbal = Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                            }
                                            else
                                            {
                                                grpbal += Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                            }
                                            grpcode = Convert.ToInt32(dt.Rows[i]["Group_Code"].ToString());
                                            groupname = dt.Rows[i]["BSGroupName"].ToString();
                                            groupsummary = dt.Rows[i]["group_Summary"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                        DataRow dr2 = dtRight.NewRow();
                        dr2["Group_Code"] = grpcode;
                        dr2["groupname"] = groupname;
                        dr2["groupamount"] = Math.Abs(grpbal);
                        dr2["summary"] = groupsummary;
                        dtRight.Rows.Add(dr2);
                        netDebit += Math.Abs(grpbal);

                        //'-----------------------------

                        dtl_Right.DataSource = dtRight;
                        dtl_Right.DataBind();

                        //------right 
                        #endregion

                        lblNetDebit.Text = netDebit.ToString();
                        lblNetCredit.Text = netCredit.ToString();

                        if (netDebit - netCredit > 0)
                        {
                            netProfit = (netDebit - netCredit);
                            lblnetProfit.Text = Math.Round(netProfit, 2).ToString();
                        }
                        else
                        {
                            netLoss = (netCredit - netDebit);
                            lblnetLoss.Text = Math.Round(netLoss, 2).ToString();
                        }

                        if (netProfit == 0)
                        {
                            lblnetprofithead.Text = "";
                            lblnetProfit.Text = "";
                        }
                        if (netLoss == 0)
                        {
                            lblnetLoss.Text = "";
                            lblnetlosshead.Text = "";

                        }

                        lbltotalCredit.Text = Math.Round((netCredit + netProfit), 2).ToString();
                        lbltotalDebit.Text = Math.Round((netLoss + netDebit), 2).ToString();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void dtLeft_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataTable dtLInner = new DataTable();
            dtLInner.Columns.Add("acname", typeof(string));
            dtLInner.Columns.Add("acamount", typeof(double));


            DataList dtLeftInner = (DataList)e.Item.FindControl("dtLeftInner");

            Label lblsummaryL = (Label)e.Item.FindControl("lblsummaryL");
            Label lblGroupCodeL = (Label)e.Item.FindControl("lblGroupCodeL");

            if (lblsummaryL.Text == "Y")
            {

            }
            else    //summary no show detail part
            {
                qry = "";
                qry =
                "select AC_CODE, Ac_Name_E,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance " +
                " from " + qryCommon + " " +
                " where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE<=" + Convert.ToInt32(Session["year"].ToString())
                + " and Group_Code=" + lblGroupCodeL.Text + " " +
                " group by AC_CODE,Ac_Name_E having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) !=0";
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                double bal = Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                if (bal < 0)         //  >0  right side asset
                                {
                                    DataRow dr = dtLInner.NewRow();
                                    dr["acname"] = dt.Rows[i]["Ac_Name_E"].ToString();
                                    dr["acamount"] = Math.Abs(bal);

                                    dtLInner.Rows.Add(dr);
                                }
                                //else
                                //{
                                //    DataRow dr = dtLInner.NewRow();
                                //    dr["acname"] = dt.Rows[i]["Ac_Name_E"].ToString();
                                //    dr["acamount"] = Math.Abs(bal);

                                //    dtLInner.Rows.Add(dr);
                                //}

                            }
                            dtLeftInner.DataSource = dtLInner;
                            dtLeftInner.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception exxx)
        {
            Response.Write("left item command err:" + exxx.Message);
        }
    }


    protected void dtl_Right_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataTable dtRInner = new DataTable();
            dtRInner.Columns.Add("acname", typeof(string));
            dtRInner.Columns.Add("acamount", typeof(double));


            DataList dtRightInner = (DataList)e.Item.FindControl("dtRightInner");

            Label lblsummaryR = (Label)e.Item.FindControl("lblsummaryR");
            Label lblGroupCodeR = (Label)e.Item.FindControl("lblGroupCodeR");

            if (lblsummaryR.Text == "Y")
            {

            }
            else    //summary no show detail part
            {
                qry = "";
                qry =
                "select AC_CODE, Ac_Name_E,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance " +
                " from " + qryCommon + " " +
                " where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE<=" + Convert.ToInt32(Session["year"].ToString()) + " and Group_Code=" + lblGroupCodeR.Text + " " +
                " group by AC_CODE,Ac_Name_E having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) !=0";
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                double bal = Convert.ToDouble(dt.Rows[i]["Balance"].ToString());
                                if (bal > 0)         //  <0  left side liabilities
                                {
                                    DataRow dr = dtRInner.NewRow();
                                    dr["acname"] = dt.Rows[i]["Ac_Name_E"].ToString();
                                    dr["acamount"] = Math.Abs(bal);

                                    dtRInner.Rows.Add(dr);
                                }

                            }
                            if (dtRInner.Rows.Count > 0)
                            {
                                dtRightInner.DataSource = dtRInner;
                                dtRightInner.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Response.Write("right item command err:" + ee.Message);
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        email = txtEmail.Text.ToString();
        if (email != string.Empty)
        {
            try
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
                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(email);
                msg.Body = "Balance Sheet";
                msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                msg.IsBodyHtml = true;
                msg.Subject = "Balance Sheet ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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

            catch (Exception e1)
            {
                Response.Write("mail err:" + e1);
                //Response.Write("<script>alert('Error sending Mail');</script>");
                return;
            }
            Response.Write("<script>alert('Mail sent successfully');</script>");
        }
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
            PrintPanel.RenderControl(tw);
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