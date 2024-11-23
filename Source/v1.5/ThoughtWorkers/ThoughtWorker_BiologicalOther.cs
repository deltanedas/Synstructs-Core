using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_BiologicalOther : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            return RelationsUtility.PawnsKnowEachOther(p, other) && !ABF_Utils.IsArtificial(other);
        }
    }
}
