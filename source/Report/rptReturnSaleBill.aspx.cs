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
using System.Web.UI.HtmlControls;
using System.Net.Mime;

public partial class Report_rptReturnSaleBill : System.Web.UI.Page
{
    string f = "../GSReports/SalesBill_.htm";
    string f_Main = "../Report/rptsalebill";
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
        qryCommon = tblPrefix + "qrySugarSaleList";
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
                qry = "select s.doc_no as #,CONVERT(VARCHAR(10),s.doc_date,103) as dt,ISNULL(DO_No,0) as DO,s.FROM_STATION as From_Place,a.Pincode as Party_Pin,s.TO_STATION as To_Place,s.LORRYNO as lorry,s.wearhouse,s.subTotal as Sub_Total,ISNULL(s.freight,0.00) as Less_Frieght," +
                    " ISNULL(s.cash_advance,0.00) as Cash_Advance,ISNULL(s.bank_commission,0.00) as Bank_Commission,ISNULL(s.OTHER_AMT,0.00) as Other_Expenses,s.Bill_Amount as Bill_Amount,a.Ac_Name_E as Party_Name,a.Address_E as Party_Address," +
                    " a.Local_Lic_No as Party_SLN,a.Tin_No as Party_TIN,a.ECC_No as Party_Ecc,a.Cst_no as Party_Cst,a.Gst_No as Party_Gst,a.CompanyPan as Party_PAN,c.city_name_e as Party_City,c.state as Party_State,b.Ac_Name_E as Mill_Name,a.Email_Id,a.Email_Id_cc,('Off.Phone: <b>'+a.OffPhone+'</b> &nbsp;&nbsp;Mobile: <b>'+a.Mobile_No+'</b>') as Party_Phone from " + tblPrefix + "SugarSaleReturn s" +
                    " left outer join " + tblPrefix + "AccountMaster a on s.Ac_Code=a.Ac_Code and s.Company_Code=a.Company_Code left outer join " + tblPrefix + "AccountMaster b on s.mill_code=b.Ac_Code and s.Company_Code=b.Company_Code" +
                    " left outer join " + tblPrefix + "CityMaster c on a.City_Code=c.city_code and a.Company_Code=c.company_code where s.doc_no IN(" + billno + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add(new DataColumn("PODetails", typeof(string)));
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
                        if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[i]["Party_PAN"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["Party_PAN"] = "&nbsp;&nbsp;PAN: " + ds.Tables[0].Rows[i]["Party_PAN"].ToString();
                        }
                    }

                    string DO_no = clsCommon.getString("Select DO_No from " + tblPrefix + "SugarSaleReturn where doc_no=" + billno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                    string carporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where doc_no=" + DO_no + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                    string delivery_type = clsCommon.getString("Select Delivery_Type from " + tblPrefix + "deliveryorder where doc_no=" + DO_no + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");

                    if (!string.IsNullOrEmpty(carporateNo))
                    {
                        string SellingType = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + carporateNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        string PODetails = clsCommon.getString("Select PODETAIL from " + tblPrefix + "CarporateSale where Doc_No=" + carporateNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        if (PODetails != string.Empty)
                        {
                            ds.Tables[0].Rows[0]["PODetails"] = "PO Details: <b>" + PODetails + "</b>";
                        }
                        if (SellingType == "P")
                        {
                            ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                            ds.Tables[0].Rows[0]["Cash_Advance"] = 0.00;
                        }
                        else
                        {
                            if (delivery_type == "C")
                            {
                                ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                            }
                        }
                    }
                    else
                    {
                        if (delivery_type == "C")
                        {
                            ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                        }
                    }
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    DataTable dt1 = new DataTable();
                    dt1 = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtEmail.Text = dt.Rows[0]["Email_Id_cc"].ToString() + "," + dt.Rows[0]["Email_Id"].ToString();
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
            Label lblCmptinNo = (Label)e.Item.FindControl("lblCmptinNo");
            //Label lblCmpMobile = (Label)e.Item.FindControl("lblCmpMobile");
            lblCmptinNo.Text = clsCommon.getString("select TIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
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

            Label lblInwords = (Label)e.Item.FindControl("lblInwords");
            Label lblNameCmp = (Label)e.Item.FindControl("lblNameCmp");
            DataList dtItemDetails = (DataList)e.Item.FindControl("dtItemDetails");
            string sbNo = lblSB_No.Text;
            lblInwords.Text = clsNoToWord.ctgword(lblBillAmount.Text);


            string city = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string Pin = clsCommon.getString("Select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("Select State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            //lblCityStatePin.Text = city + " (" + Pin + ") " + state;
            //lblCmpMobile.Text = clsCommon.getString("Select Mobile_No from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //lblCompanyName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //lblCompanyAddress.Text = clsCommon.getString("Select Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            qry = " select s.System_Name_E+' '+ISNULL(d.narration,'') as Item  ,d.bags as Bags,d.packing as Packing,d.Quantal as Qntl,d.rate as Rate,d.item_Amount as Value from " + tblPrefix + "sugarsaleDetailsReturn d" +
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


            Image imgSign = (Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            imgSign.ImageUrl = imgurl;

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
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter tw = new HtmlTextWriter(sw);
            //pnlMain.RenderControl(tw);
            //string s = sw.ToString();
            //s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");

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
                contentType.Name = "SaleBill.htm";
                Attachment attachment = new Attachment(ms, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(email);
                msg.Body = "Sale Bill";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "Sales Bill Report" + DateTime.Now.ToString("dd/MM/yyyy");
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
            //catch (Exception ee)
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
            //msg.Body = "Sales Bill";
            //msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            //msg.IsBodyHtml = true;
            ////msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            //msg.Subject = "Sales Bill Report" + DateTime.Now.ToString("dd/MM/yyyy");
            //msg.IsBodyHtml = true;
            //msg.IsBodyHtml = true;
            //if (smtpPort != string.Empty)
            //{
            //    SmtpServer.Port = Convert.ToInt32(smtpPort);
            //}
            //                    SmtpServer.EnableSsl = true;
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            //SmtpServer.Send(msg);
        }
        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }
    protected void btnPriPrinted_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }
}