using Lightness.Core;
using Lightness.Graphic;
using Lightness.IO;
using Lightness.Resources;

namespace LEContents {
	public static class TextViewer {
		public static Texture Message;

		public static int PointerY;

		public static ContentReturn Initialize(string[] InfoMsgFile) {
			string text = "";
			PointerY = 0;
			for(int i = 2; i < InfoMsgFile.Length; i++) {
				text = text + InfoMsgFile[i] + "\n";
			}
			Texture.SetTextSize(16);
			Message = Texture.CreateFromText(text);
			return ContentReturn.OK;
		}

		public static ContentReturn Exec() {
			Common.GSprite.DrawEx(Message, 100, 100, 255, 0, PointerY, 1280, 560);
			if(VirtualIO.GetButton(0, VirtualIO.ButtonID.DOWN) != 0) {
				PointerY += 4;
			}
			if(VirtualIO.GetButton(0, VirtualIO.ButtonID.UP) != 0) {
				PointerY -= 4;
			}
			if(VirtualIO.GetAnalog(0, VirtualIO.AnalogID.LY) < 0) {
				PointerY += (VirtualIO.GetAnalog(0, VirtualIO.AnalogID.LY) + 128) / 8;
			}
			if(VirtualIO.GetAnalog(0, VirtualIO.AnalogID.LY) > 0) {
				PointerY += (VirtualIO.GetAnalog(0, VirtualIO.AnalogID.LY) - 128) / 8;
			}
			if(VirtualIO.GetPoint(VirtualIO.PointID.L) != 0) {
				PointerY -= (VirtualIO.RTPointY - VirtualIO.PointY) / 16;
			}
			if(PointerY < -350) {
				PointerY = -350;
			}
			if(PointerY > Message.Height - 150) {
				PointerY = Message.Height - 150;
			}
			return ContentReturn.OK;
		}
	}
}
