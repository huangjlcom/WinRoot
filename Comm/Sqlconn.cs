using System;
using System.Collections.Generic;

using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WinRobots
{
     public  class Sqlconn
    {
        public static string constr = "data source=192.168.0.129;initial catalog=ROBOT;user id=sa;pwd=help888U";
        public static  int SCmd(string SqlCmd)
        {
            SqlConnection con = new SqlConnection(constr);
            string sql = SqlCmd;
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                return (int)com.ExecuteScalar();
            }
            catch (Exception sl)
            {
                string sls = sl.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return 0;
        }

        public static DataSet GetData(string SqlCmd)
        {
            SqlConnection con = new SqlConnection(constr);
            // con.ConnectionString = constr;  
            string sql = SqlCmd;

            DataSet _Val = new DataSet();
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                con.Open();
                com.CommandText = sql;
                dap.SelectCommand = com;
                dap.Fill(_Val, "table");
                return _Val;
            }
            catch (Exception sl)
            {
                string sls = sl.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return null;
        }

        public static void Save(DataTable table, string tableName)
        {
           
            SqlConnection Connection = new SqlConnection(constr);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(string.Format("SELECT TOP 0 * FROM {0}", tableName));
                cmd.Connection = Connection;
                adapter.SelectCommand = cmd;
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
                table.TableName = tableName;
                int num = adapter.Update(table);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
        }

        public static void SaveInfo(DataTable table, string tableName)
        {

            SqlConnection Connection = new SqlConnection(constr);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(string.Format("SELECT TOP 0 * FROM {0}", tableName));
                cmd.Connection = Connection;
                adapter.SelectCommand = cmd;
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
                table.TableName = tableName;
                int num = adapter.Update(table);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
        }

     
    }
}
