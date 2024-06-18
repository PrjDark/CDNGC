using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
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
			Error = false;
			View = false;
			Fadeout = false;
			LoadingState = 0;
			Core.SetTitle(GameCommon.Version.Title);
			for(int i = 0; i < LoadingBG.Length; i++) {
				LoadingBG[i] = Texture.CreateFromFile("Loading" + i + ".png");
			}
			Effect.Reset();
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if(LoadingState == 2) {
				CancelSE.LoadFile("DNGErr.wav");
				DecideSE.LoadFile("DNGOut.wav");
			}
			if(LoadingState == 3) {
				DNet dNet = new DNet("http://CDNGC.network.dark-x.net/Ranking/Menu.txt");
				if(dNet.Status <= 350) {
					MenuStr = dNet.GetStrings();
					BG = Texture.CreateFromFile("HelpBG.png");
					Texture.SetFont("Meiryo");
					Texture.SetTextSize(32);
					Texture.SetTextColor(0, 0, 0);
					MenuTitleText = Texture.CreateFromText("オンライン ランキング");
					RankMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
					RankMenu.SetPointer("DangoMenu.png");
					RankMenu.SetSE("Menu.wav", "DNGOut.wav");
					for(int i = 1; i < MenuStr.Length; i++) {
						RankMenu.Add(MenuStr[i]);
					}
					RankMenu.Add("もどる");
				} else {
					Error = true;
				}
			}
			if(LoadingState >= 4) {
				if(Error) {
					if(HelpText == null) {
						Texture.SetFont("Meiryo");
						Texture.SetTextSize(24);
						Texture.SetTextColor(255, 255, 255);
						HelpText = Texture.CreateFromText("データの取得に失敗しました。\nキャンセルボタンで前の画面に戻ります");
						BG = Texture.CreateFromFile("ConfigBG.png");
					}
					Core.Draw(BG, 0, 0);
					Core.Draw(HelpText, 480 - HelpText.Width / 2, 270 - HelpText.Height / 2);
					if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
						CancelSE.Play();
						Fadeout = true;
					}
				} else if(!View) {
					Core.Draw(BG, 0, 0);
					Core.Draw(MenuTitleText, 20, 20);
					if(RankMenu.Exec(360, 100) == ContentReturn.END) {
						if(RankMenu.Selected == MenuStr.Length - 1) {
							Scene.Set("Title");
							return ContentReturn.CHANGE;
						}
						View = true;
						DNet dNet2 = new DNet("http://CDNGC.network.dark-x.net/Ranking/Data" + RankMenu.Selected + ".txt");
						TextViewer.Initialize(dNet2.GetStrings());
						if(dNet2.Status > 350) {
							Error = true;
							return ContentReturn.OK;
						}
					}
					if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
						CancelSE.Play();
						Fadeout = true;
					}
				} else {
					Core.Draw(BG, 0, 0);
					TextViewer.Exec();
					if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0 || VIOEx.GetPointOnce(VirtualIO.PointID.R) != 0) {
						DecideSE.Play();
						View = false;
						Menu.Disabled = false;
					}
				}
			} else {
				Core.Draw(LoadingBG[LoadingState], 920, 560);
				Thread.Sleep(100);
				LoadingState++;
			}
			if(Fadeout && Effect.Fadeout() == ContentReturn.END) {
				Scene.Set("Title");
				return ContentReturn.CHANGE;
			}
			return ContentReturn.OK;
		}
	}
}
