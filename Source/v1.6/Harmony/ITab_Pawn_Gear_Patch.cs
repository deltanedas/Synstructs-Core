using Verse;
using HarmonyLib;
using RimWorld;

namespace ArtificialBeings
{
    public class ITab_Pawn_Gear_Patch
    {
        // Synstruct animals with draft controllers do not use equipment, and so should not try to show equipment in the equipment tab.
        [HarmonyPatch(typeof(ITab_Pawn_Gear), "ShouldShowEquipment")]
        public static class ITab_Pawn_Gear_ShouldShowEquipment_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn p, ref bool __result)
            {
                if (SC_Utils.IsSynstruct(p) && p.RaceProps.intelligence == Intelligence.Animal)
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }
    }
}