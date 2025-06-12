using Verse;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class CompGoldenCube_Patch
    {
        // Synstructs are immune to Cube effects. For now.
        [HarmonyPatch(typeof(CompGoldenCube), "ValidatePawn")]
        public static class Kill_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(ref bool __result, Pawn pawn)
            {
                if (SC_Utils.IsSynstruct(pawn))
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }
    }
}