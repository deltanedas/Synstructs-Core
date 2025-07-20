using System;
using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace ArtificialBeings
{
    public class JobDriver_ResuscitateSynstruct : JobDriver_Resurrect
    {
        private Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

        private Thing Item => job.GetTarget(TargetIndex.B).Thing;

        private Mote warmupMote;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(1200);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            toil.tickAction = delegate
            {
                CompUsable compUsable = Item.TryGetComp<CompUsable>();
                if (compUsable != null && warmupMote == null && compUsable.Props.warmupMote != null)
                {
                    warmupMote = MoteMaker.MakeAttachedOverlay(Corpse, compUsable.Props.warmupMote, Vector3.zero);
                }

                warmupMote?.Maintain();
            };
            yield return toil;
            yield return Toils_General.Do(new Action(Resurrect));
        }

        // Resurrect the targetted synstruct if it is legal to do so.
        private void Resurrect()
        {
            Pawn innerPawn = Corpse.InnerPawn;

            bool shouldbeBlank = false;
            // Sapients have a special consideration attached: if the Core is destroyed, then the dead pawn is blank upon resurrection.
            if (SC_Utils.IsSynstruct(innerPawn) && innerPawn.health.hediffSet.GetBrain() == null)
            {
                shouldbeBlank = true;
            }

            // Apply an incapacitating hediff to the pawn. This will ensure hostile units can be safely captured, and that friendly units can't be reactivated mid-combat.
            Hediff rebootHediff;
            if (shouldbeBlank)
            {
                rebootHediff = HediffMaker.MakeHediff(ABF_HediffDefOf.ABF_Hediff_Artificial_Disabled, innerPawn);
            }
            else
            {
                rebootHediff = HediffMaker.MakeHediff(ABF_HediffDefOf.ABF_Hediff_Artificial_Incapacitated, innerPawn);
            }
            innerPawn.health.AddHediff(rebootHediff);

            // This kit executes a full resurrection which removes all negative hediffs.
            ResurrectionUtility.TryResurrect(innerPawn);
            SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(innerPawn));

            if (shouldbeBlank)
            {
                SC_Utils.Duplicate(SC_Utils.GetBlank(), innerPawn, false);
                innerPawn.GetComp<CompArtificialPawn>().State = ABF_ArtificialState.Blank;
                //innerPawn.guest?.SetGuestStatus(Faction.OfPlayer);
                if (innerPawn.playerSettings != null)
                    innerPawn.playerSettings.medCare = MedicalCareCategory.Best;
            }
            // Reviving a drone should never allow them to have an Ideology tracker.
            if (ABF_Utils.IsArtificialDrone(innerPawn))
            {
                innerPawn.ideo = null;
            }

            // Notify successful resurrection and destroy the used kit.
            Messages.Message("ABF_ResurrectionSuccessful".Translate(innerPawn).CapitalizeFirst(), innerPawn, MessageTypeDefOf.PositiveEvent, true);
            Item.SplitOff(1).Destroy(DestroyMode.Vanish);
        }
    }
}
