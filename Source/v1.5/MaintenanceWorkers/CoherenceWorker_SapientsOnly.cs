using Verse;

namespace ArtificialBeings
{
    // Simple coherence worker that prevents this worker from firing on non-sapient pawns like drones.
    public class CoherenceWorker_SapientsOnly : CoherenceWorker
    {
        public override bool CanApplyTo(Pawn pawn)
        {
            // Non-humanlike intelligences and non-mechanical-sapient intelligences are illegal.
            if (!ABF_Utils.IsArtificialSapient(pawn))
            {
                return false;
            }
            return true;
        }
    }
}
