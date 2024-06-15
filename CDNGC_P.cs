using Lightness.Core;
using System;

namespace LEContents {
	public static class CDNGC_P {
		public static ContentReturn Initialize() {
			ContentReturn result = CDNGC.Initialize(0);
			CDNGC.BurnPercent = 0;
			return result;
		}

		public static ContentReturn Main() {
			return CDNGC.Main(0);
		}
	}
}
