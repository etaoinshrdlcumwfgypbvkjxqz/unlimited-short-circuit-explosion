using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using HarmonyLib;

namespace Polyipseity.UnlimitedShortCircuitExplosion {
	public class EarlyModStartup : Mod {
		internal static readonly Harmony HARMONY = new Harmony(ModStartup.ID);

		public EarlyModStartup(ModContentPack content) : base(content) {}
	}

	[StaticConstructorOnStartup]
	public static class ModStartup {
		public const string ID = nameof(Polyipseity.UnlimitedShortCircuitExplosion);

		static ModStartup() {
			EarlyModStartup.HARMONY.PatchAll();
			Log.Message($"[{ID}] Patches applied.");
		}
	}

	[HarmonyPatch(typeof(ShortCircuitUtility), "DrainBatteriesAndCauseExplosion")]
	static class Patch_UncapExplosionRadius {
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var transpiling = true;
			foreach (var instruction in instructions) {
				if (transpiling && instruction.opcode == OpCodes.Ldc_R4 && (float) instruction.operand == 14.9f) {
					transpiling = false;
					instruction.operand = float.MaxValue;
				}
				yield return instruction;
			}
		}
	}

	[HarmonyPatch]
	static class Patch_IncreaseMaxRadialPatternRadius {
		const int DEFAULT_RADICAL_PATTERN_CALCULATE_COUNT = 60;
		const int DEFAULT_RADICAL_PATTERN_COUNT = 10000;
		const int MULTIPLIER = 3;

		static IEnumerable<MethodBase> TargetMethods()
		{
			return AccessTools.GetDeclaredMethods(typeof(GenRadial))
				.Where(method => (method.Name == "SetupRadialPattern"
					|| method.Name == nameof(GenRadial.NumCellsInRadius)));
		}

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (var instruction in instructions) {
				if (instruction.opcode == OpCodes.Ldc_I4 || instruction.opcode == OpCodes.Ldc_I4_S) {
					var castedOperand = ((IConvertible) instruction.operand).ToInt32(CultureInfo.InvariantCulture); // int or sbyte
					instruction.opcode = OpCodes.Ldc_I4;
					if (castedOperand >= DEFAULT_RADICAL_PATTERN_COUNT) {
						// 10000 -> ?
						instruction.operand = Math.Max(DEFAULT_RADICAL_PATTERN_COUNT * MULTIPLIER * MULTIPLIER, castedOperand);
					} else if (castedOperand >= DEFAULT_RADICAL_PATTERN_CALCULATE_COUNT) {
						// 60 -> ?
						instruction.operand = Math.Max(DEFAULT_RADICAL_PATTERN_CALCULATE_COUNT * MULTIPLIER, castedOperand);
					} else if (castedOperand <= -DEFAULT_RADICAL_PATTERN_CALCULATE_COUNT) {
						// -60 -> ?
						instruction.operand = Math.Min(-DEFAULT_RADICAL_PATTERN_CALCULATE_COUNT * MULTIPLIER, castedOperand);
					}
				}
				yield return instruction;
			}
		}

		static void Cleanup(MethodBase original) {
			if (original?.Name == "SetupRadialPattern") {
				foreach (var field in AccessTools.GetDeclaredFields(typeof(GenRadial))) {
					var fieldType = field.FieldType;
					if (fieldType.IsArray) {
						var fieldArrayLength = (int) typeof(Array).GetProperty(nameof(Array.Length))
							.GetGetMethod().Invoke(field.GetValue(null), new object[0]);
						if (fieldArrayLength >= DEFAULT_RADICAL_PATTERN_COUNT) {
							// new T[10000] -> new T[?]
							field.SetValue(null,
								Array.CreateInstance(fieldType.GetElementType(),
									Math.Max(DEFAULT_RADICAL_PATTERN_COUNT * MULTIPLIER * MULTIPLIER, fieldArrayLength)));
						}
					}
				}
				original.Invoke(null, new object[0]);
			}
		}
	}
}
