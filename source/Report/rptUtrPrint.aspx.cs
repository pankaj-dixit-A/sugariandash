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
using System.Net.Mime;

public partial class Report_rptUtrPrint : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds;
    DataTable dt;
    string tblPrefix = string.Empty;
    string UtrNo = string.Empty;
    string f = "../GSReports/Utr_.htm";
    string f_Main = "../GSReports/Utr_";
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        UtrNo = Request.QueryString["Doc_No"];

        if (!Page.IsPostBack)
        {
            BindList();
            lblCompany.Text = "M/S. " + Session["Company_Name"].ToString();

            #region Address
            string qry = "Select * from tblVoucherHeadAddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("LeftAddress", typeof(string)));
                dt.Columns.Add(new DataColumn("MiddlePart", typeof(string)));
                dt.Columns.Add(new DataColumn("RightAddress", typeof(string)));

                string AL1 = ds.Tables[0].Rows[0]["AL1"].ToString();
                string AL2 = ds.Tables[0].Rows[0]["AL2"].ToString();
                string AL3 = ds.Tables[0].Rows[0]["AL3"].ToString();
                string AL4 = ds.Tables[0].Rows[0]["AL4"].ToString();
                string OtherDetails = ds.Tables[0].Rows[0]["Other"].ToString();

                string rnl = AL1.Replace("\n", "<br/>");
                var TabSpace = new String(' ', 4);
                string ab = rnl.Replace("\t", TabSpace);
                string la = ab.Replace(" ", "&nbsp;");
                lblAl1.Text = la;


                string rnl1 = AL2.Replace("\n", "<br/>");
                var TabSpace1 = new String(' ', 4);
                string ab1 = rnl1.Replace("\t", TabSpace1);
                string la1 = ab1.Replace(" ", "&nbsp;");
                lblAl2.Text = la1;

                string rnl2 = AL3.Replace("\n", "<br/>");
                var TabSpace2 = new String(' ', 4);
                string ab2 = rnl2.Replace("\t", TabSpace2);
                string la2 = ab2.Replace(" ", "&nbsp;");
                lblAl3.Text = la2;

                string rnl3 = AL4.Replace("\n", "<br/>");
                var TabSpace3 = new String(' ', 4);
                string ab3 = rnl3.Replace("\t", TabSpace2);
                string la3 = ab3.Replace(" ", "&nbsp;");
                lblAl4.Text = la3;

                string rnl4 = OtherDetails.Replace("\n", "<br/>");
                var TabSpace4 = new String(' ', 4);
                string ab4 = rnl4.Replace("\t", TabSpace2);
                string la4 = ab4.Replace(" ", "&nbsp;");
                lblOtherDetails.Text = la4;

            }
            #endregion
        }
    }

    private void BindList()
    {
        try
        {
            qry = "select u.doc_no as #,Convert(varchar(10),u.doc_date,103) as dt,a.Email_Id as millmail,a.Email_Id_cc,a.Ac_Name_E as mill,u.utr_no as utrno,u.amount as amt,b.Ac_Name_E as bank,a.Pincode as millpin,a.Address_E as milladdress,c.city_name_e as millcity,c.state as millstate,(select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") as Company" +
                " from " + tblPrefix + "UTR u LEFT OUTER JOIN " + tblPrefix + "AccountMaster a on u.mill_code=a.Ac_Code and u.Company_Code=a.Company_Code" +
                " LEFT OUTER JOIN " + tblPrefix + "AccountMaster b on u.bank_ac=b.Ac_Code and u.Company_Code=b.Company_Code left outer join " + tblPrefix + "CityMaster c on a.City_Code=c.city_code and a.Company_Code=c.company_code where u.doc_no=" + UtrNo + " and u.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and u.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email_Id_cc"].ToString() + "," + ds.Tables[0].Rows[0]["millmail"].ToString();

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //lblCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    //lblCmpAddress.Text = clsCommon.getString("Select Address_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string city = clsCommon.getString("Select City_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string state = clsCommon.getString("Select State_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    //lblCmpCityName.Text = city + "," + state;
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
            }
        }
        catch (Exception)
        {
            Response.Write("UTR not present");
        }
    }
    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblInwordsamount = e.Item.FindControl("lblInwordsamount") as Label;
        Label Label7 = e.Item.FindControl("Label7") as Label;
        string amount = Label7.Text.ToString();
        lblInwordsamount.Text = clsNoToWord.ctgword(amount);

        Image imgSign = (Image)e.Item.FindControl("imgSign");
        string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        imgSign.ImageUrl = imgurl;
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter tw = new HtmlTextWriter(sw);
                    pnlMain.RenderControl(tw);
                    string s = sw.ToString();
                    s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");

                    byte[] array = Encoding.UTF8.GetBytes(s);
                    ms.Write(array, 0, array.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = "UTR.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(email);
                    msg.Body = "UTR";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "UTR";
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