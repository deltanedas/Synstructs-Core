using Verse;

namespace ArtificialBeings
{
    public class HediffComp_CoherenceStageEffect : HediffComp
    {
        public HediffCompProperties_CoherenceStageEffect Props => (HediffCompProperties_CoherenceStageEffect)props;

        public override bool CompShouldRemove => Pawn.GetComp<CompCoherenceNeed>()?.Stage != Props.activeCoherenceStage;
    }
}
