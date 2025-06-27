using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    public class JobDriver_TakeStimulator : JobDriver
    {
        public ThingWithComps Target => job.GetTarget(TargetIndex.A).Thing as ThingWithComps;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            pawn.Reserve(Target, job, 10, 1);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return Toils_Ingest.PickupIngestible(TargetIndex.A, pawn);
            yield return Toils_General.Wait(400).WithProgressBarToilDelay(TargetIndex.A);
            Toil activateStimulator = ToilMaker.MakeToil("MakeNewToils");
            activateStimulator.initAction = delegate
            {
                CompSynstructStimulator stimulator = Target.GetComp<CompSynstructStimulator>();
                if (stimulator != null)
                {
                    stimulator.ApplyEffect(pawn);
                    Target.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
            };
            activateStimulator.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return activateStimulator;
        }
    }
}
