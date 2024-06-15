using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System;

namespace LEContents {
	public static class HowToPlay {
		public static Texture MenuTitleText = null;

		public static Texture HelpImageTex = null;

		public static Texture HelpHelpTex = null;

		public static Texture BG = null;

		public static bool ShowDetail = false;

		public static bool HelpInit = false;

		public static string HelpImage = "";

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static SE CancelSE = new SE();

		public static Menu MainMenu = null;

		public static bool NowFadeOut = false;

		public static int Counter = 0;

		public static ContentReturn Initialize() {
			Effect.Reset();
			return HowToPlay.InitializeEx();
		}

		public static ContentReturn InitializeEx() {
			HowToPlay.NowFadeOut = false;
			HowToPlay.BG = Texture.CreateFromFile("HelpBG.png");
			HowToPlay.CancelSE.LoadFile("DNGErr.wav");
			Texture.SetFont("Consolas");
			Texture.SetTextSize(32);
			Texture.SetTextColor(0, 0, 0);
			HowToPlay.MenuTitleText = Texture.CreateFromText("さくらのたべかた講座\u3000\u3000");
			HowToPlay.MainMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
			HowToPlay.MainMenu.SetPointer("DangoMenu.png");
			HowToPlay.MainMenu.SetSE("Menu.wav", "DNGOut.wav");
			HowToPlay.MainMenu.Add("PCでの操作");
			HowToPlay.MainMenu.Add("アーケードでの操作");
			HowToPlay.MainMenu.Add("何をすればいいのか");
			HowToPlay.MainMenu.Add("焦げているだんごに注意！");
			HowToPlay.MainMenu.Add("出荷して高得点");
			HowToPlay.MainMenu.Add("難易度による違い");
			HowToPlay.MainMenu.Add("高得点のヒント");
			HowToPlay.MainMenu.Add("もどる");
			HowToPlay.ShowDetail = false;
			HowToPlay.HelpInit = false;
			HowToPlay.HelpImage = "";
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			Core.Draw(HowToPlay.BG, 0, 0);
			if (HowToPlay.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
				Scene.Set("Title");
				Effect.Reset();
				HowToPlay.ShowDetail = false;
				HowToPlay.NowFadeOut = true;
				HowToPlay.CancelSE.Play();
			}
			if (!HowToPlay.ShowDetail) {
				Core.Draw(HowToPlay.MenuTitleText, 20, 20);
				ContentReturn contentReturn = HowToPlay.MainMenu.Exec(320, 120);
				if (contentReturn == ContentReturn.END) {
					HowToPlay.ShowDetail = true;
					switch (HowToPlay.MainMenu.Selected) {
						case 0:
							HowToPlay.HelpImage = "Help_Control.png";
							break;
						case 1:
							HowToPlay.HelpImage = "Help_Control2.png";
							break;
						case 2:
							HowToPlay.HelpImage = "Help_Basic.png";
							break;
						case 3:
							HowToPlay.HelpImage = "Help_Burn.png";
							break;
						case 4:
							HowToPlay.HelpImage = "Help_Out.png";
							break;
						case 5:
							HowToPlay.HelpImage = "Help_Diff.png";
							break;
						case 6:
							HowToPlay.HelpImage = "Help_Hint.png";
							break;
						case 7:
							Scene.Set("Title");
							Effect.Reset();
							HowToPlay.ShowDetail = false;
							HowToPlay.NowFadeOut = true;
							break;
					}
				}
			} else if (!HowToPlay.HelpInit) {
				HowToPlay.HelpImageTex = Texture.CreateFromFile(HowToPlay.HelpImage);
				Texture.SetFont("Meiryo");
				Texture.SetTextSize(24);
				Texture.SetTextColor(0, 0, 0);
				HowToPlay.HelpHelpTex = Texture.CreateFromText("クリック または 決定ボタン で前の画面へ戻ります");
				HowToPlay.HelpInit = true;
				HowToPlay.Counter = 0;
			} else {
				Core.Draw(HowToPlay.HelpImageTex, 0, 0);
				int arg_19F_0 = HowToPlay.Counter;
				HowToPlay.Counter++;
				if (HowToPlay.Counter > 60) {
					HowToPlay.Counter = 0;
				}
				if (HowToPlay.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || HowToPlay.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || HowToPlay.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0 || HowToPlay.VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
					int selected = HowToPlay.MainMenu.Selected;
					HowToPlay.InitializeEx();
					HowToPlay.MainMenu.Selected = selected;
				}
			}
			if (!HowToPlay.NowFadeOut) {
				Effect.Fadein();
			} else if (Effect.Fadeout() == ContentReturn.END) {
				return ContentReturn.CHANGE;
			}
			GameCommon.DrawCInfo();
			return ContentReturn.OK;
		}
	}
}
