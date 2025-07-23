using RimWorld;
using UnityEngine;
using Verse;

namespace ArtificialBeings
{
    public class CompEnergyReservoir_Powered : CompEnergyReservoir
    {
        public CompPowerTrader compPowerTrader;

        public override bool Usable => compPowerTrader.PowerOn && base.Usable;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compPowerTrader = parent.GetComp<CompPowerTrader>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (compPowerTrader.PowerOn)
            {
                reserve = Mathf.Clamp(reserve + Mathf.Abs(compPowerTrader.PowerOutput * Props.energyEfficiency * GenTicks.TickRareInterval) / GenDate.TicksPerDay, 0, Props.maximumReserve);
            }
            // If we are at maximum reserve, keep the power consumption much lower.
            if (reserve >= Props.maximumReserve)
            {
                compPowerTrader.PowerOutput = (0 - compPowerTrader.Props.PowerConsumption) * 0.1f;
            }
            else
            {
                compPowerTrader.PowerOutput = 0 - compPowerTrader.Props.PowerConsumption;
            }
        }

        // Energized reservoirs release all energy as an EMP upon taking damage.
        public override void ReactToDamage(DamageInfo dinfo)
        {
            if (parent.HitPoints > 0 && parent.Spawned)
            {
                GenExplosion.DoExplosion(parent.Position, parent.Map, reserve / 2, DamageDefOf.EMP, parent, (int)Mathf.Pow(reserve, 1.5f));
                reserve = 0;
            }
        }
    }
}