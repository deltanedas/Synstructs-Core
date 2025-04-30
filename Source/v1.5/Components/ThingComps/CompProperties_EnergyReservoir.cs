using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class CompProperties_EnergyReservoir : CompProperties
    {
        public CompProperties_EnergyReservoir()
        {
            compClass = typeof(CompEnergyReservoir);
        }

        public float minimumReserveBeforeReady = 0.2f;

        public float maximumReserve = -1f;

        public float energyEfficiency = 1f;

        public bool reactsIfDamagedAboveMinimumReserve = false;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (minimumReserveBeforeReady < 0 || minimumReserveBeforeReady > 1f)
            {
                yield return $"{parentDef.defName} specified a minimum reserve required to provide charge of {minimumReserveBeforeReady}, while it must be between 0 and 1.";
            }

            if (maximumReserve < 0f)
            {
                yield return $"{parentDef.defName} did not specify a maximum reserve for its energy reservoir comp.";
            }
        }
    }
}