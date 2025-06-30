using RimWorld;
using System.Collections.Generic;

namespace ArtificialBeings
{
    public class CompTargetProperties_SynstructCorpse : CompProperties_Targetable
    {
        public CompTargetProperties_SynstructCorpse()
        {
            compClass = typeof(CompTargetable_SynstructCorpse);
        }

        public List<ABF_ArtificialState> validStates;
    }
}