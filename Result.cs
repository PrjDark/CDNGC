using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Resources;
using System.IO;
using System.Text;

namespace LEContents {
	public static class Result {
		public static Texture BG = null;

		public static Texture DNRegOK = null;

		public static Texture DNRegErr = null;

		public static Texture TDiffLv = null;

		public static Texture TTotalOut = null;

		public static Texture TTotalCount = null;

		public static Texture TWorkTime = null;

		public static Texture TScore = null;

		public static Texture[] TOutCountTotal = null;

		public static bool NowFadeOut = false;

		public static bool SuccessRegister = false;

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static ContentReturn Initialize() {
			BG = Texture.CreateFromFile("GameResult.png");
			DNRegOK = Texture.CreateFromFile("DN_RegOK.png");
			DNRegErr = Texture.CreateFromFile("DN_RegErr.png");
			Effect.Reset();
			NowFadeOut = false;
			Texture.SetFont("Meiryo");
			Texture.SetTextColor(0, 0, 0);
			Texture.SetTextSize(36);
			TDiffLv = Texture.CreateFromText(CDNGC.SDiffLv);
			TOutCountTotal = new Texture[CDNGC.DangoTypes];
			TTotalOut = Texture.CreateFromText(string.Format("{0} 回", CDNGC.OutTotal));
			TTotalCount = Texture.CreateFromText(string.Format("{0} 個", CDNGC.OutCount));
			TWorkTime = Texture.CreateFromText(string.Format("{0} 秒", CDNGC.WorkTime));
			Texture.SetTextSize(24);
			string text = "";
			for(int i = 0; i < CDNGC.DangoTypes; i++) {
				TOutCountTotal[i] = Texture.CreateFromText(string.Format("x {0}", CDNGC.OutDangoCountTotal[i]));
				object obj = text;
				text = string.Concat(obj, "&Dango", i, "=", CDNGC.OutDangoCountTotal[i]);
			}
			Texture.SetTextSize(48);
			TScore = Texture.CreateFromText(string.Format("{0}", CDNGC.Score));
			ContentStream contentStream = new ContentStream("UserData.lec", true);
			StreamReader streamReader = new StreamReader(contentStream, Encoding.UTF8, true);
			string text2 = streamReader.ReadLine();
			contentStream.Close();
			if(text2 == null) {
				text2 = "";
			}
			string text3 = "*";
			try {
				ContentStream contentStream2 = new ContentStream("dNetworkKey.bin", false);
				StreamReader streamReader2 = new StreamReader(contentStream2, Encoding.ASCII, true);
				text3 = streamReader2.ReadLine();
				contentStream2.Close();
				if(text3 == null) {
					text3 = "";
				}
			} catch {
			}
			string uRL = string.Format("http://CDNGC.network.dark-x.net/Ranking/Register?Version={0}&dNetworkKey={8}&UserName={1}&Diff={2}&OutDango={3}&OutCount={4}&WorkTime={5}&Score={6}{7}", GameCommon.Version.Get(), text2, CDNGC.DiffLv, CDNGC.OutTotal, CDNGC.OutCount, CDNGC.WorkTime, CDNGC.Score, text, text3);
			if(text2 == "") {
				Config_PlayerName.ReturnToResult = true;
			}
			DNet dNet = new DNet(uRL);
			if(dNet.Status <= 350) {
				SuccessRegister = true;
			} else {
				SuccessRegister = false;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if(Config_PlayerName.ReturnToResult) {
				Scene.Set("Config_PlayerName");
				return ContentReturn.CHANGE;
			}
			Core.Draw(BG, 0, 0);
			if(SuccessRegister) {
				Core.Draw(DNRegOK, 990, 670, 255);
			} else {
				Core.Draw(DNRegErr, 990, 670, 255);
			}
			for(int i = 1; i < CDNGC.DangoTypes; i++) {
				Core.Draw(CDNGC.Dango[i], 32, 64 + i * 64);
				Core.Draw(TOutCountTotal[i], 96, 96 + i * 64);
			}
			Core.Draw(TDiffLv, 750, 70);
			Core.Draw(TTotalOut, 750, 150);
			Core.Draw(TTotalCount, 750, 210);
			Core.Draw(TWorkTime, 750, 270);
			Core.Draw(TScore, 750, 370);
			if(NowFadeOut) {
				if(Effect.Fadeout() == ContentReturn.END) {
					Scene.Set("Title");
					return ContentReturn.CHANGE;
				}
			} else if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
				NowFadeOut = true;
				Effect.Reset();
			} else {
				Effect.Fadein();
			}
			return ContentReturn.OK;
		}
	}
}
