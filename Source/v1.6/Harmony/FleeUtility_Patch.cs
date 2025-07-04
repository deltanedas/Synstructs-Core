using Verse;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class FleeUtility_Patch
    {
        // Synstruct animals that are drafted should not flee danger.
        [HarmonyPatch(typeof(FleeUtility), "ShouldAnimalFleeDanger")]
        public class FleeUtility_ShouldAnimalFleeDanger_Patch
        {
            [HarmonyPostfix]
            public static bool Listener(bool __result, Pawn pawn)
            {
                return __result && !(SC_Utils.IsSynstruct(pawn) && pawn.Drafted);
            }
        }
    }
}