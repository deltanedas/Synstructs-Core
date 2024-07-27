using HarmonyLib;
using System.Reflection;
using Verse;

namespace ArtificialBeings
{
    [StaticConstructorOnStartup]
    public static class SynstructsCore_VFEPCompatibility_PostInit
    {
        static SynstructsCore_VFEPCompatibility_PostInit()
        {
            new Harmony("SynstructsCore_VFEPCompatibility").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}