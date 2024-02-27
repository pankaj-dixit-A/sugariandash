using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

public partial class Sugar_MyLittleAdmin : System.Web.UI.Page
{
    string qry = string.Empty;
    DataSet ds = null;
    DataTable dt = null;
    public string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyLittleConstring"].ConnectionString;
    public SqlConnection _connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyLittleConstring"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            this.FillTree();
        }
    }

    public void FillTree()
    {
        try
        {
            TreeNode ParentNode = null;
            qry = "select name,database_id from sys.databases";
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                this.PopulateTreeView(dt, 0, null);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
    {
        foreach (DataRow row in dtParent.Rows)
        {
            TreeNode child = new TreeNode
            {
                Text = row["name"].ToString(),
                Value = row["database_id"].ToString()
            };
            if (parentId == 0)
            {
                //qry = "SELECT c.name 'Column_Name',t.Name 'Data_Type',c.max_length 'Max Length',c.precision , c.scale ,c.is_nullable,ISNULL(i.is_primary_key, 0) 'Primary Key'" +
                //    " FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id" +
                //    " LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id WHERE" +
                //    " c.object_id = OBJECT_ID('" + row["database_id"].ToString() + "')";
                
                qry = "select * from sys.tables";
                treeview1.Nodes.Add(child);
                SqlCommand cmd = new SqlCommand(qry, _connection);
                cmd.CommandType = CommandType.Text;
                DataSet ds2 = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds2);

                if (ds2 != null)
                {
                    DataTable dtChild = ds2.Tables[0];
                    PopulateTreeView(dtChild, int.Parse(child.Value), child);
                }
            }
            else
            {
                treeNode.ChildNodes.Add(child);
            }
        }
    }
}