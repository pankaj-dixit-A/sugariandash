using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Net;

public partial class Reports_rptsalebill : System.Web.UI.Page
{
    #region data section
    string f = "../GSReports/SalesBill_.htm";
    string f_Main = "../Report/rptsalebill";
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    int defaultAccountCode = 0;
    string email = string.Empty;
    string searchString = "";
    string prefix = string.Empty;
    static WebControl objAsp = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        string billno = Request.QueryString["billno"];
        ViewState["billno"] = billno;
        if (Session["tblPrefix"] != null)
        {
            tblPrefix = Session["tblPrefix"].ToString();
        }
        else
        {
            prefix = clsCommon.getString("Select tblPrefix from tblPrefix");
            tblPrefix = prefix.ToString();
        }
        tblHead = tblPrefix + "";
        tblDetails = tblPrefix + "";
        AccountMasterTable = tblPrefix + "AccountMaster";
        cityMasterTable = tblPrefix + "CityMaster";

        qryCommon = tblPrefix + "qrySugarSaleList";
        if (!Page.IsPostBack)
        {
            bindDatalist();
        }
    }

    private void bindDatalist()
    {
        string qry = "select s.doc_no,CONVERT(VARCHAR(10),s.doc_date,103) as doc_date,a.Ac_Name_E as PartyName,a.Address_E partyAddress,a.CityName as PartyCity,s.FROM_STATION,s.TO_STATION" +
                    " ,b.Ac_Name_E brokername,a.Tin_No as PartyTin,c.Ac_Name_E as millname,s.cash_advance,s.bank_commission,s.LESS_FRT_RATE,s.Bill_Amount from " + tblPrefix + "SugarSale s" +
                    " left outer join " + tblPrefix + "qryAccountsList a on s.Ac_Code=a.Ac_Code AND s.Company_Code=a.Company_Code" +
                    " left outer join " + AccountMasterTable + " b on s.BROKER=b.Ac_Code AND s.Company_Code=b.Company_Code " +
                    " left outer join " + AccountMasterTable + " c on s.mill_code=c.Ac_Code AND s.Company_Code=c.Company_Code where s.doc_no IN(" + ViewState["billno"].ToString() + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
        //string qry = "select * from " + qryCommon + " where doc_no in (" + ViewState["billno"].ToString() + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        DataSet ds = new DataSet();
        ds = clsDAL.SimpleQuery(qry);
        dtl.DataSource = ds;
        dtl.DataBind();
    }
    protected void dtl_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblCompany = (Label)e.Item.FindControl("lblCompanyName");
        Label lblcompanynamebottom = (Label)e.Item.FindControl("companynamebottom");
        Label lblbillNo = (Label)e.Item.FindControl("lblbillNo");
        Label lblAddr = (Label)e.Item.FindControl("lblCompanyAddress");
        Label lblPhone = (Label)e.Item.FindControl("lblPhone");
        lblCompany.Text = Session["Company_Name"].ToString();
        lblAddr.Text = clsGV.CompanyAddress;
        lblPhone.Text = clsGV.CompanyPhone;
        lblcompanynamebottom.Text = Session["Company_Name"].ToString();

        DataList dtl = (DataList)e.Item.FindControl("dtlItems");

        DataSet ds = new DataSet();
        string qry = "select * from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and doc_no=" + lblbillNo.Text + "  order by detail_id";
        ds = clsDAL.SimpleQuery(qry);
        if (ds != null)
        {
            dtl.DataSource = ds;
            dtl.DataBind();
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            email = txtEmail.Text.ToString();
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            pnlReport.RenderControl(tw);
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
            msg.Body = "Sales Bill";
            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            msg.Subject = "Sales Bill Report" + DateTime.Now.ToString("dd/MM/yyyy");
            msg.IsBodyHtml = true;
            msg.Subject = "Motor Memo " + DateTime.Now.ToString("dd/MM/yyyy");

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
        //HttpClient
        //WebClient client = new WebClient();
        ////client.CookieContainer = new System.Net.CookieContainer();
        //Stream data = client.OpenRead(HttpContext.Current.Request.Url.AbsoluteUri);
        //StreamReader reader = new StreamReader(data);
        //string s = reader.ReadToEnd();
        //data.Close();
        //reader.Close();
        string s = "";
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        //HttpWebRequest webreq = (HttpWebRequest)HttpWebRequest.Create(url);
        //webreq.CookieContainer = new CookieContainer();
        //webreq.Method = "POST";

        //var client = (HttpWebRequest)HttpWebRequest.Create(url);
        //client.CookieContainer = new CookieContainer();
        //using (WebResponse response = client.GetResponse())
        //{
        //    using (Stream data = response.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(data);
        //        s = reader.ReadToEnd();
        //        data.Close();
        //        reader.Close();
        //    }
        //}
        string responseUrl = GetHtml(url);

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
    static string GetHtmlPage(string strURL)
    {

        String strResult;
        WebResponse objResponse;
        WebRequest objRequest = HttpWebRequest.Create(strURL);
        objResponse = objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            strResult = sr.ReadToEnd();
            sr.Close();
        }
        return strResult;
    }
    public static string GetHtml(string urlAddr)
    {
        if (urlAddr == null || string.IsNullOrEmpty(urlAddr))
        {
            throw new ArgumentNullException("urlAddr");
        }
        else
        {
            string result;

            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddr);
            //request.AllowAutoRedirect = true;
            //request.MaximumAutomaticRedirections = 200;
            request.Proxy = null;
            request.UseDefaultCredentials = true;

            //2.Add the container with the active 
            CookieContainer cc = new CookieContainer();
            var s = request.RequestUri;

            //3.Must assing a cookie container for the request to pull the cookies
            request.CookieContainer = cc;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                //Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }
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
        pnlReport.RenderControl(tw);
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
}