using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ArtificialBeings
{
    public class Recipe_MakeReprogrammable : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!(thing is Pawn pawn) || !base.AvailableOnNow(pawn))
            {
                return false;
            }

            return ABF_Utils.PawnStateFor(pawn) == ABF_ArtificialState.Blank && pawn.def.GetModExtension<ABF_ArtificialPawnExtension>()?.canBeReprogrammable == true;
        }

        // Programmable drones need to have their appropriate features initialized.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            pawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Reprogrammable;

            if (pawn.RaceProps.Humanlike)
            {
                // Create a pawn that has some age and appropriate features to duplicate into the blank to act as its new intelligence
                ABF_SynstructExtension synstructExtension = pawn.def.GetModExtension<ABF_SynstructExtension>();
                PawnKindDef pawnKindDef = synstructExtension.playerReprogrammableDronePawnKindDef ?? pawn.kindDef;

                if (synstructExtension.playerReprogrammableDronePawnKindDef?.GetModExtension<ABF_ArtificialPawnKindExtension>() is ABF_ArtificialPawnKindExtension pawnKindExtension && pawnKindExtension.pawnState != ABF_ArtificialState.Reprogrammable)
                {
                    Log.Warning($"[ABF] Operation to make a pawn into a reprogrammable drone encountered an issue: {pawnKindDef.LabelCap} is not a pawn kind def for reprogrammable drones!");
                }

                if (pawnKindDef.race != pawn.def)
                {
                    Log.Error($"[ABF] Operation to make a pawn with race {pawn.def.defName} into a drone was given race {pawnKindDef.race.defName} to copy from! Severe errors may occur.");
                }
                else
                {
                    pawn.kindDef = pawnKindDef;
                }

                PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, pawn.Faction?.def, new PawnGenerationRequest(pawn.kindDef));
                pawn.Notify_DisabledWorkTypesChanged();
                pawn.skills?.Notify_SkillDisablesChanged();
            }
        }
    }
}

