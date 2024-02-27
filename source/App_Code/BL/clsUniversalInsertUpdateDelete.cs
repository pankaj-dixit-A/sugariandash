using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

public class clsUniversalInsertUpdateDelete:IDisposable
{
    #region[Contructore and destructore]
    public clsUniversalInsertUpdateDelete()
	{
	}
    ~clsUniversalInsertUpdateDelete()
    {
        Dispose();
    }
    #endregion
    #region[variables]
    DataSet ds;
    Hashtable hash;
    public string tableName { get; set;}
    public int flag { get; set; }
    public string columnNm { get; set; }
    public string values { get; set; }
    #endregion
    public DataSet insertAccountMaster(ref string str)
    {
        try
        {
            hash = new Hashtable();
            ds = new DataSet();
            hash.Add("@flag", flag);
            hash.Add("@tableName", tableName);
            hash.Add("@columnNm", columnNm);
            hash.Add("@values", values);
            ds = clsDAL.ExecuteDMLQuery(hash, "Universal_IUD", ref str);
           return ds;
        }
        catch(Exception ex)
        {
            return null;
        }
    }
    public string  insertDO(ref string str)
    {
        try
        {
            hash = new Hashtable();
            ds = new DataSet();
            hash.Add("@flag", flag);
            hash.Add("@tableName", tableName);
            hash.Add("@columnNm", columnNm);
            hash.Add("@values", values);
            ds = clsDAL.ExecuteDMLQuery(hash, "Universal_IUD", ref str);
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message + " " + columnNm+" "+values;
        }
    }
    #region IDisposable Members
    public void Dispose()
    {
        System.GC.SuppressFinalize(this);
    }
    #endregion
}