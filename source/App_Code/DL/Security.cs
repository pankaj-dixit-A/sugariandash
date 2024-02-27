using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Security
/// </summary>
public class Security
{
    public Security()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string Authenticate(string tblPrefix, string user)
    {
        string isAuthenticate = string.Empty;

        string qry = string.Empty;
        DataSet ds = new DataSet();
        //string user = clsGV.user;
        string userid = clsCommon.getString("Select User_id from  tblUser  WHERE User_Name='" + user + "'");
        string pagename = HttpContext.Current.Request.Url.AbsolutePath;
        int fromStr = pagename.LastIndexOf(@"/");

        string trim = pagename.Substring(1, fromStr - 1);
        int len = trim.LastIndexOf(@"/");
        string folderName = trim.Substring(len + 1);
        string ext = pagename.Substring(fromStr + 1);
        int toStr = ext.IndexOf(@".");
        string pgeName = ext.Substring(0, toStr);

        string pageid = clsCommon.getString("Select Page_ID from " + tblPrefix + "ApplicationPages where Name='" + pgeName + "'");
        int pgid = pageid != string.Empty ? Convert.ToInt32(pageid) : 0;

        if (pgid == 0)
        {
            string UnderId = clsCommon.getString("Select Under_id from " + tblPrefix + "Under where Name='" + folderName + "'");

            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
            {
                string rev = "";
                obj.flag = 1;
                obj.tableName = tblPrefix + "ApplicationPages";
                obj.columnNm = "Name,Under_id";
                obj.values = "'" + pgeName + "','" + UnderId + "'";
                ds = obj.insertAccountMaster(ref rev);

            }

        }
        else
        {
            qry = "Select * from " + tblPrefix + "Authorization where PageID=" + Convert.ToInt32(pgid) + " AND User_id=" + userid + "";
            ds = clsDAL.SimpleQuery(qry);
            DataTable dt = new DataTable();
            dt.Columns.Add("IsAuthenticate", typeof(int));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["IsAuthenticate"] = ds.Tables[0].Rows[i]["IsAuthenticate"].ToString();
                        dt.Rows.Add(dr);
                        isAuthenticate = dt.Rows[i]["IsAuthenticate"].ToString();
                    }
                }
                else
                {
                    isAuthenticate = "0";
                }
            }
        }
        return isAuthenticate;
    }
}