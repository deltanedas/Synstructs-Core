using RimWorld;
using Verse.AI;
using Verse;

namespace ArtificialBeings
{
    // Target controller for resurrecting pawns using a kit.
    public class CompTargetEffect_ResurrectPawn : CompTargetEffect
    {
        public CompTargetEffectProperties_ResurrectPawn Props
        {
            get
            {
                return (CompTargetEffectProperties_ResurrectPawn)props;
            }
        }

        public override bool CanApplyOn(Thing target)
        {
            return target is Corpse corpse && SC_Utils.IsSynstruct(corpse.InnerPawn) && Props.validStates.Contains(ABF_Utils.PawnStateFor(corpse.InnerPawn));
        }

        public override void DoEffectOn(Pawn user, Thing target)
        {
            // Only player controlled pawns that can reach the target can use the kit, and only on pawns in a valid state.
            if (user.Faction == Faction.OfPlayer && target is Corpse corpse && Props.validStates.Contains(ABF_Utils.PawnStateFor(corpse.InnerPawn)) && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
            {
                Job job = JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_ResurrectArtificial, target, parent);
                job.count = 1;
                user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            }
        }
    }
}
