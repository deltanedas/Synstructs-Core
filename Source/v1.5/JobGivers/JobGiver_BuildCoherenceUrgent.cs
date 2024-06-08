using RimWorld;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    public class JobGiver_BuildCoherenceUrgent : ThinkNode_JobGiver
    {
        // Pawn ThinkTrees occasionally sort jobs to take on a priority.
        public override float GetPriority(Pawn pawn)
        {
            TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
            {
                return 9.5f;
            }
            else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
            {
                return 8f;
            }
            return 5f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            CompCoherenceNeed compCoherenceNeed = pawn.GetComp<CompCoherenceNeed>();

            if (compCoherenceNeed == null || pawn.InAggroMentalState || !pawn.Spawned || pawn.Downed)
            {
                return null;
            }

            // Urgent re-cohering is not done if the target level is below the poor coherence threshold.
            if (compCoherenceNeed.TargetCoherenceLevel <= 0.3)
            {
                return null;
            }

            // If the pawn is currently urgently rebuilding coherence and is not at their target level, continue building coherence.
            if (pawn.CurJobDef == ABF_JobDefOf.ABF_Job_Synstruct_BuildCoherenceUrgently && compCoherenceNeed.TargetCoherenceLevel > compCoherenceNeed.CoherenceLevel)
            {
                return JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_BuildCoherenceUrgently, pawn.Position, pawn.InBed() ? ((LocalTargetInfo)pawn.CurrentBed()) : new LocalTargetInfo(pawn.Position));
            }

            // Urgent coherence building is otherwise skipped if it is less than 30% away from the target level and above Poor.
            if (compCoherenceNeed.CoherenceLevel + 0.3f > compCoherenceNeed.TargetCoherenceLevel && compCoherenceNeed.Stage > ABF_CoherenceStage.Poor)
            {
                return null;
            }

            // FindMeditationSpot will find a place that is valid and will allow this job to continue. If it is invalid, then there is nowhere to build coherence and no job is given.
            LocalTargetInfo coherenceSpot = CoherenceUtility.FindCoherenceSpot(pawn);
            if (coherenceSpot.IsValid)
            {
                return JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_BuildCoherenceUrgently, coherenceSpot.Cell, pawn.InBed() ? ((LocalTargetInfo)pawn.CurrentBed()) : new LocalTargetInfo(coherenceSpot.Cell));
            }
            return null;
        }
    }
}