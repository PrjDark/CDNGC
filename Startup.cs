using Lightness.Core;
using Lightness.Framework;
using Lightness.Graphic;
using Lightness.Resources;
using System;
using System.Threading;

namespace LEContents {
	public static class Startup {
		public static Texture[] LoadingBG = new Texture[4];

		public static int LoadingState = 0;

		public static ContentReturn Initialize() {
			Startup.LoadingState = 0;
			Core.SetTitle(GameCommon.Version.Title);
			for (int i = 0; i < Startup.LoadingBG.Length; i++) {
				Startup.LoadingBG[i] = Texture.CreateFromFile("Loading" + i + ".png");
			}
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			if (Startup.LoadingState >= 2) {
				GameCommon.CheckNetworkStatus();
			}
			if (Startup.LoadingState >= 4) {
				Scene.Set("PDAdvertise");
				if (GameCommon.UpdateAvailable) {
					Scene.Set("OnlineUpdate");
				}
				return ContentReturn.CHANGE;
			}
			Core.Draw(Startup.LoadingBG[Startup.LoadingState], 920, 560);
			Thread.Sleep(100);
			Startup.LoadingState++;
			return ContentReturn.OK;
		}
	}
}
