using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System;
using System.Threading;

namespace LEContents {
	public static class Ranking {
		public static Texture[] LoadingBG = new Texture[4];

		public static Texture BG = null;

		public static Texture HelpText = null;

		public static Texture MenuTitleText = null;

		public static SE CancelSE = new SE();

		public static SE DecideSE = new SE();

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static int LoadingState = 0;

		public static bool Error = false;

		public static bool View = false;

		public static bool Fadeout = false;

		public static string[] MenuStr = null;

		public static Menu RankMenu = null;

		public static ContentReturn Initialize() {
			Ranking.Error = false;
			Ranking.View = false;
			Ranking.Fadeout = false;
			Ranking.LoadingState = 0;
			Core.SetTitle(GameCommon.Version.Title);
			for (int i = 0; i < Ranking.LoadingBG.Length; i++) {
				Ranking.LoadingBG[i] = Texture.CreateFromFile("Loading" + i + ".png");
			}
			Effect.Reset();
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if (Ranking.LoadingState == 2) {
				Ranking.CancelSE.LoadFile("DNGErr.wav");
				Ranking.DecideSE.LoadFile("DNGOut.wav");
			}
			if (Ranking.LoadingState == 3) {
				DNet dNet = new DNet("http://CDNGC.network.xprj.net/Ranking/Menu.txt");
				if (dNet.Status <= 350) {
					Ranking.MenuStr = dNet.GetStrings();
					Ranking.BG = Texture.CreateFromFile("HelpBG.png");
					Texture.SetFont("Meiryo");
					Texture.SetTextSize(32);
					Texture.SetTextColor(0, 0, 0);
					Ranking.MenuTitleText = Texture.CreateFromText("オンライン ランキング");
					Ranking.RankMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
					Ranking.RankMenu.SetPointer("DangoMenu.png");
					Ranking.RankMenu.SetSE("Menu.wav", "DNGOut.wav");
					for (int i = 1; i < Ranking.MenuStr.Length; i++) {
						Ranking.RankMenu.Add(Ranking.MenuStr[i]);
					}
					Ranking.RankMenu.Add("もどる");
				} else {
					Ranking.Error = true;
				}
			}
			if (Ranking.LoadingState >= 4) {
				if (Ranking.Error) {
					if (Ranking.HelpText == null) {
						Texture.SetFont("Meiryo");
						Texture.SetTextSize(24);
						Texture.SetTextColor(255, 255, 255);
						Ranking.HelpText = Texture.CreateFromText("データの取得に失敗しました。\nキャンセルボタンで前の画面に戻ります");
						Ranking.BG = Texture.CreateFromFile("ConfigBG.png");
					}
					Core.Draw(Ranking.BG, 0, 0);
					Core.Draw(Ranking.HelpText, 480 - Ranking.HelpText.Width / 2, 270 - Ranking.HelpText.Height / 2);
					if (Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || Ranking.VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
						Ranking.CancelSE.Play();
						Ranking.Fadeout = true;
					}
				} else if (!Ranking.View) {
					Core.Draw(Ranking.BG, 0, 0);
					Core.Draw(Ranking.MenuTitleText, 20, 20);
					if (Ranking.RankMenu.Exec(360, 100) == ContentReturn.END) {
						if (Ranking.RankMenu.Selected == Ranking.MenuStr.Length - 1) {
							Scene.Set("Title");
							return ContentReturn.CHANGE;
						}
						Ranking.View = true;
						DNet dNet2 = new DNet("http://CDNGC.network.xprj.net/Ranking/Data" + Ranking.RankMenu.Selected + ".txt");
						TextViewer.Initialize(dNet2.GetStrings());
						if (dNet2.Status > 350) {
							Ranking.Error = true;
							return ContentReturn.OK;
						}
					}
					if (Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
						Ranking.CancelSE.Play();
						Ranking.Fadeout = true;
					}
				} else {
					Core.Draw(Ranking.BG, 0, 0);
					TextViewer.Exec();
					if (Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0 || Ranking.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0 || Ranking.VIOEx.GetPointOnce(VirtualIO.PointID.R) != 0) {
						Ranking.DecideSE.Play();
						Ranking.View = false;
						Menu.Disabled = false;
					}
				}
			} else {
				Core.Draw(Ranking.LoadingBG[Ranking.LoadingState], 920, 560);
				Thread.Sleep(100);
				Ranking.LoadingState++;
			}
			if (Ranking.Fadeout && Effect.Fadeout() == ContentReturn.END) {
				Scene.Set("Title");
				return ContentReturn.CHANGE;
			}
			return ContentReturn.OK;
		}
	}
}
