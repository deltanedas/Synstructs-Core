using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    // Synstructs should build coherence if they can't find anything else to do - this effectively replaces default vanilla's wandering behavior.
    public class JobGiver_BuildCoherenceIdle : ThinkNode_JobGiver
    {
        // Pawn ThinkTrees occasionally sort jobs to take on a priority. This is exceedingly low priority for idle coherence building.
        public override float GetPriority(Pawn pawn)
        {
            return 0.5f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            CompCoherenceNeed compCoherenceNeed = pawn.GetComp<CompCoherenceNeed>();

            if (compCoherenceNeed == null || compCoherenceNeed.Disabled || pawn.InAggroMentalState || compCoherenceNeed.CoherenceLevel > 0.95f)
            {
                return null;
            }

            // If this pawn's current position is legal for coherence, use it.
            if (cachedPawnCoherenceSpots.ContainsKey(pawn.thingIDNumber))
            {
                Pair<IntVec3, int> cachedCoherenceTimedSpot = cachedPawnCoherenceSpots[pawn.thingIDNumber];
                if (cachedCoherenceTimedSpot != null && cachedCoherenceTimedSpot.First == pawn.Position && Find.TickManager.TicksGame - cachedCoherenceTimedSpot.Second < 30000 && CoherenceUtility.SafeEnvironmentalConditions(pawn, pawn.Position, pawn.Map) && pawn.Position.Standable(pawn.Map) && !pawn.Position.IsForbidden(pawn) && pawn.CanReserveAndReach(pawn.Position, PathEndMode.OnCell, Danger.None))
                {
                    cachedPawnCoherenceSpots[pawn.thingIDNumber] = new Pair<IntVec3, int>(pawn.Position, Find.TickManager.TicksGame);
                    return JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_BuildCoherenceIdly, pawn.Position, pawn.InBed() ? ((LocalTargetInfo)pawn.CurrentBed()) : new LocalTargetInfo(pawn.Position));
                }
                else
                {
                    cachedPawnCoherenceSpots.Remove(pawn.thingIDNumber);
                }
            }

            // Find a valid place to build coherence, and store it in the cache for later use.
            LocalTargetInfo coherenceSpot = CoherenceUtility.FindCoherenceSpot(pawn);
            if (coherenceSpot.IsValid)
            {
                cachedPawnCoherenceSpots[pawn.thingIDNumber] = new Pair<IntVec3, int>(coherenceSpot.Cell, Find.TickManager.TicksGame);
                return JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_BuildCoherenceIdly, coherenceSpot.Cell, pawn.InBed() ? ((LocalTargetInfo)pawn.CurrentBed()) : new LocalTargetInfo(pawn.Position));
            }
            return null;
        }

        // A cached dictionary with Pawn ThingID keys and IntVec3,int Pair values matching a pawn's id to their last coherence spot and when they started building coherence there.
        // If the stored tick for the last coherence is less than 30,000 ticks old (1/2 of a day), reuse the spot instead of finding a new one.
        private static Dictionary<int, Pair<IntVec3, int>> cachedPawnCoherenceSpots = new Dictionary<int, Pair<IntVec3, int>>();
    }
}
