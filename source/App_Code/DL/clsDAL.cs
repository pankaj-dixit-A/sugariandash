
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;


/// <summary>
/// Summary description for clsDAL
/// </summary>
public class clsDAL
{
    #region --------------------------------- Declaration ---------------------------------

    public static string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    public static SqlConnection _connection = new SqlConnection();
    private static DataSet _ds = null;
    private static DataTable _dt = null;
    private static SqlDataReader _reader = null;
    private static SqlDataAdapter _adapter = null;
    private static SqlCommand _sqlCmd = null;
    private static SqlTransaction _transaction = null;
    private static string _getQuery = string.Empty;
    public static bool _transCheck = false;

    #endregion

    #region --------------------------------- Constructor ------------------------------------------



    public clsDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    ~clsDAL()
    {
        //
        // TODO: Add destructor logic here
        //
        Dispose();
    }
    public void Dispose()
    {
        System.GC.SuppressFinalize(this);
    }

    #endregion

    #region ------------------------------- Private Connection -------------------------------------

    /// <summary>
    /// OPEN THE SPECIFIED CONNECTION
    /// </summary>
    /// <returns></returns>
    /// 

    public static bool OpenConnection()
    {
        try
        {
            if (_connection.State != ConnectionState.Open)
            {
                //   String strConnectionString = String.Empty;
                //strConnectionString = ConfigurationManager.AppSettings["AccountingManagementConnection"].ToString();
                //   strConnectionString="Data Source=WIN-GPRFEELTEST\SQLEXPRESSFT;Initial Catalog=BankNewDB;User ID=gpr_services";
                //   strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string str;
                str = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();
                _connection.ConnectionString = str;
                _connection.Open();
            }
            return true;
        }
        catch (Exception ex)
        {
            //clsException.Publish(ex);
            String strException = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// CLOSE THE CONNECTION IF OPENED
    /// </summary>
    public static void CloseConnection()
    {
        try
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
        catch (Exception e)
        {
            String str;
            str = e.Message;
            //clsException.Publish(e);
        }
    }

    #endregion

    #region --------------------------------- Private Methods ---------------------------------

    /// <summary>
    /// Fill Data Set
    /// </summary>
    /// <param name="strProcedureName">Procudure Name</param>
    /// <returns>Data Set</returns>
    public static DataSet FillDataSet(string strProcedureName)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _adapter = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                _adapter.Fill(_ds);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            _sqlCmd.Dispose();
            _adapter.Dispose();
            CloseConnection();
        }
        return _ds;
    }
    public static DataSet xmlExecuteDMLQryReport1(string strProcedureName, string Xmlfile)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;

                //_sqlCmd.Parameters.AddWithValue("@company_code", company_code).ToString();
                //_sqlCmd.Parameters.AddWithValue("@year_Code", year_Code).ToString();
                //_sqlCmd.Parameters.AddWithValue("@From_Date", From_Date).ToString();
                //_sqlCmd.Parameters.AddWithValue("@To_Date", To_Date).ToString();
                //SqlParameter outParameter = new SqlParameter("@Msg", SqlDbType.VarChar, 255); //New Update
                //outParameter.Direction = ParameterDirection.InputOutput;
                //outParameter.Value = "";
                //_sqlCmd.Parameters.Add(outParameter);
                _sqlCmd.Parameters.AddWithValue("@xmlDocument", Xmlfile).ToString();
                _adapter = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                _adapter.Fill(_ds);
                //Return_Dataset = _sqlCmd.Parameters["@Return_Dataset"].Value.ToString();
                return _ds;

                //_sqlCmd = new SqlCommand(str, _connection);
                //_adapter = new SqlDataAdapter(_sqlCmd);
                //_ds = new DataSet();
                //_adapter.Fill(_ds);
                //return _ds;


            }
        }
        catch (Exception ex)
        {
            clsNoToWord n = new clsNoToWord();
            n.WriteToFile(ex.ToString());
            //output = _sqlCmd.Parameters["@Msg"].Value.ToString();
            throw;
            //return null;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return _ds;
    }
    public static string AuthenticateUser(string strProcedure, string Username, string Password, string msg)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedure, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramUsername = new SqlParameter("@User_name", Username);
                SqlParameter paramPassword = new SqlParameter("@Password", Password);
                _sqlCmd.Parameters.Add(paramUsername);
                _sqlCmd.Parameters.Add(paramPassword);

                SqlDataReader rdr = _sqlCmd.ExecuteReader();
                while (rdr.Read())
                {
                    int RetryAttempts = Convert.ToInt32(rdr["RetryAttempts"]);
                    if (Convert.ToBoolean(rdr["AccountLocked"]))
                    {
                        msg = "Account has locked. Please contact administrator";
                    }
                    else if (RetryAttempts > 0)
                    {
                        int AttemptsLeft = (4 - RetryAttempts);
                        msg = "Invalid user name and/or password. " +
                            AttemptsLeft.ToString() + "attempt(s) left";
                    }
                    else if (Convert.ToBoolean(rdr["Authenticated"]))
                    {
                        msg = "1";
                    }
                }
                rdr.Close();
            }
        }
        catch
        {
        }
        return msg;
    }


    #region[Database's simple Query]
    public static DataSet SimpleQuery(string str)
    {
        //DataSet ds1 = new DataSet();
        try
        {
            //using (clsDataProvider objDataProvider = new clsDataProvider())
            //{
            //    ds1 = objDataProvider.GetDataSet(str);
            //}

            if (OpenConnection())
            {
                _adapter = new SqlDataAdapter(str, _connection);
                _ds = new DataSet();
                _adapter.Fill(_ds);
                return _ds;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            CloseConnection();
        }

    }
    #endregion

    /// <summary>
    /// Fill Data Set
    /// </summary>
    /// <param name="strProcedureName"></param>
    /// <param name="hshtblCollection"></param>
    /// <returns>Data Set</returns>
    /// 
    public static DataSet FillDataSet(string strProcedureName, Hashtable hshtblCollection)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                }
                //SqlParameter outParameter = new SqlParameter("@vchrMsg", SqlDbType.VarChar, 255); //New Update
                // outParameter.Direction = ParameterDirection.Output;
                // outParameter.Value = "";
                // _sqlCmd.Parameters.Add(outParameter);
                _adapter = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                if (_adapter != null)
                {
                    _adapter.Fill(_ds);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            _sqlCmd.Dispose();
            _adapter.Dispose();
            CloseConnection();
        }
        return _ds;
    }

    /// <summary>
    /// Fill Data Table 
    /// </summary> 
    /// <param name="strProcedureName">Procedure Name</param>
    /// <returns>Data Table</returns>
    /// 
    public static DataTable FillDataTable(string strProcedureName)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _adapter = new SqlDataAdapter(_sqlCmd);
                _dt = new DataTable();
                _adapter.Fill(_dt);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
        finally
        {
            _sqlCmd.Dispose();
            _adapter.Dispose();
            CloseConnection();
        }
        return _dt;
    }
    /// <summary>
    /// Fill Data Table 
    /// </summary> 
    /// <param name="strProcedureName">Procedure Name</param>
    /// <param name="hshtblCollection">Hash Table</param>
    /// <returns>Data Table</returns>

    public static DataTable FillDataTable(string strProcedureName, Hashtable hshtblCollection)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);

                _sqlCmd.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                }
                //SqlParameter outParameter = new SqlParameter("@vchrMsg", SqlDbType.VarChar, 200); //New Update
                //outParameter.Direction = ParameterDirection.Output;
                //outParameter.Value = "";
                //_sqlCmd.Parameters.Add(outParameter);
                //_sqlCmd.ExecuteNonQuery();
                _adapter = new SqlDataAdapter(_sqlCmd);
                _dt = new DataTable();
                _adapter.Fill(_dt);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            _sqlCmd.Dispose();
            _adapter.Dispose();
            CloseConnection();
        }
        return _dt;
    }

    /// <summary>
    /// It Executes The Insert/Update/Delete Query.
    /// </summary>
    /// <param name="strProcedureName">Procedure Name</param>
    /// <returns>It Returns Flase for The Exception Else true </returns>

    public static bool ExecuteDMLQuery(string strProcedureName)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.Transaction = _transaction;
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _adapter = new SqlDataAdapter(_sqlCmd);
                //  _sqlCmd.ExecuteNonQuery();

            }
        }
        catch (Exception ex)
        {

            throw;
            return false;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return true;
    }

    public static DataSet xmlExecuteDMLQry(string strProcedureName, string Xmlfile, ref string output, int flag, ref string returnmaxno)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.CommandTimeout = 40;
                //IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                //while (_enumerator.MoveNext())
                //{
                //    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                //}
                _sqlCmd.Parameters.AddWithValue("@xmlDocument", Xmlfile).ToString();
                _sqlCmd.Parameters.AddWithValue("@flag", flag);

                SqlParameter outParameter = new SqlParameter("@Msg", SqlDbType.VarChar, 255); //New Update

                SqlParameter outParameter1 = new SqlParameter("@returnmax", SqlDbType.VarChar, 255);
                outParameter.Direction = ParameterDirection.InputOutput;
                outParameter.Value = "";
                _sqlCmd.Parameters.Add(outParameter);

                outParameter1.Direction = ParameterDirection.InputOutput;
                outParameter1.Value = "";
                _sqlCmd.Parameters.Add(outParameter1);
                _adapter = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                _adapter.Fill(_ds);
                output = _sqlCmd.Parameters["@Msg"].Value.ToString();
                returnmaxno = _sqlCmd.Parameters["@returnmax"].Value.ToString();

            }
        }
        catch (Exception ex)
        {
            clsNoToWord n = new clsNoToWord();
            n.WriteToFile(ex.ToString());
            output = _sqlCmd.Parameters["@Msg"].Value.ToString();
            throw;
            return null;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return _ds;
    }

    /// <summary>
    /// It Executes The Insert/Update/Delete Query.
    /// </summary>
    /// <param name="strProcedureName"> Procedure Name</param>
    /// <param name="hshtblCollection">Hashtable</param>
    /// <returns>It Returns Flase for The Exception Else true </returns>

    public static bool ExecuteDMLQuery(string strProcedureName, Hashtable hshtblCollection)
    {
        try
        {

            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);

                _sqlCmd.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                }
                _adapter = new SqlDataAdapter(_sqlCmd);
                //  _sqlCmd.ExecuteNonQuery();
            }

        }
        catch (Exception ex)
        {

            throw;
            return false;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return true;
    }

    /// <summary>
    /// It Executes The Insert/Update/Delete Query.
    /// </summary>
    /// <param name="strProcedureName"> Procedure Name</param>
    /// <param name="hshtblCollection">Hashtable</param>
    /// <returns>It Returns Object</returns>

    public static string GetWordAtPosition(string text, int position)
    {
        if (text.Length - 1 < position || text[position] == ' ') return null;

        int start = position;
        int end = position;
        while (text[start] != ' ' && start > 0) start--;
        while (text[end] != ' ' && end < text.Length - 1) end++;

        return text.Substring(start == 0 ? 0 : start + 1, end - start - 1);

    }
    [WebMethod]
    public string ExecuteXMLQry(string XML, string status, string spname)
    {
        string msgReturn = "";
        try
        {
            //msgReturn = XML.Length.ToString();
            //   XML.Replace("''", """");
            int pos = 0;
            msgReturn = GetWordAtPosition(XML, pos);
            XDocument XDoc = XDocument.Parse(XML, LoadOptions.None);
            XML = XDoc.ToString(SaveOptions.DisableFormatting);
            //SqlCommand cmd1 = new SqlCommand();

            //cmd1.CommandText = spname;
            //cmd1.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            //string spname = "DeliveryOrder";
            string xmlfile = XML;
            string op = "";
            string returnmaxno = "";
            int flag;

            if (status == "Update")
            {
                flag = 2;
            }
            else if (status == "Save")
            {
                flag = 1;
            }
            else if (status == "Transfer")
            {
                flag = 6;
            }
            else
            {
                flag = 3;
            }
            ds = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);

            if (flag != 3)
            {
                return returnmaxno;
            }
            else
            {
                return op;
            }
        }
        catch (Exception exx)
        {

            return "";
        }
    }
    public static object ExecuteDMLQuery(Hashtable hshtblCollection, string strProcedureName)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                }
                SqlParameter outParameter = new SqlParameter("@vchrMsg", SqlDbType.VarChar, 255); //New Update
                outParameter.Direction = ParameterDirection.InputOutput;
                outParameter.Value = "";
                _sqlCmd.Parameters.Add(outParameter);
                _adapter = new SqlDataAdapter(_sqlCmd);
                //  _sqlCmd.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            throw;
            return _sqlCmd.Parameters["@vchrMsg"].Value;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return _sqlCmd.Parameters["@vchrMsg"].Value;
    }

    /// <summary>
    /// It Executes The Insert/Update/Delete Query.
    /// </summary>
    /// <param name="strProcedureName"> Procedure Name</param>
    /// <param name="hshtblCollection">Hashtable</param>
    /// <returns>It Returns Object</returns>


    //output return

    public static DataSet ExecuteDMLQuery(Hashtable hshtblCollection, string strProcedureName, ref string output)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(strProcedureName, _connection);
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator _enumerator = hshtblCollection.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                }
                SqlParameter outParameter = new SqlParameter("@Msg", SqlDbType.VarChar, 255); //New Update
                outParameter.Direction = ParameterDirection.InputOutput;
                outParameter.Value = "";
                _sqlCmd.Parameters.Add(outParameter);
                _adapter = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                _adapter.Fill(_ds);
                output = _sqlCmd.Parameters["@Msg"].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            clsNoToWord n = new clsNoToWord();
            n.WriteToFile(ex.ToString());
            output = _sqlCmd.Parameters["@Msg"].Value.ToString();
            throw;
            return null;
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return _ds;
    }


    /// <summary>
    /// Transaction with sql command and bulk copy of datatable
    /// </summary>
    /// <param name="TableList">Array Of DataTable </param>
    /// <param name="strProcedureName">Array Of Procedure Name</param>
    /// /// <param name="hshtblCollection">Array Of Hash Table</param>
    /// <returns>boolean </returns>

    public static object _objTransCheck = null;

    public static object ExecuteTransact(DataTable[] TableList, string[] strProcedureName, Hashtable[] hshtblCollection)
    {
        SqlCommand[] sqlCommand = new SqlCommand[strProcedureName.Length];
        try
        {
            if (OpenConnection())
            {
                //Start a local transaction.
                _transaction = _connection.BeginTransaction();
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                SqlBulkCopy blkCpy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, _transaction);

                for (int i = 0; i < strProcedureName.Length; i++)
                {
                    if (strProcedureName[i] != null && strProcedureName[i] != "")
                    {
                        sqlCommand[i] = new SqlCommand(strProcedureName[i], _connection);
                        sqlCommand[i].Transaction = _transaction;
                        sqlCommand[i].CommandType = CommandType.StoredProcedure;
                        if (hshtblCollection[i] != null)
                        {
                            IDictionaryEnumerator _enumerator = hshtblCollection[i].GetEnumerator();
                            while (_enumerator.MoveNext())
                            {
                                sqlCommand[i].Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                            }
                        }
                        SqlParameter outParameter = new SqlParameter("@vchrMsg", SqlDbType.VarChar, 200); //New Update
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter.Value = "";
                        sqlCommand[i].Parameters.Add(outParameter);
                        sqlCommand[i].ExecuteNonQuery();
                        _objTransCheck = sqlCommand[i].Parameters["@vchrMsg"].Value;
                        //sqlCommand[i].ExecuteNonQuery();
                    }
                }
                if (TableList != null && _objTransCheck.ToString().Equals("SUCCESS"))//string.IsNullOrEmpty(_objTransCheck.ToString())
                    for (int t = 0; t < TableList.Length; t++)
                    {
                        if (TableList[t] != null && TableList[t].Rows.Count >= 1)
                        {
                            blkCpy.DestinationTableName = TableList[t].TableName;
                            blkCpy.WriteToServer(TableList[t]);
                        }
                    }
                //if (string.IsNullOrEmpty(_objTransCheck.ToString())) _transaction.Commit();
                if (_objTransCheck.ToString().Equals("SUCCESS")) _transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            _transaction.Rollback();
            throw;
            return _objTransCheck;
        }
        finally
        {
            CloseConnection();
        }
        return _objTransCheck;
    }

    public static string ExcecuteAll(List<object> tableList, string strProc, string output)
    {
        //try
        //{
        SqlTransaction tran = null;
        if (OpenConnection())
        {
            tran = _connection.BeginTransaction();
            try
            {
                for (int i = 0; i < tableList.Count; i++)
                {
                    Hashtable hash = (Hashtable)tableList[i];
                    _sqlCmd = new SqlCommand(strProc, _connection);
                    _sqlCmd.Transaction = tran;
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator _enumerator = hash.GetEnumerator();
                    while (_enumerator.MoveNext())
                    {
                        _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                    }
                    SqlParameter outParameter = new SqlParameter("@Msg", SqlDbType.VarChar, 255); //New Update
                    outParameter.Direction = ParameterDirection.InputOutput;
                    outParameter.Value = "";
                    _sqlCmd.Parameters.Add(outParameter);
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _ds = new DataSet();
                    _adapter.Fill(_ds);
                    output = _sqlCmd.Parameters["@Msg"].Value.ToString();
                }
                tran.Commit();
                output = "Success";
            }
            catch (Exception)
            {
                output = "Error";
                tran.Rollback();
            }
            finally
            {
                _connection.Close();
            }
        }
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
        return output;
    }


    /// <summary>
    /// Transaction with sql command and bulk copy of datatable
    /// </summary>
    /// <param name="TableList">Array Of DataTable </param>
    /// <param name="strProcedureName">Array Of Procedure Name</param>
    /// /// <param name="hshtblCollection">Array Of Hash Table</param>
    /// <returns>boolean </returns>

    public static bool ExecuteTransact(string[] strProcedureName, Hashtable[] hshtblCollection, DataTable[] TableList)
    {
        SqlCommand[] sqlCommand = new SqlCommand[strProcedureName.Length];
        try
        {
            if (OpenConnection())
            {
                // Start a local transaction.
                _transaction = _connection.BeginTransaction();
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                SqlBulkCopy blkCpy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, _transaction);

                for (int i = 0; i < strProcedureName.Length; i++)
                {
                    if (strProcedureName[i] != null && strProcedureName[i] != "")
                    {
                        sqlCommand[i] = new SqlCommand(strProcedureName[i], _connection);
                        sqlCommand[i].Transaction = _transaction;
                        sqlCommand[i].CommandType = CommandType.StoredProcedure;
                        if (hshtblCollection[i] != null)
                        {
                            IDictionaryEnumerator _enumerator = hshtblCollection[i].GetEnumerator();
                            while (_enumerator.MoveNext())
                            {
                                sqlCommand[i].Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                            }
                        }
                        sqlCommand[i].ExecuteNonQuery();
                    }
                }
                if (TableList != null)
                    for (int t = 0; t < TableList.Length; t++)
                    {
                        if (TableList[t] != null && TableList[t].Rows.Count >= 1)
                        {
                            blkCpy.DestinationTableName = TableList[t].TableName;
                            blkCpy.WriteToServer(TableList[t]);
                        }
                    }
                _transaction.Commit();
                _transCheck = true;
            }
        }
        catch (Exception ex)
        {
            _transaction.Rollback();
            throw;
            _transCheck = false;
        }
        finally
        {
            CloseConnection();
        }
        return _transCheck;
    }

    public static string ExecuteDelete(string query)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand(query, _connection);
                _sqlCmd.CommandType = CommandType.Text;
                if (OpenConnection())
                {
                    var i = _sqlCmd.ExecuteNonQuery();
                }
                return "S";
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {
            return "F";
        }
        finally
        {
            CloseConnection();
        }
    }


    public static string ExecuteSP(string TableName, string ColumnName, int OldAcCode, int NewAcCode, string WhereCondition)
    {
        string result = string.Empty;
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand("spClubAccount", _connection);
                _sqlCmd.Transaction = _transaction;
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Parameters.AddWithValue("@TableName", TableName);
                string columnNames = ColumnName + "=" + NewAcCode + " where " + ColumnName + "=" + OldAcCode + " and " + WhereCondition;
                _sqlCmd.Parameters.AddWithValue("@ColumnName", columnNames);
                _sqlCmd.ExecuteNonQuery();
                result = "1";
            }
        }
        catch (Exception ex)
        {
            result = "0";
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
        return result;
    }

    public static void ExecuteMergedAccountsSP(string RightAccountCode, string WrongAccountCode, string Created_By, string RightAccountFilePath, string WrongAccountFilePath, string Company_Code, string Year_Code)
    {
        try
        {
            if (OpenConnection())
            {
                _sqlCmd = new SqlCommand("spInsertMergedAccount", _connection);
                _sqlCmd.Transaction = _transaction;
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Parameters.AddWithValue("@RightAccountCode", RightAccountCode);
                _sqlCmd.Parameters.AddWithValue("@WrongAccountCode", WrongAccountCode);
                _sqlCmd.Parameters.AddWithValue("@Created_By", Created_By);
                _sqlCmd.Parameters.AddWithValue("@RightAccountFilePath", RightAccountFilePath);
                _sqlCmd.Parameters.AddWithValue("@WrongAccountFilePath", WrongAccountFilePath);
                _sqlCmd.Parameters.AddWithValue("@Company_Code", Company_Code);
                _sqlCmd.Parameters.AddWithValue("@Year_Code", Year_Code);
                _sqlCmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            _sqlCmd.Dispose();
            CloseConnection();
        }
    }
    #endregion

    #region single string

    public static string GetString(string qry)
    {
        string returnString = string.Empty;
        try
        {

            if (OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _connection;
                var retValue = cmd.ExecuteScalar();
                returnString = Convert.ToString(retValue);
            }
        }
        catch (Exception ex)
        {
            clsLog.Publish(ex);
            throw;
        }

        return returnString;
    }

    #endregion

  
}
