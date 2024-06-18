using Lightness.Core;
using Lightness.Framework;
using Lightness.Media;
using Lightness.Resources;

namespace LEContents {
	public class PDAdvertise {
		private static bool SetAdvertise;

		public static int AdvertiseID = 99999;

		public static ContentReturn Initialize() {
			MediaCommon.CloseAll();
			SetAdvertise = false;
			AdvertiseID = 0;
			GameCommon.CheckNetworkStatus();
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if(!SetAdvertise) {
				switch(AdvertiseID) {
					case 0:
						Advertise.Set("WarnMsg_CDNGC.png", 10, 120);
						break;
					case 1:
						Advertise.Set("CircleLogo.png", 10, 60);
						break;
					case 2:
						Advertise.Set("TeamDangoLogo.png", 10, 60);
						break;
					case 3:
						Advertise.Set("DNLogo.png", 10, 60);
						break;
					default:
						Scene.Set("Title");
						return ContentReturn.CHANGE;
				}
				AdvertiseID++;
				SetAdvertise = true;
			}
			if(Advertise.Exec() == ContentReturn.END) {
				SetAdvertise = false;
			}
			GameCommon.DrawNetworkError();
			return ContentReturn.OK;
		}
	}
}
