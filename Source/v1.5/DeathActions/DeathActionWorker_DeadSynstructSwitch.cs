using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace ArtificialBeings
{
    public class DeathActionWorker_DeadSynstructSwitch : DeathActionWorker
    {
        public DeathActionProperties_DeadSynstructSwitch Props
        {
            get
            {
                return props as DeathActionProperties_DeadSynstructSwitch;
            }
        }

        public override bool DangerousInMelee => true;

        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            List<Thing> thingsToIgnore = new List<Thing>();
            if (Props.ignoreThingsOfSameFaction)
            {
                Faction faction = corpse.InnerPawn.Faction;
                List<Thing> mapThings = corpse.MapHeld.listerThings.AllThings;
                for (int i = mapThings.Count; --i >= 0;)
                {
                    if ((mapThings[i].Faction == faction && faction != null) || (Props.ignoreThingsOfSameDef && mapThings[i].def == corpse.InnerPawn.def))
                    {
                        thingsToIgnore.Add(mapThings[i]);
                    }
                }
            }
            else if (Props.ignoreThingsOfSameDef)
            {
                thingsToIgnore = corpse.MapHeld.listerThings.ThingsOfDef(corpse.InnerPawn.def);
            }
            GenExplosion.DoExplosion(corpse.Position, corpse.MapHeld, Props.explosionRadius, Props.damageType ?? DamageDefOf.Bomb, corpse, Props.damageAmount, Props.armorPenetration, Props.explosionSound, preExplosionSpawnThingDef: Props.preExplosionSpawnThingDef, preExplosionSpawnChance: Props.preExplosionSpawnChance, preExplosionSpawnThingCount: Props.preExplosionSpawnThingCount, postExplosionSpawnThingDef: Props.postExplosionSpawnThingDef, postExplosionSpawnChance: Props.postExplosionSpawnChance, postExplosionSpawnThingCount: Props.postExplosionSpawnThingCount, ignoredThings: thingsToIgnore);
            if (Props.destroyCorpse)
            {
                corpse.Destroy();
            }
        }
    }
}
