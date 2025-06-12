using Verse;

namespace ArtificialBeings
{
    public class HediffComp_CoherenceThresholdToRemove : HediffComp
    {
        private CompCoherenceNeed compCoherenceNeed;

        public HediffCompProperties_CoherenceThresholdToRemove Props => (HediffCompProperties_CoherenceThresholdToRemove)props;

        public override bool CompShouldRemove => Props.shouldBeHigherThanToRemove ? CompCoherenceNeed.coherenceEffectTicks > Props.coherenceThresholdDays * 60000 : CompCoherenceNeed.coherenceEffectTicks < Props.coherenceThresholdDays * 60000;

        public CompCoherenceNeed CompCoherenceNeed
        {
            get
            {
                if (compCoherenceNeed == null)
                {
                    compCoherenceNeed = Pawn.GetComp<CompCoherenceNeed>();
                }
                return compCoherenceNeed;
            }
        }
    }
}
