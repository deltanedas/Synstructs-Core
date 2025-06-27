using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    // Simple ThingComp that allows synstructs to consume it directly via an order to apply a hediff.
    public class CompSynstructStimulator : ThingComp
    {
        public CompProperties_SynstructStimulator Props
        {
            get
            {
                return props as CompProperties_SynstructStimulator;
            }
        }


        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            base.CompFloatMenuOptions(selPawn);
            // No reason to show an option for a race which has no artificial need that is fulfilled by this.
            if (!SC_Utils.IsSynstruct(selPawn))
            {
                yield break;
            }

            // Force consuming one of this item.
            yield return new FloatMenuOption("ABF_ForceConsumption".Translate(parent.LabelNoCount), delegate () {
                Job job = JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_TakeStimulator, parent);
                job.count = 1;
                selPawn.jobs.TryTakeOrderedJob(job, JobTag.SatisfyingNeeds);
            });
        }

        public virtual void ApplyEffect(Pawn pawn)
        {
            pawn.health.AddHediff(Props.hediffToApply);
        }
    }
}