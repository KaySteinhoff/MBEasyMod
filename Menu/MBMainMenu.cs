using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MBEasyMod.Menu
{
    public class MBMainMenu
    {
        public static void AddMenuOption(string buttonText, int orderIndex, Action clickAction)
        {
            Module.CurrentModule.AddInitialStateOption(
                new InitialStateOption(
                    buttonText.Replace(" ", ""),
                    new TextObject(buttonText, null),
                    orderIndex, 
                    clickAction, 
                    () => { return (false, null); }
                    )
                );
        }
    }
}
