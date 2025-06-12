using Verse;

namespace ArtificialBeings
{
    public class HediffCompProperties_CoherenceThresholdToRemove : HediffCompProperties
    {
        public HediffCompProperties_CoherenceThresholdToRemove()
        {
            compClass = typeof(HediffComp_CoherenceThresholdToRemove);
        }

        public bool shouldBeHigherThanToRemove = true;

        public float coherenceThresholdDays;
    }
}
