using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System;
using System.Threading;

namespace LEContents {
	public static class CDNGC {
		public enum DangoID {
			None,
			Pink,
			Blue,
			Green,
			Black,
			Yellow,
			Burn
		}

		public static Texture[] LoadingBG = new Texture[4];

		public static Texture BG = null;

		public static Texture BG_HDPadding = null;

		public static Texture GameBox = null;

		public static Texture[] Pointer = new Texture[2];

		public static Texture[] OutButton = new Texture[4];

		public static Texture TBoxTotal = null;

		public static Texture TOutTotal = null;

		public static Texture TOutCount = null;

		public static Texture TRemain = null;

		public static Texture TDiffLv = null;

		public static Texture TAddSpeed = null;

		public static Texture[] TOutDangoCount = null;

		public static Texture TScore = null;

		public static Texture TCoolDown = null;

		public static string BGFile = "";

		public static int BoxTotal = 0;

		public static int OutTotal = 0;

		public static int OutCount = 0;

		public static int Remain = 0;

		public static int WorkTime = 0;

		public static int FCounter = 0;

		public static int DiffLv = 0;

		public static string SDiffLv = "";

		public static int AddSpeed = 0;

		public static int Score = 0;

		public static int CoolDown = 0;

		public static int BurnPercent = 0;

		public static int StartDangoLines = 3;

		public static int PointerX = 0;

		public static int PointerY = 0;

		public static int SelPointerX = 0;

		public static int SelPointerY = 0;

		public static int OldPointerX = 0;

		public static int OldPointerY = 0;

		public static int OutButtonMode = 0;

		public static int OldPXY = 0;

		public static int DangoTypes = 7;

		public static Texture[] Dango = new Texture[DangoTypes];

		public static int[] OutDangoCount = new int[DangoTypes];

		public static int[] OutDangoCountTotal = new int[DangoTypes];

		public static int[][] DangoTable = null;

		public static Random rnd = new Random();

		public static bool Loading = true;

		public static int LoadingPhase = 0;

		public static bool Selected = false;

		public static bool GameEnd = false;

		public static bool FadeOut = false;

		public static SE MoveSE = new SE();

		public static SE SelectSE = new SE();

		public static SE DecideSE = new SE();

		public static SE DangoOutSE = new SE();

		public static SE CancelSE = new SE();

		public static SE AddSE = new SE();

		public static SE BurnSE = new SE();

		public static SE GameOverSE = new SE();

		public static BGM GameBGM = new BGM();

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static Paint PWhite = new Paint(255, 255, 255, 255);

		public static Texture White = PWhite.ToTexture();

		public static ContentReturn Initialize(int LV) {
			BoxTotal = 0;
			OutTotal = 0;
			OutCount = 0;
			Remain = 180;
			WorkTime = 0;
			FCounter = 0;
			DiffLv = LV;
			SDiffLv = "";
			AddSpeed = 0;
			BGFile = "BG1.png";
			StartDangoLines = 3;
			BurnPercent = 0;
			if(LV == 0) {
				SDiffLv = "試食会";
				BGFile = "BG0.png";
				AddSpeed = 10;
				Remain = 180;
			}
			if(LV == 1) {
				SDiffLv = "のんびり";
				BurnPercent = 100;
				AddSpeed = 10;
				Remain = 120;
			}
			if(LV == 2) {
				SDiffLv = "ひとりで";
				StartDangoLines = 5;
				BurnPercent = 200;
				AddSpeed = 5;
				Remain = 120;
			}
			if(LV == 3) {
				SDiffLv = "挑戦状";
				StartDangoLines = 10;
				BurnPercent = 300;
				BGFile = "BG3.png";
				Remain = 60;
				AddSpeed = 5;
			}
			if(LV == 4) {
				SDiffLv = "新挑戦状";
				StartDangoLines = 0;
				BurnPercent = 300;
				BGFile = "BG4.png";
				Remain = 60;
				AddSpeed = 2;
			}
			for(int i = 0; i < DangoTypes; i++) {
				Dango[i] = null;
				OutDangoCount[i] = 0;
				OutDangoCountTotal[i] = 0;
			}
			DangoTable = new int[12][];
			for(int j = 0; j < DangoTable.Length; j++) {
				DangoTable[j] = new int[DangoTypes];
			}
			Texture.SetFont("Meiryo");
			Texture.SetTextColor(0, 0, 0);
			Texture.SetTextSize(24);
			TDiffLv = Texture.CreateFromText(SDiffLv);
			TOutTotal = Texture.CreateFromText(OutTotal.ToString());
			TOutCount = Texture.CreateFromText(OutCount.ToString());
			for(int k = 0; k < LoadingBG.Length; k++) {
				LoadingBG[k] = Texture.CreateFromFile("Loading" + k + ".png");
			}
			Texture tCoolDown = Texture.CreateFromText("0");
			Texture texture = Texture.CreateFromText("x 0");
			TOutDangoCount = new Texture[DangoTypes];
			for(int l = 0; l < DangoTypes; l++) {
				TOutDangoCount[l] = texture;
			}
			TBoxTotal = (TScore = (TCoolDown = tCoolDown));
			Loading = true;
			LoadingPhase = 0;
			GameEnd = (FadeOut = (Selected = false));
			OutButtonMode = 0;
			PointerX = (PointerY = (OldPointerX = (OldPointerY = (SelPointerX = (SelPointerY = (OldPXY = 0))))));
			Effect.Reset();
			for(int m = 0; m < StartDangoLines; m++) {
				AddDangoLine();
				ProceDango();
			}
			return ContentReturn.OK;
		}

		public static ContentReturn Main(int LV) {
			if(Loading) {
				Core.Draw(LoadingBG[LoadingPhase % 4], 920, 560);
				switch(LoadingPhase) {
					case 0:
						Dango[0] = Texture.CreateFromFile("DangoNone.png");
						break;
					case 1:
						Dango[1] = Texture.CreateFromFile("DangoPink.png");
						break;
					case 2:
						Dango[3] = Texture.CreateFromFile("DangoGreen.png");
						break;
					case 3:
						Dango[2] = Texture.CreateFromFile("DangoWater.png");
						break;
					case 4:
						Dango[4] = Texture.CreateFromFile("DangoBlack.png");
						break;
					case 5:
						Dango[6] = Texture.CreateFromFile("DangoBurn.png");
						break;
					case 6:
						MoveSE.LoadFile("Menu.wav");
						break;
					case 7:
						SelectSE.LoadFile("Select.wav");
						break;
					case 8:
						DecideSE.LoadFile("DNGOK.wav");
						break;
					case 9:
						DangoOutSE.LoadFile("DNGOut.wav");
						break;
					case 10:
						CancelSE.LoadFile("DNGErr.wav");
						break;
					case 11:
						AddSE.LoadFile("DNGAdd.wav");
						break;
					case 12:
						BurnSE.LoadFile("DNGBurn.wav");
						break;
					case 13:
						GameOverSE.LoadFile("DNGGO.wav");
						break;
					case 14:
						GameBGM.LoadFile("BGM_Game.wav");
						break;
					case 15:
						BG = Texture.CreateFromFile(BGFile);
						break;
					case 16:
						GameBox = Texture.CreateFromFile("GameBox.png");
						break;
					case 17:
						Pointer[0] = Texture.CreateFromFile("Select.png");
						break;
					case 18:
						Pointer[1] = Texture.CreateFromFile("Decide.png");
						break;
					case 19:
						OutButton[0] = Texture.CreateFromFile("OutButton1.png");
						break;
					case 20:
						OutButton[1] = Texture.CreateFromFile("OutButton2.png");
						break;
					case 21:
						OutButton[2] = Texture.CreateFromFile("OutButton3.png");
						break;
					case 22:
						OutButton[3] = Texture.CreateFromFile("OutButton4.png");
						break;
					case 23:
						Dango[5] = Texture.CreateFromFile("DangoYellow.png");
						break;
					case 24:
						GameBGM.Play();
						break;
					case 25:
						BG_HDPadding = Texture.CreateFromFile("GameBGPadding.png");
						break;
					default:
						Loading = false;
						break;
				}
				Thread.Sleep(50);
				LoadingPhase++;
				return ContentReturn.OK;
			}
			Core.Draw(BG_HDPadding, 0, 0);
			Core.Draw(BG, 0, 0);
			Core.Draw(GameBox, 0, 0);
			Core.Draw(TOutTotal, 350, 8);
			Core.Draw(TOutCount, 350, 33);
			Core.Draw(TDiffLv, 570, 8);
			Core.Draw(TAddSpeed, 570, 33);
			Texture.SetTextSize(48);
			TRemain = Texture.CreateFromText(Remain.ToString());
			Core.Draw(TRemain, 800, 60);
			Core.Draw(TScore, 800, 8);
			for(int i = 1; i < DangoTypes; i++) {
				Core.Draw(Dango[i], 20, 64 + i * 64);
				Core.Draw(TOutDangoCount[i], 84, 76 + i * 64);
			}
			Core.Draw(TBoxTotal, 110, 95);
			DrawOutButton();
			ProceDango();
			DrawDango();
			Core.Draw(Pointer[0], PointerX * 64 + 160, PointerY * 64 + 128);
			if(Selected) {
				Core.Draw(Pointer[1], SelPointerX * 64 + 160, SelPointerY * 64 + 128);
			}
			if(!GameEnd) {
				Effect.Fadein();
				if(FCounter >= 60) {
					FCounter = 0;
					Remain--;
					WorkTime++;
					if(WorkTime % AddSpeed == 0) {
						AddDangoLine();
					}
				}
				FCounter++;
				if(Remain == 0) {
					GameEnd = true;
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0) {
					GameEnd = true;
				}
				int analogAsButtonOnce = VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.LX);
				int analogAsButtonOnce2 = VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.LY);
				int analogAsButtonOnce3 = VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.RX);
				int analogAsButtonOnce4 = VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.RY);
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.DOWN) != 0 || analogAsButtonOnce2 > 0 || analogAsButtonOnce4 > 0) {
					PointerY++;
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.UP) != 0 || analogAsButtonOnce2 < 0 || analogAsButtonOnce4 < 0) {
					PointerY--;
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.RIGHT) != 0 || analogAsButtonOnce > 0 || analogAsButtonOnce3 > 0) {
					PointerX++;
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.LEFT) != 0 || analogAsButtonOnce < 0 || analogAsButtonOnce3 < 0) {
					PointerX--;
				}
				if(OldPXY - (VirtualIO.RTPointX + VirtualIO.RTPointY) >= 8 || VirtualIO.RTPointX + VirtualIO.RTPointY - OldPXY >= 8) {
					OldPXY = VirtualIO.RTPointX + VirtualIO.RTPointY;
					PointerX = (VirtualIO.RTPointX - 160) / 64;
					PointerY = (VirtualIO.RTPointY - 128) / 64;
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0) {
					ProceOutButton();
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || VIOEx.GetPointOnce(VirtualIO.PointID.R) != 0) {
					ProceClick(true);
				}
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0) {
					ProceClick(false);
				}
				if(VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
					if(280 <= VirtualIO.RTPointX && VirtualIO.RTPointX <= 700 && 80 <= VirtualIO.RTPointY && VirtualIO.RTPointY <= 115) {
						ProceOutButton();
					} else {
						ProceClick(false);
					}
				}
				if(PointerX < 0) {
					PointerX = 0;
				}
				if(PointerY < 0) {
					PointerY = 0;
				}
				if(PointerX >= 12) {
					PointerX = 11;
				}
				if(PointerY >= 6) {
					PointerY = 5;
				}
				return ContentReturn.OK;
			}
			if(!FadeOut) {
				GameBGM.Close();
				GameOverSE.Play();
				Effect.Reset();
				FadeOut = true;
			}
			if(Effect.Fadeout() == ContentReturn.END) {
				Scene.Set("Result");
				return ContentReturn.CHANGE;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddDango(DangoID DNGID, int X, int Y) {
			int num = rnd.Next(1000);
			if(num < 1000 - BurnPercent) {
				DangoTable[X][Y] = (int)DNGID;
			} else {
				DangoTable[X][Y] = 6;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddDangoLine() {
			int num = 11;
			int num2 = 0;
			for(int i = 0; i < DangoTable[num].Length; i++) {
				num2 += DangoTable[num][i];
			}
			if(num2 != 0) {
				GameEnd = true;
				return ContentReturn.END;
			}
			for(int j = 0; j < 6; j++) {
				AddDango((DangoID)(rnd.Next(DangoTypes - 2) + 1), 11, j);
			}
			ProceDango();
			return ContentReturn.OK;
		}

		public static ContentReturn DrawDango() {
			for(int i = 0; i < DangoTable.Length; i++) {
				for(int j = 0; j < DangoTable[i].Length; j++) {
					Core.Draw(Dango[DangoTable[i][j]], i * 64 + 160, j * 64 + 128);
				}
			}
			return ContentReturn.OK;
		}

		public static ContentReturn ProceDango() {
			for(int i = 0; i < DangoTable.Length; i++) {
				int num = 0;
				for(int j = 0; j < DangoTable[i].Length; j++) {
					num += DangoTable[i][j];
				}
				if(num == 0 && i != 11) {
					DangoTable[i + 1].CopyTo(DangoTable[i], 0);
					for(int k = 0; k < DangoTable[i + 1].Length; k++) {
						DangoTable[i + 1][k] = 0;
					}
				}
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddOutDango(int DangoID) {
			BoxTotal++;
			OutDangoCount[DangoID]++;
			OutDangoCountTotal[DangoID]++;
			Texture.SetTextSize(24);
			TBoxTotal = Texture.CreateFromText(string.Format("{0}", BoxTotal));
			TOutDangoCount[DangoID] = Texture.CreateFromText(string.Format("x {0}", OutDangoCount[DangoID]));
			return ContentReturn.OK;
		}

		public static ContentReturn AddScore(int ScoreToAdd) {
			Score += ScoreToAdd;
			Texture.SetTextSize(24);
			TScore = Texture.CreateFromText(string.Format("{0}", Score));
			return ContentReturn.OK;
		}

		public static ContentReturn DrawOutButton() {
			if(BoxTotal == 0) {
				Core.Draw(OutButton[3], 280, 80);
			} else {
				Core.Draw(OutButton[0], 280, 80);
			}
			return ContentReturn.OK;
		}

		public static ContentReturn ProceOutButton() {
			bool[] array = new bool[32];
			int num = 0;
			string text = "";
			if(BoxTotal == 0 || CoolDown != 0) {
				CancelSE.Play();
				return ContentReturn.END;
			}
			DangoOutSE.Play();
			for(int i = 0; i < DangoTypes; i++) {
				if(OutDangoCount[i] != 0) {
					array[i] = true;
					num++;
				} else {
					array[i] = false;
				}
			}
			AddScore(BoxTotal * 800);
			if(num == 1) {
				AddScore(BoxTotal * 2500);
				text += "\n１種類パック！";
			}
			if(!array[6]) {
				AddScore(BoxTotal * 1000);
				text += "\n焦げてない！";
			}
			if(array[6] && OutDangoCount[6] == BoxTotal) {
				AddScore(BoxTotal * 500);
				text += "\nおこげセット！";
			}
			int num2 = 0;
			for(int j = 1; j < DangoTypes; j++) {
				if(OutDangoCount[j] == OutDangoCount[1]) {
					num2++;
				}
			}
			if(num2 == DangoTypes - 1) {
				AddScore(BoxTotal * 10000 / 4);
				text += "\n全部同じ数！";
			}
			Debug.Log('*', "Game", "SPName: {0}", text);
			Texture.SetTextSize(24);
			OutTotal += BoxTotal;
			OutCount++;
			BoxTotal = 0;
			for(int k = 0; k < DangoTypes; k++) {
				OutDangoCount[k] = 0;
				TOutDangoCount[k] = Texture.CreateFromText(string.Format("x {0}", OutDangoCount[k]));
			}
			TOutTotal = Texture.CreateFromText(string.Format("{0}", OutTotal));
			TOutCount = Texture.CreateFromText(string.Format("{0}", OutCount));
			TBoxTotal = Texture.CreateFromText(string.Format("{0}", BoxTotal));
			AddDangoLine();
			return ContentReturn.OK;
		}

		public static ContentReturn ProceClick(bool Type) {
			if(DangoTable[PointerX][PointerY] == 0) {
				Selected = false;
				return ContentReturn.OK;
			}
			if(!Type) {
				if(DangoTable[PointerX][PointerY] != 6) {
					if(PointerX == SelPointerX && PointerY == SelPointerY) {
						Selected = false;
					}
					if(Selected) {
						if(DangoTable[PointerX][PointerY] == DangoTable[SelPointerX][SelPointerY]) {
							DecideSE.Play();
							AddOutDango(DangoTable[PointerX][PointerY]);
							DangoTable[PointerX][PointerY] = (DangoTable[SelPointerX][SelPointerY] = 0);
							AddScore(100);
						} else {
							CancelSE.Play();
						}
						Selected = false;
					} else {
						SelectSE.Play();
						Selected = true;
						SelPointerX = PointerX;
						SelPointerY = PointerY;
					}
				} else {
					Selected = false;
					BurnSE.Play();
					AddDangoLine();
				}
			} else {
				Selected = false;
				if(DangoTable[PointerX][PointerY] != 6) {
					CancelSE.Play();
					if(DiffLv >= 3) {
						AddDangoLine();
					}
				} else {
					DecideSE.Play();
					AddOutDango(DangoTable[PointerX][PointerY]);
					DangoTable[PointerX][PointerY] = 0;
					AddScore(200);
				}
			}
			return ContentReturn.OK;
		}
	}
}
