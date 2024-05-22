using HarmonyLib;
using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Player charge-capable pawns have modified nutritional intake values.
    [HarmonyPatch(typeof(Thing), "IngestedCalculateAmounts")]
    public class Thing_IngestedCalculateAmounts_Patch
    {
        [HarmonyPostfix]
        public static void Listener(Pawn ingester, ref float nutritionIngested)
        {
            // No patching is done on ingesting loss
            if (nutritionIngested <= 0f)
                return;

            // Player charge-capable synstructs (to avoid issues with foreign pawns not bringing enough food) have their ingested nutrition modified.
            if (SC_Utils.IsSynstruct(ingester) && SC_Utils.CanCharge(ingester) && ingester.Faction == Faction.OfPlayer)
            {
                nutritionIngested *= ingester.GetStatValue(ABF_StatDefOf.ABF_NutritionalIntakeEfficiency);
            }
        }
    }
}
