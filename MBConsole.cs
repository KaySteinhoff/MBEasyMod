using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MBEasyMod
{
    public class MBConsole
    {
        public static Color ConsoleColor { get; set; } = Color.White;

        public static void Log(string format, params object[] args)
        {
            InformationManager.DisplayMessage(new InformationMessage(String.Format(format, args), ConsoleColor));
        }
    }
}
