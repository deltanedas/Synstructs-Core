using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_DronesOfSameModelMood : ThoughtWorker
    {
        public static ThoughtState CurrentThoughtState(Pawn p)
        {
            if (!p.Spawned)
            {
                return ThoughtState.Inactive;
            }

            List<Pawn> colonyPawns = p.Map.mapPawns.FreeColonists;
            int dronesOfSameModel = 0;
            for (int i = colonyPawns.Count; --i >= 0;)
            {
                if (colonyPawns[i].def == p.def && ABF_Utils.IsArtificialDrone(colonyPawns[i]))
                {
                    dronesOfSameModel++;
                }
            }

            if (dronesOfSameModel == 0)
            {
                return ThoughtState.Inactive;
            }
            else if (dronesOfSameModel == 1)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            else if (dronesOfSameModel <= 4)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            else
            {
                return ThoughtState.ActiveAtStage(2);
            }
        }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (ThoughtUtility.ThoughtNullified(p, def) || !SC_Utils.IsSynstruct(p) || !ABF_Utils.IsArtificialSapient(p) || !p.Spawned)
            {
                return ThoughtState.Inactive;
            }
            return CurrentThoughtState(p);
        }
    }
}
