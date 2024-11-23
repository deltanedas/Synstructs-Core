using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Simple thought worker that will return True if the pawn is assigned to Hunting. The ThoughtDef controls who gets it.
    public class ThoughtWorker_BiophileUnhappyWorkTypes : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            return p.workSettings?.WorkIsActive(WorkTypeDefOf.Hunting) == true;
        }
    }
}
