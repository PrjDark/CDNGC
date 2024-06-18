using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System.IO;
using System.Text;
using System.Threading;

namespace LEContents {
	public static class Config_PlayerName {
		public static bool ReturnToResult = false;

		public static Texture MenuTitleText = null;

		public static Texture VerText = null;

		public static Texture BG = null;

		public static Texture TPlayerName = null;

		public static Texture MainText = null;

		public static Texture HelpText = null;

		public static string PlayerName = "";

		public static string PlayerNameOld = "*";

		public static int BlinkCount = 0;

		public static SE DecideSE = new SE();

		public static SE CancelSE = new SE();

		public static SE SelectSE = new SE();

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static ContentStream UserDataFile = null;

		public static StreamReader UserData = null;

		public static ContentReturn Initialize() {
			BlinkCount = 0;
			PlayerNameOld = "*";
			Effect.Reset();
			UserDataFile = new ContentStream("UserData.lec", true);
			UserData = new StreamReader(UserDataFile, Encoding.UTF8, true);
			PlayerName = UserData.ReadLine();
			if(PlayerName == null) {
				PlayerName = "";
			}
			DecideSE.LoadFile("DNGOut.wav");
			CancelSE.LoadFile("DNGErr.wav");
			SelectSE.LoadFile("Select.wav");
			Texture.SetFont("Meiryo");
			Texture.SetTextSize(32);
			Texture.SetTextColor(255, 255, 255);
			MenuTitleText = Texture.CreateFromText("プレイヤー名の登録");
			Texture.SetTextSize(24);
			MainText = Texture.CreateFromText("ランキングで使用する公開プレイヤー名を入力してください。");
			Texture.SetTextSize(20);
			HelpText = Texture.CreateFromText("START / [Enter] : 決定 - SELECT / [ESC]: 戻る\n[Delete]: 全文字削除");
			Texture.SetTextSize(14);
			VerText = Texture.CreateFromText(GameCommon.Version.Get());
			BG = Texture.CreateFromFile("ConfigBG.png");
			TPlayerName = Texture.CreateFromText(PlayerName + " ");
			Control.LastKey = "";
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			Core.Draw(BG, 0, 0);
			Core.Draw(MenuTitleText, 20, 20);
			Core.Draw(VerText, 20, 80);
			Core.Draw(MainText, 60, 140);
			if(BlinkCount < 30) {
				Core.Draw(HelpText, 260, 440);
			}
			if(BlinkCount > 60) {
				BlinkCount = 0;
			}
			if(PlayerName != PlayerNameOld) {
				Texture.SetFont("Consolas");
				Texture.SetTextColor(255, 255, 255);
				PlayerNameOld = PlayerName;
				if(PlayerName != "") {
					Texture.SetTextSize(96);
					TPlayerName = Texture.CreateFromText(PlayerName);
				} else {
					Texture.SetTextSize(42);
					TPlayerName = Texture.CreateFromText("(キーボードで入力してください)\u3000\u3000");
				}
			}
			Core.Draw(TPlayerName, 520 - TPlayerName.Width / 2, 230);
			if(Control.LastKey.Length >= 3) {
				if(Control.LastKey == "Back" && PlayerName.Length != 0) {
					PlayerName = PlayerName.Substring(0, PlayerName.Length - 1);
					SelectSE.Play();
				}
				if(Control.LastKey == "Delete") {
					PlayerName = "";
					SelectSE.Play();
				}
				if(Control.LastKey == "OemBackslash") {
					Control.LastKey = "_";
				}
				if(Control.LastKey == "OemMinus") {
					Control.LastKey = "-";
				}
				if(Control.LastKey == "OemPeriod") {
					Control.LastKey = ".";
				}
				if(Control.LastKey == "Decimal") {
					Control.LastKey = ".";
				}
				if(Control.LastKey == "Subtract") {
					Control.LastKey = "-";
				}
			}
			if(Control.LastKey.Length == 2 && Control.LastKey.Substring(0, 1) == "D") {
				Control.LastKey = Control.LastKey.Substring(1);
			}
			if(Control.LastKey.Length == 7 && Control.LastKey.Substring(0, 6) == "NumPad") {
				Control.LastKey = Control.LastKey.Substring(6);
			}
			if(Control.LastKey.Length == 1) {
				SelectSE.Play();
				PlayerName += Control.LastKey;
			}
			if(PlayerName.Length > 8) {
				PlayerName = PlayerName.Substring(0, 8);
				CancelSE.Play();
			}
			if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) == 0) {
				if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0) {
					CancelSE.Play();
					if(!ReturnToResult) {
						Scene.Set("Config");
						UserData.Close();
						Thread.Sleep(250);
						return ContentReturn.CHANGE;
					}
				}
				Control.LastKey = "";
				BlinkCount++;
				return ContentReturn.OK;
			}
			if(PlayerName.Length == 0) {
				CancelSE.Play();
				Control.LastKey = "";
				return ContentReturn.OK;
			}
			DecideSE.Play();
			Scene.Set("Config");
			if(ReturnToResult) {
				Scene.Set("Result");
				ReturnToResult = false;
			}
			try {
				UserDataFile.Close();
				UserDataFile = new ContentStream();
				byte[] bytes = Encoding.UTF8.GetBytes(PlayerName);
				UserDataFile.Write(bytes, 0, bytes.Length);
				UserDataFile.SaveFile("UserData.lec");
			} catch {
			}
			Thread.Sleep(250);
			return ContentReturn.CHANGE;
		}
	}
}
