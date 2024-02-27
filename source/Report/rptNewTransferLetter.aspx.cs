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

public partial class Report_rptNewTransferLetter : System.Web.UI.Page
{
    string f = "../GSReports/transferletter_.htm";
    string f_Main = "../GSReports/TL_";
    string tblPrefix = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    string Do_Number = string.Empty;
    string DOCODE = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        cityMasterTable = tblPrefix + "CityMaster";
        Do_Number = Request.QueryString["DONO"];
        DOCODE = Request.QueryString["DOCODE"];
        if (!Page.IsPostBack)
        {
            this.BindReport();
        }
    }

    private void BindReport()
    {
        try
        {
            qry = "select d.doc_no as DoNo,CONVERT(varchar(10),d.doc_date,103) as Do_Date,a.Email_Id as MillMail,a.Email_Id_cc,a.Ac_Name_E as MillName,d.mill_code,d.quantal as Qntl,b.Ac_Name_E as Getpass,b.City_Code as GetpassCity,b.Cst_no as GetPassCST,d.grade as Grade," +
                  " b.Address_E as GetPassAddress,b.Mobile_No as GetPassMobile,d.truck_no as Truck,b.Tin_No as GetPassTin," +
                  " d.mill_rate as MR,c.city_name_e as GetPassCity,c.state as GetPassState,d.final_amout as Amount from " + tblPrefix + "deliveryorder d" +
                  " LEFT OUTER JOIN " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code" +
                  " LEFT OUTER JOIN " + tblPrefix + "AccountMaster b on d.GETPASSCODE=b.Ac_Code and d.company_code=b.Company_Code" +
                  " LEFT OUTER JOIN " + tblPrefix + "CityMaster c on b.City_Code=c.city_code and b.Company_Code=c.company_code" +
                  " where d.tran_type='DO' and desp_type='DI' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.doc_no=" + Do_Number + "";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string getpasscitycode = ds.Tables[0].Rows[0]["GetpassCity"].ToString();
                ds.Tables[0].Columns.Add(new DataColumn("CmpName", typeof(string)));
                ds.Tables[0].Rows[0]["CmpName"] = Session["Company_Name"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email_Id_cc"].ToString() + "," + ds.Tables[0].Rows[0]["MillMail"].ToString();
                string millCode = ds.Tables[0].Rows[0]["mill_code"].ToString();
                string Pincode = clsCommon.getString("Select Pincode from " + tblPrefix + "AccountMaster where Ac_Code=" + millCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string citycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + millCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string city = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + citycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string state = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code=" + citycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string getpasscity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + getpasscitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                ds.Tables[0].Rows[0]["Getpass"] = ds.Tables[0].Rows[0]["Getpass"] + " " + getpasscity;
                ds.Tables[0].Columns.Add(new DataColumn("millcitystate", typeof(string)));
                ds.Tables[0].Rows[0]["millcitystate"] = "City:<b>" + city + "</b> State:<b> " + state + "</b> Pincode:<b>" + Pincode + "</b>";

                dt = new DataTable();
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                    dtlist2.DataSource = dt;
                    dtlist2.DataBind();
                }
            }
        }
        catch (Exception)
        {
            Response.Write("Number Not Present");
            //throw;
        }
    }

    protected void dtlist_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            imgSign.ImageUrl = imgurl;

            Label lblCompany = (Label)e.Item.FindControl("lblCompany");
            lblCompany.Text = "M/S. " + Session["Company_Name"].ToString();
            Label lblAl1 = (Label)e.Item.FindControl("lblAl1");
            Label lblAl2 = (Label)e.Item.FindControl("lblAl2");
            Label lblAl3 = (Label)e.Item.FindControl("lblAl3");
            Label lblAl4 = (Label)e.Item.FindControl("lblAl4");
            Label lblOtherDetails = (Label)e.Item.FindControl("lblOtherDetails");

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
        catch (Exception)
        {
            throw;
        }
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
                    contentType.Name = "TransferLetter.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(email);
                    msg.Body = "Voucher";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "Transfer Letter " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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

                //StringWriter sw = new StringWriter();
                //HtmlTextWriter tw = new HtmlTextWriter(sw);
                //pnlMain.RenderControl(tw);
                //string s = sw.ToString();
                //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                //try
                //{
                //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                //    {
                //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                //        {
                //            w.WriteLine(s);
                //        }
                //    }
                //}
                //catch (Exception)
                //{
                //    f = f_Main + ".htm";
                //    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                //    {
                //        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                //        {
                //            w.WriteLine(s);
                //        }
                //    }
                //}

                //string mailFrom = Session["EmailId"].ToString();
                //string smtpPort = "587";
                //string emailPassword = Session["EmailPassword"].ToString();
                //MailMessage msg = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                //SmtpServer.Host = clsGV.Email_Address;
                //msg.From = new MailAddress(mailFrom);
                //msg.To.Add(email);
                //msg.Body = "Transfer Letter";
                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "Transfer Letter " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                //msg.IsBodyHtml = true;
                //if (smtpPort != string.Empty)
                //{
                //    SmtpServer.Port = Convert.ToInt32(smtpPort);
                //}
                //                    SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                //SmtpServer.Send(msg);
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
}