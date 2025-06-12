using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_OrganicVsBiophobe : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            if (!RelationsUtility.PawnsKnowEachOther(p, other) || ABF_Utils.IsArtificial(p))
            {
                return false;
            }

            Trait bioPreferenceTrait = other.story.traits.GetTrait(ABF_TraitDefOf.ABF_Trait_Synstruct_BiologicalPreference);
            if (bioPreferenceTrait == null || bioPreferenceTrait.Degree >= 0) 
            {
                return false;
            }
            return true;
        }
    }
}
