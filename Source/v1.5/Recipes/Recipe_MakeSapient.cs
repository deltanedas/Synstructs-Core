using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ArtificialBeings
{
    public class Recipe_MakeSapient : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!(thing is Pawn pawn) || !base.AvailableOnNow(pawn))
            {
                return false;
            }

            return ABF_Utils.PawnStateFor(pawn) == ABF_ArtificialState.Blank && pawn.def.GetModExtension<ABF_ArtificialPawnExtension>()?.canBeSapient == true;
        }

        // Sapients need to have their intelligence generated and appropriate story and skills populated.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            pawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Sapient;

            // Create a pawn that has some age and appropriate features to duplicate into the blank to act as its new intelligence.
            PawnGenerationRequest request = new PawnGenerationRequest(pawn.kindDef, Faction.OfPlayer, forceNoBackstory: true, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowAddictions: false, fixedBiologicalAge: 50, forceNoIdeo: true, colonistRelationChanceFactor: 0, forceBaselinerChance: 1f);
            Pawn personality = PawnGenerator.GeneratePawn(request);
            ABF_SynstructExtension synstructExtension = personality.def.GetModExtension<ABF_SynstructExtension>();
            personality.story.Childhood = synstructExtension?.sapientChildhoodBackstoryDef;
            personality.story.Adulthood = synstructExtension?.sapientAdulthoodBackstoryDef;
            SC_Utils.Duplicate(personality, pawn, false);
            Hediff rebootHediff = pawn.health.hediffSet.GetFirstHediffOfDef(ABF_HediffDefOf.ABF_Hediff_Artificial_Incapacitated);
            if (rebootHediff == null)
            {
                rebootHediff = HediffMaker.MakeHediff(ABF_HediffDefOf.ABF_Hediff_Artificial_Incapacitated, pawn, null);
                pawn.health.AddHediff(rebootHediff);
            }
            rebootHediff.Severity = 1;

            // Allow the player to pick a few passions and a trait for the new synstruct, akin to child growth moments in Biotech.
            if (ModLister.BiotechInstalled)
            {
                ChoiceLetter_PersonalityShift choiceLetter = (ChoiceLetter_PersonalityShift)LetterMaker.MakeLetter(ABF_LetterDefOf.ABF_Letter_Synstruct_PersonalityShift);
                choiceLetter.ConfigureChoiceLetter(pawn, 3, 3, false, false);
                choiceLetter.Label = "ABF_PersonalityShiftNewboot".Translate();
                choiceLetter.disappearAtTick = Find.TickManager.TicksGame;
                choiceLetter.OpenLetter();
            }
        }
    }
}

