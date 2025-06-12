using RimWorld;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    // The default MentalBreakWorker supports required traits, but not specific trait degrees.
    // Rather than making a DefModExtension and checking it for a degree, I decided it was easier to just make it a hardcoded trait check.
    public class MentalBreakWorker_HasHostileGrassObsession : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            if (base.BreakCanOccur(pawn))
            {
                Trait trait = pawn.story?.traits?.GetTrait(ABF_TraitDefOf.ABF_Trait_Synstruct_BiologicalPreference);
                if (trait != null && trait.Degree == -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
