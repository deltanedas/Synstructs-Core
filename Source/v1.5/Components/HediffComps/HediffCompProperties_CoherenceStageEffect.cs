using Verse;

namespace ArtificialBeings
{
    public class HediffCompProperties_CoherenceStageEffect : HediffCompProperties
    {
        public HediffCompProperties_CoherenceStageEffect()
        {
            compClass = typeof(HediffComp_CoherenceStageEffect);
        }

        public ABF_CoherenceStage activeCoherenceStage;
    }
}
