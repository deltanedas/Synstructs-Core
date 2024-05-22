using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    // Simplified version of BuildCoherenceUrgent that will continue to do this job until it is interrupted by something else or its timer elapses. Coherence effects are applied when the job terminates.
    // It will not track how much coherence it has, or its target coherence level, allowing it to potentially waste time (but otherwise the pawn would just wander uselessly so this is fine).
    public class JobDriver_BuildCoherenceIdle : JobDriver
    {
        protected const TargetIndex SpotInd = TargetIndex.A;

        protected const TargetIndex BedInd = TargetIndex.B;

        private const int JobEndInterval = 4000;

        private bool FromBed => job.GetTarget(BedInd).Thing is Building_Bed;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(SpotInd), job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil meditate = ToilMaker.MakeToil("MakeNewToils");
            meditate.socialMode = RandomSocialMode.Off;
            if (FromBed)
            {
                this.KeepLyingDown(BedInd);
                if (SC_Utils.CanCharge(pawn))
                {
                    meditate = Toils_LayDownPower.LayDown(BedInd, FromBed, lookForOtherJobs: true, canSleep: false);
                }
                else
                {
                    meditate = Toils_LayDown.LayDown(BedInd, FromBed, lookForOtherJobs: true, canSleep: false);
                }
            }
            else
            {
                yield return Toils_Goto.GotoCell(SpotInd, PathEndMode.OnCell);
            }
            meditate.defaultCompleteMode = ToilCompleteMode.Delay;
            meditate.defaultDuration = JobEndInterval;
            meditate.FailOn(() => !MeditationUtility.SafeEnvironmentalConditions(pawn, TargetLocA, Map));
            meditate.AddFinishAction(delegate
            {
                pawn.GetComp<CompCoherenceNeed>().ChangeCoherenceLevel((Find.TickManager.TicksGame - startTick) * 0.00003f);
            });
            yield return meditate;
        }
    }
}