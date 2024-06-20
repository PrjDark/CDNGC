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
			NowFadeOut = false;
			GameSelectMode = false;
			LoadingState = 0;
			Loading = true;
			for(int i = 0; i < LoadingBG.Length; i++) {
				LoadingBG[i] = Texture.CreateFromFile("Loading" + i + ".png");
			}
			Texture.SetFont("Consolas");
			Texture.SetTextSize(20);
			Texture.SetTextColor(0, 0, 0);
			VersionText = Texture.CreateFromText(GameCommon.Version.Get() + "*");
			MainMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
			MainMenu.Add("DANGO: The Puzzle");
			MainMenu.Add("How to play");
			MainMenu.Add("Online Ranking");
			MainMenu.Add("Settings");
			MainMenu.Add("Manually update check");
			MainMenu.Add("End Game");
			GameMenu = new Menu("Meiryo", 26, 0, 0, 0, 255);
			GameMenu.Add("Dango all you want: taste");
			GameMenu.Add("Dango all you want: slowly");
			GameMenu.Add("Dango all you want: alone");
			GameMenu.Add("Challange from Dango");
			GameMenu.Add("New・Challange from Dango");
			GameMenu.Add("Back");
			FramesCount = 0;
			Effect.Reset();
			GameCommon.CheckNetworkStatus();
			if(GameCommon.NetworkStatus) {
				NetStatText = Texture.CreateFromFile("DN_OK.png");
			} else {
				NetStatText = Texture.CreateFromFile("DN_ERR.png");
			}
			ICStatIcon = Texture.CreateFromFile("DNIC_ERR.png");
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if(Loading) {
				Core.Draw(LoadingBG[LoadingState % 4], 920, 560);
				switch(LoadingState) {
					case 0:
						CancelSE.LoadFile("DNGErr.wav");
						break;
					case 1:
						TitleBGM.LoadFile("BGM_Title.wav");
						break;
					case 2:
						BG = Texture.CreateFromFile("Title.png");
						break;
					case 3:
						MainMenu.SetPointer("DangoMenu.png");
						break;
					case 4:
						MainMenu.SetSE("Menu.wav", "DNGOut.wav");
						break;
					case 5:
						GameMenu.SetPointer("DangoMenu.png");
						break;
					case 6:
						GameMenu.SetSE("Menu.wav", "DNGOut.wav");
						break;
					case 7:
						TitleBGM.Play();
						break;
					case 8:
						Loading = false;
						break;
				}
				LoadingState++;
				return ContentReturn.OK;
			}
			Core.Draw(BG, 0, 0);
			Core.Draw(VersionText, 10, 10);
			Core.Draw(NetStatText, 10, 35);
			Core.Draw(ICStatIcon, 79, 35);
			ContentReturn contentReturn = ContentReturn.OK;
			if(!GameSelectMode) {
				contentReturn = MainMenu.Exec(480, 400);
				if(contentReturn == ContentReturn.CHANGE) {
					FramesCount = 0;
				}
				if(contentReturn == ContentReturn.END) {
					Effect.Reset();
					NowFadeOut = true;
					switch(MainMenu.Selected) {
						case 0:
							GameSelectMode = true;
							NowFadeOut = false;
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
			if(GameSelectMode) {
				contentReturn = GameMenu.Exec(480, 400);
				if(contentReturn == ContentReturn.END) {
					Effect.Reset();
					NowFadeOut = true;
					switch(GameMenu.Selected) {
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
							GameSelectMode = false;
							NowFadeOut = false;
							Effect.Level = 255;
							Menu.Disabled = false;
							return ContentReturn.OK;
					}
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0) {
					CancelSE.Play();
					GameSelectMode = false;
				}
			}
			if(!NowFadeOut) {
				Effect.Fadein();
				FramesCount++;
				if(FramesCount > 1200 && !GameSelectMode) {
					MediaCommon.CloseAll();
					Scene.Set("PDAdvertise");
					return ContentReturn.CHANGE;
				}
			} else if(Effect.Fadeout() == ContentReturn.END) {
				MediaCommon.CloseAll();
				return ContentReturn.CHANGE;
			}
			GameCommon.DrawNetworkError();
			GameCommon.DrawCInfo();
			return ContentReturn.OK;
		}
	}
}
