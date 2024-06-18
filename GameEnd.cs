using Lightness.Core;
using Lightness.Framework;

namespace LEContents {
	public static class GameEnd {
		public static ContentReturn Initialize() {
			return ContentReturn.OK;
		}

		public static ContentReturn Main() {
			return Effect.Fadeout();
		}
	}
}
