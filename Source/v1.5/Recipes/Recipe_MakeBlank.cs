using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace ArtificialBeings
{
    public class Recipe_MakeBlank : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!(thing is Pawn pawn) ||  !base.AvailableOnNow(pawn))
            {
                return false;
            }

            ABF_ArtificialState state = ABF_Utils.PawnStateFor(pawn);
            return state != ABF_ArtificialState.Unknown && state != ABF_ArtificialState.Blank;
        }

        // Making a pawn blank has special considerations. The state-change in the comp handles most of it, but making a sapient synstruct a blank is considered murder.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            if (ABF_Utils.PawnStateFor(pawn) == ABF_ArtificialState.Sapient)
            {
                PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Died);
                Pawn spouse = pawn.relations?.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse);
                if (spouse != null && !spouse.Dead && spouse.needs.mood != null)
                {
                    MemoryThoughtHandler memories = spouse.needs.mood.thoughts.memories;
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
                }
                Traverse.Create(pawn.relations).Method("AffectBondedAnimalsOnMyDeath").GetValue();
                pawn.health.NotifyPlayerOfKilled(null, null, null);
            }

            // Becoming blank always resets backstories.
            if (pawn.story != null)
            {
                pawn.story.Childhood = ABF_BackstoryDefOf.ABF_Backstory_Synstruct_Childhood_Blank;
                pawn.story.Adulthood = ABF_BackstoryDefOf.ABF_Backstory_Synstruct_Adulthood_Blank;
                pawn.Notify_DisabledWorkTypesChanged();
                pawn.skills?.Notify_SkillDisablesChanged();
            }

            pawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Blank;

            // Ensure the pawn is disabled as it is blank.
            pawn.health.AddHediff(ABF_HediffDefOf.ABF_Hediff_Artificial_Disabled);
        }
    }
}
