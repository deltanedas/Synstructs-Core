using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_BiologicalPreferenceTowardOrganics : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.Faction != Faction.OfPlayerSilentFail || p.Map == null)
            {
                return false;
            }

            int num = SC_Utils.playerOrganicPawnCount.GetWithFallback(p.Map, 0);
            if (num >= 20)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            else if (num >= 10)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            else if (num >= 5)
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