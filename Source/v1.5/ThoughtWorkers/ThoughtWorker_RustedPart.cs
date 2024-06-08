using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_RustedPart : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (ThoughtUtility.ThoughtNullified(p, def) || !SC_Utils.IsSynstruct(p))
            {
                return ThoughtState.Inactive;
            }

            for (int i = p.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
            {
                if (p.health.hediffSet.hediffs[i].def == ABF_HediffDefOf.ABF_Hediff_Synstruct_Coherence_RustedPart)
                    return true;
            }
            return ThoughtState.Inactive;
        }
    }
}
