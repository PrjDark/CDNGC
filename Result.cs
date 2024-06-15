using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Resources;
using System;
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
			Result.BG = Texture.CreateFromFile("GameResult.png");
			Result.DNRegOK = Texture.CreateFromFile("DN_RegOK.png");
			Result.DNRegErr = Texture.CreateFromFile("DN_RegErr.png");
			Effect.Reset();
			Result.NowFadeOut = false;
			Texture.SetFont("Meiryo");
			Texture.SetTextColor(0, 0, 0);
			Texture.SetTextSize(36);
			Result.TDiffLv = Texture.CreateFromText(CDNGC.SDiffLv);
			Result.TOutCountTotal = new Texture[CDNGC.DangoTypes];
			Result.TTotalOut = Texture.CreateFromText(string.Format("{0} 回", CDNGC.OutTotal));
			Result.TTotalCount = Texture.CreateFromText(string.Format("{0} 個", CDNGC.OutCount));
			Result.TWorkTime = Texture.CreateFromText(string.Format("{0} 秒", CDNGC.WorkTime));
			Texture.SetTextSize(24);
			string text = "";
			for (int i = 0; i < CDNGC.DangoTypes; i++) {
				Result.TOutCountTotal[i] = Texture.CreateFromText(string.Format("x {0}", CDNGC.OutDangoCountTotal[i]));
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"&Dango",
					i,
					"=",
					CDNGC.OutDangoCountTotal[i]
				});
			}
			Texture.SetTextSize(48);
			Result.TScore = Texture.CreateFromText(string.Format("{0}", CDNGC.Score));
			ContentStream contentStream = new ContentStream("UserData.lec", true);
			StreamReader streamReader = new StreamReader(contentStream, Encoding.UTF8, true);
			string text2 = streamReader.ReadLine();
			contentStream.Close();
			if (text2 == null) {
				text2 = "";
			}
			string text3 = "*";
			try {
				ContentStream contentStream2 = new ContentStream("dNetworkKey.bin", false);
				StreamReader streamReader2 = new StreamReader(contentStream2, Encoding.ASCII, true);
				text3 = streamReader2.ReadLine();
				contentStream2.Close();
				if (text3 == null) {
					text3 = "";
				}
			} catch {
			}
			string uRL = string.Format("http://CDNGC.network.xprj.net/Ranking/Register?Version={0}&dNetworkKey={8}&UserName={1}&Diff={2}&OutDango={3}&OutCount={4}&WorkTime={5}&Score={6}{7}", new object[]
			{
				GameCommon.Version.Get(),
				text2,
				CDNGC.DiffLv,
				CDNGC.OutTotal,
				CDNGC.OutCount,
				CDNGC.WorkTime,
				CDNGC.Score,
				text,
				text3
			});
			if (text2 == "") {
				Config_PlayerName.ReturnToResult = true;
			}
			DNet dNet = new DNet(uRL);
			if (dNet.Status <= 350) {
				Result.SuccessRegister = true;
			} else {
				Result.SuccessRegister = false;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if (Config_PlayerName.ReturnToResult) {
				Scene.Set("Config_PlayerName");
				return ContentReturn.CHANGE;
			}
			Core.Draw(Result.BG, 0, 0);
			if (Result.SuccessRegister) {
				Core.Draw(Result.DNRegOK, 990, 670, 255);
			} else {
				Core.Draw(Result.DNRegErr, 990, 670, 255);
			}
			for (int i = 1; i < CDNGC.DangoTypes; i++) {
				Core.Draw(CDNGC.Dango[i], 32, 64 + i * 64);
				Core.Draw(Result.TOutCountTotal[i], 96, 96 + i * 64);
			}
			Core.Draw(Result.TDiffLv, 750, 70);
			Core.Draw(Result.TTotalOut, 750, 150);
			Core.Draw(Result.TTotalCount, 750, 210);
			Core.Draw(Result.TWorkTime, 750, 270);
			Core.Draw(Result.TScore, 750, 370);
			if (Result.NowFadeOut) {
				if (Effect.Fadeout() == ContentReturn.END) {
					Scene.Set("Title");
					return ContentReturn.CHANGE;
				}
			} else if (Result.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || Result.VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
				Result.NowFadeOut = true;
				Effect.Reset();
			} else {
				Effect.Fadein();
			}
			return ContentReturn.OK;
		}
	}
}
