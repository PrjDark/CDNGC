using Lightness.Core;
using Lightness.DNetwork;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Media;
using Lightness.Resources;
using System.Threading;

namespace LEContents {
	public static class Config_CircleInfo {
		public static Texture MenuTitleText = null;

		public static Texture HelpText = null;

		public static Texture BG = null;

		public static SE CancelSE = new SE();

		public static SE DecideSE = new SE();

		public static VirtualIOEx VIOEx = new VirtualIOEx();

		public static int Counter = 0;

		public static ContentReturn Initialize() {
			CancelSE.LoadFile("DNGErr.wav");
			DecideSE.LoadFile("DNGOut.wav");
			BG = Texture.CreateFromFile("ConfigBG.png");
			Texture.SetFont("Meiryo");
			Texture.SetTextSize(32);
			Texture.SetTextColor(255, 255, 255);
			MenuTitleText = Texture.CreateFromText("サークルからのお知らせ");
			Texture.SetTextSize(16);
			HelpText = Texture.CreateFromText("決定・キャンセルボタンで前の画面に戻ります。 / ドラッグするとスクロールします。");
			TextViewer.Initialize(new DNet("http://CDNGC.network.xprj.net/Information/Circle.txt").GetStrings());
			GameCommon.CheckNetworkStatus();
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			Core.Draw(BG, 0, 0);
			Core.Draw(MenuTitleText, 20, 20);
			TextViewer.Exec();
			if(VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.OK) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.CANCEL) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.START) != 0 || VIOEx.GetButtonOnce(0, VirtualIO.ButtonID.MENU) != 0 || VIOEx.GetPointOnce(VirtualIO.PointID.R) != 0) {
				DecideSE.Play();
				Scene.Set("Config");
				Thread.Sleep(250);
				return ContentReturn.CHANGE;
			}
			Counter++;
			if(Counter > 30) {
				Core.Draw(HelpText, 160, 510);
			}
			if(Counter > 60) {
				Counter = 0;
			}
			return ContentReturn.OK;
		}
	}
}
