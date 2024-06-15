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

		public static Texture[] Dango = new Texture[CDNGC.DangoTypes];

		public static int[] OutDangoCount = new int[CDNGC.DangoTypes];

		public static int[] OutDangoCountTotal = new int[CDNGC.DangoTypes];

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

		public static Texture White = CDNGC.PWhite.ToTexture();

		public static ContentReturn Initialize(int LV) {
			CDNGC.BoxTotal = 0;
			CDNGC.OutTotal = 0;
			CDNGC.OutCount = 0;
			CDNGC.Remain = 180;
			CDNGC.WorkTime = 0;
			CDNGC.FCounter = 0;
			CDNGC.DiffLv = LV;
			CDNGC.SDiffLv = "";
			CDNGC.AddSpeed = 0;
			CDNGC.BGFile = "BG1.png";
			CDNGC.StartDangoLines = 3;
			CDNGC.BurnPercent = 0;
			if (LV == 0) {
				CDNGC.SDiffLv = "試食会";
				CDNGC.BGFile = "BG0.png";
				CDNGC.AddSpeed = 10;
				CDNGC.Remain = 180;
			}
			if (LV == 1) {
				CDNGC.SDiffLv = "のんびり";
				CDNGC.BurnPercent = 100;
				CDNGC.AddSpeed = 10;
				CDNGC.Remain = 120;
			}
			if (LV == 2) {
				CDNGC.SDiffLv = "ひとりで";
				CDNGC.StartDangoLines = 5;
				CDNGC.BurnPercent = 200;
				CDNGC.AddSpeed = 5;
				CDNGC.Remain = 120;
			}
			if (LV == 3) {
				CDNGC.SDiffLv = "挑戦状";
				CDNGC.StartDangoLines = 10;
				CDNGC.BurnPercent = 300;
				CDNGC.BGFile = "BG3.png";
				CDNGC.Remain = 60;
				CDNGC.AddSpeed = 5;
			}
			if (LV == 4) {
				CDNGC.SDiffLv = "新挑戦状";
				CDNGC.StartDangoLines = 0;
				CDNGC.BurnPercent = 300;
				CDNGC.BGFile = "BG4.png";
				CDNGC.Remain = 60;
				CDNGC.AddSpeed = 2;
			}
			for (int i = 0; i < CDNGC.DangoTypes; i++) {
				CDNGC.Dango[i] = null;
				CDNGC.OutDangoCount[i] = 0;
				CDNGC.OutDangoCountTotal[i] = 0;
			}
			CDNGC.DangoTable = new int[12][];
			for (int j = 0; j < CDNGC.DangoTable.Length; j++) {
				CDNGC.DangoTable[j] = new int[CDNGC.DangoTypes];
			}
			Texture.SetFont("Meiryo");
			Texture.SetTextColor(0, 0, 0);
			Texture.SetTextSize(24);
			CDNGC.TDiffLv = Texture.CreateFromText(CDNGC.SDiffLv);
			CDNGC.TOutTotal = Texture.CreateFromText(CDNGC.OutTotal.ToString());
			CDNGC.TOutCount = Texture.CreateFromText(CDNGC.OutCount.ToString());
			for (int k = 0; k < CDNGC.LoadingBG.Length; k++) {
				CDNGC.LoadingBG[k] = Texture.CreateFromFile("Loading" + k + ".png");
			}
			Texture tCoolDown = Texture.CreateFromText("0");
			Texture texture = Texture.CreateFromText("x 0");
			CDNGC.TOutDangoCount = new Texture[CDNGC.DangoTypes];
			for (int l = 0; l < CDNGC.DangoTypes; l++) {
				CDNGC.TOutDangoCount[l] = texture;
			}
			CDNGC.TBoxTotal = (CDNGC.TScore = (CDNGC.TCoolDown = tCoolDown));
			CDNGC.Loading = true;
			CDNGC.LoadingPhase = 0;
			CDNGC.GameEnd = (CDNGC.FadeOut = (CDNGC.Selected = false));
			CDNGC.OutButtonMode = 0;
			CDNGC.PointerX = (CDNGC.PointerY = (CDNGC.OldPointerX = (CDNGC.OldPointerY = (CDNGC.SelPointerX = (CDNGC.SelPointerY = (CDNGC.OldPXY = 0))))));
			Effect.Reset();
			for (int m = 0; m < CDNGC.StartDangoLines; m++) {
				CDNGC.AddDangoLine();
				CDNGC.ProceDango();
			}
			return ContentReturn.OK;
		}

		public static ContentReturn Main(int LV) {
			if (CDNGC.Loading) {
				Core.Draw(CDNGC.LoadingBG[CDNGC.LoadingPhase % 4], 920, 560);
				switch (CDNGC.LoadingPhase) {
					case 0:
						CDNGC.Dango[0] = Texture.CreateFromFile("DangoNone.png");
						break;
					case 1:
						CDNGC.Dango[1] = Texture.CreateFromFile("DangoPink.png");
						break;
					case 2:
						CDNGC.Dango[3] = Texture.CreateFromFile("DangoGreen.png");
						break;
					case 3:
						CDNGC.Dango[2] = Texture.CreateFromFile("DangoWater.png");
						break;
					case 4:
						CDNGC.Dango[4] = Texture.CreateFromFile("DangoBlack.png");
						break;
					case 5:
						CDNGC.Dango[6] = Texture.CreateFromFile("DangoBurn.png");
						break;
					case 6:
						CDNGC.MoveSE.LoadFile("Menu.wav");
						break;
					case 7:
						CDNGC.SelectSE.LoadFile("Select.wav");
						break;
					case 8:
						CDNGC.DecideSE.LoadFile("DNGOK.wav");
						break;
					case 9:
						CDNGC.DangoOutSE.LoadFile("DNGOut.wav");
						break;
					case 10:
						CDNGC.CancelSE.LoadFile("DNGErr.wav");
						break;
					case 11:
						CDNGC.AddSE.LoadFile("DNGAdd.wav");
						break;
					case 12:
						CDNGC.BurnSE.LoadFile("DNGBurn.wav");
						break;
					case 13:
						CDNGC.GameOverSE.LoadFile("DNGGO.wav");
						break;
					case 14:
						CDNGC.GameBGM.LoadFile("BGM_Game.wav");
						break;
					case 15:
						CDNGC.BG = Texture.CreateFromFile(CDNGC.BGFile);
						break;
					case 16:
						CDNGC.GameBox = Texture.CreateFromFile("GameBox.png");
						break;
					case 17:
						CDNGC.Pointer[0] = Texture.CreateFromFile("Select.png");
						break;
					case 18:
						CDNGC.Pointer[1] = Texture.CreateFromFile("Decide.png");
						break;
					case 19:
						CDNGC.OutButton[0] = Texture.CreateFromFile("OutButton1.png");
						break;
					case 20:
						CDNGC.OutButton[1] = Texture.CreateFromFile("OutButton2.png");
						break;
					case 21:
						CDNGC.OutButton[2] = Texture.CreateFromFile("OutButton3.png");
						break;
					case 22:
						CDNGC.OutButton[3] = Texture.CreateFromFile("OutButton4.png");
						break;
					case 23:
						CDNGC.Dango[5] = Texture.CreateFromFile("DangoYellow.png");
						break;
					case 24:
						CDNGC.GameBGM.Play();
						break;
					case 25:
						CDNGC.BG_HDPadding = Texture.CreateFromFile("GameBGPadding.png");
						break;
					default:
						CDNGC.Loading = false;
						break;
				}
				Thread.Sleep(50);
				CDNGC.LoadingPhase++;
				return ContentReturn.OK;
			}
			Core.Draw(CDNGC.BG_HDPadding, 0, 0);
			Core.Draw(CDNGC.BG, 0, 0);
			Core.Draw(CDNGC.GameBox, 0, 0);
			Core.Draw(CDNGC.TOutTotal, 350, 8);
			Core.Draw(CDNGC.TOutCount, 350, 33);
			Core.Draw(CDNGC.TDiffLv, 570, 8);
			Core.Draw(CDNGC.TAddSpeed, 570, 33);
			Texture.SetTextSize(48);
			CDNGC.TRemain = Texture.CreateFromText(CDNGC.Remain.ToString());
			Core.Draw(CDNGC.TRemain, 800, 60);
			Core.Draw(CDNGC.TScore, 800, 8);
			for (int i = 1; i < CDNGC.DangoTypes; i++) {
				Core.Draw(CDNGC.Dango[i], 20, 64 + i * 64);
				Core.Draw(CDNGC.TOutDangoCount[i], 84, 76 + i * 64);
			}
			Core.Draw(CDNGC.TBoxTotal, 110, 95);
			CDNGC.DrawOutButton();
			CDNGC.ProceDango();
			CDNGC.DrawDango();
			Core.Draw(CDNGC.Pointer[0], CDNGC.PointerX * 64 + 160, CDNGC.PointerY * 64 + 128);
			if (CDNGC.Selected) {
				Core.Draw(CDNGC.Pointer[1], CDNGC.SelPointerX * 64 + 160, CDNGC.SelPointerY * 64 + 128);
			}
			if (!CDNGC.GameEnd) {
				Effect.Fadein();
				if (CDNGC.FCounter >= 60) {
					CDNGC.FCounter = 0;
					CDNGC.Remain--;
					CDNGC.WorkTime++;
					if (CDNGC.WorkTime % CDNGC.AddSpeed == 0) {
						CDNGC.AddDangoLine();
					}
				}
				CDNGC.FCounter++;
				if (CDNGC.Remain == 0) {
					CDNGC.GameEnd = true;
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0) {
					CDNGC.GameEnd = true;
				}
				int analogAsButtonOnce = CDNGC.VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.LX);
				int analogAsButtonOnce2 = CDNGC.VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.LY);
				int analogAsButtonOnce3 = CDNGC.VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.RX);
				int analogAsButtonOnce4 = CDNGC.VIOEx.GetAnalogAsButtonOnce(0, VirtualIO.AnalogID.RY);
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.DOWN) != 0 || analogAsButtonOnce2 > 0 || analogAsButtonOnce4 > 0) {
					CDNGC.PointerY++;
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.UP) != 0 || analogAsButtonOnce2 < 0 || analogAsButtonOnce4 < 0) {
					CDNGC.PointerY--;
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.RIGHT) != 0 || analogAsButtonOnce > 0 || analogAsButtonOnce3 > 0) {
					CDNGC.PointerX++;
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.LEFT) != 0 || analogAsButtonOnce < 0 || analogAsButtonOnce3 < 0) {
					CDNGC.PointerX--;
				}
				if (CDNGC.OldPXY - (VirtualIO.RTPointX + VirtualIO.RTPointY) >= 8 || VirtualIO.RTPointX + VirtualIO.RTPointY - CDNGC.OldPXY >= 8) {
					CDNGC.OldPXY = VirtualIO.RTPointX + VirtualIO.RTPointY;
					CDNGC.PointerX = (VirtualIO.RTPointX - 160) / 64;
					CDNGC.PointerY = (VirtualIO.RTPointY - 128) / 64;
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0) {
					CDNGC.ProceOutButton();
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || CDNGC.VIOEx.GetPointOnce(VirtualIO.PointID.R) != 0) {
					CDNGC.ProceClick(true);
				}
				if (CDNGC.VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0) {
					CDNGC.ProceClick(false);
				}
				if (CDNGC.VIOEx.GetPointOnce(VirtualIO.PointID.L) != 0) {
					if (280 <= VirtualIO.RTPointX && VirtualIO.RTPointX <= 700 && 80 <= VirtualIO.RTPointY && VirtualIO.RTPointY <= 115) {
						CDNGC.ProceOutButton();
					} else {
						CDNGC.ProceClick(false);
					}
				}
				if (CDNGC.PointerX < 0) {
					CDNGC.PointerX = 0;
				}
				if (CDNGC.PointerY < 0) {
					CDNGC.PointerY = 0;
				}
				if (CDNGC.PointerX >= 12) {
					CDNGC.PointerX = 11;
				}
				if (CDNGC.PointerY >= 6) {
					CDNGC.PointerY = 5;
				}
				return ContentReturn.OK;
			}
			if (!CDNGC.FadeOut) {
				CDNGC.GameBGM.Close();
				CDNGC.GameOverSE.Play();
				Effect.Reset();
				CDNGC.FadeOut = true;
			}
			if (Effect.Fadeout() == ContentReturn.END) {
				Scene.Set("Result");
				return ContentReturn.CHANGE;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddDango(CDNGC.DangoID DNGID, int X, int Y) {
			int num = CDNGC.rnd.Next(1000);
			if (num < 1000 - CDNGC.BurnPercent) {
				CDNGC.DangoTable[X][Y] = (int)DNGID;
			} else {
				CDNGC.DangoTable[X][Y] = 6;
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddDangoLine() {
			int num = 11;
			int num2 = 0;
			for (int i = 0; i < CDNGC.DangoTable[num].Length; i++) {
				num2 += CDNGC.DangoTable[num][i];
			}
			if (num2 != 0) {
				CDNGC.GameEnd = true;
				return ContentReturn.END;
			}
			for (int j = 0; j < 6; j++) {
				CDNGC.AddDango(CDNGC.rnd.Next(CDNGC.DangoTypes - 2) + CDNGC.DangoID.Pink, 11, j);
			}
			CDNGC.ProceDango();
			return ContentReturn.OK;
		}

		public static ContentReturn DrawDango() {
			for (int i = 0; i < CDNGC.DangoTable.Length; i++) {
				for (int j = 0; j < CDNGC.DangoTable[i].Length; j++) {
					Core.Draw(CDNGC.Dango[CDNGC.DangoTable[i][j]], i * 64 + 160, j * 64 + 128);
				}
			}
			return ContentReturn.OK;
		}

		public static ContentReturn ProceDango() {
			for (int i = 0; i < CDNGC.DangoTable.Length; i++) {
				int num = 0;
				for (int j = 0; j < CDNGC.DangoTable[i].Length; j++) {
					num += CDNGC.DangoTable[i][j];
				}
				if (num == 0 && i != 11) {
					CDNGC.DangoTable[i + 1].CopyTo(CDNGC.DangoTable[i], 0);
					for (int k = 0; k < CDNGC.DangoTable[i + 1].Length; k++) {
						CDNGC.DangoTable[i + 1][k] = 0;
					}
				}
			}
			return ContentReturn.OK;
		}

		public static ContentReturn AddOutDango(int DangoID) {
			CDNGC.BoxTotal++;
			CDNGC.OutDangoCount[DangoID]++;
			CDNGC.OutDangoCountTotal[DangoID]++;
			Texture.SetTextSize(24);
			CDNGC.TBoxTotal = Texture.CreateFromText(string.Format("{0}", CDNGC.BoxTotal));
			CDNGC.TOutDangoCount[DangoID] = Texture.CreateFromText(string.Format("x {0}", CDNGC.OutDangoCount[DangoID]));
			return ContentReturn.OK;
		}

		public static ContentReturn AddScore(int ScoreToAdd) {
			CDNGC.Score += ScoreToAdd;
			Texture.SetTextSize(24);
			CDNGC.TScore = Texture.CreateFromText(string.Format("{0}", CDNGC.Score));
			return ContentReturn.OK;
		}

		public static ContentReturn DrawOutButton() {
			if (CDNGC.BoxTotal == 0) {
				Core.Draw(CDNGC.OutButton[3], 280, 80);
			} else {
				Core.Draw(CDNGC.OutButton[0], 280, 80);
			}
			return ContentReturn.OK;
		}

		public static ContentReturn ProceOutButton() {
			bool[] array = new bool[32];
			int num = 0;
			string text = "";
			if (CDNGC.BoxTotal == 0 || CDNGC.CoolDown != 0) {
				CDNGC.CancelSE.Play();
				return ContentReturn.END;
			}
			CDNGC.DangoOutSE.Play();
			for (int i = 0; i < CDNGC.DangoTypes; i++) {
				if (CDNGC.OutDangoCount[i] != 0) {
					array[i] = true;
					num++;
				} else {
					array[i] = false;
				}
			}
			CDNGC.AddScore(CDNGC.BoxTotal * 800);
			if (num == 1) {
				CDNGC.AddScore(CDNGC.BoxTotal * 2500);
				text += "\n１種類パック！";
			}
			if (!array[6]) {
				CDNGC.AddScore(CDNGC.BoxTotal * 1000);
				text += "\n焦げてない！";
			}
			if (array[6] && CDNGC.OutDangoCount[6] == CDNGC.BoxTotal) {
				CDNGC.AddScore(CDNGC.BoxTotal * 500);
				text += "\nおこげセット！";
			}
			int num2 = 0;
			for (int j = 1; j < CDNGC.DangoTypes; j++) {
				if (CDNGC.OutDangoCount[j] == CDNGC.OutDangoCount[1]) {
					num2++;
				}
			}
			if (num2 == CDNGC.DangoTypes - 1) {
				CDNGC.AddScore(CDNGC.BoxTotal * 10000 / 4);
				text += "\n全部同じ数！";
			}
			Debug.Log('*', "Game", "SPName: {0}", new object[]
			{
				text
			});
			Texture.SetTextSize(24);
			CDNGC.OutTotal += CDNGC.BoxTotal;
			CDNGC.OutCount++;
			CDNGC.BoxTotal = 0;
			for (int k = 0; k < CDNGC.DangoTypes; k++) {
				CDNGC.OutDangoCount[k] = 0;
				CDNGC.TOutDangoCount[k] = Texture.CreateFromText(string.Format("x {0}", CDNGC.OutDangoCount[k]));
			}
			CDNGC.TOutTotal = Texture.CreateFromText(string.Format("{0}", CDNGC.OutTotal));
			CDNGC.TOutCount = Texture.CreateFromText(string.Format("{0}", CDNGC.OutCount));
			CDNGC.TBoxTotal = Texture.CreateFromText(string.Format("{0}", CDNGC.BoxTotal));
			CDNGC.AddDangoLine();
			return ContentReturn.OK;
		}

		public static ContentReturn ProceClick(bool Type) {
			if (CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] == 0) {
				CDNGC.Selected = false;
				return ContentReturn.OK;
			}
			if (!Type) {
				if (CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] != 6) {
					if (CDNGC.PointerX == CDNGC.SelPointerX && CDNGC.PointerY == CDNGC.SelPointerY) {
						CDNGC.Selected = false;
					}
					if (CDNGC.Selected) {
						if (CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] == CDNGC.DangoTable[CDNGC.SelPointerX][CDNGC.SelPointerY]) {
							CDNGC.DecideSE.Play();
							CDNGC.AddOutDango(CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY]);
							CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] = (CDNGC.DangoTable[CDNGC.SelPointerX][CDNGC.SelPointerY] = 0);
							CDNGC.AddScore(100);
						} else {
							CDNGC.CancelSE.Play();
						}
						CDNGC.Selected = false;
					} else {
						CDNGC.SelectSE.Play();
						CDNGC.Selected = true;
						CDNGC.SelPointerX = CDNGC.PointerX;
						CDNGC.SelPointerY = CDNGC.PointerY;
					}
				} else {
					CDNGC.Selected = false;
					CDNGC.BurnSE.Play();
					CDNGC.AddDangoLine();
				}
			} else {
				CDNGC.Selected = false;
				if (CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] != 6) {
					CDNGC.CancelSE.Play();
					if (CDNGC.DiffLv >= 3) {
						CDNGC.AddDangoLine();
					}
				} else {
					CDNGC.DecideSE.Play();
					CDNGC.AddOutDango(CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY]);
					CDNGC.DangoTable[CDNGC.PointerX][CDNGC.PointerY] = 0;
					CDNGC.AddScore(200);
				}
			}
			return ContentReturn.OK;
		}
	}
}
