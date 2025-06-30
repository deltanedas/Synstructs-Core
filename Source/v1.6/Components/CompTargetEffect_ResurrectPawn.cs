using RimWorld;
using Verse.AI;
using Verse;

namespace ArtificialBeings
{
    // Target controller for resurrecting pawns using a kit.
    public class CompTargetEffect_ResurrectPawn : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            Job job = JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_ResurrectArtificial, target, parent);
            job.count = 1;
            user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }
    }
}
