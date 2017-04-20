using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LogManager
    {
        //将log文件存到我的文档中
        private readonly static string errorLogFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Process.GetCurrentProcess().ProcessName, "Logs");

        /// <summary>
        /// 将异常信息保存到我的文档中
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="s">自定义信息</param>
        public static void LogError(Exception e, string s = null)
        {

            var sb = new StringBuilder();
            sb.AppendLine("<===========================Strat==========================>");
            sb.AppendLine(DateTime.Now.ToString() + " " + e.ToString());
            if (s != null)
            {
                sb.Append(s);
            }
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            sb.AppendLine("内部异常信息：");
            sb.AppendLine(e.InnerException?.Message ?? "");
            sb.AppendLine("<===========================End============================>");
            sb.AppendLine("");
            WriteLog(sb);
        }

        /// <summary>
        /// 记录自定义信息
        /// </summary>
        /// <param name="s"></param>
        public static void LogString(string s)
        {
            var sb = new StringBuilder();
            sb.AppendLine(s);
            sb.AppendLine("");
            WriteLog(sb);
        }

        private static void WriteLog(StringBuilder sb)
        {
            if (!Directory.Exists(errorLogFolder))
            {
                //如果存储错误的目录不存在，则创建该目录
                Directory.CreateDirectory(errorLogFolder);
            }
            //如果错误文件超过10个，就删除旧的
            var errorfiles = Directory.GetFiles(errorLogFolder, "*.log").ToList();
            errorfiles.Sort();
            if (errorfiles.Count > 10)
            {
                File.Delete(errorfiles.FirstOrDefault());
            }

            var logFileName = Path.Combine(errorLogFolder, DateTime.Now.ToString("yyyyMMdd") + ".log");
            if (File.Exists(logFileName))
            {
                //如果文件存在，并且大小大于10M，则将其重命名为日期+时间的Log名称
                FileInfo info = new FileInfo(logFileName);
                if (info.Length > 10000000)
                {
                    File.Move(logFileName, Path.Combine(errorLogFolder, DateTime.Now.ToString("yyyyMMddhhmmss") + ".log"));
                }
            }
            //追加到现有Log文件当中
            File.AppendAllText(logFileName, sb.ToString());
        }
    }
}
