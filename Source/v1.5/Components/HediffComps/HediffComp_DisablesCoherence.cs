using Verse;

namespace ArtificialBeings
{
    // A very simple hediff comp that notifies the coherence need component that it should be disabled when applied and reenabled when removed if there are no other disabling sources.
    public class HediffComp_DisablesCoherence : HediffComp
    {
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            Pawn.GetComp<CompCoherenceNeed>()?.IncrementDisablingSources();
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            Pawn.GetComp<CompCoherenceNeed>().DecrementDisablingSources();
        }
    }
}
