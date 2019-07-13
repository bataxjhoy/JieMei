using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JieMei
{
    public class FileOperation
    {

        /// <summary>
        /// 按行读取文本文件:
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <returns></returns>
        public string[] ReadTxtFile(string txtFileName)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            string[] text = new string[] { };
            if (System.IO.File.Exists(path + txtFileName))
            {
                //System.Diagnostics.Process.Start(path + txtFileName);//打开此文件:
                text = File.ReadAllLines(path + txtFileName, Encoding.Default);
            }
            return text;
        }


        /// <summary>
        /// 打开或创建文本文件:
        /// </summary>
        /// <param name="txtFileName"></param>
        /// <param name="text"></param>
        public void WriteTxtFile(string txtFileName, string text)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。 
            FileStream fs = new FileStream(path + txtFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(text);
            sw.Close();
            fs.Close();

            //保存成功后打开此文件:
            //if (System.IO.File.Exists(path + txtFileName))
            //{
            //  System.Diagnostics.Process.Start(path + txtFileName);
            //}
        }
        /// <summary>
        /// 改变某一行的值
        /// </summary>
        /// <param name="txtFileName">文件名称</param>
        /// <param name="lineNum">对应行</param>
        /// <param name="text">更改后的值</param>
        public void ChangeLineTxtFile(string txtFileName,int lineNum, string text)
        {
            string path = Directory.GetCurrentDirectory() + "\\"; //获取应用程序的当前工作目录。
            if (System.IO.File.Exists(path + txtFileName))
            {
                string[] line = File.ReadAllLines(path + txtFileName, Encoding.Default);

                line[lineNum - 1] = text;
                WriteTxtFile(txtFileName, line[0] + "\r\n" + line[1] + "\r\n" + line[2] + "\r\n" + line[3] + "\r\n" + line[4]);
            }
            //StreamReader m_streamReader = new StreamReader(fs);
            //m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //string arry = "";
            //string strLine = m_streamReader.ReadLine();

            //do
            //{
            //    //你把查询换成试用你的就行了
            //    string[] split = strLine.Split('=');
            //    string a = split[0];
            //    if (a.ToLower() == "ip")
            //    {
            //        arry += strLine + "\n";

            //    }
            //    strLine = m_streamReader.ReadLine();

            //} while (strLine != null && strLine != "");
            //m_streamReader.Close();
            //m_streamReader.Dispose();
            //fs.Close();
            //fs.Dispose();
            //Console.Write(arry);
            //Console.ReadLine(); 
        }
    }
}
