using Verse;

namespace VARIABLE_NAMESPACE {
	public class EarlyModStartup : Mod {
		public EarlyModStartup(ModContentPack content) : base(content) {}
	}

	[StaticConstructorOnStartup]
	public static class ModStartup {
		public const string ID = nameof(VARIABLE_NAMESPACE);

		static ModStartup() {}
	}
}
