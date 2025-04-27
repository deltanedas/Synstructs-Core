using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public abstract class CompRoomCharger : ThingComp
    {
        public CompProperties_RoomCharger Props
        {
            get
            {
                return props as CompProperties_RoomCharger;
            }
        }

        public virtual bool Active => true;

        protected int tickInterval;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            tickInterval = parent.def.tickerType == TickerType.Long ? GenTicks.TickLongInterval : parent.def.tickerType == TickerType.Rare ? GenTicks.TickRareInterval : 1;
            // Add this charger to the map set so each charger can find the others easily and force a recache.
            SC_MapComponent mapComponent = parent.Map.GetComponent<SC_MapComponent>();
            mapComponent.chargers.Add(this);
            if (!respawningAfterLoad)
            {
                SC_Utils.UpdateRoomChargers(mapComponent.chargers);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            // Remove the charger from the charger set. Only force a recache if it actually needed to be removed.
            SC_MapComponent mapComponent = map.GetComponent<SC_MapComponent>();
            if (mapComponent.chargers.Remove(this))
            {
                SC_Utils.UpdateRoomChargers(mapComponent.chargers);
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            // Remove the charger from the charger set. Only force a recache if it actually needed to be removed.
            SC_MapComponent mapComponent = previousMap.GetComponent<SC_MapComponent>();
            if (mapComponent.chargers.Remove(this))
            {
                SC_Utils.UpdateRoomChargers(mapComponent.chargers);
            }
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            // Remove the charger from the charger set. Only force a recache if it actually needed to be removed.
            SC_MapComponent mapComponent = prevMap.GetComponent<SC_MapComponent>();
            if (mapComponent.chargers.Remove(this))
            {
                SC_Utils.UpdateRoomChargers(mapComponent.chargers);
            }
        }

        public override void Notify_MapRemoved()
        {
            base.Notify_MapRemoved();
            // Remove the charger from the charger set. Only force a recache if it actually needed to be removed.
            SC_MapComponent mapComponent = parent.Map.GetComponent<SC_MapComponent>();
            if (mapComponent.chargers.Remove(this))
            {
                SC_Utils.UpdateRoomChargers(mapComponent.chargers);
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(GenTicks.TickRareInterval))
            {
                DoChargePulse();
            }
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            DoChargePulse();
        }

        public override void CompTickLong()
        {
            base.CompTickLong();
            DoChargePulse();
        }

        // Using the provided room and lists of chargers and pawns in the room, calculate the effects of charging. Baseline Only implies the device should be treated as inoperable.
        // All roomChargers are in the parentRoom, and will include this CompRoomCharger.
        public abstract void RecalculateCaches(Room parentRoom, List<CompRoomCharger> roomChargers, List<Pawn> roomPawns, bool baselineOnly = false);

        // Applies the effect of the pulse, whatever that may be.
        public abstract void DoChargePulse();

        public override string CompInspectStringExtra()
        {
            if (Props.inoperableOutdoors && parent.GetRoom()?.UsesOutdoorTemperature == true)
            {
                return "ABF_InoperableOutdoors".Translate();
            }
            else if (Props.inoperableInLargeRooms && parent.GetRoom()?.IsHuge != false)
            {
                return "ABF_InoperableInLargeRooms".Translate();
            }
            return null;
        }
    }
}