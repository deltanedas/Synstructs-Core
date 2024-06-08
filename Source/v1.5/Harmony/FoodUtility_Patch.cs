using HarmonyLib;
using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class FoodUtility_Patch
    {
        // Modify the estimated nutrition for a food by the pawn's NutritionalIntakeEfficiency
        [HarmonyPatch(typeof(FoodUtility), "GetNutrition")]
        public class FoodUtility_GetNutrition_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn eater, Thing foodSource, ThingDef foodDef, ref float __result)
            {
                __result *= eater.GetStatValue(ABF_StatDefOf.ABF_Stat_Synstruct_NutritionalIntakeEfficiency, cacheStaleAfterTicks: 1200);
            }
        }
    }
}
