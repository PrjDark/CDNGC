using Ionic.Zip;
using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.Resources;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace LEContents {
	public static class OnlineUpdate {
		public static Texture MenuTitleText = null;

		public static Texture MainText = null;

		public static Texture BG = null;

		public static Paint PGreen;

		public static Texture Green;

		public static Paint PGreenL;

		public static Texture GreenL;

		public static ZipFile Z = null;

		public static long Pct = 0L;

		public static string FilePath = "";

		public static string[] FileList = null;

		public static int Count = 0;

		public static int TotalCount = 0;

		public static int State = 0;

		public static DNet D = null;

		public static ContentReturn Initialize() {
			PGreen = new Paint(0, 96, 0, 255);
			Green = PGreen.ToTexture();
			PGreenL = new Paint();
			PGreenL.SetColor(0, 96, 0, 255);
			PGreenL.Box(0, 0, 904, 29);
			GreenL = PGreenL.ToTexture();
			BG = Texture.CreateFromFile("ConfigBG.png");
			Texture.SetFont("Meiryo");
			Texture.SetTextSize(32);
			Texture.SetTextColor(255, 255, 255);
			MenuTitleText = Texture.CreateFromText("Online Update");
			Texture.SetTextSize(24);
			MainText = Texture.CreateFromText("Please wait...");
			State = 0;
			TotalCount = (Count = 0);
			FilePath = Path.GetTempPath() + "\\Lightness_Updater_" + GameCommon.Version.APPID + ".bin";
			FileList = new string[1024];
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			Core.Draw(MenuTitleText, 20, 20);
			Core.Draw(MainText, 30, 220);
			Core.Draw(GreenL, 28, 268);
			Common.GSprite.DrawEx(Green, 30, 270, 255, 0, 100, 900 * (int)Pct / 1000, 25);
			if(State == 0) {
				MainText = Texture.CreateFromText(string.Format("認証中..."));
				D = new DNet(GameCommon.NewVerURL);
				State = 1;
			}
			if(State == 1) {
				Pct = (long)D.SFEXReadTotal * 1000L / D.FileSize;
				MainText = Texture.CreateFromText(string.Format("Downloading... {0}/{1}\u3000", D.SFEXReadTotal, D.FileSize, Pct));
				if(D.SaveFileEx(FilePath) == ContentReturn.END) {
					Pct = 1000L;
					State = 2;
				}
			}
			if(State == 2) {
				MainText = Texture.CreateFromText(string.Format("Verifying..."));
				ReadOptions readOptions = new ReadOptions();
				readOptions.Encoding = Encoding.GetEncoding("shift_jis");
				try {
					Z = ZipFile.Read(FilePath, readOptions);
				} catch {
					Scene.Set("PDAdvertise");
					return ContentReturn.CHANGE;
				}
				Z.Password = "CDNGC:J:B:A";
				Z.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
				foreach(string entryFileName in Z.EntryFileNames) {
					FileList[TotalCount] = entryFileName;
					TotalCount++;
				}
				State = 3;
				Pct = 0L;
			}
			if(State == 3) {
				try {
					Pct = Count * 1000 / TotalCount;
				} catch {
				}
				MainText = Texture.CreateFromText(string.Format("Updating... {0}/{1}\u3000", Count + 1, TotalCount));
				ZipEntry zipEntry = Z[FileList[Count]];
				zipEntry.Extract(".", ExtractExistingFileAction.OverwriteSilently);
				Count++;
				if(Count == TotalCount) {
					State = 4;
					Pct = 1000L;
					TotalCount = 60;
					Count = 0;
				}
			}
			if(State == 4) {
				Pct = 0L;
				MainText = Texture.CreateFromText(string.Format("Completed。Restarting...\u3000"));
				Count++;
				if(Count > TotalCount) {
					Process process = new Process();
					process.StartInfo.FileName = Assembly.GetEntryAssembly().Location;
					process.StartInfo.Arguments = "";
					process.Start();
					return ContentReturn.END;
				}
			}
			Texture.SetTextSize(24);
			return ContentReturn.OK;
		}
	}
}
