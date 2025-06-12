using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple StatWorker that will only display stats that use this worker if the pawn needs coherence.
    public class StatWorker_Coherence : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            return base.ShouldShowFor(req) && req.Thing != null && req.Thing is Pawn pawn && pawn.def.HasComp<CompCoherenceNeed>();
        }
    }
}
