using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.Resources;

namespace LEContents {
	public static class GameCommon {
		public static VersionInfo Version;

		public static bool NetworkStatus;

		public static bool UpdateAvailable;

		public static string[] DNetMarker;

		public static string[] DNetCInfo;

		public static string[] DNetNewVer;

		public static string NewVerURL;

		public static Texture TDNetCInfo;

		public static Texture DNErrMsg;

		public static Texture DNNewVer;

		public static bool InitDNE;

		public static int DCIPos;

		public static ContentReturn CheckNetworkStatus() {
			try {
				UpdateAvailable = false;
				DNetMarker = new DNet("http://CDNGC.network.dark-x.net/dNetwork.txt").GetStrings();
				if(DNetMarker[0] == "d-Network") {
					NetworkStatus = true;
					DNetCInfo = new DNet("http://CDNGC.network.dark-x.net/Information/Circle.txt").GetStrings();
					if(DNetCInfo[0] == "dNetwork.Information.Circle") {
						Texture.SetFont("Meiryo");
						Texture.SetTextSize(20);
						Texture.SetTextColor(255, 255, 255);
						TDNetCInfo = Texture.CreateFromText(DNetCInfo[1]);
					}
					DNet dNet = new DNet("http://CDNGC.update.network.dark-x.net/" + Version.GetNet() + ".txt");
					if(dNet.Status <= 350) {
						DNetNewVer = dNet.GetStrings();
					} else {
						dNet = new DNet("http://CDNGC.update.network.dark-x.net/" + Version.APPID + "_" + Version.SKU + "_" + Version.TYPE + "_" + Version.REV + ".txt");
						if(dNet.Status <= 350) {
							DNetNewVer = dNet.GetStrings();
						} else {
							dNet = new DNet("http://CDNGC.update.network.dark-x.net/" + Version.APPID + ".txt");
							if(dNet.Status <= 350) {
								DNetNewVer = dNet.GetStrings();
							} else {
								dNet = new DNet("http://CDNGC.update.network.dark-x.net/Version.txt");
								if(dNet.Status > 350) {
									return ContentReturn.END;
								}
								DNetNewVer = dNet.GetStrings();
							}
						}
					}
					NewVerURL = DNetNewVer[1];
					if(Version.Get() != DNetNewVer[0]) {
						Debug.Log('I', "DNetwork", "New Version is detected: {0}", DNetNewVer[0]);
						UpdateAvailable = true;
					} else {
						Debug.Log('I', "DNetwork", "Recent Version is running: {0}", DNetNewVer[0]);
						UpdateAvailable = false;
					}
				} else {
					UpdateAvailable = false;
					NetworkStatus = false;
				}
			} catch {
				Debug.Log('W', "DNetwork", "Failed");
			}
			if(!InitDNE) {
				DNErrMsg = Texture.CreateFromFile("DN_ErrMsg.png");
				DNNewVer = Texture.CreateFromFile("DN_NewVer.png");
				InitDNE = true;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn DrawNetworkError() {
			if(!NetworkStatus) {
				Core.Draw(DNErrMsg, 660, 10, 255);
			}
			if(UpdateAvailable) {
				Core.Draw(DNNewVer, 660, 10, 255);
			}
			return ContentReturn.OK;
		}

		public static ContentReturn DrawCInfo() {
			try {
				Core.Draw(Effect.Black, 0, 690);
				Core.Draw(TDNetCInfo, Common.WindowX - DCIPos, 690);
				DCIPos += 2;
				if(DCIPos >= Common.WindowX + TDNetCInfo.Width) {
					DCIPos = 0;
				}
			} catch {
			}
			return ContentReturn.OK;
		}

		static GameCommon() {
			Version = new VersionInfo("CDNGC:E:B:B:20191231002", "DANGO: The Puzzle 3 - Skewered Dangos -");
			NetworkStatus = false;
			UpdateAvailable = false;
			DNetMarker = null;
			DNetCInfo = null;
			DNetNewVer = null;
			NewVerURL = "";
			TDNetCInfo = Texture.CreateFromText(" ");
			DNErrMsg = Texture.CreateFromText(" ");
			DNNewVer = Texture.CreateFromText(" ");
			InitDNE = false;
			DCIPos = 0;
		}
	}
}
