using System.Collections.Generic;
using Verse;

namespace ArtificialBeings
{
    public class CompTargetEffectProperties_ResurrectPawn : CompProperties
    {
        public CompTargetEffectProperties_ResurrectPawn()
        {
            compClass = typeof(CompTargetEffect_ResurrectPawn);
        }

        public List<ABF_ArtificialState> validStates;
    }
}