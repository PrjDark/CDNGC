using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System;
using System.Diagnostics;

namespace LEContents {
	public static class Title {
		public static Texture[] LoadingBG = new Texture[4];

		public static int LoadingState = 0;

		public static bool Loading = true;

		public static Texture VersionText = null;

		public static Texture NetStatText = null;

		public static Texture ICStatIcon = null;

		public static Texture BG = null;

		public static int FramesCount = 0;

		public static SE TitleBGM = new SE();

		public static SE CancelSE = new SE();

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static bool NowFadeOut = false;

		public static bool GameSelectMode = false;

		public static Menu MainMenu = null;

		public static Menu GameMenu = null;

		public static ContentReturn Initialize() {
			MediaCommon.CloseAll();
			Title.NowFadeOut = false;
			Title.GameSelectMode = false;
			Title.LoadingState = 0;
			Title.Loading = true;
			for (int i = 0; i < Title.LoadingBG.Length; i++) {
				Title.LoadingBG[i] = Texture.CreateFromFile("Loading" + i + ".png");
			}
			Texture.SetFont("Consolas");
			Texture.SetTextSize(20);
			Texture.SetTextColor(0, 0, 0);
			Title.VersionText = Texture.CreateFromText(GameCommon.Version.Get() + "*");
			Title.MainMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
			Title.MainMenu.Add("だんごたべほうだい");
			Title.MainMenu.Add("さくらのたべかた講座");
			Title.MainMenu.Add("オンラインランキング");
			Title.MainMenu.Add("設定");
			Title.MainMenu.Add("手動アップデートの確認");
			Title.MainMenu.Add("終了");
			Title.GameMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
			Title.GameMenu.Add("だんご試食会");
			Title.GameMenu.Add("のんびりたべほうだい");
			Title.GameMenu.Add("ひとりでたべほうだい");
			Title.GameMenu.Add("だんごたちの挑戦状");
			Title.GameMenu.Add("新・だんごたちの挑戦状");
			Title.GameMenu.Add("もどる");
			Title.FramesCount = 0;
			Effect.Reset();
			GameCommon.CheckNetworkStatus();
			if (GameCommon.NetworkStatus) {
				Title.NetStatText = Texture.CreateFromFile("DN_OK.png");
			} else {
				Title.NetStatText = Texture.CreateFromFile("DN_ERR.png");
			}
			Title.ICStatIcon = Texture.CreateFromFile("DNIC_ERR.png");
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if (Title.Loading) {
				Core.Draw(Title.LoadingBG[Title.LoadingState % 4], 920, 560);
				switch (Title.LoadingState) {
					case 0:
						Title.CancelSE.LoadFile("DNGErr.wav");
						break;
					case 1:
						Title.TitleBGM.LoadFile("BGM_Title.wav");
						break;
					case 2:
						Title.BG = Texture.CreateFromFile("Title.png");
						break;
					case 3:
						Title.MainMenu.SetPointer("DangoMenu.png");
						break;
					case 4:
						Title.MainMenu.SetSE("Menu.wav", "DNGOut.wav");
						break;
					case 5:
						Title.GameMenu.SetPointer("DangoMenu.png");
						break;
					case 6:
						Title.GameMenu.SetSE("Menu.wav", "DNGOut.wav");
						break;
					case 7:
						Title.TitleBGM.Play();
						break;
					case 8:
						Title.Loading = false;
						break;
				}
				Title.LoadingState++;
				return ContentReturn.OK;
			}
			Core.Draw(Title.BG, 0, 0);
			Core.Draw(Title.VersionText, 10, 10);
			Core.Draw(Title.NetStatText, 10, 35);
			Core.Draw(Title.ICStatIcon, 79, 35);
			if (!Title.GameSelectMode) {
				ContentReturn contentReturn = Title.MainMenu.Exec(480, 400);
				if (contentReturn == ContentReturn.CHANGE) {
					Title.FramesCount = 0;
				}
				if (contentReturn == ContentReturn.END) {
					Effect.Reset();
					Title.NowFadeOut = true;
					switch (Title.MainMenu.Selected) {
						case 0:
							Title.GameSelectMode = true;
							Title.NowFadeOut = false;
							Effect.Level = 255;
							Menu.Disabled = false;
							return ContentReturn.OK;
						case 1:
							Scene.Set("HowToPlay");
							break;
						case 2:
							Scene.Set("Ranking");
							break;
						case 3:
							Scene.Set("Config");
							break;
						case 4:
							Process.Start("http://project.xprj.net/game/CDNGC");
							Menu.Disabled = false;
							Environment.Exit(0);
							break;
						case 5:
							Scene.Set("GameEnd");
							break;
					}
				}
			}
			if (Title.GameSelectMode) {
				ContentReturn contentReturn = Title.GameMenu.Exec(480, 400);
				if (contentReturn == ContentReturn.END) {
					Effect.Reset();
					Title.NowFadeOut = true;
					switch (Title.GameMenu.Selected) {
						case 0:
							Scene.Set("CDNGC_P");
							break;
						case 1:
							Scene.Set("CDNGC_E");
							break;
						case 2:
							Scene.Set("CDNGC_N");
							break;
						case 3:
							Scene.Set("CDNGC_H");
							break;
						case 4:
							Scene.Set("CDNGC_X");
							break;
						case 5:
							Title.GameSelectMode = false;
							Title.NowFadeOut = false;
							Effect.Level = 255;
							Menu.Disabled = false;
							return ContentReturn.OK;
					}
				}
				if (Title.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
					Title.CancelSE.Play();
					Title.GameSelectMode = false;
				}
			}
			if (!Title.NowFadeOut) {
				Effect.Fadein();
				Title.FramesCount++;
				if (Title.FramesCount > 1200 && !Title.GameSelectMode) {
					MediaCommon.CloseAll();
					Scene.Set("PDAdvertise");
					return ContentReturn.CHANGE;
				}
			} else if (Effect.Fadeout() == ContentReturn.END) {
				MediaCommon.CloseAll();
				return ContentReturn.CHANGE;
			}
			GameCommon.DrawNetworkError();
			GameCommon.DrawCInfo();
			return ContentReturn.OK;
		}
	}
}
