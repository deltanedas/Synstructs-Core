using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_CoherenceMood : ThoughtWorker
    {
        public static ThoughtState CurrentThoughtState(Pawn p)
        {
            float coherenceEffect = p.GetComp<CompCoherenceNeed>().coherenceEffectTicks;
            // Abysmal: 15 or more days of poor coherence effect.
            if (coherenceEffect < -900000)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            // Terrible: 9 or more days of poor coherence effect.
            else if (coherenceEffect < -540000)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            // Poor: 3 or more days of poor coherence effect.
            else if (coherenceEffect < -180000)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            // Standard: less than 3 days of poor or good coherence effect.
            else if (coherenceEffect < 180000)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            // Decent: 3 or more days of good coherence effect.
            else if (coherenceEffect < 540000)
            {
                return ThoughtState.ActiveAtStage(4);
            }
            // Excellent: 9 or more days of good coherence effect.
            else if (coherenceEffect < 900000)
            {
                return ThoughtState.ActiveAtStage(5);
            }
            // Immaculate: 15 or more days of good coherence effect.
            else
            {
                return ThoughtState.ActiveAtStage(6);
            }
        }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (ThoughtUtility.ThoughtNullified(p, def) || p.GetComp<CompCoherenceNeed>() == null)
            {
                return ThoughtState.Inactive;
            }
            return CurrentThoughtState(p);
        }
    }
}
