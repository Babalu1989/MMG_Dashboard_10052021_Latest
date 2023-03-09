
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.OleDb;
/// <summary>
/// Summary description for DAL
/// </summary>
/// 

namespace common
{
    public class DAL
    {

        public OleDbConnection cnnConnection;
        OleDbCommand cmd;

        //public string GetConnectionString
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        //    }
        //}

        private static string getConnectionString()
        {
            NDS ObjNDS = new NDS();
            return ObjNDS.DcrepCon();
        }

        public void OpenConnection()
        {
            try
            {
                string connectionString = getConnectionString();
                cnnConnection = new OleDbConnection(connectionString);
                cnnConnection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection(OleDbConnection con)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsDuplicate(string strTableName, string strWhereClause, ref OleDbConnection cnnDuplicate)
        {
            bool functionReturnValue = false;

            OleDbDataReader dtrDuplicate = default(OleDbDataReader);
            OleDbCommand cmdDuplicate = default(OleDbCommand);
            string query = null;

            query = "SELECT * FROM " + strTableName;

            if (!string.IsNullOrEmpty(strWhereClause))
            {
                query = query + " WHERE (" + strWhereClause + ")";
            }

            cmdDuplicate = new OleDbCommand(query, cnnDuplicate);

            try
            {

                dtrDuplicate = cmdDuplicate.ExecuteReader();
                if (dtrDuplicate.Read())
                {
                    functionReturnValue = true;
                }
                else
                {
                    functionReturnValue = false;
                }

                dtrDuplicate.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdDuplicate.Dispose();
                dtrDuplicate.Dispose();
            }
            return functionReturnValue;
        }

        public bool IsDuplicate(string strTableName, string strWhereClause, ref OleDbConnection cnnDuplicate, ref OleDbTransaction objTrans)
        {
            bool functionReturnValue = false;

            OleDbDataReader dtrDuplicate = default(OleDbDataReader);
            OleDbCommand cmdDuplicate = default(OleDbCommand);
            string query = null;

            query = "SELECT * FROM " + strTableName;

            if (!string.IsNullOrEmpty(strWhereClause))
            {
                query = query + " WHERE (" + strWhereClause + ")";
            }

            cmdDuplicate = new OleDbCommand(query, cnnDuplicate);
            cmdDuplicate.Transaction = objTrans;
            try
            {

                dtrDuplicate = cmdDuplicate.ExecuteReader();
                if (dtrDuplicate.Read())
                {
                    functionReturnValue = true;
                }
                else
                {
                    functionReturnValue = false;
                }

                dtrDuplicate.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdDuplicate.Dispose();
                dtrDuplicate.Dispose();
            }
            return functionReturnValue;
        }

        public OleDbCommand GetCommand(string commandString, OleDbConnection connection)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand(commandString, connection);
                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public DataSet FillDataSet(string commandString)
        {
            try
            {
                DataSet dstData = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(commandString, getConnectionString());
                //adapter.Fill(dstData, "EMP");
                adapter.Fill(dstData);
                return dstData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FillDataSet(CommandType cmdType, string commandString, ref OleDbConnection connection, ref OleDbTransaction objTrans)
        {
            try
            {
                DataSet objDS = new DataSet();
                cmd = GetCommand(commandString, connection);
                cmd.Transaction = objTrans;
                if (cmdType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                OleDbDataAdapter objDA = new OleDbDataAdapter(cmd);
                objDA.Fill(objDS);
                return objDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertRecord(string tableName, string fieldsName, string fieldsValue, ref OleDbConnection cnnAdd, ref OleDbTransaction objTrans)
        {
            string strInsertQuery;
            OleDbCommand cmdAdd;
            bool flg = false;

            strInsertQuery = "Insert Into " + tableName + " (" + fieldsName + ") Values(" + fieldsValue + ")";
            cmdAdd = new OleDbCommand(strInsertQuery, cnnAdd);
            cmdAdd.Transaction = objTrans;

            try
            {
                cmdAdd.ExecuteNonQuery();
                cmdAdd.Dispose();
                flg = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flg;
        }

        public bool ModifyRecord(string tableName, string fieldNames, string fieldValues, string whereClause, ref OleDbConnection cnnModify, ref OleDbTransaction objTrans)
        {
            OleDbCommand cmdUpdate;
            Array arrField;
            Array arrValue;
            char[] arrCharSeperator;
            string strSeperator;
            StringBuilder strBuild = new StringBuilder();

            int intUBound;
            int intCounter;
            strSeperator = "$";
            arrCharSeperator = strSeperator.ToCharArray();

            arrField = fieldNames.Split(arrCharSeperator);
            arrValue = fieldValues.Split(arrCharSeperator);

            strBuild.Append("Update ");
            strBuild.Append(tableName);
            strBuild.Append(" Set ");

            if ((arrField.GetUpperBound(0)) == (arrValue.GetUpperBound(0)))
            {
                intUBound = Convert.ToInt16(arrField.GetUpperBound(0));
                for (intCounter = 0; intCounter < intUBound; intCounter++)
                {
                    if (intCounter == 0)
                    {
                        strBuild.Append(arrField.GetValue(intCounter));
                        strBuild.Append(" = ");
                        strBuild.Append(arrValue.GetValue(intCounter));
                    }
                    else
                    {
                        strBuild.Append(",");
                        strBuild.Append(arrField.GetValue(intCounter));
                        strBuild.Append(" = ");
                        strBuild.Append(arrValue.GetValue(intCounter));
                    }
                }
            }
            else
            {
                return (false);
            }

            strBuild.Append(" Where ");
            strBuild.Append(whereClause);
            cmdUpdate = new OleDbCommand(strBuild.ToString(), cnnModify);
            cmdUpdate.Transaction = objTrans;

            try
            {
                cmdUpdate.ExecuteNonQuery();
                return (true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //''------------------Releasing the variables------------------
                cmdUpdate.Dispose();
            }
        }

        public int GetPKId(string strTableName, string strPrimaryKey, ref OleDbConnection connection)
        {
            OleDbCommand cmdSelect = default(OleDbCommand);

            string query = null;
            string strYear = null;

            int intPK = 0;

            strYear = Convert.ToString(DateTime.Now.Year);

            query = "SELECT (CASE WHEN  MAX(" + strPrimaryKey + ") IS NULL THEN 1 ELSE MAX(" + strPrimaryKey + ") + 1 END) AS NEXTVAL FROM " + strTableName + "";
            try
            {
                cmdSelect = new OleDbCommand(query, connection);
                intPK = Convert.ToInt32(cmdSelect.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }

            cmdSelect = null;
            query = null;
            return intPK;

        }

        public int GetPKId(string strTableName, string strPrimaryKey, ref OleDbConnection connection, ref OleDbTransaction objTrans)
        {
            OleDbCommand cmdSelect = default(OleDbCommand);

            string query = null;
            string strYear = null;

            int intPK = 0;

            strYear = Convert.ToString(DateTime.Now.Year);

            query = "SELECT (CASE WHEN  MAX(" + strPrimaryKey + ") IS NULL THEN 1 ELSE MAX(" + strPrimaryKey + ") + 1 END) AS NEXTVAL FROM " + strTableName + "";
            try
            {
                cmdSelect = new OleDbCommand(query, connection);
                cmdSelect.Transaction = objTrans;
                intPK = Convert.ToInt32(cmdSelect.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }

            cmdSelect = null;
            query = null;
            return intPK;

        }

        public int GetValue(string query, ref OleDbConnection connection, ref OleDbTransaction objTrans)
        {
            OleDbCommand cmdSelect = default(OleDbCommand);
            int intPK = 0;


            try
            {
                cmdSelect = new OleDbCommand(query, connection);
                cmdSelect.Transaction = objTrans;
                intPK = Convert.ToInt32(cmdSelect.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }

            cmdSelect = null;
            query = null;
            return intPK;

        }

        public bool DeleteRecord(string tableName, string strWhereClause, ref OleDbConnection cnnAdd, ref OleDbTransaction objTrans)
        {
            string strInsertQuery;
            OleDbCommand cmdAdd;
            bool flg = false;

            strInsertQuery = "Delete From " + tableName + " Where " + strWhereClause + "";
            cmdAdd = new OleDbCommand(strInsertQuery, cnnAdd);
            cmdAdd.Transaction = objTrans;

            try
            {
                cmdAdd.ExecuteNonQuery();
                cmdAdd.Dispose();
                flg = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flg;
        }

    }

}
