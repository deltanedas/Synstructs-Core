using Verse;
using HarmonyLib;
using RimWorld;
using System.Linq;

namespace ArtificialBeings
{
    public class Pawn_Patch
    {
        // Some things explode when they die and are destroyed instantly. Drones with Martyrdom turn into slag when killed.
        [HarmonyPatch(typeof(Pawn), "Kill")]
        public static class Kill_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(ref Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
            {
                ABF_DetonateOnIncapacitation detonationExtension = __instance.def.GetModExtension<ABF_DetonateOnIncapacitation>();
                bool isExploding = detonationExtension != null;
                bool isMartyr = ABF_Utils.IsProgrammableDrone(__instance) && !__instance.Destroyed && __instance.GetComp<CompArtificialPawn>().ActiveDirectives.Contains(ABF_DirectiveDefOf.ABF_Directive_Synstruct_Martyr);
                if (!__instance.Destroyed && (isExploding || isMartyr))
                {
                    // Save details and destroy before doing the explosion to avoid the damage hitting the pawn, killing them again.
                    IntVec3 tempPos = __instance.Position;
                    Map tempMap = __instance.Map;
                    __instance.Destroy();
                    if (isExploding)
                    {
                        GenExplosion.DoExplosion(tempPos, tempMap, detonationExtension.explosionRadius, detonationExtension.damageType, __instance, detonationExtension.damageAmount, postExplosionSpawnThingDef: detonationExtension.itemToSpawnOnDetonation, postExplosionSpawnChance: detonationExtension.itemToSpawnOnDetonation != null ? 1 : 0, postExplosionSpawnThingCount: detonationExtension.itemCountToSpawn);
                    }
                    if (isMartyr)
                    {
                        GenExplosion.DoExplosion(tempPos, tempMap, 0.4f, DamageDefOf.Bomb, __instance, 1);
                        Thing slag = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel);
                        GenSpawn.Spawn(slag, tempPos, tempMap);
                    }
                    return false;
                }
                return true;
            }
        }

        // Let Synstruct animals take orders.
        [HarmonyPatch(typeof(Pawn), "get_CanTakeOrder")]
        public static class Pawn_get_CanTakeOrder_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(ref Pawn __instance, ref bool __result)
            {
                if (SC_Utils.IsSynstruct(__instance) && __instance.RaceProps.intelligence == Intelligence.Animal)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }
    }
}