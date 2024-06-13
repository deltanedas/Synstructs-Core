using HarmonyLib;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using RimWorld;

namespace ArtificialBeings
{
    public class SynstructsCore : Mod
    {

        public SynstructsCore(ModContentPack content) : base(content)
        {
            new Harmony("SynstructsCore").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [StaticConstructorOnStartup]
    public static class SynstructsCore_PostInit
    {
        static SynstructsCore_PostInit()
        {
            CompProperties_Facility bedsideChargerLinkables = ABF_ThingDefOf.ABF_Thing_Synstruct_BedsideCharger.GetCompProperties<CompProperties_Facility>();

            // Dynamically modify some ThingDefs based on certain qualifications.
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                // Check race to see if the thingDef is for a Pawn.
                if (thingDef.race != null && thingDef.HasModExtension<ABF_SynstructExtension>())
                {
                    SC_Utils.cachedSynstructs.Add(thingDef);
                }

                // Beds that accept linkables should have the bedside charger added as a linkable.
                if (thingDef.IsBed && thingDef.GetCompProperties<CompProperties_AffectedByFacilities>() is CompProperties_AffectedByFacilities linkable)
                {
                    linkable.linkableFacilities.Add(ABF_ThingDefOf.ABF_Thing_Synstruct_BedsideCharger);
                    bedsideChargerLinkables.linkableBuildings.Add(thingDef);
                }
            }

            // Coherence Effect workers must have their def and extension references manually defined here as they are created via a def mod extension which is def-blind and does not self-initialize.
            foreach (HediffDef hediffDef in DefDatabase<HediffDef>.AllDefsListForReading)
            {
                if (hediffDef.GetModExtension<ABF_CoherenceEffectExtension>() is ABF_CoherenceEffectExtension effectExtension)
                {
                    if (effectExtension.CoherenceWorkers is List<CoherenceWorker> coherenceWorkers)
                    {
                        foreach (CoherenceWorker coherenceWorker in coherenceWorkers)
                        {
                            coherenceWorker.def = hediffDef;
                        }
                    }
                }
            }
        }
    }
}