using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;
using Verse;
using System;
using UnityEngine;

namespace ArtificialBeings
{
    public class JobDriver_RechargeSelf : JobDriver
    {
        public ThingWithComps Reservoir => job.GetTarget(TargetIndex.A).Thing as ThingWithComps;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(600);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return toil;
            yield return Toils_General.Do(new Action(Recharge));
        }

        // Replenish charge and drain the reservoir
        private void Recharge()
        {
            if (!(Reservoir is ThingWithComps thing) || !(thing.GetComp<CompEnergyReservoir>() is CompEnergyReservoir reservoir) || !(pawn.needs.TryGetNeed(ABF_NeedDefOf.ABF_Need_Synstruct_Energy) is Need_SynstructEnergy need))
            {
                return;
            }

            float toCharge = Mathf.Min(reservoir.reserve, need.AmountDesired);
            need.CurLevel += toCharge;
            reservoir.reserve -= toCharge;
        }
    }
}