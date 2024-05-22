using Verse;
using HarmonyLib;
using RimWorld.QuestGen;
using RimWorld;
using System.Collections.Generic;
using System;

namespace ArtificialBeings
{
    public class QuestGen_Pawns_Patch_OBSOLETE
    {
        // Quest pawns for synstruct factions should use their faction's default pawn kind def. This prefix modifies the pawn kind and allows the code to resume.
        //[HarmonyPatch(typeof(QuestGen_Pawns), "GeneratePawn")]
        //[HarmonyPatch(new Type[] { typeof(Quest), typeof(PawnKindDef), typeof(Faction), typeof(bool), typeof(IEnumerable<TraitDef>), typeof(float), typeof(bool), typeof(Pawn), typeof(float), typeof(float), typeof(bool), typeof(bool), typeof(DevelopmentalStage), typeof(bool) })]
        //public class GeneratePawn_Patch
        //{
        //    [HarmonyPrefix]
        //    public static bool Prefix(ref PawnKindDef kindDef, Faction faction)
        //    {
        //        if (kindDef == PawnKindDefOf.SpaceRefugee || kindDef == PawnKindDefOf.Refugee)
        //        {
        //            // Factionless quest pawns for synstruct faction players should be synstructs.
        //            if (faction == null && Faction.OfPlayer.def.GetModExtension<ABF_FactionSynstructExtension>() is ABF_FactionSynstructExtension playerFactionExtension && playerFactionExtension.membersShouldBeSynstructs)
        //            {
        //                kindDef = Faction.OfPlayer.def.basicMemberKind;
        //            }
        //            // Quest pawns that are members of an synstruct faction should be synstructs.
        //            else if (faction != null && faction.def.GetModExtension<ABF_FactionSynstructExtension>() is ABF_FactionSynstructExtension factionExtension && factionExtension.membersShouldBeSynstructs)
        //            {
        //                kindDef = faction.def.basicMemberKind;
        //            }
        //        }
        //        return true;
        //    }
        //}
    }
}