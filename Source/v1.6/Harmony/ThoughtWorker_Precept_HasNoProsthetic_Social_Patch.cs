using Verse;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class ThoughtWorker_Precept_HasNoProsthetic_Social_Patch
    {
        // Synstructs do not have thoughts about a lack of prosthetics, nor do others have thoughts about them lacking prosthetics.
        [HarmonyPatch(typeof(ThoughtWorker_Precept_HasNoProsthetic_Social), "ShouldHaveThought")]
        public static class ThoughtWorker_Precept_HasNoProsthetic_Social_ShouldHaveThought_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn p, Pawn otherPawn, ref ThoughtState __result)
            {
                if (SC_Utils.IsSynstruct(otherPawn))
                {
                    __result = ThoughtState.Inactive;
                    return false;
                }
                return true;
            }
        }
    }
}