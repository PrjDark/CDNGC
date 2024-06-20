using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.Resources;

namespace LEContents {
	public static class GameCommon {
		public static VersionInfo Version = new VersionInfo("CDNGC:J:A:C:20230815001", "だんごたべほうだい２ ～新・だんご達の挑戦状～");

		public static bool NetworkStatus = false;

		public static bool UpdateAvailable = false;

		public static string[] DNetMarker = null;

		public static string[] DNetCInfo = null;

		public static string[] DNetNewVer = null;

		public static string NewVerURL = "";

		public static Texture TDNetCInfo = Texture.CreateFromText(" ");

		public static Texture DNErrMsg = Texture.CreateFromText(" ");

		public static Texture DNNewVer = Texture.CreateFromText(" ");

		public static bool InitDNE = false;

		public static int DCIPos = 0;

		public static ContentReturn CheckNetworkStatus() {
			try {
				UpdateAvailable = false;
				DNet dNet = new DNet("http://CDNGC.network.xprj.net/dNetwork.txt");
				DNetMarker = dNet.GetStrings();
				if(DNetMarker[0] == "d-Network") {
					NetworkStatus = true;
					DNetCInfo = new DNet("http://CDNGC.network.xprj.net/Information/Circle.txt").GetStrings();
					if(DNetCInfo[0] == "dNetwork.Information.Circle") {
						Texture.SetFont("Meiryo");
						Texture.SetTextSize(20);
						Texture.SetTextColor(255, 255, 255);
						TDNetCInfo = Texture.CreateFromText(DNetCInfo[1]);
					}
					DNet dNet2 = new DNet("http://CDNGC.update.network.xprj.net/" + Version.GetNet() + ".txt");
					if(dNet2.Status <= 350) {
						DNetNewVer = dNet2.GetStrings();
					} else {
						dNet2 = new DNet("http://CDNGC.update.network.xprj.net/" + Version.APPID + "_" + Version.SKU + "_" + Version.TYPE + "_" + Version.REV + ".txt");
						if(dNet2.Status <= 350) {
							DNetNewVer = dNet2.GetStrings();
						} else {
							dNet2 = new DNet("http://CDNGC.update.network.xprj.net/" + Version.APPID + ".txt");
							if(dNet2.Status <= 350) {
								DNetNewVer = dNet2.GetStrings();
							} else {
								dNet2 = new DNet("http://CDNGC.update.network.xprj.net/Version.txt");
								if(dNet2.Status > 350) {
									return ContentReturn.END;
								}
								DNetNewVer = dNet2.GetStrings();
							}
						}
					}
					NewVerURL = DNetNewVer[1];
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 1;
					try {
						num = int.Parse(Version.DATE);
						num3 = int.Parse(Version.CNT);
						VersionInfo versionInfo = new VersionInfo(DNetNewVer[0]);
						num2 = int.Parse(versionInfo.DATE);
						num4 = int.Parse(versionInfo.CNT);
					} catch {
					}
					if(num < num2 || (num == num2 && num3 < num4)) {
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
	}
}
