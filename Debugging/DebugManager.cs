using System;
using System.Windows.Forms;
using System.Threading;

namespace MBEasyMod.Debugging
{
    public class DebugManager
    {
        private static DebugManager instance = null;

        public static DebugManager Instance
        {
            get
            {
                if(instance == null)
                    instance = new DebugManager();
                return instance;
            }
        }

        public event EventHandler<Exception> UnhandledExceptionThrown;

        private DebugManager()
        {
            
        }

        public void EnableDebugMode()
        {
            try
            {
                Application.ThreadException += Application_UnhandledExceptionThrown;
                AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledExceptionThrown;
            }catch(Exception)
            { }
        }

        private void AppDomain_UnhandledExceptionThrown(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if(UnhandledExceptionThrown != null)
                    UnhandledExceptionThrown.Invoke(sender, e.ExceptionObject as Exception);
            }
            catch(Exception)
            { }
        }

        private void Application_UnhandledExceptionThrown(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                if(UnhandledExceptionThrown != null)
                    UnhandledExceptionThrown.Invoke(sender, e.Exception);
            }catch(Exception)
            { }
        }

        public void DisableDebugMode()
        {
            try
            {
                Application.ThreadException -= Application_UnhandledExceptionThrown;
                AppDomain.CurrentDomain.UnhandledException -= AppDomain_UnhandledExceptionThrown;
            }catch(Exception)
            { }
        }
    }
}
