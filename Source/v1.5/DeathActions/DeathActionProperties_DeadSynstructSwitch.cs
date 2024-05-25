using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class DeathActionProperties_DeadSynstructSwitch : DeathActionProperties
    {
        public float explosionRadius = 0.9f;
        public DamageDef damageType;
        public int damageAmount = -1;
        public float armorPenetration = -1;
        public SoundDef explosionSound;

        public ThingDef preExplosionSpawnThingDef;
        public float preExplosionSpawnChance = 0;
        public int preExplosionSpawnThingCount = 1;

        public ThingDef postExplosionSpawnThingDef;
        public float postExplosionSpawnChance = 0;
        public int postExplosionSpawnThingCount = 1;

        public bool ignoreThingsOfSameDef;
        public bool ignoreThingsOfSameFaction;
        public bool destroyCorpse;

        public DeathActionProperties_DeadSynstructSwitch()
        {
            workerClass = typeof(DeathActionWorker_DeadSynstructSwitch);
        }
    }
}
