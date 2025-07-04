using Verse;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class PawnAttackGizmoUtility_Patch
    {
        // Synstruct animals with the appropriate draft controller should be permitted to be given orders.
        [HarmonyPatch(typeof(PawnAttackGizmoUtility), "CanOrderPlayerPawn")]
        public static class PawnAttackGizmoUtility_CanOrderPlayerPawn_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn pawn, ref bool __result)
            {
                if (SC_Utils.IsSynstruct(pawn) && pawn.RaceProps.intelligence == Intelligence.Animal)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }
    }
}