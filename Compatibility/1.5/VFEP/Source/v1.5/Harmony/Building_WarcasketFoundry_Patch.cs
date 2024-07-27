using Verse;
using HarmonyLib;
using VFEPirates;

namespace ArtificialBeings
{
    public class Building_WarcasketFoundry_Patch
    {
        // Synstructs cannot be entombed into warcaskets.
        [HarmonyPatch(typeof(Building_WarcasketFoundry), nameof(Building_WarcasketFoundry.CannotUseNowReason))]
        public class Building_WarcasketFoundry_CannotUseNowReason_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref string __result, Pawn selPawn)
            {
                if (__result == null && SC_Utils.IsSynstruct(selPawn))
                {
                    __result = "ABF_SynstructsCannotUseThis".Translate();
                }
            }
        }
    }
}