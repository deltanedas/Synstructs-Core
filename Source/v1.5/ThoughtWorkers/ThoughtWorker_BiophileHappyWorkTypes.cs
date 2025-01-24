using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple thought worker that will return True if the pawn is assigned to handling or growing. The ThoughtDef controls who gets it.
    public class ThoughtWorker_BiophileHappyWorkTypes : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Pawn_WorkSettings settings = p.workSettings;

            if (settings == null || !settings.Initialized)
            {
                return false;
            }

            return settings.WorkIsActive(WorkTypeDefOf.Handling) || settings.WorkIsActive(WorkTypeDefOf.Growing);
        }
    }
}
