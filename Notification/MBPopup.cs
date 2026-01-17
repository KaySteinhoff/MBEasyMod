using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MBEasyMod.Notification
{
	public class MBPopup
	{
		public static void ShowMultipleSelectionPopup(MultiSelectionInquiryData data, bool pauseGame = false, bool prioritize = false)
		{
			MBInformationManager.ShowMultiSelectionInquiry(data, pauseGame, prioritize);
		}

		public static void ShowSimplePopup(string title, string description, string approveText, string disapproveText, Action approveAction, Action disapproveAction)
		{
			InformationManager.ShowInquiry(new InquiryData(title, description, true, true, approveText, disapproveText, approveAction, disapproveAction));
		}
		
		public static void ShowSimpleNotificationPopup(string title, string description, string dismissText, Action dismissAction)
		{
			InformationManager.ShowInquiry(new InquiryData(title, description, true, false, dismissText, "", dismissAction, () => { }));
		}

		public static void ShowSimpleAlertPopup(string title, string description, string dismissText, Action dismissAction)
		{
			InformationManager.ShowInquiry(new InquiryData(title, description, false, true, "", dismissText, () => { }, dismissAction));
		}
	}
}
