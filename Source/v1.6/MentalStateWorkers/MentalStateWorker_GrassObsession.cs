using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    public class MentalStateWorker_GrassObsession : MentalStateWorker
    {
        public override bool StateCanOccur(Pawn pawn)
        {
            if (!base.StateCanOccur(pawn))
            {
                return false;
            }

            if (!pawn.Spawned)
            {
                return true;
            }

            Map map = pawn.Map;
            List<Thing> availableGrass = map.listerThings.ThingsOfDef(ThingDefOf.Plant_Grass);
            if (availableGrass.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
