using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_SynstructHasArchotechImplant : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!SC_Utils.IsSynstruct(p))
            {
                return false;
            }

            List<Hediff> archotechHediffs = new List<Hediff>();
            p.health.hediffSet.GetHediffs(ref archotechHediffs, hediff => hediff.def.spawnThingOnRemoved?.techLevel == TechLevel.Archotech);
            int archotechImplants = archotechHediffs.Count;

            if (archotechImplants > 0)
            {
                return ThoughtState.ActiveAtStage(archotechImplants - 1);
            }
            return false;
        }
    }
}
