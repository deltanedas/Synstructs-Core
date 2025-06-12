using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_BiologicalPreferenceTowardBiomimetics : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.Faction != Faction.OfPlayerSilentFail || p.Map == null)
            {
                return false;
            }

            int num = SC_Utils.playerBiomimeticCount.GetWithFallback(p.Map, 0);
            if (num >= 10)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            else if (num >= 6)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            else if (num >= 3)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            else if (num > 0)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            return false;
        }
    }
}