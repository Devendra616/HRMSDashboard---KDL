using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using context = System.Web.HttpContext;

namespace HRMSDashboard
{
    public class Logger
    {
        private static String  Errormsg, exurl, ErrorLocation, stackTrace,error;
        
        public static void log(Exception exception,string user)
        {
            var line = Environment.NewLine + Environment.NewLine;

            var ErrorlineNo = (new StackTrace(exception, true)).GetFrame(0).GetFileLineNumber(); // GetFrame(st.FrameCount - 1);
            Errormsg = exception.GetType().Name.ToString();             
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = exception.Message.ToString();
            stackTrace = exception.StackTrace;
            try
            {
                string filepath = context.Current.Server.MapPath("~/LogFiles/");  //Text File Path
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Error Location :" + " " + ErrorLocation + line + "Error Page Url:" + " " + exurl + line + "User:" + user  + line+ "Stack Trace: "+ stackTrace+line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }

        }

        public static void log(String logMessage, String user)
        {
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                string filepath = context.Current.Server.MapPath("~/LogFiles/");  //Text File Path
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    logMessage = "Log Written Date:" + " " + DateTime.Now.ToString() + " USERID: " + user + line + logMessage;
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(logMessage);
                    sw.WriteLine("--------------------------------END----------------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        }

}
