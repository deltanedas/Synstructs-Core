using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_CoerciveVoiceSynthesizer : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            return RelationsUtility.PawnsKnowEachOther(p, other) && other.health.hediffSet.HasHediff(ABF_HediffDefOf.ABF_Hediff_Synstruct_Utility_CoerciveVocalizer);
        }
    }
}
