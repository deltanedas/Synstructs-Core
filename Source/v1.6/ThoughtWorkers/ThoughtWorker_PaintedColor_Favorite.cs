using RimWorld;
using Verse;

namespace ArtificialBeings
{
    public class ThoughtWorker_PaintedColor_Favorite : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.story?.favoriteColor != null && SC_Utils.cachedSynstructs.Contains(p.def))
            {
                return p.story?.favoriteColor.color == p.story?.SkinColorBase;
            }
            return false;
        }
    }
}
