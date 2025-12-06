using System;
using System.IO;
using System.Text;
using MBEasyMod.Helpers;

namespace MBEasyMod.Managers
{
    public class LoggingManager
    {
        private static LoggingManager instance = null;
        private StreamWriter logFile = null;

        private static LoggingManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoggingManager();
                return instance;
            }
        }

        private LoggingManager()
        {
            try
            {
                if (!Directory.Exists($"{ConstantsHelper.ModuleFolder}/Logs"))
                    Directory.CreateDirectory($"{ConstantsHelper.ModuleFolder}/Logs");
                logFile = new StreamWriter($"{ConstantsHelper.ModuleFolder}/Logs/{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", false, Encoding.UTF8);
            }
            catch
            {
                // IDK what to even do here
                logFile = null;
            }
            finally
            {
                LogMessage($"'LoggingManager' of '{ConstantsHelper.Name}' successfully initialized!");
            }
        }

        public static bool LogMessage(string message)
        {
            if (Instance.logFile == null)
                return false;

            try
            {
                Instance.logFile.WriteLine($"[{DateTime.Now.TimeOfDay.ToString("h\\:m\\:s")}] {message}");
                Instance.logFile.Flush();
            }
            catch (Exception e)
            {
                LogException(e);
            }
            return true;
        }

        public static bool LogException(Exception exception)
        {
            if (Instance.logFile == null)
                return false;

            try
            {
                Instance.logFile.WriteLine($"{exception.GetType()}: '{exception.Message}'\n\tStacktrace:\n{exception.StackTrace}");
                Instance.logFile.Flush();
            } catch { }
            return true;
        }

        public static void Dispose()
        {
            if (Instance.logFile == null)
                return;
            LogMessage("Terminating...");
            Instance.logFile.Close();
        }
    }
}