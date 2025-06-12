using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple coherence worker that removes all xp that was learned since the last midnight.
    public class CoherenceWorker_RandomMemoryLoss : CoherenceWorker
    {
        public override void OnApplied(Pawn pawn, BodyPartRecord part)
        {
            if (pawn.skills is Pawn_SkillTracker skillTracker)
            {
                foreach (SkillRecord record in skillTracker.skills)
                {
                    if (record.TotallyDisabled)
                    {
                        continue;
                    }

                    record.Learn(-record.xpSinceMidnight, true);
                }
            }
        }
    }
}
