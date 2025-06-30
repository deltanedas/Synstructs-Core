using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    public class CompRoomCharger_PoweredHeatGenerating : CompRoomCharger
    {
        protected CompPowerTrader compPowerTrader;

        protected float cachedPowerConsumption = 0f;

        protected float cachedHeatGeneration = 0f;

        private List<Pawn> chargingPawns;

        public override bool Active
        {
            get
            {
                if (compPowerTrader?.PowerOn == true)
                {
                    return true;
                }
                return false;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            base.PostSpawnSetup(respawningAfterLoad);
        }

        // Using the provided room and lists of chargers and pawns in the room, calculate the effects of charging. baselineOnly being true implies the device should be treated as inoperable.
        public override void RecalculateCaches(Room parentRoom, List<CompRoomCharger> roomChargers, List<Pawn> roomPawns, bool baselineOnly = false)
        {
            // Recalculating the cache should store the list of valid charging pawns for the purpose of the pulse.
            chargingPawns = roomPawns;
            if (baselineOnly || roomChargers.NullOrEmpty() || roomPawns.NullOrEmpty())
            {
                cachedPowerConsumption = Mathf.Max(compPowerTrader.Props.idlePowerDraw, 0f);
                cachedHeatGeneration = 0f;
                return;
            }

            int activeChargers = 0;
            // Count only chargers which are active.
            for (int i = roomChargers.Count - 1; i >= 0; i--)
            {
                if (roomChargers[i].Active)
                {
                    activeChargers++;
                }
            }

            int validPawns = roomPawns.Count;
            float powerConsumptionModifier = validPawns + (0.25f * (validPawns - 1) / Mathf.Max(activeChargers, 1f));
            cachedPowerConsumption = compPowerTrader.Props.PowerConsumption * powerConsumptionModifier;
            cachedHeatGeneration = 0.5f * validPawns;
        }

        public override void DoChargePulse()
        {
            // Do nothing if not spawned.
            if (!parent.Spawned)
            {
                return;
            }

            // Update the power comp so it remains synced, regardless of whether the device is operating.
            compPowerTrader.PowerOutput = -cachedPowerConsumption;

            // Does nothing if flicked off, unpowered, or there are no pawns to charge.
            if (!Active || chargingPawns.NullOrEmpty())
            {
                return;
            }

            Room currentRoom = parent.Map?.regionGrid.GetValidRegionAt_NoRebuild(parent.Position)?.Room;
            if (currentRoom == null || !currentRoom.ProperRoom)
            {
                return;
            }

            // The amount to replenish depends on the tick rate of the device. A longer tick interval means it is being called less, and should charge more to compensate.
            // The parent comp treats normal ticker types as Rare, so Rare is the shortest acceptable interval.
            int effectiveTicks = parent.def.tickerType == TickerType.Long ? GenTicks.TickLongInterval : GenTicks.TickRareInterval;
            float effectiveCharge = Props.baseChargeRatePerDay / GenDate.TicksPerDay * effectiveTicks;

            // The heat pusher output is not modifiable. Simply push the cached heat (or cold) if necessary.
            if (cachedHeatGeneration != 0f)
            {
                GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, cachedHeatGeneration * (GenTicks.SecondsPerTick * effectiveTicks));
            }

            foreach (Pawn pawn in chargingPawns)
            {
                if (pawn.needs?.TryGetNeed(ABF_NeedDefOf.ABF_Need_Synstruct_Energy) is Need need)
                {
                    need.CurLevel += effectiveCharge;
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            string output = base.CompInspectStringExtra();
            if (output != null)
            {
                return output;
            }
            if (!chargingPawns.NullOrEmpty() && chargingPawns.Count > 1)
            {
                return "ABF_IncreasedLoad".Translate();
            }
            return null;
        }
    }
}