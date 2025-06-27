using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class CompProperties_SynstructStimulator : CompProperties
    {
        public CompProperties_SynstructStimulator()
        {
            compClass = typeof(CompSynstructStimulator);
        }

        public HediffDef hediffToApply;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (hediffToApply is null)
            {
                yield return $"{parentDef.defName} did not provide a hediff to apply on consumption!";
            }
        }
    }
}