using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace ArtificialBeings
{
    // Give a job to wander near grass, and also plants I guess.
    public class JobGiver_WanderGrassObsession : JobGiver_Wander
    {
        public JobGiver_WanderGrassObsession()
        {
            wanderRadius = 2f;
            ticksBetweenWandersRange = new IntRange(240, 360);
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            Map map = pawn.Map;
            if (map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count == 0)
            {
                return null;
            }
            return base.TryGiveJob(pawn);
        }

        protected override IntVec3 GetWanderRoot(Pawn pawn)
        {
            Map map = pawn.Map;
            return map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).RandomElementByWeight(plant => 
                (pawn.CanReach(plant.Position, PathEndMode.Touch, Danger.Deadly) ? 1 : 0) * // Do not wander near forbidden plant. No touch.
                (plant.def == ThingDefOf.Plant_Grass ? 3 : 1) * // Prefer grass.
                (pawn.Map.areaManager.Home.ActiveCells.Contains(plant.Position) ? 8 : 1) * // Prefer plants in the home area.
                (plant.Position.InAllowedArea(pawn) ? 6 : 1) // Prefer plants in the pawn's allowed area (stacks with home area).
                ).Position;
        }
    }
}
