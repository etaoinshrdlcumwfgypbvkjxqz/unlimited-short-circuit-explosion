using Verse;

namespace Polyipseity.UnlimitedShortCircuitExplosion {
	public class EarlyModStartup : Mod {
		public EarlyModStartup(ModContentPack content) : base(content) {}
	}

	[StaticConstructorOnStartup]
	public static class ModStartup {
		public const string ID = nameof(Polyipseity.UnlimitedShortCircuitExplosion);

		static ModStartup() {}
	}
}
