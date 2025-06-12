using Verse;
using HarmonyLib;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;

namespace ArtificialBeings
{
    public class Pawn_JobTracker_Patch
    {
        // Charge-capable pawns that are tucked in (via capture, rescue, for operations, etc) to a charge-capable bed will charge instead of resting normally.
        [HarmonyPatch(typeof(Pawn_JobTracker), "Notify_TuckedIntoBed")]
        public static class Notify_TuckedIntoBed_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(Pawn_JobTracker __instance, Pawn ___pawn, Building_Bed bed)
            {
                if (SC_Utils.CanCharge(___pawn) && __instance.curJob?.def != ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf && bed.GetComp<CompAffectedByFacilities>()?.LinkedFacilitiesListForReading is List<Thing> linkedFacilities)
                {
                    // Search for a linked facility that is a pawn charger.
                    foreach (Thing linkedBuilding in linkedFacilities)
                    {
                        if (linkedBuilding.HasComp<CompPawnCharger>() && linkedBuilding.TryGetComp<CompPowerTrader>()?.PowerOn == true)
                        {
                            ___pawn.Position = RestUtility.GetBedSleepingSlotPosFor(___pawn, bed);
                            ___pawn.Notify_Teleported(endCurrentJob: false);
                            ___pawn.stances.CancelBusyStanceHard();
                            __instance.StartJob(JobMaker.MakeJob(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, bed), JobCondition.InterruptForced, tag: JobTag.TuckedIntoBed);
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}