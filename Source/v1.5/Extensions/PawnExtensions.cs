using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple mod extension that marks the race as a synstruct and holds data for what backstories should be applied when becoming a drone/sapient.
    public class ABF_SynstructExtension : DefModExtension
    {
        public BackstoryDef droneChildhoodBackstoryDef;
        public BackstoryDef droneAdulthoodBackstoryDef;
        public BackstoryDef sapientChildhoodBackstoryDef;
        public BackstoryDef sapientAdulthoodBackstoryDef;
    }

    // Mod extension for particular races to mark them as something that should explode on downed or death.
    public class ABF_DetonateOnIncapacitation : DefModExtension
    {
        public float explosionRadius = 0.5f;
        public DamageDef damageType;
        public int damageAmount;

        public ThingDef itemToSpawnOnDetonation;
        public int itemCountToSpawn;
    }
}