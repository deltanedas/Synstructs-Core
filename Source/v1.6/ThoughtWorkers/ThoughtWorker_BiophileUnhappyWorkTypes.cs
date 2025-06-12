using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple thought worker that will return True if the pawn is assigned to Hunting. The ThoughtDef controls who gets it.
    public class ThoughtWorker_BiophileUnhappyWorkTypes : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Pawn_WorkSettings settings = p.workSettings;

            if (settings == null || !settings.Initialized)
            {
                return false;
            }

            return settings.WorkIsActive(WorkTypeDefOf.Hunting) == true;
        }
    }
}
