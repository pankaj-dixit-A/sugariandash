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
using System.Net.Mime;

public partial class Report_rptWayBill : System.Web.UI.Page
{
    string f = "WayBill_";
    string f_Main = "../Report/rptWayBill_";
    string tblPrefix = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    string Doc_No = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        cityMasterTable = tblPrefix + "CityMaster";
        Doc_No = Request.QueryString["Doc_No"];
        f = f + DateTime.Now + ".htm";
        if (!Page.IsPostBack)
        {

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
            this.BindReport();
        }
    }

    private void BindReport()
    {
        try
        {
            if (!string.IsNullOrEmpty(Doc_No))
            {
                qry = "select (d.GetPassName+','+ISNULL(c.city_name_e,'')) as GetpassName,d.GETPASSCODE,d.doc_no as #,Convert(varchar(10),d.doc_date,103) as dt,d.millName as MillName,(a.Address_E + ' City:'+e.city_name_e) as MillAddress,a.Tin_No as MillTIN," +
                    " d.final_amount as amount,e.city_name_e as MillCity,c.city_name_e as GetpassCity,d.truck_no as lorry,e.state as MillState,d.TransportName as TransportName,d.quantal as Qntl,d.driver_no,d.Invoice_No,d.Carporate_Sale_No,d.CheckPost,d.sale_rate from " + tblPrefix + "qryDeliveryOrderListReport d" +
                    " left outer join " + cityMasterTable + " c on d.getpasscityCode=c.city_code and d.company_code=c.company_code left outer join " + tblPrefix + "AccountMaster a on d.mill_code=a.Ac_Code and d.company_code=a.Company_Code" +
                    " left outer join " + cityMasterTable + " e on a.City_Code=e.city_code and a.company_code=e.company_code where d.tran_type='DO' and d.doc_no=" + Doc_No + " and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Carporate_Sale_No = ds.Tables[0].Rows[0]["Carporate_Sale_No"].ToString();
                    string getpasscode = ds.Tables[0].Rows[0]["GETPASSCODE"].ToString();
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["lorry"].ToString()))
                    {
                        ViewState["lorry"] = ds.Tables[0].Rows[0]["lorry"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Qntl"].ToString()))
                    {
                        ViewState["Qntl"] = ds.Tables[0].Rows[0]["Qntl"].ToString();
                    }

                    if (!string.IsNullOrEmpty(Carporate_Sale_No))
                    {
                        string SellingType = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        if (SellingType == "P")
                        {
                            double qntl2 = Convert.ToDouble(ds.Tables[0].Rows[0]["Qntl"].ToString());
                            double sale_rate = Convert.ToDouble(ds.Tables[0].Rows[0]["sale_rate"].ToString());
                            ds.Tables[0].Rows[0]["amount"] = qntl2 * sale_rate;
                            string party_Code = clsCommon.getString("Select Unit_Code from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                            string partyname = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + party_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string citycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + party_Code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string city_name_e = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + citycode + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            ds.Tables[0].Rows[0]["GetpassName"] = partyname + "," + city_name_e;
                            ds.Tables[0].Rows[0]["GetpassCity"] = city_name_e;
                            getpasscode = party_Code;

                            string self_ac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            string millcitycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + self_ac + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string millCity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + millcitycode + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            ds.Tables[0].Rows[0]["MillName"] = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + self_ac + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            ds.Tables[0].Rows[0]["MillAddress"] = clsCommon.getString("Select Address_E from " + tblPrefix + "AccountMaster where Ac_Code=" + self_ac + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "") + " City:" + millCity;

                            ds.Tables[0].Rows[0]["MillTIN"] = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + self_ac + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            ds.Tables[0].Rows[0]["MillCity"] = millCity;
                            ds.Tables[0].Rows[0]["MillState"] = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code=" + millcitycode + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            

                        }
                    }
                    string ccmail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Ac_Code=" + getpasscode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    txtEmail.Text = ccmail + "," + clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + getpasscode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    ds.Tables[0].Columns.Add(new DataColumn("QntlToKg", typeof(string)));
                    double qntl = Convert.ToDouble(ds.Tables[0].Rows[0]["Qntl"].ToString());
                    double QntlToKg = qntl * 100;
                    ds.Tables[0].Rows[0]["QntlToKg"] = QntlToKg;

                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MillState"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["MillState"] = "State:" + ds.Tables[0].Rows[0]["MillState"].ToString();
                    }
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblCmpAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string cmpcity = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string state = clsCommon.getString("Select State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblCmpCityName.Text = cmpcity + "," + state;
                        //lblCompanyMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                    }
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
            string email = txtEmail.Text;
            if (txtEmail.Text != string.Empty)
            {
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter tw = new HtmlTextWriter(sw);
                //pnlMain.RenderControl(tw);
                //string s = sw.ToString();
                //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                //emailsending sd = new emailsending();
                //sd.sendmail(s, email, "WayBill");
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
                    contentType.Name = "waybillstream.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    //byte[] array = Encoding.ASCII.GetBytes(s);
                    //MemoryStream ms = new MemoryStream(File.ReadAllBytes(array.ToString()));

                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(email);
                    msg.Body = "Way Bill";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    string lorry = string.Empty;
                    string qntl = string.Empty;
                    if (ViewState["Qntl"] != null)
                    {
                        qntl = "Quintal:" + ViewState["Qntl"].ToString();
                    }
                    if (ViewState["lorry"] != null)
                    {
                        lorry = "Lorry:" + ViewState["lorry"].ToString();
                    }
                    msg.Subject = "Ref.No:" + Request.QueryString["Doc_No"] + " " + lorry + " " + qntl;
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
                //    using (FileStream fs = new FileStream(f, FileMode.Create))
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
                //    using (FileStream fs = new FileStream(f, FileMode.Append))
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
                ////byte[] array = Encoding.ASCII.GetBytes(s);
                ////MemoryStream ms = new MemoryStream(File.ReadAllBytes(array.ToString()));

                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                //SmtpServer.Host = clsGV.Email_Address;
                //msg.From = new MailAddress(mailFrom);
                //msg.To.Add(email);
                //msg.Body = "Way Bill";

                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                ////if (msg.Attachments[0].ContentStream == ms)
                ////{
                ////    long ss = msg.Attachments[0].ContentStream.Length;
                ////}
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "Way Bill " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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

    protected virtual bool IsFileinUse(FileInfo file)
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }
        return false;
    }
}