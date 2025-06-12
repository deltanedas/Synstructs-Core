using Verse;

namespace ArtificialBeings
{
    // Very simple coherence worker that blocks the coherence effect being applied to the pawn's Consciousness source(s).
    public class CoherenceWorker_NoConsciousnessSource : CoherenceWorker
    {
        public override bool CanApplyOnPart(Pawn pawn, BodyPartRecord part)
        {
            return part == null || pawn.health.hediffSet.GetBrain() != part;
        }
    }
}
