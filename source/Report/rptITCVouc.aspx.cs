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

public partial class Report_rptITCVouc : System.Web.UI.Page
{
    string f = "../GSReports/CarporateVouc_.htm";
    string f_Main = "../GSReports/CarporateVouc_";
    string tblPrefix = string.Empty;
    string millEmail = string.Empty;
    string cityMasterTable = string.Empty;
    string mail = string.Empty;
    string qry = "";
    DataSet ds;
    DataTable dt;
    string Doc_No = string.Empty;
    string DOCODE = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        cityMasterTable = tblPrefix + "CityMaster";
        Doc_No = Request.QueryString["Doc_No"];
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
                qry = "select v.Doc_No as #,v.Doc_Date as dt,v.Ac_Code,v.Unit_Code,a.City_Code,v.PartyName as PartyName,v.DO_No,v.PartyAddress as PartyAddress,a.Local_Lic_No,a.Cst_no,a.Gst_No,a.Tin_No,a.ECC_No,a.CompanyPan," +
                      " (ISNULL('City: <b>'+c.city_name_e+'</b>',' ') +' '+ISNULL('State: <b>'+c.state+'</b>',' ')+' Pin Code: <b>'+ISNULL(a.Pincode+'</b>',' ')) as PartyCity,v.From_Place as Dispatch_From,v.To_Place as Dispatch_To,v.Lorry_No as lorry,(ISNULL(v.Quantal,0)+ISNULL(v.Quantal1,0)) as Qntl," +
                      " (ISNULL(v.BAGS,0)+ISNULL(v.BAGS1,0)) as Bags,v.Grade as Grade,v.MillName as MillName,brok.Short_Name as brokshort,v.BrokerName as BrokerName,brok.Address_E AS brokAddress,brok.Pincode as brokpin,d.city_name_e as BrokerCity, d.state as BrokerState" +
                      " ,v.Voucher_Amount as VoucherAmount,(Convert(varchar(20),v.Sale_Rate,20)+' '+v.BrokerShort) as millbrokerSR,v.Sale_Rate as SR from " + tblPrefix + "qryVoucherList v left outer join " + tblPrefix + "AccountMaster a on v.Ac_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                      " left outer join " + cityMasterTable + " c on a.City_Code=c.city_code and a.Company_Code=c.company_code left outer join " + tblPrefix + "AccountMaster x on v.Broker_CODE=x.Ac_Code and v.Company_Code=x.Company_Code" +
                      " left outer join " + cityMasterTable + " d on x.City_Code=d.city_code and x.Company_Code=d.company_code left outer join " + tblPrefix + "AccountMaster brok on v.Broker_CODE=brok.Ac_Code and v.Company_Code=brok.Company_Code where v.Tran_Type='OV' and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and v.Doc_No=" + Doc_No + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string DO_No = ds.Tables[0].Rows[0]["DO_No"].ToString();
                    string CarporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no=" + DO_No + " and tran_type='DO'");

                    if (CarporateNo != "0")
                    {
                        string Ac_Code = ds.Tables[0].Rows[0]["Ac_Code"].ToString();
                        string Unit_Code = ds.Tables[0].Rows[0]["Unit_Code"].ToString();
                        string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                        string unitMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Unit_Code);
                        string acMailcc = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                        string unitMailcc = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Unit_Code);
                        txtEmail.Text = acMailcc + "," + acMail + "," + unitMail;
                    }
                    else
                    {
                        string Ac_Code = ds.Tables[0].Rows[0]["Ac_Code"].ToString();
                        string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                        string ccMail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                        txtEmail.Text = ccMail + "," + acMail;
                    }
                    ds.Tables[0].Columns.Add(new DataColumn("CT", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("PartyNameC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("PartyAddressC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("party_cityC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("Cst_noC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("Gst_NoC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("Tin_NoC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("Local_Lic_NoC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("ECC_NoC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("CompanyPanC", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("BrokerShortNew", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("driver_no", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("SRPerKg", typeof(string)));
                    string vno = ds.Tables[0].Rows[0]["#"].ToString();
                    string doNo = clsCommon.getString("Select DO_No from " + tblPrefix + "qryVoucherList where Doc_No=" + vno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    string driverMobile = clsCommon.getString("Select driver_no from " + tblPrefix + "deliveryorder where doc_no=" + doNo + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and voucher_no=" + vno + " and voucher_type='OV'");
                    string csNo = clsCommon.getString("select Carporate_Sale_No from " + tblPrefix + "deliveryorder where doc_no=" + doNo + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and voucher_no=" + vno + " and voucher_type='OV'");
                    string podetails = clsCommon.getString("Select PODETAIL from " + tblPrefix + "CarporateSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Doc_No=" + csNo);


                    ds.Tables[0].Columns.Add(new DataColumn("driverMobile", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("PODetails", typeof(string)));
                    ds.Tables[0].Rows[0]["driverMobile"] = driverMobile;
                    ds.Tables[0].Rows[0]["PODetails"] = podetails;

                    ds.Tables[0].Columns.Add(new DataColumn("InWords", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("party_city", typeof(string)));
                    ds.Tables[0].Columns.Add(new DataColumn("party_state", typeof(string)));
                    string partyCityCode = ds.Tables[0].Rows[0]["City_Code"].ToString();
                    string partyCity = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string partyState = clsCommon.getString("select state from " + cityMasterTable + " where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    ds.Tables[0].Rows[0]["party_city"] = partyCity;
                    ds.Tables[0].Rows[0]["party_state"] = partyState;



                    ds.Tables[0].Columns.Add(new DataColumn("CmpName", typeof(string)));
                    ds.Tables[0].Rows[0]["CmpName"] = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string VoucherAmount = ds.Tables[0].Rows[0]["VoucherAmount"].ToString();
                    ds.Tables[0].Rows[0]["InWords"] = clsNoToWord.ctgword(VoucherAmount);
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Local_Lic_No"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["Local_Lic_No"] = "LIC No: " + ds.Tables[0].Rows[0]["Local_Lic_No"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Tin_No"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["Tin_No"] = "TIN: " + ds.Tables[0].Rows[0]["Tin_No"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Cst_no"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["Cst_no"] = "CST: " + ds.Tables[0].Rows[0]["Cst_no"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Gst_no"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["Gst_no"] = "GST: " + ds.Tables[0].Rows[0]["Gst_no"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["CompanyPan"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["CompanyPan"] = "PAN: " + ds.Tables[0].Rows[0]["CompanyPan"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ECC_No"].ToString()))
                    {
                        ds.Tables[0].Rows[0]["ECC_No"] = "ECC: " + ds.Tables[0].Rows[0]["ECC_No"].ToString();
                    }



                    string ac_code = ds.Tables[0].Rows[0]["Ac_Code"].ToString();
                    string unit_code = ds.Tables[0].Rows[0]["Unit_Code"].ToString();


                    //string Do_No = ds.Tables[0].Rows[0]["DO_No"].ToString();

                    //string Driver_no = clsCommon.getString("Select driver_no from " + tblPrefix + "deliveryorder where doc_no=" + Do_No + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                    //if (!string.IsNullOrWhiteSpace(Driver_no))
                    //{
                    //    ds.Tables[0].Rows[0]["driver_no"] = "Driver Mobile:" + Driver_no;
                    //}

                    //ViewState["Qntl"] = ds.Tables[0].Rows[0]["Quantal"].ToString();
                    //ViewState["lorry"] = ds.Tables[0].Rows[0]["Lorry_No"].ToString();
                    //ViewState["PartyName"] = ds.Tables[0].Rows[0]["PartyName"].ToString();
                    if (ac_code != unit_code)
                    {
                        if (unit_code != "0")
                        {
                            ds.Tables[0].Rows[0]["CT"] = "Consigned To,";
                            string PartyNameC = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string PartyAddressC = clsCommon.getString("Select Address_E from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string city_code = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string party_cityC = clsCommon.getString("Select 'City:<b>'+city_name_e+'</b>&nbsp;&nbsp;&nbsp;State:<b>'+state+'</b>' from " + tblPrefix + "CityMaster where city_code=" + city_code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string Cst_noC = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string Gst_NoC = clsCommon.getString("Select Gst_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string Tin_NoC = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string Local_Lic_NoC = clsCommon.getString("Select Local_Lic_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string ECC_NoC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string CompanyPanC = clsCommon.getString("Select CompanyPan from " + tblPrefix + "AccountMaster where Ac_Code=" + unit_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            ViewState["PartyName"] = PartyNameC;
                            ds.Tables[0].Rows[0]["PartyNameC"] = PartyNameC;
                            ds.Tables[0].Rows[0]["PartyAddressC"] = PartyAddressC;
                            ds.Tables[0].Rows[0]["party_cityC"] = party_cityC;

                            if (!string.IsNullOrWhiteSpace(Local_Lic_NoC))
                            {
                                ds.Tables[0].Rows[0]["Local_Lic_NoC"] = "LIC No: " + Local_Lic_NoC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["Local_Lic_NoC"] = Local_Lic_NoC;
                            }
                            if (!string.IsNullOrWhiteSpace(Tin_NoC))
                            {
                                ds.Tables[0].Rows[0]["Tin_NoC"] = "TIN: " + Tin_NoC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["Tin_NoC"] = Tin_NoC;
                            }
                            if (!string.IsNullOrWhiteSpace(Cst_noC))
                            {
                                ds.Tables[0].Rows[0]["Cst_noC"] = "CST: " + Cst_noC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["Cst_noC"] = Cst_noC;
                            }
                            if (!string.IsNullOrWhiteSpace(Gst_NoC))
                            {
                                ds.Tables[0].Rows[0]["Gst_NoC"] = "GST: " + Gst_NoC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["Gst_NoC"] = Gst_NoC;
                            }
                            if (!string.IsNullOrWhiteSpace(CompanyPanC))
                            {
                                ds.Tables[0].Rows[0]["CompanyPanC"] = "PAN: " + CompanyPanC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["CompanyPanC"] = CompanyPanC;
                            }
                            if (!string.IsNullOrWhiteSpace(ECC_NoC))
                            {
                                ds.Tables[0].Rows[0]["ECC_NoC"] = "ECC: " + ECC_NoC;
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["ECC_NoC"] = ECC_NoC;
                            }
                        }
                    }

                    double vamt = Convert.ToDouble(VoucherAmount);
                    double qntl = Convert.ToDouble(ds.Tables[0].Rows[0]["Qntl"].ToString());

                    double perKg = Math.Round((vamt / 100 / qntl), 2);
                    ds.Tables[0].Rows[0]["SRPerKg"] = "Sale Rate: " + ds.Tables[0].Rows[0]["SR"].ToString() + " (" + perKg + "/Kg)";


                    dt = new DataTable();
                    dt = ds.Tables[0];
                    DataTable dt2 = new DataTable();
                    dt2 = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //lblSubjToCity.Text = "Subject to <b>" + clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())) + " </b>jurisdiction";
                        //lblCompanyMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblCmpAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblCmpCityName.Text = clsCommon.getString("Select (City_E+','+State_E) from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        dtlist.DataSource = dt;
                        dtlist.DataBind();
                        dtlist2.DataSource = dt2;
                        dtlist2.DataBind();
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
                    contentType.Name = "ITC.htm";
                    Attachment attachment = new Attachment(ms, contentType);

                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    SmtpServer.Host = clsGV.Email_Address;
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(email);
                    msg.Body = "ITC Voucher ";
                    msg.Attachments.Add(attachment);
                    msg.IsBodyHtml = true;
                    msg.Subject = "ITC Voucher " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
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
                //msg.Body = "Form E";
                //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                //msg.IsBodyHtml = true;
                ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";

                //msg.Subject = "Carporate Voucher " + "ON" + " " + DateTime.Now.ToString("dd/MM/yyyy");
                //msg.IsBodyHtml = true;
                //if (smtpPort != string.Empty)
                //{
                //    SmtpServer.Port = Convert.ToInt32(smtpPort);
                //}
                //                    SmtpServer.EnableSsl = true;
                ////SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                ////System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                ////    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                ////    System.Security.Cryptography.X509Certificates.X509Chain chain,
                ////    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                ////{
                ////    return true;
                ////};
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