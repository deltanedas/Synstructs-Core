using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    public class JobDriver_BuildCoherenceUrgent : JobDriver
    {
        protected const TargetIndex SpotInd = TargetIndex.A;

        protected const TargetIndex BedInd = TargetIndex.B;

        private const int TicksBetweenMotesBase = 100;

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
                meditate = Toils_LayDown.LayDown(BedInd, FromBed, lookForOtherJobs: false, canSleep: false);
            }
            else
            {
                yield return Toils_Goto.GotoCell(SpotInd, PathEndMode.OnCell);
            }
            meditate.defaultCompleteMode = ToilCompleteMode.Delay;
            meditate.defaultDuration = JobEndInterval;
            meditate.FailOn(() => !MeditationUtility.SafeEnvironmentalConditions(pawn, TargetLocA, Map));
            meditate.FailOn(() => pawn.GetComp<CompCoherenceNeed>().CoherenceLevel >= pawn.GetComp<CompCoherenceNeed>().TargetCoherenceLevel);
            meditate.AddPreTickIntervalAction(delegate(int delta)
            {
                CoherenceTick(delta);
            });
            yield return meditate;
        }

        protected void CoherenceTick(int delta)
        {
            pawn.skills?.Learn(SkillDefOf.Intellectual, 0.0060000011f * delta);
            pawn.GainComfortFromCellIfPossible(delta);
            if (pawn.IsHashIntervalTick(TicksBetweenMotesBase))
            {
                FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Meditating);
            }
            pawn.GetComp<CompCoherenceNeed>().ChangeCoherenceLevel(0.00003f * delta);
        }
    }
}