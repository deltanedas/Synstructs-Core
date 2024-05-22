using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_PaintedColor_Favorite : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.story?.favoriteColor.HasValue == true && SC_Utils.cachedSynstructs.Contains(p.def))
            {
                return p.story?.favoriteColor.Value == p.story?.SkinColorBase;
            }
            return false;
        }
    }
}
