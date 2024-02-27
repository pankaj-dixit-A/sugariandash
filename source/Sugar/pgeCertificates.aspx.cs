using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

public partial class Sugar_pgeCertificates : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                Bindgrid();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }

    private void Bindgrid()
    {
        try
        {
            qry = "Select Computer_User,IPAddress,CONVERT(varchar(10),Created_Date,103) as Created_Date from tblSecurityCertificate";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        datagrid.DataSource = dt;
                        datagrid.DataBind();
                    }
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void datagrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void datagrid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        datagrid.PageIndex = e.NewPageIndex;
        Bindgrid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gr in datagrid.Rows)
            {
                CheckBox chk = gr.Cells[3].FindControl("grdCB") as CheckBox;
                if (chk.Checked == true)
                {
                    string computeruser = gr.Cells[0].Text.ToString();
                    string ipaddress = gr.Cells[1].Text.ToString();
                    DataSet ds = new DataSet();
                    qry = "delete From tblSecurityCertificate where IPAddress='" + ipaddress + "'";
                    ds = clsDAL.SimpleQuery(qry);
                }
            }
            Bindgrid();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void Upload(object sender, EventArgs e)
    {
        ////Upload and save the file
        //string excelPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
        //FileUpload1.SaveAs(excelPath);

        //string conString = string.Empty;
        //string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        //switch (extension)
        //{
        //    case ".xls": //Excel 97-03
        //        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
        //        break;
        //    case ".xlsx": //Excel 07 or higher
        //        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
        //        break;

        //}
        //conString = string.Format(conString, excelPath);
        //using (OleDbConnection excel_con = new OleDbConnection(conString))
        //{
        //    excel_con.Open();
        //    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
        //    DataTable dtExcelData = new DataTable();

        //    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
        //    dtExcelData.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
        //        new DataColumn("Name", typeof(string)),
        //        new DataColumn("Salary",typeof(decimal)) });

        //    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
        //    {
        //        oda.Fill(dtExcelData);
        //    }
        //    excel_con.Close();

        //    string consString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(consString))
        //    {
        //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //        {
        //            //Set the database table name
        //            sqlBulkCopy.DestinationTableName = "dbo.tblPersons";

        //            //[OPTIONAL]: Map the Excel columns with that of the database table
        //            sqlBulkCopy.ColumnMappings.Add("Id", "PersonId");
        //            sqlBulkCopy.ColumnMappings.Add("Name", "Name");
        //            sqlBulkCopy.ColumnMappings.Add("Salary", "Salary");
        //            con.Open();
        //            sqlBulkCopy.WriteToServer(dtExcelData);
        //            con.Close();
        //        }
        //    }
        //}
    }
}