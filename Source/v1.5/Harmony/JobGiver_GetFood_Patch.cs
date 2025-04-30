using Verse;
using Verse.AI;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class JobGiver_GetFood_Patch
    {
        // Override job for getting food based on whether the pawn can charge instead, and on whether the pawn can effectively eat at all.
        [HarmonyPatch(typeof(JobGiver_GetFood), "TryGiveJob")]
        public class JobGiver_GetFood_TryGiveJob_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(Pawn pawn, ref Job __result)
            {
                // Only affect pawns that are on a map.
                if (pawn == null || pawn.Map == null)
                {
                    return true;
                }

                float nutritionalEfficiency = pawn.GetStatValue(ABF_StatDefOf.ABF_Stat_Synstruct_NutritionalIntakeEfficiency, cacheStaleAfterTicks: 1200);
                float chargingEfficiency = pawn.GetStatValue(ABF_StatDefOf.ABF_Stat_Synstruct_ChargingSpeed, cacheStaleAfterTicks: 1200);

                // If the pawn can neither charge nor consume food, return no job and don't bother looking for food or a charging spot.
                if (nutritionalEfficiency <= 0 && chargingEfficiency <= 0)
                {
                    __result = null;
                    return false;
                }

                // Non-player pawns as well as pawns that can not charge do not need to search for a charging spot and can use normal behavior. Do not override non-spawned or drafted pawns.
                if ((pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer) || chargingEfficiency <= 0 || !pawn.Spawned || pawn.Drafted)
                {
                    return true;
                }

                // Attempt to locate an available charging reservoir.
                if (SC_Utils.GetReservoir(pawn) is Thing reservoir)
                {
                    __result = new Job(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, new LocalTargetInfo(reservoir));
                    return false;
                }

                // Attempt to locate a viable charging bed for the pawn. This can suit comfort, rest, and room needs whereas the charging station can not.
                Building_Bed bed;
                bed = SC_Utils.GetChargingBed(pawn, pawn);
                if (bed != null)
                {
                    pawn.ownership.ClaimBedIfNonMedical(bed);
                    __result = new Job(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, new LocalTargetInfo(bed));
                    return false;
                }

                // Downed pawns can not use charging stations.
                if (!pawn.Downed)
                {
                    // Attempt to locate a viable charging station. Set the result to this if one is found.
                    Building_ChargingStation station = (Building_ChargingStation)GenClosest.ClosestThingReachable(pawn.PositionHeld, pawn.MapHeld, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.Touch, TraverseParms.For(pawn), validator: building => building is Building_ChargingStation chargeStation && pawn.CanReach(chargeStation, PathEndMode.Touch, Danger.Some) && building.Position.InAllowedArea(pawn) && building.TryGetComp<CompPowerTrader>()?.PowerOn == true && chargeStation.GetOpenRechargeSpot(pawn) != IntVec3.Invalid);
                    if (station != null)
                    {
                        __result = new Job(ABF_JobDefOf.ABF_Job_Synstruct_ChargeSelf, new LocalTargetInfo(station.GetOpenRechargeSpot(pawn)), new LocalTargetInfo(station));
                        return false;
                    }
                }

                // If there is no viable charging bed or charging station and the pawn can not eat food, give no job.
                if (nutritionalEfficiency <= 0)
                {
                    __result = null;
                    return false;
                }
                // If the pawn can eat food, then allow normal behavior to continue.
                else
                {
                    return true;
                }
            }
        }
    }
}