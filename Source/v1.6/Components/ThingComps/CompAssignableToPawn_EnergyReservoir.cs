using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ArtificialBeings
{
    // A simple CompAssignable subclass that allows pawns to be assigned an energy reservoir. There are no restrictions besides being able to charge (no 1-1 restrictions, etc).
    public class CompAssignableToPawn_EnergyReservoir : CompAssignableToPawn
    {
        // All animals and colonists are possible candidates (with the exception of Dryads, which Core seems to think have a special use case).
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                List<Pawn> candidates = new List<Pawn>();
                foreach (Pawn pawn in parent.Map.mapPawns.SpawnedColonyAnimals)
                {
                    if (!pawn.RaceProps.Dryad)
                    {
                        candidates.Add(pawn);
                    }
                }
                foreach (Pawn pawn in parent.Map.mapPawns.FreeColonists)
                {
                    candidates.Add(pawn);
                }
                return candidates.OrderByDescending(delegate (Pawn p)
                {
                    if (!CanAssignTo(p).Accepted)
                    {
                        return 0;
                    }
                    return 1;
                }).ThenBy((Pawn p) => p.LabelShort);
            }
        }

        // Can be assigned if the pawn can charge.
        public override AcceptanceReport CanAssignTo(Pawn pawn)
        {
            if (pawn.GetStatValue(ABF_StatDefOf.ABF_Stat_Synstruct_ChargingSpeed, cacheStaleAfterTicks:GenTicks.TickLongInterval) <= 0f)
            {
                return "ABF_IncapableOfCharging".Translate(pawn.LabelShortCap);
            }
            return AcceptanceReport.WasAccepted;
        }
    }
}
