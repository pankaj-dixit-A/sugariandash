using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;

public partial class Report_pgeVoucherPrint : System.Web.UI.Page
{
    string f = "../GSReports/voucher.htm";
    string f_Main = "../GSReports/voucher";
    string tblHead = "";
    string tblPrefix = string.Empty;
    string AccountMasterTable = string.Empty;
    static WebControl objAsp = null;
    string searchstring = string.Empty;
    string user = string.Empty;
    string strTextBox = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();//"NT_1_";
        tblHead = tblPrefix + "Voucher";
        user = Session["user"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                //this.fillGrid();     
                pnlVNo.Visible = false;
                pnlParty.Visible = false;
                pnldate.Visible = true;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }

        if (objAsp != null)
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);

        if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
        {
            pnlPopup.Style["display"] = "none";
        }
        else
        {
            pnlPopup.Style["display"] = "block";
            objAsp = btnSearch;
        }
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        try
        {
            string whrCondition = "";
            string s_item = "";
            string vtype = drpVoucherType.SelectedValue.ToString();
            if (drpFilter.SelectedValue == "D")
            {
                s_item = "D";
                string fromDt = "";
                string toDt = "";
                if (txtFromDt.Text != string.Empty)
                {
                    fromDt = DateTime.Parse(txtFromDt.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    return;
                }
                if (txtToDt.Text != string.Empty)
                {
                    toDt = DateTime.Parse(txtToDt.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    return;
                }
                whrCondition = "where Tran_Type='" + vtype + "' and Doc_Date between '" + fromDt + "' and '" + toDt + "'";
            }
            if (drpFilter.SelectedValue == "P")
            {
                s_item = "P";
                string fromDt = "";
                string toDt = "";
                string party = "";
                if (txtDtFrom1.Text != string.Empty)
                {
                    fromDt = DateTime.Parse(txtDtFrom1.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    return;
                }
                if (txtDtTo1.Text != string.Empty)
                {
                    toDt = DateTime.Parse(txtDtTo1.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                }
                else
                {
                    return;
                }
                if (txtPary.Text != string.Empty)
                {
                    party = txtPary.Text;
                }
                else
                {
                    return;
                }
                whrCondition = "where Tran_Type='" + vtype + "' and Doc_Date between '" + fromDt + "' and '" + toDt + "' and " + tblHead + ".Ac_Code=" + party;
            }
            if (drpFilter.SelectedValue == "V")
            {
                s_item = "V";
                string fromNo = "";
                string toNo = "";
                if (txtVNoFrom.Text != string.Empty)
                {
                    fromNo = txtVNoFrom.Text;
                }
                else
                {
                    return;
                }
                if (txtVNoTo.Text != string.Empty)
                {
                    toNo = txtVNoTo.Text;
                }
                else
                {
                    return;
                }
                whrCondition = "where Tran_Type='" + vtype + "' and Doc_No between " + fromNo + " and " + toNo;
            }

            this.fillGrid(whrCondition);
        }
        catch
        {

        }
    }

    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string s_item = "";
            s_item = drpFilter.SelectedValue;
            if (s_item != string.Empty)
            {
                if (s_item == "D")
                {
                    pnldate.Visible = true;
                    pnlVNo.Visible = false;
                    pnlParty.Visible = false;
                }
                if (s_item == "V")
                {
                    pnldate.Visible = false;
                    pnlVNo.Visible = true;
                    pnlParty.Visible = false;
                }
                if (s_item == "P")
                {
                    pnldate.Visible = false;
                    pnlVNo.Visible = false;
                    pnlParty.Visible = true;
                }
            }
            txtDtFrom1.Text = "";
            txtToDt.Text = "";
            txtPary.Text = "";
            txtFromDt.Text = "";
            txtDtTo1.Text = "";
            txtVNoFrom.Text = "";
            txtVNoTo.Text = "";
        }
        catch
        {

        }
    }

    protected void txtDtFrom1_TextChanged(object sender, EventArgs e)
    {
        hdnfClosePopup.Value = "Close";
        setFocusControl(txtDtTo1);
    }
    protected void btnParty_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtParty";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    private void fillGrid(string whr)
    {
        try
        {
            string qry = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            qry = "SELECT     " + tblHead + ".Doc_No, Convert(varchar(10)," + tblHead + ".Doc_Date,103) as Voucher_Date, Party.Ac_Name_E AS [Party Name], Mill.Ac_Name_E AS [Mill Name], " + tblHead + ".Quantal, " +
                " " + tblHead + ".Mill_Rate, " + tblHead + ".Voucher_Amount, Party.Email_Id,Party.Mobile_No as PartyMobile," + tblHead + ".Mill_Code" +
                " FROM  " + tblPrefix + "Voucher LEFT OUTER JOIN " +
                " " + AccountMasterTable + " AS Party ON " + tblHead + ".Company_Code = Party.Company_Code AND " + tblHead + ".Ac_Code = Party.Ac_Code LEFT OUTER JOIN " +
                " " + AccountMasterTable + " AS Mill ON " + tblHead + ".Company_Code = Mill.Company_Code AND " + tblHead + ".Mill_Code = Mill.Ac_Code ";
            qry = qry + " " + whr + " and  " + tblHead + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and  " + tblHead + ".Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Party.Ac_Name_E ";

            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                    }
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                    }
                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }
            }
            else
            {
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }

        }
        catch
        {
            grdDetail.DataSource = null;
            grdDetail.DataBind();
        }
    }

    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 1;
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[2].ControlStyle.Width = Unit.Pixel(120);
            e.Row.Cells[3].Style["overflow"] = "hidden";

            e.Row.Cells[3].ControlStyle.Width = Unit.Pixel(120);
            e.Row.Cells[4].ControlStyle.Width = Unit.Pixel(50);
            e.Row.Cells[5].ControlStyle.Width = Unit.Pixel(50);
            e.Row.Cells[6].ControlStyle.Width = Unit.Pixel(50);
            e.Row.Cells[0].ControlStyle.Width = Unit.Pixel(30);
            e.Row.Cells[9].ControlStyle.Width = Unit.Pixel(20);
            e.Row.Cells[8].ControlStyle.Width = Unit.Pixel(90);
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].ControlStyle.Width = Unit.Pixel(90);
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[1].ControlStyle.Width = Unit.Pixel(50);
            e.Row.Cells[10].ControlStyle.Width = Unit.Pixel(30);
            e.Row.Cells[11].ControlStyle.Width = Unit.Pixel(20);
            if (e.Row.RowType != DataControlRowType.Header)
                e.Row.Cells[10].Text = "N";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Style["overflow"] == "hidden")
                    {
                        cell.ToolTip = cell.Text;
                    }
                }
            }
        }
        catch
        {

        }
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtParty")
            {
                if (searchstring != string.Empty)
                {
                    txtSearchText.Text = searchstring;
                }
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E from " + AccountMasterTable +
                " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Popup Button Code]
    protected void showPopup(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdPopup.DataSource = dt;
                        grdPopup.DataBind();
                        hdnfPageCount.Value = grdPopup.PageCount.ToString();

                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdnfPageCount.Value = "0";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "Close")
            {
                txtSearchText.Text = string.Empty;
                pnlPopup.Style["display"] = "none";
                grdPopup.DataSource = null;
                grdPopup.DataBind();
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        int i = 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

        }

    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
           e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion


    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("VNO", typeof(string));
            dtTemp.Columns.Add("mailID", typeof(string));


            string voucherno = "";
            string mailID = "";
            string millcode = "";

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
                if (chk.Checked)
                {
                    mailID = "ankushptl22@rediffmail.com";// grdDetail.Rows[i].Cells[7].Text;

                    if (voucherno == string.Empty)
                    {
                        voucherno = grdDetail.Rows[i].Cells[0].Text;
                        millcode = grdDetail.Rows[i].Cells[11].Text;
                    }
                    else
                    {
                        if (millcode == grdDetail.Rows[i].Cells[11].Text)
                        {
                            voucherno = voucherno + "," + grdDetail.Rows[i].Cells[0].Text;
                        }
                        else
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["VNO"] = voucherno;
                            dr["mailID"] = mailID;
                            dtTemp.Rows.Add(dr);


                            voucherno = grdDetail.Rows[i].Cells[0].Text;
                            millcode = grdDetail.Rows[i].Cells[11].Text;

                            if (i == grdDetail.Rows.Count - 1)
                            {
                                dr = dtTemp.NewRow();
                                dr["VNO"] = voucherno;
                                dr["mailID"] = mailID;
                                dtTemp.Rows.Add(dr);
                            }
                        }
                    }
                }
                else
                {
                    if (voucherno != "")
                    {
                        DataRow dr = dtTemp.NewRow();
                        dr = dtTemp.NewRow();
                        dr["VNO"] = voucherno;
                        dr["mailID"] = mailID;
                        dtTemp.Rows.Add(dr);
                        voucherno = "";
                    }
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Select Record To Send!')", true);
                }
            }
            //mail sending code
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                string pageBreak = "Y";
                if (chkPageBreak.Checked == true)
                {
                    pageBreak = "Y";
                }
                else
                {
                    pageBreak = "N";
                }

                ifrVoucher.Attributes["src"] = "rptVouchersNew.aspx?VNO=" + dtTemp.Rows[i]["VNO"].ToString() + "&mailID=" + dtTemp.Rows[i]["mailID"].ToString() + "&pageBreak=" + pageBreak;
                ifrVoucher.DataBind();
                string src = "rptVouchersNew.aspx?VNO=" + dtTemp.Rows[i]["VNO"].ToString() + "&mailID=" + dtTemp.Rows[i]["mailID"].ToString() + "&pageBreak=" + pageBreak;

                string urltoRead = HttpContext.Current.Request.Url.AbsoluteUri;
                string urll = Request.Url.ToString();
                int j = urltoRead.LastIndexOf('/');
                urltoRead = urltoRead.Substring(0, j + 1);
                urltoRead = urltoRead + src;
                txtFrameSrc.Text = urltoRead;
                WebClient client = new WebClient();
                Stream data = client.OpenRead(urltoRead);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                data.Close();
                reader.Close();

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
                    // Response.Write("file error:"+ee.Message);
                    f = f_Main + DateTime.Now.ToString("ss") + ".htm";
                    using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(s);
                        }
                    }
                }

                try
                {
                    string mailFrom = Session["EmailId"].ToString();
                    string smtpPort = "587";
                    string emailPassword = Session["EmailPassword"].ToString();
                    MailMessage msg = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                    msg.From = new MailAddress(mailFrom);
                    msg.To.Add(mailID);
                    msg.Body = "Voucher";
                    msg.Attachments.Add(new Attachment(Server.MapPath(f)));
                    msg.IsBodyHtml = true;
                    //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
                    msg.Subject = "Voucher Report  " + DateTime.Now.ToString("dd/MM/yyyy");
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
                //mail sent
            }

            #region [scrap]
            //for (int i = 0; i < grdDetail.Rows.Count; i++)
            //{

            //    CheckBox chk = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
            //    if (chk.Checked)
            //    {



            //        ifrVoucher.Attributes["src"] = "rptVoucher.aspx?VNO=" + voucherno + "&mailID=" + mailID;
            //        ifrVoucher.DataBind();
            //        string src = "rptVoucher.aspx?VNO=" + voucherno + "&mailID=" + mailID;

            //        string urltoRead = HttpContext.Current.Request.Url.AbsoluteUri;
            //        string urll = Request.Url.ToString();
            //        int j = urltoRead.LastIndexOf('/');
            //        urltoRead = urltoRead.Substring(0, j + 1);
            //        urltoRead = urltoRead + src;
            //        txtFrameSrc.Text = urltoRead;
            //        WebClient client = new WebClient();
            //        Stream data = client.OpenRead(urltoRead);


            //        StreamReader reader = new StreamReader(data);
            //        string s = reader.ReadToEnd();

            //        data.Close();
            //        reader.Close();
            //        try
            //        {
            //            using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            //            {
            //                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            //                {
            //                    w.WriteLine(s);
            //                }
            //            }
            //            Response.Write("<script>alert('file write');</script>");
            //        }
            //        catch (Exception ee)
            //        {
            //            // Response.Write("file error:"+ee.Message);
            //            f = f_Main + DateTime.Now.ToString("ss") + ".htm";
            //            using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            //            {
            //                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            //                {
            //                    w.WriteLine(s);
            //                }
            //            }
            //        }

            //        string mailFrom = Session["EmailId"].ToString();// clsGV.Email_Address;
            //        string smtpPort = "25";//clsGV.smtpServerPort;// Session["smtpServerPort"].ToString();
            //        string emailPassword = Session["EmailPassword"].ToString();// Session["EmailPassword"].ToString();

            //        MailMessage msg = new MailMessage();

            //        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //        SmtpServer.Host = clsGV.Email_Address;

            //        //                          SmtpServer.EnableSsl = true;
            //        msg.From = new MailAddress(mailFrom);
            //        msg.To.Add(mailID);
            //        //mail.To.Add("toaddress2@gmail.com");
            //        msg.Body = "Voucher Report";
            //        msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            //        msg.IsBodyHtml = true;
            //        msg.Subject = "Voucher Report  " + DateTime.Now.ToString("dd/MM/yyyy");

            //        msg.IsBodyHtml = true;
            //        if (smtpPort != string.Empty)
            //        {
            //            SmtpServer.Port = Convert.ToInt32(smtpPort);
            //        }
            //        SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            //        //                      SmtpServer.EnableSsl = true;
            //        SmtpServer.Send(msg);
            //    }
            //    //mail sent
            //    grdDetail.Rows[i].Cells[10].Text = "Sent";
            //}

            #endregion

        }
        catch (Exception e1)
        {
            Response.Write("mail err:" + e1);
            //Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
    }

    protected void chkselectAll_checkchanged(object sender, EventArgs e)
    {
        //try
        //{
        //    CheckBox chkselectAll = (CheckBox)grdDetail.HeaderRow.Cells[9].FindControl("chkSelectAll");

        //    for (int i = 0; i < grdDetail.Rows.Count; i++)
        //    {
        //        CheckBox chkIsPrint = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
        //        if (chkselectAll.Checked)
        //        {
        //            chkIsPrint.Checked = true;
        //        }
        //        else
        //        {
        //            chkIsPrint.Checked = false;
        //        }
        //    }
        //}
        //catch
        //{
        //}
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("VNO", typeof(string));
            dtTemp.Columns.Add("mailID", typeof(string));
            string vtype = drpVoucherType.SelectedValue.ToString();
            string voucherno = "";
            string mailID = "";
            string millcode = "";
            string VNO = "";

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
                if (chk.Checked)
                {
                    mailID = grdDetail.Rows[i].Cells[7].Text;
                    VNO = VNO + grdDetail.Rows[i].Cells[0].Text + ",";
                    DataRow dr = dtTemp.NewRow();
                    dr = dtTemp.NewRow();
                    dr["VNO"] = voucherno;
                    dr["mailID"] = mailID;
                    dtTemp.Rows.Add(dr);
                    voucherno = "";
                }
            }

            VNO = VNO.Substring(0, VNO.Length - 1);

            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{
            string pageBreak = "Y";
            if (chkPageBreak.Checked == true)
            {
                pageBreak = "Y";
            }
            else
            {
                pageBreak = "N";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + VNO + "','','" + pageBreak + "','" + vtype + "')", true);
            //}
        }
        catch (Exception exc)
        {
            Response.Write(exc.Message);
        }
    }
    protected void txtPary_TextChanged(object sender, EventArgs e)
    {
        searchstring = txtPary.Text;
        strTextBox = "txtPary";
        csCalculation();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
    private void csCalculation()
    {
        if (strTextBox == "txtPary")
        {
            if (txtPary.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPary.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    lblPartyName.Text = str;
                    setFocusControl(txtPary);
                }
                else
                {
                    lblPartyName.Text = string.Empty;
                    txtPary.Text = string.Empty;
                    setFocusControl(txtPary);
                }
            }
            else
            {
                lblPartyName.Text = string.Empty;
                txtPary.Text = string.Empty;
                setFocusControl(txtPary);
            }
        }
    }
    protected void btnMailToEach_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("VNO", typeof(string));
            dtTemp.Columns.Add("mailID", typeof(string));
            string type = drpVoucherType.SelectedValue.ToString();
            string voucherno = string.Empty;
            string mailID = string.Empty;
            //string qryCommon = tblPrefix + "qryVoucherList";

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdDetail.Rows[i].Cells[9].FindControl("chkIsPrint");
                if (chk.Checked)
                {
                    mailID = Server.HtmlEncode(grdDetail.Rows[i].Cells[7].Text);
                    if (!string.IsNullOrEmpty(mailID.Trim().ToString()))
                    {
                        voucherno = grdDetail.Rows[i].Cells[0].Text;
                        DataRow dr = dtTemp.NewRow();
                        dr["VNO"] = voucherno;
                        dtTemp.Rows.Add(dr);
                    }
                }
            }

            if (dtTemp.Rows.Count > 0)
            {
                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {
                    string docNo = dtTemp.Rows[k]["VNO"].ToString();
                    mailID = BindVoucher(type, mailID, docNo);
                    SendMail(mailID, docNo);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void SendMail(string mailID, string VNO)
    {
        if (mailID != string.Empty)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //StringBuilder sb = new StringBuilder();
                ////sb.Append("<!DOCTYPE html PUBLIC ?-//W3C//DTD XHTML 1.0 Transitional//EN? ?http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd?>");
                ////sb.Append("<html xmlns=?http://www.w3.org/1999/xhtml?><head runat=?server?>");
                //sb.Append("<html xmlns=?http://www.w3.org/1999/xhtml?><head runat=?server?>");
                //sb.Append(clsGV.printcss);
                //sb.Append("<script type=?text/javascript?>function print_invoice() {var printContents = document.getElementById(?pnl?).innerHTML;");
                //sb.Append("var originalContents = document.body.innerHTML;document.body.innerHTML = printContents;window.print();");
                //sb.Append("document.body.innerHTML = originalContents; }</script>");
                //sb.Append("</head><body class=?printhalf?><form id=?form1? runat=?server?> <div align=?left?><input type=?button? onclick=?print_invoice();? id=?input? value=?Click Here For Print? /></div>");
                //sb.Append("<div align=?center? style=?width:100%;?>");
                //sb.Replace('?', '"');
                //sb.Replace("../", "http://" + clsGV.Website + "/");
                StringWriter sw = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(sw);
                pnlMain.RenderControl(tw);
                string s = sw.ToString();
                s = s.Replace("../Images", "http://" + clsGV.Website + "/Images");
                //sb.Append(s);
                //sb.Append("</div>");
                //sb.Append("</form></body></html>");
                //string a = sb.ToString();

                byte[] array = Encoding.UTF8.GetBytes(s);
                ms.Write(array, 0, array.Length);
                ms.Seek(0, SeekOrigin.Begin);
                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Octet;
                contentType.Name = "Voucher.htm";
                Attachment attachment = new Attachment(ms, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mailID);
                msg.Body = "Voucher";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "V.No:" + Convert.ToString(VNO) + " " + ViewState["lorry"].ToString() + " Qt:" + ViewState["Qntl"].ToString() + " " + ViewState["PartyName"].ToString();
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
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }

    private string BindVoucher(string type, string mailID, string docNo)
    {
        string qry = "select * from " + tblPrefix + "qryVoucherList where Doc_No=" + docNo + " and Tran_Type='" + type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        DataSet dsV = new DataSet();
        DataTable dt = new DataTable();

        dsV = clsDAL.SimpleQuery(qry);
        if (dsV.Tables[0].Rows.Count > 0)
        {
            string DO_No = dsV.Tables[0].Rows[0]["DO_No"].ToString();
            string CarporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no=" + DO_No + " and tran_type='DO'");

            if (CarporateNo != "0")
            {
                string Ac_Code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                string Unit_Code = dsV.Tables[0].Rows[0]["Unit_Code"].ToString();
                string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                string unitMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Unit_Code);
                mailID = acMail + "," + unitMail;
            }
            else
            {
                string Ac_Code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                string acMail = clsCommon.getString("Select Email_Id from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                string ccMail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Ac_Code);
                mailID = ccMail + "," + acMail;
            }

            dt = dsV.Tables[0];
            DataTable dt2 = new DataTable();
            dt2 = dsV.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dtlDetails.DataSource = dt;
                dtlDetails.DataBind();
            }
        }
        return mailID;
    }

    protected void dtlDetails_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtl = (DataList)e.Item.FindControl("dtl");
            Label lblDocno = (Label)e.Item.FindControl("lblDocno");


            string vno = lblDocno.Text;
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            string vtype = lbltype.Text;

            string qry = "select * from " + tblPrefix + "qryVoucherList where Doc_No=" + vno + " and Tran_Type='" + vtype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsV = new DataSet();
            DataTable dt = new DataTable();
            dsV = clsDAL.SimpleQuery(qry);
            if (dsV.Tables[0].Rows.Count > 0)
            {
                dsV.Tables[0].Columns.Add(new DataColumn("CT", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("PartyNameC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("PartyAddressC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_cityC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Cst_noC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Gst_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Tin_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("Local_Lic_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("ECC_NoC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("CompanyPanC", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("BrokerShortNew", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("driver_no", typeof(string)));

                dsV.Tables[0].Columns.Add(new DataColumn("InWords", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_city", typeof(string)));
                dsV.Tables[0].Columns.Add(new DataColumn("party_state", typeof(string)));
                string partyCityCode = dsV.Tables[0].Rows[0]["City_Code"].ToString();
                string partyCity = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string partyState = clsCommon.getString("select state from " + tblPrefix + "CityMaster where city_code='" + partyCityCode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                dsV.Tables[0].Rows[0]["party_city"] = partyCity;
                dsV.Tables[0].Rows[0]["party_state"] = partyState;

                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["ASN_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["ASN_No"] = "ASN / GRN No: " + dsV.Tables[0].Rows[0]["ASN_No"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Local_Lic_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Local_Lic_No"] = "LIC No: " + dsV.Tables[0].Rows[0]["Local_Lic_No"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Tin_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Tin_No"] = "TIN: " + dsV.Tables[0].Rows[0]["Tin_No"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Cst_no"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Cst_no"] = "CST: " + dsV.Tables[0].Rows[0]["Cst_no"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["Gst_no"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["Gst_no"] = "GST: " + dsV.Tables[0].Rows[0]["Gst_no"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["CompanyPan"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["CompanyPan"] = "PAN: " + dsV.Tables[0].Rows[0]["CompanyPan"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dsV.Tables[0].Rows[0]["ECC_No"].ToString()))
                {
                    dsV.Tables[0].Rows[0]["ECC_No"] = "ECC: " + dsV.Tables[0].Rows[0]["ECC_No"].ToString();
                }

                string Delivery_Type = dsV.Tables[0].Rows[0]["Delivery_Type"].ToString();

                if (Delivery_Type == "N")
                {
                    double LESSDIFF = dsV.Tables[0].Rows[0]["Diff_Rate"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Diff_Rate"].ToString()) : 0.00;
                    double BANK_COMMISSION = dsV.Tables[0].Rows[0]["BANK_COMMISSION"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["BANK_COMMISSION"].ToString()) : 0.00;
                    double Brokrage = dsV.Tables[0].Rows[0]["Brokrage"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Brokrage"].ToString()) : 0.00;
                    double RATEDIFF = dsV.Tables[0].Rows[0]["RATEDIFF"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["RATEDIFF"].ToString()) : 0.00;
                    double Commission_Amount = dsV.Tables[0].Rows[0]["Commission_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Commission_Amount"].ToString()) : 0.00;
                    double FREIGHT = dsV.Tables[0].Rows[0]["FREIGHT"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["FREIGHT"].ToString()) : 0.00;
                    double Postage = dsV.Tables[0].Rows[0]["Postage"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Postage"].ToString()) : 0.00;
                    double Interest = dsV.Tables[0].Rows[0]["Interest"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Interest"].ToString()) : 0.00;
                    double Cash_Ac_Amount = dsV.Tables[0].Rows[0]["Cash_Ac_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Cash_Ac_Amount"].ToString()) : 0.00;
                    double OTHER_Expenses = dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString()) : 0.00;
                    double Transport_Amount = dsV.Tables[0].Rows[0]["Transport_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Transport_Amount"].ToString()) : 0.00;

                    double TotalAmount = LESSDIFF + BANK_COMMISSION + Brokrage + RATEDIFF + Commission_Amount + FREIGHT + Postage + Interest + Cash_Ac_Amount + OTHER_Expenses + Transport_Amount;

                    dsV.Tables[0].Rows[0]["FREIGHT"] = TotalAmount;
                    dsV.Tables[0].Rows[0]["Diff_Rate"] = 0.00;
                    dsV.Tables[0].Rows[0]["BANK_COMMISSION"] = 0.00;
                    dsV.Tables[0].Rows[0]["Brokrage"] = 0.00;
                    dsV.Tables[0].Rows[0]["RATEDIFF"] = 0.00;
                    dsV.Tables[0].Rows[0]["Commission_Amount"] = 0.00;
                    dsV.Tables[0].Rows[0]["Postage"] = 0.00;
                    dsV.Tables[0].Rows[0]["Interest"] = 0.00;
                    dsV.Tables[0].Rows[0]["Cash_Ac_Amount"] = 0.00;
                    dsV.Tables[0].Rows[0]["OTHER_Expenses"] = 0.00;

                }
                else
                {
                    double OTHER_Expenses = dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["OTHER_Expenses"].ToString()) : 0.00;
                    double Transport_Amount = dsV.Tables[0].Rows[0]["Transport_Amount"].ToString() != string.Empty ? Convert.ToDouble(dsV.Tables[0].Rows[0]["Transport_Amount"].ToString()) : 0.00;

                    double otherAmount = OTHER_Expenses + Transport_Amount;
                    if (otherAmount != 0)
                    {
                        dsV.Tables[0].Rows[0]["OTHER_Expenses"] = otherAmount;
                    }
                    else
                    {
                        dsV.Tables[0].Rows[0]["OTHER_Expenses"] = 0.00;
                    }
                }


                dsV.Tables[0].Columns.Add(new DataColumn("VoucherNo", typeof(string)));
                double vouchamt = Convert.ToDouble(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());
                dsV.Tables[0].Rows[0]["InWords"] = clsNoToWord.ctgword(dsV.Tables[0].Rows[0]["Voucher_Amount"].ToString());

                string millshort = dsV.Tables[0].Rows[0]["millshortname"].ToString();
                string qntl = dsV.Tables[0].Rows[0]["Quantal"].ToString();
                string SR = dsV.Tables[0].Rows[0]["Sale_Rate"].ToString();
                string broker = dsV.Tables[0].Rows[0]["BrokerShort"].ToString();
                if (broker != "Self" && broker != string.Empty)
                {
                    dsV.Tables[0].Rows[0]["BrokerShortNew"] = "Broker: " + broker;
                }

                string narration = dsV.Tables[0].Rows[0]["Narration1"].ToString();
                string finalNarration = "";
                if (broker != "Self")
                {
                    finalNarration = millshort + "-" + qntl + "-" + SR + "-" + broker;
                }
                else
                {
                    finalNarration = millshort + "-" + qntl + "-" + SR;
                }

                dsV.Tables[0].Rows[0]["Narration1"] = finalNarration;

                string ac_code = dsV.Tables[0].Rows[0]["Ac_Code"].ToString();
                string unit_code = dsV.Tables[0].Rows[0]["Unit_Code"].ToString();

                string Do_No = dsV.Tables[0].Rows[0]["DO_No"].ToString();

                string Driver_no = clsCommon.getString("Select driver_no from " + tblPrefix + "deliveryorder where doc_no=" + Do_No + " and tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                if (!string.IsNullOrWhiteSpace(Driver_no))
                {
                    dsV.Tables[0].Rows[0]["driver_no"] = "Driver Mobile:" + Driver_no;
                }

                ViewState["Qntl"] = dsV.Tables[0].Rows[0]["Quantal"].ToString();
                ViewState["lorry"] = dsV.Tables[0].Rows[0]["Lorry_No"].ToString();
                ViewState["PartyName"] = dsV.Tables[0].Rows[0]["PartyName"].ToString();
                if (ac_code != unit_code)
                {
                    if (unit_code != "0")
                    {
                        dsV.Tables[0].Rows[0]["CT"] = "Consigned To,";
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
                        dsV.Tables[0].Rows[0]["PartyNameC"] = PartyNameC;
                        dsV.Tables[0].Rows[0]["PartyAddressC"] = PartyAddressC;
                        dsV.Tables[0].Rows[0]["party_cityC"] = party_cityC;

                        if (!string.IsNullOrWhiteSpace(Local_Lic_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Local_Lic_NoC"] = "LIC No: " + Local_Lic_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Local_Lic_NoC"] = Local_Lic_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(Tin_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Tin_NoC"] = "TIN: " + Tin_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Tin_NoC"] = Tin_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(Cst_noC))
                        {
                            dsV.Tables[0].Rows[0]["Cst_noC"] = "CST: " + Cst_noC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Cst_noC"] = Cst_noC;
                        }
                        if (!string.IsNullOrWhiteSpace(Gst_NoC))
                        {
                            dsV.Tables[0].Rows[0]["Gst_NoC"] = "GST: " + Gst_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["Gst_NoC"] = Gst_NoC;
                        }
                        if (!string.IsNullOrWhiteSpace(CompanyPanC))
                        {
                            dsV.Tables[0].Rows[0]["CompanyPanC"] = "PAN: " + CompanyPanC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["CompanyPanC"] = CompanyPanC;
                        }
                        if (!string.IsNullOrWhiteSpace(ECC_NoC))
                        {
                            dsV.Tables[0].Rows[0]["ECC_NoC"] = "ECC: " + ECC_NoC;
                        }
                        else
                        {
                            dsV.Tables[0].Rows[0]["ECC_NoC"] = ECC_NoC;
                        }
                    }
                }
                dt = dsV.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dtl.DataSource = dt;
                    dtl.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dtl_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            Label lblSignCmpName = (Label)e.Item.FindControl("lblSignCmpName");
            lblSignCmpName.Text = clsCommon.getString("Select Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

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
            System.Web.UI.WebControls.Image imgSign = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSign");
            string imgurl = clsCommon.getString("Select ImagePath from tblSign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string url = Server.MapPath(imgurl);
            imgSign.ImageUrl = imgurl;
        }
        catch (Exception)
        {
            throw;
        }
    }
}