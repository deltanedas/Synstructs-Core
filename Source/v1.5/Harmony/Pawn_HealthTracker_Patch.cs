using Verse;
using HarmonyLib;
using RimWorld;
using System.Linq;

namespace ArtificialBeings
{
    public class Pawn_HealthTracker_Patch
    {
        // Upon a pawn being downed, some pawns should die automatically.
        [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
        public static class MakeDowned_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff, ref Pawn ___pawn)
            {
                // Pawns that should detonate on incapacitation die on being downed. Drones with the Martyrdom Directive also die on being downed.
                if (___pawn.def.HasModExtension<ABF_DetonateOnIncapacitation>()
                    || (ABF_Utils.IsProgrammableDrone(___pawn) && ___pawn.Faction != Faction.OfPlayerSilentFail
                    && ___pawn.GetComp<CompArtificialPawn>().ActiveDirectives.Contains(ABF_DirectiveDefOf.ABF_DirectiveMartyrdom)))
                {
                    ___pawn.Kill(dinfo, hediff);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}