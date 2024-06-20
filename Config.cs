using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System.Diagnostics;

namespace LEContents {
	public static class Config {
		public static Texture MenuTitleText = null;

		public static Texture VerText = null;

		public static Texture MainText = null;

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
			MediaCommon.CloseAll();
			GameCommon.CheckNetworkStatus();
			BG = Texture.CreateFromFile("ConfigBG.png");
			CancelSE.LoadFile("DNGErr.wav");
			Texture.SetFont("Meiryo");
			Texture.SetTextSize(32);
			Texture.SetTextColor(255, 255, 255);
			MenuTitleText = Texture.CreateFromText("設定");
			Texture.SetTextSize(14);
			VerText = Texture.CreateFromText(GameCommon.Version.Get());
			MainMenu = new Menu("Meiryo", 26, 255, 255, 255, 255);
			MainMenu.SetPointer("DangoMenu.png");
			MainMenu.SetSE("Menu.wav", "DNGOut.wav");
			MainMenu.Add("プレイヤー名の変更");
			if(GameCommon.NetworkStatus) {
				MainMenu.Add("アップデート履歴");
				MainMenu.Add("サークルからのお知らせ");
			}
			MainMenu.Add("サークルサイトへ");
			MainMenu.Add("サークル Twitter: @PrjDark へ");
			MainMenu.Add("もどる");
			ShowDetail = false;
			HelpInit = false;
			HelpImage = "";
			NowFadeOut = false;
			Effect.Reset();
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
				Scene.Set("Title");
				Effect.Reset();
				ShowDetail = false;
				NowFadeOut = true;
				CancelSE.Play();
			}
			Core.Draw(BG, 0, 0);
			Core.Draw(MenuTitleText, 20, 20);
			Core.Draw(VerText, 20, 110);
			ContentReturn contentReturn = MainMenu.Exec(320, 120);
			if(contentReturn == ContentReturn.END) {
				ShowDetail = true;
				if(MainMenu.Selected == 0) {
					Scene.Set("Config_PlayerName");
					return ContentReturn.CHANGE;
				}
				if(GameCommon.NetworkStatus) {
					if(MainMenu.Selected == 1) {
						Scene.Set("Config_UpdateHistory");
						return ContentReturn.CHANGE;
					}
					if(MainMenu.Selected == 2) {
						Scene.Set("Config_CircleInfo");
						return ContentReturn.CHANGE;
					}
				}
				if(MainMenu.Selected == MainMenu.MenuList.Count - 3) {
					Process.Start("http://c.xprj.net/");
					Menu.Disabled = false;
				}
				if(MainMenu.Selected == MainMenu.MenuList.Count - 2) {
					Process.Start("http://twitter.com/PrjDark");
					Menu.Disabled = false;
				}
				if(MainMenu.Selected == MainMenu.MenuList.Count - 1) {
					Scene.Set("Title");
					NowFadeOut = true;
					Effect.Reset();
				}
			}
			if(!NowFadeOut) {
				Effect.Fadein();
			} else if(Effect.Fadeout() == ContentReturn.END) {
				return ContentReturn.CHANGE;
			}
			GameCommon.DrawCInfo();
			GameCommon.DrawNetworkError();
			return ContentReturn.OK;
		}
	}
}
