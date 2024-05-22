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

            return ABF_Utils.PawnStateFor(pawn) == ABF_ArtificialState.Blank && pawn.def.GetModExtension<ABF_ArtificialPawnExtension>()?.canBeDrone == true;
        }

        // Drones need to have their appropriate features initialized.
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            // Set the pawn's backstories to its drone backstories, controlled via its synstruct mod settings. Null backstories are okay.
            if (pawn.story != null)
            {
                ABF_SynstructExtension synstructExtension = pawn.def.GetModExtension<ABF_SynstructExtension>();
                pawn.story.Childhood = synstructExtension?.droneChildhoodBackstoryDef;
                pawn.story.Adulthood = synstructExtension?.droneAdulthoodBackstoryDef;
                pawn.Notify_DisabledWorkTypesChanged();
                pawn.skills?.Notify_SkillDisablesChanged();
            }

            pawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Drone;
        }
    }
}
