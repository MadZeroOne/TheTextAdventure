using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheTextAdventureW
{
    public class clsDBConnect
    {

        private static string localPath = null;// = Environment.CurrentDirectory.Substring(Environment.CurrentDirectory.LastIndexOf('\\') - 1);

        /// <summary>
        /// Asyncconnect
        /// </summary>
        /// <returns></returns>
        public static SqlCeConnection GetDBConnectAsync()
        {
            SqlCeConnection sqlceConn = null;
            try
            {
#if DEBUG
                if (localPath == null)
                {
                    localPath = Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.LastIndexOf('\\'));
                    localPath = localPath.Remove(localPath.LastIndexOf('\\'));
                }
                string connString = "Data Source='" + localPath + "\\DB\\DataDB.sdf'; LCID=1031;   Password=Midas01; Encrypt = TRUE;";
                sqlceConn = new SqlCeConnection(connString);
                sqlceConn.Open();
#else
                string connString = "Data Source='DataDB.sdf'; LCID=1031;   Password=Midas01; Encrypt = TRUE;";
                sqlceConn = new SqlCeConnection(connString);
                sqlceConn.Open();
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error00001: " + ex.Message);
            }
            return sqlceConn;
        }
        /// <summary>
        /// Verbindung schließen
        /// </summary>
        public static void CloseAsyncConnect(SqlCeConnection sqlceConn)
        {
            try
            {
                if (sqlceConn != null)
                {
                    if (sqlceConn.State == System.Data.ConnectionState.Open)
                    {
                        sqlceConn.Close();
                        sqlceConn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error00002: " + ex.Message);
            }
        }
        /// <summary>
        /// Select Data
        /// </summary>
        /// <param name="sqlStatement">SQL select mit oder ohne where Klausel</param>
        /// <param name="dtRet">Rückgabe der Daten in DataTable</param>
        /// <param name="sqlceConn">Aktuelle Connection</param>
        /// <param name="dcParams">Parameter für Where Klausel in Dictionary</param>
        /// <returns></returns>
        public static Exception SelectData(string sqlStatement, out DataTable dtRet, SqlCeConnection sqlceConn, Dictionary<string, object> dcParams)
        {
            Exception exRet = null;
            dtRet = null;
            try
            {
                SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlStatement, sqlceConn);
                if (dcParams != null)
                {
                    foreach (String dcKey in dcParams.Keys)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue(dcKey, dcParams[dcKey]);
                    }
                }
                dtRet = new DataTable();
                adapter.Fill(dtRet);
            }
            catch (Exception ex)
            {
                exRet = ex;
            }
            return exRet;
        }
        /// <summary>
        /// Update Data
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="sqlceConn"></param>
        /// <param name="dcParams"></param>
        /// <param name="updatetRows"></param>
        /// <returns></returns>
        public static Exception UpdateData(string sqlStatement, SqlCeConnection sqlceConn, Dictionary<string, object> dcParams, ref Nullable<int> updatetRows)
        {
            Exception exRet = null;
            try
            {
                SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlStatement, sqlceConn);
                if (dcParams != null)
                {
                    foreach (String dcKey in dcParams.Keys)
                    {
                        adapter.UpdateCommand.Parameters.AddWithValue(dcKey, dcParams[dcKey]);
                    }
                }
                updatetRows = adapter.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                exRet = ex;
            }
            return exRet;
        }
        /// <summary>
        /// Insert Data
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="sqlceConn"></param>
        /// <param name="dcParams"></param>
        /// <param name="insertedRow"></param>
        /// <returns></returns>
        public static Exception InsertData(string sqlStatement, SqlCeConnection sqlceConn, Dictionary<string, object> dcParams, ref Nullable<int> insertedRow)
        {
            Exception exRet = null;
            try
            {
                SqlCeDataAdapter adapter = new SqlCeDataAdapter(sqlStatement, sqlceConn);
                if (dcParams != null)
                {
                    foreach (String dcKey in dcParams.Keys)
                    {
                        adapter.UpdateCommand.Parameters.AddWithValue(dcKey, dcParams[dcKey]);
                    }
                }
                insertedRow = adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                exRet = ex;
            }
            return exRet;
        }
    }
}
