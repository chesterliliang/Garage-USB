using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Garage_USB
{
    public class csv_log
    {
        public static string path;
        public static void gen_table(string path, DataTable dt)
        {
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Result", System.Type.GetType("System.String"));
            dt.Columns.Add("Bin", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Stage", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Gray Level", System.Type.GetType("System.Int32"));
            dt.Columns.Add("RV", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Noise", typeof(float));
            dt.Columns.Add("SNR", typeof(float));
            dt.Columns.Add("DR", typeof(float));
            dt.Columns.Add("Current", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Voltage", typeof(float));
            dt.Columns.Add("Download", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Activate", System.Type.GetType("System.Int32"));
            dt.Columns.Add("SN", System.Type.GetType("System.String"));
            dt.Columns.Add("Remains", System.Type.GetType("System.String"));
            dt.Columns.Add("Version", System.Type.GetType("System.String"));
            dt.Columns.Add("Parameter", System.Type.GetType("System.String"));
            dt.Columns.Add("Station", System.Type.GetType("System.String"));
            dt.Columns.Add("Date", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("Project", System.Type.GetType("System.String"));
            dt.Columns.Add("Product", System.Type.GetType("System.String"));


        }

        public static void save_bmp(string name,byte[] data)
        {
            string str = path + name + ".bmp";
            FileStream fs = new FileStream(str, FileMode.OpenOrCreate);
            fs.Write(data, 0, 1078 + config.sensor_width*config.sensor_height);
            fs.Close();
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <param name="k">可选参数表示最后K行不算记录默认0</param>
        public static DataTable csv2dt(FileStream fs, int n, DataTable dt) //这个dt 是个空白的没有任何行列的DataTable
        {
            String csvSplitBy = "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)";
            StreamReader reader = new StreamReader(fs, System.Text.Encoding.Default, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    if (m == n + 1) //如果是字段行，则自动加入字段。
                    {
                        MatchCollection mcs = Regex.Matches(str, csvSplitBy);
                        foreach (Match mc in mcs)
                        {
                            dt.Columns.Add(mc.Value); //增加列标题
                        }

                    }
                    else
                    {
                        MatchCollection mcs = Regex.Matches(str, "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
                        i = 0;
                        System.Data.DataRow dr = dt.NewRow();
                        foreach (Match mc in mcs)
                        {
                            dr[i] = mc.Value;
                            i++;
                        }
                        dt.Rows.Add(dr);  //DataTable 增加一行     
                    }

                }
            }
            return dt;
        }

        public static void dt2csv(FileStream fs, DataTable dt)//table数据写入csv
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";

            for (int i = 0; i < dt.Columns.Count; i++)//写入列名
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            for (int i = 0; i < dt.Rows.Count; i++) //写入各行数据
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                        || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
        }
    }
}
