using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace ArtificialBeings
{
    public class ThingSetMaker_RefugeePod_Patch_OBSOLETE
    {
        // TODO: Is this necessary? Truly? Recheck HAR's options for these things.
        // Factionless refugees for synstructs or refugees for synstruct factions should be synstructs. Synstruct refugees should do a full restart on crash.
        //[HarmonyPatch(typeof(ThingSetMaker_RefugeePod), "Generate")]
        //public class Generate_Patch
        //{
        //    [HarmonyPostfix]
        //    public static void Listener(ThingSetMakerParams parms, ref List<Thing> outThings)
        //    {
        //        for (int i = outThings.Count - 1; i >= 0; i--)
        //        {
        //            Thing thing = outThings[i];
        //            if (thing is Pawn pawn)
        //            {
        //                if (!ABF_Utils.IsConsideredMechanical(pawn) && (pawn.Faction != null && pawn.Faction.def.GetModExtension<ABF_FactionSynstructExtension>()?.membersShouldBeSynstructs == true || pawn.Faction == null && Faction.OfPlayer.def.GetModExtension<ABF_FactionSynstructExtension>()?.membersShouldBeSynstructs == true))
        //                {
        //                    if (pawn.Faction != null)
        //                    {
        //                        pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawn.Faction.def.basicMemberKind, pawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowFood: true));
        //                    }
        //                    else
        //                    {
        //                        pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, null, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowFood: true));
        //                    }
        //                    HealthUtility.DamageUntilDowned(pawn);
        //                    outThings.Replace(thing, pawn);
        //                }

        //                if (ABF_Utils.IsConsideredMechanical(pawn))
        //                {
        //                    pawn.health.AddHediff(ABF_HediffDefOf.ABF_Incapacitated);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}