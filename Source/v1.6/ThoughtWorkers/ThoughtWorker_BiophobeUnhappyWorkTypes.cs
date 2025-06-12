using RimWorld;
using Verse;

namespace ArtificialBeings
{
    // Thought worker determining whether a pawn is working with organic life. The ThoughtDef controls who gets it.
    public class ThoughtWorker_BiophobeUnhappyWorkTypes : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Pawn_WorkSettings settings = p.workSettings;

            if (settings == null || !settings.Initialized)
            {
                return false;
            }

            // Any of these three work types mean working with plants or organic materials.
            if (settings.WorkIsActive(ABF_WorkTypeDefOf.Cooking) || settings.WorkIsActive(WorkTypeDefOf.Growing) || settings.WorkIsActive(WorkTypeDefOf.PlantCutting))
            {
                return true;
            }

            // Handling is only working with organic life if there are any player organic animals on the pawn's map. Biomimetics do not count.
            if (p.Map == null)
            {
                return false;
            }
            return settings.WorkIsActive(WorkTypeDefOf.Handling) && SC_Utils.playerOrganicAnimalCount.GetWithFallback(p.Map, 0) > 0;
        }
    }
}
