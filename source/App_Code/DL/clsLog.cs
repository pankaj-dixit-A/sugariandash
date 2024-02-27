using System;
using System.Data;
using System.Data.SqlClient;

public static class clsLog
{
    public static void Publish(Exception ex)
    {
        try
        {
            String strMessage = ex.Message;
            String strInnerException = Convert.ToString(ex.InnerException);
            String strExceptionId = "0";
            String PageName = clsAdvanceUtility.GetCurrentPageName();
            using (clsDataProvider objDataProvider = new clsDataProvider())
            {
                SqlParameter[] lcparam = new SqlParameter[5];
                lcparam[0] = new SqlParameter("@ExceptionId", strExceptionId);
                lcparam[0].Direction = ParameterDirection.InputOutput;
                lcparam[0].SqlDbType = SqlDbType.BigInt;
                lcparam[1] = new SqlParameter("@ExceptionName", strInnerException);
                lcparam[2] = new SqlParameter("@ExceptionDetails", strMessage);
                lcparam[3] = new SqlParameter("@PageName", PageName);
                objDataProvider.ExecuteStoredProc("SP_LogException", lcparam);
            }
        }
        catch
        {
        }
    }
}