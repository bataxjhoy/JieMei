
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
//using static System.Net.Mime.MediaTypeNames;

namespace Models
{
    public class MyDataLib
    {
        /// <summary>
        /// 获取连接字串
        /// </summary>
        /// <returns></returns>
        public static string ConnString()
        {
            string conn = "";
            StreamReader sr = new StreamReader("" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "conn.txt", Encoding.GetEncoding("GB2312"));//读取文件conn.txt
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                conn = line.ToString();
            }
            try
            {
                byte[] c = Convert.FromBase64String(conn);
                conn = Encoding.Default.GetString(c);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return conn;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="barcode">一维码</param>
        /// <param name="machineid">机台号</param>
        /// <param name="banc">厂商</param>
        /// <param name="id"></param>
        /// <param name="js"></param>
        /// <param name="xuliehao">序列号</param>
        /// <returns></returns>
        public static int delete(string barcode, string machineid, string banc, int id, string js, int xuliehao)
        {
            string sql = "update  dbo.barToPrint set yxbz='N' where pkid =" + id + "";

            int i = ExecNoneQueryBySql(sql);
            sql = "update MesBarcode set UsedFlag=1,UsedDate=getdate() where Invalid=0 and  barcode='" + barcode + "'";
            i = ExecNoneQueryBySql(sql);
            sql = "exec insertMesBarcode '" + barcode + "','" + machineid + "','" + banc + "','" + js + "','" + xuliehao + "'";
            i = ExecNoneQueryBySql(sql);
            return i;
        }

       
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="barcode">一维码</param>
        /// <param name="machineid">机台号</param>
        /// <param name="banc">厂商</param>
        /// <param name="js"></param>
        /// <param name="xuliehao"></param>
        /// <returns></returns>
        public static int insert(string barcode, string machineid, string banc, string js, int xuliehao)
        {
            string sql = "exec insertMesBarcode '" + barcode + "','" + machineid + "','" + banc + "','" + js + "','" + xuliehao + "'";
            int i = ExecNoneQueryBySql(sql);
            return i;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sql">查找语句</param>
        /// <returns>表</returns>
        public static DataTable SearchItem(string sql)
        {
            SqlConnection conn = new SqlConnection(ConnString());
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable("batch");
            try
            {
                //string sql = "select * from dbo.barToPrint where yxbz='Y'";

                StringBuilder sb = new StringBuilder();
                sb.Append(sql);
                //Console.WriteLine(sb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = sb.ToString();
                adp.SelectCommand = cmd;
                adp.Fill(dt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static int ExecNoneQueryBySql(string sql)
        {
            SqlCommand comd = new SqlCommand { Connection = OpenDatabase(ConnString()) };
            int result = 0;
            try
            {
                comd.CommandTimeout = 1800;
                comd.CommandText = sql;
                comd.Parameters.Clear();

                result = comd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                result = -1;
            }
            finally
            {
                CloseDatabase(comd.Connection);
            }
            return result;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="mainSql"></param>
        /// <returns></returns>
        public static bool transactionOp_list(ArrayList mainSql)
        {
            bool res;
            SqlConnection conn = new SqlConnection(ConnString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.Transaction = conn.BeginTransaction();//开启事务 

                foreach (string item in mainSql)
                {

                    cmd.CommandText = item;
                    cmd.ExecuteNonQuery();
                }
                cmd.Transaction.Commit();//提交事务  
                res = true;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();//回滚事务  
                }
                log(ex.Message + "\r\n" + ex.StackTrace);
                //throw ex;
                res = false;

            }
            finally
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction = null;//清空事务  
                }
                conn.Close();

            }
            return res;
        }
        /// <summary>
        /// 连接打开数据库
        /// </summary>
        /// <param name="connString">连接字串</param>
        /// <returns></returns>
        public static SqlConnection OpenDatabase(string connString)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = connString;
                conn.Open();
            }
            catch (Exception ex)
            {
                //Log("数据库连接失败:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
            return conn;
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int CloseDatabase(SqlConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                log("Close the database error:" + ex.Message + "\r\n" + ex.StackTrace);
            }
            return -1;
        }



        /// <summary>
        /// 将日志写入数据库中
        /// </summary>
        /// <param name="logType">日志类型，例如error,login,logout,warning,delete</param>
        /// <param name="subject">主题</param>
        /// <param name="exMessage">消息</param>
        /// <param name="exTrace">调试信息</param>
        public static int logToDB(string logType, string subject, string exMessage, string exTrace)
        {
            try
            {
                string sql = "insert into Agvrunning_log(logType,subject,exMessage,exStackTrace) values('"+ logType + "','"+ subject + "','"+ exMessage + "','"+ exTrace + "')";

                return ExecNoneQueryBySql(sql);
            }
            catch (Exception ex)
            {
                log(ex.Message + "\r\n" + ex.StackTrace);
                return -1;
            }
        }


        /// <summary>
        /// 本地日志记录
        /// </summary>
        /// <param name="str">要记录的日志内容</param>
        public static void log(string str)
        {
            string AppPath = Environment.CurrentDirectory + "\\";//本地路径
            string filePath = AppPath + "error.log";             //文件名称
            string content = DateTime.Now.ToString("yyyyMMddHHmmss:") + str;//时间
            if (!System.IO.File.Exists(filePath))//文件路径不存在
            {
                System.IO.File.AppendAllText(filePath, content);
                return;
            }
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(writeLog);
            Thread thread = new Thread(threadStart);//开启线程
            thread.Name = "AGvrunning.log";
            thread.Start(str);
        }

        public static void writeLog(object str)
        {
            string AppPath = Environment.CurrentDirectory + "\\";
            string filePath = AppPath + "error.log";
            string content = "\r\n" + DateTime.Now.ToString("yyyyMMddHHmmss:") + str.ToString();
            System.IO.FileInfo info = new System.IO.FileInfo(filePath);
            if (info.Length > 1024 * 1024 * 5)
            {
                while (IsFileInUse(filePath))
                    Thread.Sleep(100);
                System.IO.File.Move(filePath, AppPath + "error" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                System.IO.File.Delete(filePath);
            }
            while (IsFileInUse(filePath))
                Thread.Sleep(100);
            if (!IsFileInUse(filePath))
            {
                #region write file
                System.IO.FileStream fs = null;
                try
                {
                    fs = new System.IO.FileStream(filePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    fs.Write(Encoding.UTF8.GetBytes(content), 0, Encoding.UTF8.GetByteCount(content));
                }
                catch
                {
                    ;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
                #endregion
            }
        }


        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            System.IO.FileStream fs = null;
            try
            {
                fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                inUse = false;
            }
            catch
            {
                inUse = true;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;
        }
    }
}
