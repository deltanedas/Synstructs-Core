using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;
using RimWorld;
using Verse;
using System;
using UnityEngine;

namespace ArtificialBeings
{
    public class JobDriver_RechargeSelf : JobDriver
    {
        public Building_Bed Bed => job.GetTarget(TargetIndex.A).Thing as Building_Bed;

        public ThingWithComps Reservoir => job.GetTarget(TargetIndex.A).Thing as ThingWithComps;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (Bed != null && !pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed))
            {
                return false;
            }
            // Reservoirs don't have reservations and can accept infinite pawns at once (stacking is allowed).
            else if (Reservoir is ThingWithComps thing && thing.HasComp<CompEnergyReservoir>())
            {
                return true;
            }
            pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
            return true;
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (TargetThingA is Building_Bed)
            {
                yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A);
                yield return Toils_Bed.GotoBed(TargetIndex.A);
                yield return Toils_LayDownPower.LayDown(TargetIndex.A, true);
            }
            else if (TargetThingA is ThingWithComps)
            {
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
                Toil toil = Toils_General.Wait(600);
                toil.WithProgressBarToilDelay(TargetIndex.A);
                toil.FailOnDespawnedOrNull(TargetIndex.A);
                toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
                yield return toil;
                yield return Toils_General.Do(new Action(Recharge));
            }
            else
            {
                yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
                yield return Toils_LayDownPower.LayDown(TargetIndex.B, false);
            }
        }

        // Replenish charge and drain the reservoir
        private void Recharge()
        {
            if (!(Reservoir is ThingWithComps thing) || !(thing.GetComp<CompEnergyReservoir>() is CompEnergyReservoir reservoir) || !(pawn.needs.TryGetNeed(NeedDefOf.Food) is Need need))
            {
                return;
            }

            float toCharge = Mathf.Min(reservoir.reserve, need.MaxLevel - need.CurLevel);
            need.CurLevel += toCharge;
            reservoir.reserve -= toCharge;
        }
    }
}