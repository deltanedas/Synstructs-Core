using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ArtificialBeings
{
    public class Recipe_MakeDrone : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!(thing is Pawn pawn) || !base.AvailableOnNow(pawn))
            {
                return false;
            }

            return ABF_Utils.IsArtificialBlank(pawn) && pawn.def.GetModExtension<ABF_ArtificialPawnExtension>()?.canBeDrone == true;
        }

        // Drones need to have their appropriate features initialized.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            pawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Drone;

            if (pawn.RaceProps.Humanlike)
            {
                // Create a pawn that has some age and appropriate features to duplicate into the blank to act as its new intelligence
                ABF_SynstructExtension synstructExtension = pawn.def.GetModExtension<ABF_SynstructExtension>();
                PawnKindDef pawnKindDef = synstructExtension.playerDronePawnKindDef ?? pawn.kindDef;

                if (synstructExtension.playerDronePawnKindDef?.GetModExtension<ABF_ArtificialPawnKindExtension>() is ABF_ArtificialPawnKindExtension pawnKindExtension && pawnKindExtension.pawnState != ABF_ArtificialState.Drone)
                {
                    Log.Warning($"[ABF] Operation to make a pawn into a drone encountered an issue: {pawnKindDef.LabelCap} is not a pawn kind def for drones!");
                }

                if (pawnKindDef.race != pawn.def)
                {
                    Log.Error($"[ABF] Operation to make a pawn with race {pawn.def.defName} into a drone was given race {pawnKindDef.race.defName} to copy from! Severe errors may occur.");
                }
                else
                {
                    pawn.kindDef = pawnKindDef;
                }

                PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, Faction.OfPlayer, forceGenerateNewPawn: true, canGeneratePawnRelations: false, allowAddictions: false, fixedBiologicalAge: 50, forceNoIdeo: true, colonistRelationChanceFactor: 0, forceBaselinerChance: 1f);
                Pawn personality = PawnGenerator.GeneratePawn(request);
                SC_Utils.Duplicate(personality, pawn, false);
            }
        }
    }
}
